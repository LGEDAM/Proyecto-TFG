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
    public class ProveedorVM : ObservableCollection<Proveedor>
    {
        private ObservableCollection<Proveedor> listaProveedores;

        public ObservableCollection<Proveedor> ListaProveedores
        {
            get { return listaProveedores; }
        }

        string cabeceraInfo = (string)Application.Current.FindResource("informacion");
        string mensajeBox = null;

        public ProveedorVM()
        {
            //Creo la lista
            listaProveedores = new ObservableCollection<Proveedor>();

            // LLamo a la instancia Singleton y conecto a la DB
            MySQLDB.Instance.ConnectDB("Proveedores");

            // Recorrer el array de proveedores y coger los proveedores y asginarlos a la lista
            if (MySQLDB.Instance.ArrayProveedores != null)
            {
                foreach (Proveedor c in MySQLDB.Instance.ArrayProveedores)
                {
                    listaProveedores.Add(c);
                }
            }
        }

        #region Comandos
        private ICommand btnDeleteProveedor;
        public ICommand BtnDeleteProveedor
        {
            get
            {
                if (btnDeleteProveedor == null)
                {
                    btnDeleteProveedor = new RelayCommand(param => this.borrarProveedor((Proveedor)param));
                }
                return btnDeleteProveedor;
            }
        }

        private ICommand btnAddProveedor;
        public ICommand BtnAddProveedor
        {
            get
            {
                if (btnAddProveedor == null)
                {
                    btnAddProveedor = new RelayCommand(param => this.AccionAdd(), null);
                }
                return btnAddProveedor;
            }
        }


        private ICommand btnEditProveedor;
        public ICommand BtnEditProveedor
        {
            get
            {
                if (btnEditProveedor == null)
                {
                    btnEditProveedor = new RelayCommand(param => this.AccionEdit((Proveedor)param));
                }
                return btnEditProveedor;
            }
        }

        private ICommand exportProveedor;
        public ICommand ExportProveedor
        {
            get
            {
                if (exportProveedor == null)
                {
                    exportProveedor = new RelayCommand(param => this.AccionExportar((DataGrid)param));
                }
                return exportProveedor;
            }
        }
        private ICommand importProveedor;
        public ICommand ImportProveedor
        {
            get
            {
                if (importProveedor == null)
                {
                    importProveedor = new RelayCommand(param => this.AccionImportar(), null);
                }
                return importProveedor;
            }
        }


        #endregion

        #region Acciones
        //Borrar
        public void borrarProveedor(Proveedor p)
        {
            try
            {
                mensajeBox = (string)Application.Current.FindResource("deseaEliminarProveedor");
                if (MessageBox.Show(mensajeBox + "\n" + p.Id + " - " + p.NombreCompania, cabeceraInfo,
                        MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    if ((MySQLDB.Instance.BorrarProveedor(p)))
                    {
                        listaProveedores.Remove(p);

                        mensajeBox = (string)Application.Current.FindResource("proveedorEliminado");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }


            }
            catch (Exception e)
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarProveedor");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }
        }



        private void AccionAdd()
        {
            AddEditProveedor page = new AddEditProveedor(this);
            page.ShowDialog();
        }


        private void AccionEdit(Proveedor p)
        {
            if (p != null)
            {
                AddEditProveedor page = new AddEditProveedor(this, p);
                page.ShowDialog();
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarProveedor");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //Añadir
        public void addProveedor(Proveedor p)
        {
            if (p.Id != null && !p.Id.Trim().Equals("") && p.Id.Length <= 10)
            {
                if (comprobarCampos(p))
                {
                    if (MySQLDB.Instance.InsertarProveedor(p))
                    {
                        listaProveedores.Add(p);

                        UIGlobal.AddEditProveedor.Close();
                        mensajeBox = (string)Application.Current.FindResource("proveedorAñadido");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("proveedorIdNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditProveedor.id.Focus();
            }

        }
        //Editar
        public void editProveedor(Proveedor p)
        {
            if (comprobarCampos(p))
            {
                if (MySQLDB.Instance.ModificarProveedor(p))
                {
                    //Comprobar cual es el piloto de la lista que tenemos que modificar
                    for (int i = 0; i < listaProveedores.Count; i++)
                    {
                        Proveedor prov = listaProveedores[i];
                        if (prov.Id.Equals(p.Id))
                        {
                            listaProveedores[i] = p;
                        }
                    }

                    UIGlobal.AddEditProveedor.Close();
                    mensajeBox = (string)Application.Current.FindResource("proveedorActualizado");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }

        public Boolean comprobarCampos(Proveedor p)
        {
            Boolean exito = false;
            if (p.NombreCompania != null && !p.NombreCompania.Trim().Equals("") && p.NombreCompania.Length <= 20)
            {
                if (p.NombreContacto1 != null && !p.NombreContacto1.Trim().Equals("") && p.NombreContacto1.Length <= 20
                    && p.NombreContacto2 != null && !p.NombreContacto2.Trim().Equals("") && p.NombreContacto2.Length <= 20)
                {
                    if (p.Direccion1 != null && !p.Direccion1.Trim().Equals("") && p.Direccion1.Length <= 40
                    && p.Direccion2 != null && !p.Direccion2.Trim().Equals("") && p.Direccion2.Length <= 40)
                    {

                        if (p.Ciudad != null && !p.Ciudad.Trim().Equals("") && p.Ciudad.Length <= 20)
                        {
                            if (p.Pais != null && !p.Pais.Trim().Equals("") && p.Pais.Length <= 20)
                            {
                                if (p.Tlno !=null && !p.Tlno.Trim().Equals("") && p.Tlno.Length <= 20)
                                {
                                    if (p.Email != null && !p.Email.Trim().Equals("") && p.Email.Length <= 40)
                                    {
                                        exito = true;
                                       
                                    }
                                    else
                                    {
                                        mensajeBox = (string)Application.Current.FindResource("proveedorEmailNoVacio");
                                        cabeceraInfo = (string)Application.Current.FindResource("error");
                                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                                        UIGlobal.AddEditProveedor.pais.Focus();
                                    }
                                }
                                else
                                {
                                    mensajeBox = (string)Application.Current.FindResource("proveedorTlnoNoVacio");
                                    cabeceraInfo = (string)Application.Current.FindResource("error");
                                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                                    UIGlobal.AddEditProveedor.tlfno.Focus();
                                }
                            }
                            else
                            {
                                mensajeBox = (string)Application.Current.FindResource("proveedorPaisNoVacio");
                                cabeceraInfo = (string)Application.Current.FindResource("error");
                                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                                UIGlobal.AddEditProveedor.pais.Focus();
                            }

                        }
                        else
                        {
                            mensajeBox = (string)Application.Current.FindResource("proveedorCiudadNoVacio");
                            cabeceraInfo = (string)Application.Current.FindResource("error");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                            UIGlobal.AddEditProveedor.ciudad.Focus();
                        }
                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("proveedorDirNoVacio");
                        cabeceraInfo = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                        UIGlobal.AddEditProveedor.dir.Focus();
                    }
                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("proveedorContactoNoVacio");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditProveedor.contacto1.Focus();
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("proveedorNombreNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditProveedor.empresa.Focus();
            }
            return exito;
        }

        private void AccionExportar(DataGrid da)
        {
            if (da.Items.Count != 0)
            {

                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "CSV Files| *.csv";
                saveFileDialog.Title = (string)Application.Current.FindResource("guardarFichero");
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {

                    StringBuilder builder = new StringBuilder();
                    string proveedores = (string)Application.Current.FindResource("Proveedores");
                    builder.AppendLine(proveedores);
                    foreach (Proveedor p in ListaProveedores)
                    {
                        string registro = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", p.Id,
                            p.NombreCompania, p.NombreContacto1, p.NombreContacto2, p.Direccion1, p.Direccion2,
                            p.Ciudad, p.Pais, p.Tlno, p.Email);
                        builder.AppendLine(registro);
                    }
                    // Cerrar el fichero
                    File.WriteAllText(saveFileDialog.FileName, builder.ToString());


                    mensajeBox = (string)Application.Current.FindResource("exportarExcel");
                    if (MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        ExportarExcel(da);
                    }
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("noInfoCSV");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportarExcel(DataGrid da)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet = (Worksheet)workbook.Sheets[1];

                for (int j = 0; j < da.Columns.Count; j++)
                {
                    sheet.Range["A1"].Offset[0, j].Value = da.Columns[j].Header;
                    sheet.Cells[1, j + 1].Font.Bold = true;
                }

                var row = 1;
                foreach (Proveedor p in ListaProveedores)
                {
                    row++;
                    sheet.Cells[row, "A"] = p.Id;
                    sheet.Cells[row, "B"] = p.NombreCompania;
                    sheet.Cells[row, "C"] = p.NombreContacto1;
                    sheet.Cells[row, "D"] = p.NombreContacto2;
                    sheet.Cells[row, "E"] = p.Direccion1;
                    sheet.Cells[row, "F"] = p.Direccion2;
                    sheet.Cells[row, "G"] = p.Ciudad;
                    sheet.Cells[row, "H"] = p.Pais;
                    sheet.Cells[row, "I"] = p.Tlno;
                    sheet.Cells[row, "J"] = p.Email;
                }
                sheet.Columns.AutoFit();
            }
            catch (Exception e)
            {
                mensajeBox = (string)Application.Current.FindResource("microsoftExcel");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }
        }


        private void AccionImportar()
        {
            System.Windows.Forms.OpenFileDialog dialogoBuscarArchivo = new System.Windows.Forms.OpenFileDialog();

            dialogoBuscarArchivo.Filter = "CSV Files|*.csv";
            dialogoBuscarArchivo.Title = (string)Application.Current.FindResource("abrirFicheroCSV");
            dialogoBuscarArchivo.FilterIndex = 1;
            dialogoBuscarArchivo.Multiselect = false;
            if (dialogoBuscarArchivo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Stream stream = dialogoBuscarArchivo.OpenFile())
                {
                    string ruta = dialogoBuscarArchivo.FileName;

                    StreamReader file = new StreamReader(ruta);
                    string line;

                    string m = (string)Application.Current.FindResource("Proveedores");
                    if (file.ReadLine().StartsWith(m))
                    {
                        int error = 0;
                        int numLinea = 0;
                        List<Proveedor> arrayProveedores = new List<Proveedor>();

                        while ((line = file.ReadLine()) != null)
                        {
                            String[] l = line.Split(';');
                            Proveedor p;
                            numLinea++;
                            MySQLDB.Instance.ConnectDB("IdProveedores");
                            String[] proveedores = MySQLDB.Instance.proveedores;
                            try
                            {
                                if (!l[0].Equals("") && !l[1].Equals("") && !l[2].Equals("") && !l[3].Equals("")
                                    && !l[5].Equals("") && !l[6].Equals("") && !l[7].Equals("") && !l[8].Equals(""))
                                {
                                    int count = 0;
                                    for (int i = 0; i < proveedores.Count(); i++)
                                    {
                                        if (l[0].Equals(proveedores[i]))
                                        {
                                            count++;
                                        }
                                    }

                                    if (count == 0)
                                    {
                                        p = new Proveedor(l[0], l[1], l[2], l[3], l[4], l[5], l[6], l[7], l[8], l[9]);
                                        arrayProveedores.Add(p);

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

                        if (error > 0)
                        {
                            mensajeBox = (string)Application.Current.FindResource("errorImportar");
                            string mensajeBox1 = (string)Application.Current.FindResource("errorImportar2");
                            string mensajeBox2 = (string)Application.Current.FindResource("proveedorCSV");
                            MessageBox.Show(mensajeBox + " " + numLinea + "\n\n" + mensajeBox1 + "\n\n" + mensajeBox2
                                , cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            foreach (Proveedor p in arrayProveedores)
                            {
                                if (MySQLDB.Instance.InsertarProveedor(p))
                                {
                                    listaProveedores.Add(p);
                                }
                            }
                            mensajeBox = (string)Application.Current.FindResource("importarCSVCorrecto");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                            UIGlobal.PageProveedor.datagrid.Items.Refresh();
                        }
                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("importarCSVIncorrecto");
                        string mensajeBox2 = (string)Application.Current.FindResource("Categorias");
                        MessageBox.Show(mensajeBox + " '" + mensajeBox2 + "'", cabeceraInfo,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    file.Close();
                }
            }
        }

        #endregion
    }
}
