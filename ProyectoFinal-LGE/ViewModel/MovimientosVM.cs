using Microsoft.Office.Interop.Excel;
using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace ProyectoFinal_LGE.ViewModel
{
    /// <summary>
    /// Vista modelo para transacciones.
    /// </summary>
    public class MovimientosVM
    {
 
        private ObservableCollection<Transaccion> listaTransacciones;
        /// <summary>
        /// Lista de transacciones.
        /// </summary>
        public ObservableCollection<Transaccion> ListaTransacciones
        {
            get { return listaTransacciones; }
        }

        // Titulo de la cabecera del MessageBox
        string cabeceraInfo = (string)Application.Current.FindResource("informacion");
        // Mensaje del MessageBox
        string mensajeBox = null;

        /// <summary>
        /// Constructor de MovimientosVM.
        /// </summary>
        public MovimientosVM()
        {
            //Creo la lista de transacciones
            listaTransacciones = new ObservableCollection<Transaccion>();

            // LLamo a la instancia y la conecto a la DB
            MySQLDB.Instance.ConnectDB("Transacciones");

            // Recorrer el array de transacciones y coger las transacciones y asginarlas a la lista
            if (MySQLDB.Instance.ArrayTransacciones != null)
            {
                foreach (Transaccion a in MySQLDB.Instance.ArrayTransacciones)
                {
                    listaTransacciones.Add(a);
                }
            }
        }
      

        #region Transacciones

        private ICommand btnBorrarT;
        /// <summary>
        /// Comando del botón eliminar transaccion. Llama al método BorrarTransaccion(Transaccion).
        /// </summary>
        public ICommand BtnBorrarT
        {
            get
            {
                if (btnBorrarT == null)
                {
                    btnBorrarT = new RelayCommand(param => this.BorrarTransaccion((Transaccion)param));
                }
                return btnBorrarT;
            }
        }

        private ICommand exportTransaccion;
        /// <summary>
        ///  Comando del botón exportar transacciones. Llama al método AccionExportar(Datagrid).
        /// </summary>
        public ICommand ExportTransaccion
        {
            get
            {
                if (exportTransaccion == null)
                {
                    exportTransaccion = new RelayCommand(param => this.AccionExportar((DataGrid)param));
                }
                return exportTransaccion;
            }
        }

        /// <summary>
        /// Método que elimina la transaccién pasada como parámetro.
        /// </summary>
        /// <param name="t">Transacción a eliminar</param>
        public void BorrarTransaccion(Transaccion t)
        {
            // preguntar si desea eliminar la transaccion
            mensajeBox = (string)Application.Current.FindResource("deseaEliminarTransaccion");
            if (MessageBox.Show(mensajeBox, cabeceraInfo,
                         MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                if (MySQLDB.Instance.BorrarTransaccion(t)) // eliminar la transaccion de la BD
                {
                    listaTransacciones.Remove(t);
                    mensajeBox = (string)Application.Current.FindResource("transaccionEliminada");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    UIGlobal.PageMovimientos.datagridT.Items.Refresh();
                }
            }

        }

        /// <summary>
        /// Método que exporta las transacciones a un fichero CSV abriendo el explorador de archivos
        /// para seleccionar donde guardar el fichero. Exportado correctamente, se pregunta si se quiere abrir la información
        /// en Microsoft Excel.
        /// </summary>
        /// <param name="da">Datagrid de transacciones a exportar</param>
        private void AccionExportar(DataGrid da)
        {
            if (da.Items.Count != 0) // debe haber informacion para exportar
            {
                // Abrir explorador de archivos para seleccionar donde guardar el fichero
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

                // Filtrar fichero por CSV
                saveFileDialog.Filter = "CSV Files| *.csv";
                saveFileDialog.Title = (string)Application.Current.FindResource("guardarFichero");
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "") // si se selecciona un fichero
                {

                    StringBuilder builder = new StringBuilder();

                    // se escribe en la primera linea del fichero 'Transacciones'
                    string transacciones = (string)Application.Current.FindResource("Transacciones");
                    builder.AppendLine(transacciones);

                    // se añade cada transacción separando los campos por ';'
                    foreach (Transaccion t in ListaTransacciones)
                    {
                        string registro = string.Format("{0};{1};{2};{3};{4}", t.Fecha, t.Producto, t.AlmacenN, t.AlmacenV, t.Unidades);
                        builder.AppendLine(registro);
                    }
                    // escribir todo el texto al fichero
                    File.WriteAllText(saveFileDialog.FileName, builder.ToString());

                    // preguntar si se quiere abrir el fichero en Excel
                    mensajeBox = (string)Application.Current.FindResource("exportarExcel");
                    if (MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        ExportarExcel(da); // Exportar a Excel
                    }
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("noInfoCSV");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Método que exporta a Microsoft Excel el datagrid con la información de las transacciones. 
        /// </summary>
        /// <param name="da">Datagrid de transacciones a exportar</param>
        private void ExportarExcel(DataGrid da)
        {
            try
            {
                // Abrir Microsoft Excel
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet = (Worksheet)workbook.Sheets[1];
                // Escribir los nombres de las columnas en negrita
                for (int j = 0; j < 5; j++)
                {
                    sheet.Range["A1"].Offset[0, j].Value = da.Columns[j].Header;
                    sheet.Cells[1, j + 1].Font.Bold = true;
                }

                // Por cada almacen se escriben sus campos en las celdas
                var row = 1;
                foreach (Transaccion t in ListaTransacciones)
                {
                    row++;
                    sheet.Cells[row, "A"] = t.Fecha;
                    sheet.Cells[row, "B"] = t.Producto;
                    sheet.Cells[row, "C"] = t.AlmacenV;
                    sheet.Cells[row, "D"] = t.AlmacenN;
                    sheet.Cells[row, "E"] = t.Unidades;
                }
                sheet.Columns.AutoFit(); // Ajustar el contenido
            }
            catch (Exception e) // No existe el programa Microsoft Excel
            {
                mensajeBox = (string)Application.Current.FindResource("microsoftExcel");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }
        }


        #endregion
    }
}
