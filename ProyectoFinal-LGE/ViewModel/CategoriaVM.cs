using Microsoft.Office.Interop.Excel;
using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.View.VentanasAddEdit;
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
    /// Vista modelo para categorías.
    /// </summary>
    public class CategoriaVM : ObservableCollection<Categoria>
    {
        private ObservableCollection<Categoria> listaCategorias;

        /// <summary>
        /// Lista de categorías.
        /// </summary>
        public ObservableCollection<Categoria> ListaCategorias
        {
            get { return listaCategorias; }
        }

        // Titulo de la cabecera del MessageBox
        string cabeceraInfo = (string)Application.Current.FindResource("informacion");
        // Mensaje del MessageBox
        string mensajeBox = null;

        /// <summary>
        /// Constructor de CategoriaVM.
        /// </summary>
        public CategoriaVM()
        {
            //Creo la lista de categorias
            listaCategorias = new ObservableCollection<Categoria>();

            // LLamo a la instancia y la conecto a la DB
            MySQLDB.Instance.ConnectDB("Categorias");

            // Recorrer el array de categorias y coger las categorias y asginarlos a la lista
            if (MySQLDB.Instance.ArrayCategorias != null)
            {
                foreach (Categoria c in MySQLDB.Instance.ArrayCategorias)
                {
                    listaCategorias.Add(c);
                }
            }


        }

        #region Comandos
        private ICommand btnDeleteCategoria;
        /// <summary>
        /// Comando del botón eliminar categoría. Llama al método BorrarCategoria(Categoria).
        /// </summary>
        public ICommand BtnDeleteCategoria
        {
            get
            {
                if (btnDeleteCategoria == null)
                {
                    btnDeleteCategoria = new RelayCommand(param => this.BorrarCategoria((Categoria)param));
                }
                return btnDeleteCategoria;
            }
        }

        private ICommand btnAddCategoria;
        /// <summary>
        ///  Comando del botón añadir categoría. Llama al método AccionAdd().
        /// </summary>
        public ICommand BtnAddCategoria
        {
            get
            {
                if (btnAddCategoria == null)
                {
                    btnAddCategoria = new RelayCommand(param => this.AccionAdd(), null);
                }
                return btnAddCategoria;
            }
        }

        private ICommand btnEditCategoria;
        /// <summary>
        /// Comando del botón editar categoría. Llama al método AccionEdit(Categoria).
        /// </summary>
        public ICommand BtnEditCategoria
        {
            get
            {
                if (btnEditCategoria == null)
                {
                    btnEditCategoria = new RelayCommand(param => this.AccionEdit((Categoria)param));
                }
                return btnEditCategoria;
            }
        }

        private ICommand exportCategoria;
        /// <summary>
        ///  Comando del botón exportar categorías. Llama al método AccionExportar(Datagrid).
        /// </summary>
        public ICommand ExportCategoria
        {
            get
            {
                if (exportCategoria == null)
                {
                    exportCategoria = new RelayCommand(param => this.AccionExportar((DataGrid)param));
                }
                return exportCategoria;
            }
        }


        private ICommand importCategoria;
        /// <summary>
        /// Comando del botón importar categorías. Llama al método AccionImportar().
        /// </summary>
        public ICommand ImportCategoria
        {
            get
            {
                if (importCategoria == null)
                {
                    importCategoria = new RelayCommand(param => this.AccionImportar(), null);
                }
                return importCategoria;
            }
        }


        #endregion

        #region Acciones
        /// <summary>
        /// Método que elimina la categoría pasada como parámetro.
        /// </summary>
        /// <param name="c">Categoría a eliminar</param>
        public void BorrarCategoria(Categoria c)
        {
            if (c != null)// debe estar seleccionado una categoría
            {
                // preguntar si desea eliminar la categoría
                mensajeBox = (string)Application.Current.FindResource("deseaEliminarCategoria");
                if (MessageBox.Show(mensajeBox + "\n" + c.Id + " - " + c.Nombre, cabeceraInfo,
                        MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    if ((MySQLDB.Instance.BorrarCategoria(c))) // eliminar la categoría de la BD
                    {
                        listaCategorias.Remove(c);

                        mensajeBox = (string)Application.Current.FindResource("categoriaEliminada");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarCategoria");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Metodo que abre la ventana de añadir categoría (AddEditCategoria).
        /// </summary>
        private void AccionAdd()
        {
            AddEditCategoria page = new AddEditCategoria(this);
            page.ShowDialog();
        }

        /// <summary>
        /// Método que abre la ventana de modificar categoría (AddEditCategoria).
        /// </summary>
        /// <param name="c">Categoría a modificar</param>
        private void AccionEdit(Categoria c)
        {
            if (c != null) // Debe estar seleccionado una categoría
            {
                AddEditCategoria page = new AddEditCategoria(this, c);
                page.ShowDialog();
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarCategoria");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Método que añade la categoría pasada como parámetro a la base de datos comprobando que ciertos campos
        /// no esten vacios y tengan el formato correcto.
        /// </summary>
        /// <param name="c">Categoría a añadir</param>
        public void AddCategoria(Categoria c)
        {
            // codigo no este vacio y ocupe mas de 3 caracteres
            if (c.Id != null && !c.Id.Trim().Equals("") && c.Id.Length <= 3)
            {
                c.Id = c.Id.Trim();
                // nombre no este vacio y ocupe mas de 20 caracteres
                if (c.Nombre != null && !c.Nombre.Trim().Equals("") && c.Id.Length <= 20)
                {
                    // descripcion no este vacia y ocupe mas de 90 caracteres
                    if (c.Descripcion != null && !c.Descripcion.Trim().Equals("") && c.Descripcion.Length <= 90)
                    {
                        // codigo no este repetido
                        bool repetidoId = false;
                        for (int i = 0; i < listaCategorias.Count; i++)
                        {
                            Categoria am = listaCategorias[i];
                            if (am.Id.Equals(c.Id))
                            {
                                repetidoId = true;
                            }
                        }
                        if (!repetidoId) // si la categoria no existe en la BD
                        {
                            // añadir categoria a la base de datos
                            if (MySQLDB.Instance.InsertarCategoria(c))
                            {
                                listaCategorias.Add(c);// añadir a la lista
                                                       // cerrar ventana
                                UIGlobal.AddEditCategoria.Close();
                                mensajeBox = (string)Application.Current.FindResource("categoriaAñadida");
                                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            mensajeBox = (string)Application.Current.FindResource("categoriaIdRepetido");
                            cabeceraInfo = (string)Application.Current.FindResource("error");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                            UIGlobal.AddEditCategoria.id.Focus();
                        }
                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("categoriaDescMax");
                        cabeceraInfo = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                        UIGlobal.AddEditCategoria.desc.Focus();
                    }
                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("categoriaNombreNoVacio");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditCategoria.nombre.Focus();
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("categoriaIdNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditCategoria.id.Focus();
            }

        }

        /// <summary>
        /// Método que modifica la categoría pasada como parámetro en la base de datos comprobando que ciertos campos
        /// no esten vacios y tengan el formato correcto.
        /// </summary>
        /// <param name="c">Categoría a modificar</param>
        public void EditCategoria(Categoria c)
        {
            // nombre no este vacio y ocupe mas de 20 caracteres
            if (c.Nombre != null && !c.Nombre.Trim().Equals("") && c.Id.Length <= 20)
            {
                // descripcion no ocupe mas de 40 caracteres
                if (c.Descripcion.Length <= 90)
                {
                    if (MySQLDB.Instance.ModificarCategoria(c)) // modificar categoria en la base de datos
                    {
                        //Comprobar cual es la categoria de la lista que tenemos que modificar
                        for (int i = 0; i < listaCategorias.Count; i++)
                        {
                            Categoria am = listaCategorias[i];
                            if (am.Id.Equals(c.Id))
                            {
                                listaCategorias[i] = c;
                            }
                        }
                        // cerrar ventana
                        UIGlobal.AddEditCategoria.Close();
                        mensajeBox = (string)Application.Current.FindResource("categoriaActualizada");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("categoriaDescMax");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditCategoria.desc.Focus();
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("categoriaNombreNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditCategoria.nombre.Focus();
            }

        }

        /// <summary>
        /// Método que exporta las categorías a un fichero CSV abriendo el explorador de archivos
        /// para seleccionar donde guardar el fichero. Exportado correctamente, se pregunta si se quiere abrir la información
        /// en Microsoft Excel.
        /// </summary>
        /// <param name="da">Datagrid de categorías a exportar</param>
        private void AccionExportar(DataGrid da)
        {
            if (da.Items.Count != 0)// debe haber informacion para exportar
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

                    // se escribe en la primera linea del fichero 'Categorias'
                    string categorias = (string)Application.Current.FindResource("Categorias");
                    builder.AppendLine(categorias);
                    // se añade cada categoria separando los campos por ';'
                    foreach (Categoria c in ListaCategorias)
                    {
                        string registro = string.Format("{0};{1};{2}", c.Id, c.Nombre, c.Descripcion);
                        builder.AppendLine(registro);
                    }
                    // escribir todo el texto al fichero
                    File.WriteAllText(saveFileDialog.FileName, builder.ToString());

                    // preguntar si se quiere abrir el fichero en Excel
                    mensajeBox = (string)Application.Current.FindResource("exportarExcel");
                    if (MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        ExportarExcel(da);// Exportar a Excel
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
        /// Método que exporta a Microsoft Excel el datagrid con la información de las categorías. 
        /// </summary>
        /// <param name="da">Datagrid de categorías a exportar</param>
        private void ExportarExcel(DataGrid da)
        {
            try
            { // Abrir Microsoft Excel
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet = (Worksheet)workbook.Sheets[1];

                // Escribir los nombres de las columnas en negrita
                for (int j = 0; j < da.Columns.Count; j++)
                {
                    sheet.Range["A1"].Offset[0, j].Value = da.Columns[j].Header;
                    sheet.Cells[1, j + 1].Font.Bold = true;
                }

                // Por cada categoría se escriben sus campos en las celdas
                var row = 1;
                foreach (Categoria c in ListaCategorias)
                {
                    row++;
                    sheet.Cells[row, "A"] = c.Id;
                    sheet.Cells[row, "B"] = c.Nombre;
                    sheet.Cells[row, "C"] = c.Descripcion;
                }
                sheet.Columns.AutoFit();// Ajustar el contenido
            }
            catch (Exception e) // No existe el programa Microsoft Excel
            {
                mensajeBox = (string)Application.Current.FindResource("microsoftExcel");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }
        }

        /// <summary>
        /// Método que importa categorías desde un fichero CSV. Abre el explorador de archivos para obtener el fichero
        /// a importar y controla que sea un fichero de categorias, esten todos los campos, no haya categorías repetidass y
        /// no esten ciertos campos vacios o con formatos erroneos.
        /// </summary>
        private void AccionImportar()
        {
            // abrir el explorador de archivos
            System.Windows.Forms.OpenFileDialog dialogoBuscarArchivo = new System.Windows.Forms.OpenFileDialog();
            // filtrar for ficheros csv
            dialogoBuscarArchivo.Filter = "CSV Files|*.csv";
            dialogoBuscarArchivo.Title = (string)Application.Current.FindResource("abrirFicheroCSV");
            dialogoBuscarArchivo.FilterIndex = 1;
            dialogoBuscarArchivo.Multiselect = false;

            if (dialogoBuscarArchivo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Stream stream = dialogoBuscarArchivo.OpenFile()) // abrir el ficher
                {
                    string ruta = dialogoBuscarArchivo.FileName;

                    StreamReader file = new StreamReader(ruta);
                    string line;
                    // Fichero debe comenzar en la primera linea con 'Categorias'
                    string m = (string)Application.Current.FindResource("Categorias");
                    if (file.ReadLine().Contains(m))
                    {
                        int error = 0; // controlar errores
                        int numLinea = 0; // controlar el numero de linea
                        List<Categoria> arrayCategorias = new List<Categoria>(); // categorias a añadir

                        while ((line = file.ReadLine()) != null) // leer cada linea del fichero
                        {
                            String[] l = line.Split(';'); // obtener los campos
                            numLinea++;

                            Categoria c;
                            MySQLDB.Instance.ConnectDB("IdCategorias");
                            String[] categorias = MySQLDB.Instance.categorias; // obtener id de las categorias en la BD

                            try
                            {
                                // Campos no esten vacios y tengan el formato correcto
                                if (!l[0].Equals("")  && l[0].Length <=3 && !l[0].Trim().Equals("")
                                    && !l[1].Equals("") && l[1].Length <= 20 && !l[1].Trim().Equals("")
                                     && l[2].Length <= 90)
                                {
                                    // comprobar que el id de la categoria no exista ya en la BD
                                    int count = 0;
                                    for (int i = 0; i < categorias.Count(); i++)
                                    {
                                        if (l[0].Equals(categorias[i]))
                                        {
                                            count++;
                                        }
                                    }

                                    if (count == 0)// si no existe
                                    {
                                        c = new Categoria(l[0].Trim(), l[1], l[2]); // creamos categoria
                                        arrayCategorias.Add(c); // añadimos a la lista

                                    }
                                    else { error++; break; }
                                }
                                else { error++; break; }
                            }
                            catch (Exception e)
                            {
                                error++;
                                FicheroLog.Instance.EscribirFichero(e.Message);
                                break;
                            }
                        }

                        if (error > 0) // si hay errores se deja de leer el archivo y se muestra el mensaje con la linea del error
                        {
                            mensajeBox = (string)Application.Current.FindResource("errorImportar");
                            string mensajeBox1 = (string)Application.Current.FindResource("errorImportar2");
                            string mensajeBox2 = (string)Application.Current.FindResource("categoriaCSV");
                            MessageBox.Show(mensajeBox + " " + numLinea + "\n\n" + mensajeBox1 + "\n\n" + mensajeBox2
                                , cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else // si no hay errores por cada categoria en la lista la añadimos a la BD
                        {
                            foreach (Categoria c in arrayCategorias)
                            {
                                if (MySQLDB.Instance.InsertarCategoria(c))
                                {
                                    listaCategorias.Add(c);
                                }
                            }
                            mensajeBox = (string)Application.Current.FindResource("importarCSVCorrecto");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                            UIGlobal.PageCategoria.datagrid.Items.Refresh();
                        }
                    }
                    else // si el fichero no corresponde a categorias, debe comenzar con 'Categorias'
                    {
                        mensajeBox = (string)Application.Current.FindResource("importarCSVIncorrecto");
                        string mensajeBox2 = (string)Application.Current.FindResource("Categorias");
                        MessageBox.Show(mensajeBox + " '" + mensajeBox2 + "'", cabeceraInfo,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    //cerrar fichero
                    file.Close();
                }
            }
        }

        #endregion
    }
}
