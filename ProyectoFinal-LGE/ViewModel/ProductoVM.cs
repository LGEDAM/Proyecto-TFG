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
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using ProyectoFinal_LGE.View.VentanasAddEdit;
using Application = System.Windows.Application;

namespace ProyectoFinal_LGE.ViewModel
{
    public class ProductoVM : ObservableCollection<Producto>
    {

        private ObservableCollection<Producto> listaProductos;

        public ObservableCollection<Producto> ListaProductos
        {
            get { return listaProductos; }
        }

        string cabeceraInfo = (string)Application.Current.FindResource("informacion");
        string mensajeBox = null;

        private Producto producto;

        public Producto Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        public ProductoVM()
        {
            //Creo la lista de productos
            listaProductos = new ObservableCollection<Producto>();

            // LLamo a la instancia Singleton y conecto a la DB
            MySQLDB.Instance.ConnectDB("Productos");

            // Recorrer el array de productos y coger los productos y asginarlos a la lista
            if (MySQLDB.Instance.ArrayProductos != null)
            {
                foreach (Producto c in MySQLDB.Instance.ArrayProductos)
                {
                    listaProductos.Add(c);
                }
            }

            producto = new Producto();
        }


        #region Comandos
        private ICommand btnDeleteProducto;
        public ICommand BtnDeleteProducto
        {
            get
            {
                if (btnDeleteProducto == null)
                {
                    btnDeleteProducto = new RelayCommand(param => this.borrarProducto((Producto)param));
                }
                return btnDeleteProducto;
            }
        }
        private ICommand btnVerImagen;
        public ICommand BtnVerImagen
        {
            get
            {
                if (btnVerImagen == null)
                {
                    btnVerImagen = new RelayCommand(param => this.verImagen((Producto)param));
                }
                return btnVerImagen;
            }
        }



        private ICommand btnAddProducto;
        public ICommand BtnAddProducto
        {
            get
            {
                if (btnAddProducto == null)
                {
                    btnAddProducto = new RelayCommand(param => this.AccionAdd(), null);
                }
                return btnAddProducto;
            }
        }


        private ICommand btnEditProducto;
        public ICommand BtnEditProducto
        {
            get
            {
                if (btnEditProducto == null)
                {
                    btnEditProducto = new RelayCommand(param => this.AccionEdit((Producto)param));
                }
                return btnEditProducto;
            }
        }


        private ICommand exportProducto;
        public ICommand ExportProducto
        {
            get
            {
                if (exportProducto == null)
                {
                    exportProducto = new RelayCommand(param => this.AccionExportar((DataGrid)param));
                }
                return exportProducto;
            }
        }


        private ICommand importProducto;
        public ICommand ImportProducto
        {
            get
            {
                if (importProducto == null)
                {
                    importProducto = new RelayCommand(param => this.AccionImportar(), null);
                }
                return importProducto;
            }
        }

        #endregion


        #region Acciones
        //Borrar
        public void borrarProducto(Producto p)
        {
            try
            {
                mensajeBox = (string)Application.Current.FindResource("deseaEliminarProducto");
                if (MessageBox.Show(mensajeBox + "\n" + p.Codigo + " - " + p.Nombre, cabeceraInfo,
                        MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    if ((MySQLDB.Instance.BorrarProducto(p)))
                    {
                        listaProductos.Remove(p);

                        mensajeBox = (string)Application.Current.FindResource("productoEliminado");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception e)
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarProducto");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }
        }

        private void verImagen(Producto p)
        {
            if (p.Imagen != null) //Si tiene imagen
            {

                System.Drawing.Image img;
                //Tranformar imagen
                using (MemoryStream memstr = new MemoryStream(p.Imagen))
                {
                    img = System.Drawing.Image.FromStream(memstr);
                }

                //Mostrar imagen
                System.Windows.Forms.Form form = new System.Windows.Forms.Form();
                form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                form.Width = img.Width;
                form.Height = img.Width;
                form.MaximumSize = img.Size;
                form.MinimumSize = img.Size;
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                
                System.Windows.Forms.PictureBox pb = new System.Windows.Forms.PictureBox
                {
                    Image = img,
                    Width = img.Width,
                    Height = img.Height,
                    SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
                };
                form.Controls.Add(pb);
                form.ShowDialog();
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("verImagenProducto");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        private void AccionAdd()
        {
            AddEditProducto page = new AddEditProducto(this);
            page.ShowDialog();
        }

        private void AccionEdit(Producto p)
        {
            if (p != null)
            {
                AddEditProducto page = new AddEditProducto(this, p);
                page.ShowDialog();
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarProducto");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //Añadir
        public void addProducto(Producto p)
        {
            if (p.Codigo != null && !p.Codigo.Trim().Equals("") && p.Codigo.Length <= 10)
            {
                if (comprobarCampos(p))
                {
                    if (MySQLDB.Instance.InsertarProducto(p))
                    {
                        listaProductos.Add(p);

                        UIGlobal.AddEditProducto.Close();
                        mensajeBox = (string)Application.Current.FindResource("productoAñadido");
                        cabeceraInfo = (string)Application.Current.FindResource("informacion");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                        //DateTime d = DateTime.Now;
                        //string pr = p.Codigo;
                        //string prov = p.Proveedor;
                        //string a = p.Almacen;
                        //int s = p.Unidades;
                        //double t = p.Unidades * p.PrecioCompra;
                        //Recepcion r = new Recepcion(d, pr, prov, a, s, t);

                        //MySQLDB.Instance.insertarRecepcion(r);

                    }
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("productoIdNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditProducto.id.Focus();
            }


        }

        public Boolean comprobarCampos(Producto p)
        {
            Boolean exito = false;
            if (p.Almacen != null)
            {
                if (p.Nombre != null && !p.Nombre.Trim().Equals("") && p.Nombre.Length <= 90)
                {
                    if (p.Descripcion != null && !p.Descripcion.Trim().Equals("") && p.Descripcion.Length <= 90)
                    {
                        if (p.Categoria != null)
                        {
                            if (p.Proveedor != null )
                            {
                                // Hay imagen
                                String imagen = UIGlobal.AddEditProducto.imagen.Text;
                                string imagenSi= (string)Application.Current.FindResource("TieneImagen");
                                string imagenNo = (string)Application.Current.FindResource("NoTieneImagen");

                                if (imagen != null && !imagen.Trim().Equals("") && !imagen.Equals(imagenSi)
                                    && !imagen.Equals(imagenNo))
                                {
                                    if ((imagen.Trim().EndsWith(".png") || imagen.Trim().EndsWith(".jpg")
                                        && File.Exists(imagen.Trim())))

                                    {
                                        exito = true;

                                    }
                                    else
                                    {
                                        mensajeBox = (string)Application.Current.FindResource("productoImagen");
                                        if (MessageBox.Show(mensajeBox,  cabeceraInfo,
                                                MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                                        {
                                            exito = true;
                                        }
                                    }
                                }
                                else
                                {
                                    exito = true;
                                }

                            }
                            else
                            {
                                mensajeBox = (string)Application.Current.FindResource("productoProvNoVacio");
                                cabeceraInfo = (string)Application.Current.FindResource("error");
                                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                                UIGlobal.AddEditProducto.comboProveedor.Focus();
                            }

                        }
                        else
                        {
                            mensajeBox = (string)Application.Current.FindResource("productoCatNoVacio");
                            cabeceraInfo = (string)Application.Current.FindResource("error");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                            UIGlobal.AddEditProducto.Categoria.Focus();
                        }

                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("productoDescNoVacio");
                        cabeceraInfo = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                        UIGlobal.AddEditProducto.desc.Focus();
                    }

                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("productoNombreNoVacio");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditProducto.nombre.Focus();
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("productoAlmacenNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditProducto.comboAlmacen.Focus();
            }
            return exito;
        }
        //Editar
        public Boolean editProducto(Producto p)
        {
            bool exito = false;

            if (comprobarCampos(p))
            {
                if (MySQLDB.Instance.ModificarProducto(p))
                {
                    //Comprobar cual es el piloto de la lista que tenemos que modificar
                    for (int i = 0; i < listaProductos.Count; i++)
                    {
                        Producto prod = listaProductos[i];
                        if (prod.Codigo.Equals(p.Codigo))
                        {
                            listaProductos[i] = p;
                        }


                        UIGlobal.PageProducto.datagrid.Items.Refresh();
                        exito = true;
                    }
                    UIGlobal.AddEditProducto.Close();
                    mensajeBox = (string)Application.Current.FindResource("productoActualizado");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
                    string productos = (string)Application.Current.FindResource("Productos");
                    builder.AppendLine(productos);
                    foreach (Producto p in ListaProductos)
                    {
                        string registro = string.Format("{0};{1};;{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}", p.Codigo,
                            p.Nombre, p.Imagen, p.Descripcion, p.Categoria, p.Almacen, p.Unidades, p.Ancho, p.Largo, p.Alto, p.Proveedor,
                            p.PrecioCompra, p.PrecioVenta, p.Disponible);
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
                sheet.Cells[1, 3] = (string)Application.Current.FindResource("Descripcion");

                var row = 1;
                foreach (Producto p in ListaProductos)
                {
                    row++;
                    sheet.Cells[row, "A"] = p.Codigo;
                    sheet.Cells[row, "B"] = p.Nombre;
                    sheet.Cells[row, "C"] = p.Descripcion;
                    sheet.Cells[row, "D"] = p.Categoria;
                    sheet.Cells[row, "E"] = p.Almacen;
                    sheet.Cells[row, "F"] = p.Unidades;
                    sheet.Cells[row, "G"] = p.Ancho;
                    sheet.Cells[row, "H"] = p.Largo;
                    sheet.Cells[row, "I"] = p.Alto;
                    sheet.Cells[row, "J"] = p.Proveedor;
                    sheet.Cells[row, "K"] = p.PrecioCompra;
                    sheet.Cells[row, "L"] = p.PrecioVenta;
                    sheet.Cells[row, "M"] = p.Disponible;
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
                    string mensajeBoxImagen = "";

                    string m = (string)Application.Current.FindResource("Productos");
                    if (file.ReadLine().StartsWith(m))
                    {
                        int error = 0;
                        int numLinea = 0;
                        List<Producto> arrayProductos = new List<Producto>();

                        while ((line = file.ReadLine()) != null)
                        {
                            String[] l = line.Split(';');
                            Producto p;
                            numLinea++;

                            MySQLDB.Instance.ConnectDB("IdProductos");
                            String[] productos = MySQLDB.Instance.productos;
                            MySQLDB.Instance.ConnectDB("IdAlmacenes");
                            String[] almacenes = MySQLDB.Instance.almacenes;
                            MySQLDB.Instance.ConnectDB("IdProveedor");
                            String[] proveedores = MySQLDB.Instance.proveedores;

                            try
                            {
                                //Valores NOT NULL
                                if (!l[0].Equals("") && !l[1].Equals("") && !l[3].Equals("")
                                     && !l[4].Equals("") && !l[5].Equals("") && !l[9].Equals("") && !l[10].Equals("")
                                     && !l[11].Equals("") && !l[12].Equals("") && !l[13].Equals(""))
                                {

                                    //PRIMARY KEY no repetida
                                    int countId = 0;
                                    for (int i = 0; i < productos.Count(); i++)
                                    {
                                        if (l[0].Equals(productos[i]))
                                        {
                                            countId++;
                                        }
                                    }
                                    //Exista el almacen
                                    int countAlmacen = 0;
                                    for (int i = 0; i < almacenes.Count(); i++)
                                    {
                                        if (l[5].Equals(almacenes[i]))
                                        {
                                            countAlmacen++;
                                        }
                                    }
                                    //Exista el proveedor
                                    int countProveedor = 0;
                                    for (int i = 0; i < proveedores.Count(); i++)
                                    {
                                        if (l[10].Equals(proveedores[i]))
                                        {
                                            countProveedor++;
                                        }
                                    }

                                    if (countId == 0 && countAlmacen == 1 && countProveedor == 1)
                                    {
                                        bool disponible = false;
                                        if (l[13].Equals("True"))
                                        {
                                            disponible = true;
                                        }
                                        else if (l[13].Equals("Falso"))
                                        {
                                            disponible = false;
                                        }

                                        //hay imagen y la ruta es correcta
                                        if ((l[2].Trim().EndsWith(".png") || l[2].Trim().EndsWith(".jpg"))
                                            && File.Exists(l[2].Trim()))
                                        {
                                            System.Drawing.Image img = System.Drawing.Image.FromFile(l[2].Trim());
                                            //Asignar la imagen al reactivo
                                            using (var ms2 = new MemoryStream())
                                            {
                                                img.Save(ms2, img.RawFormat);
                                                p = new Producto(l[0], l[1], ms2.ToArray(), l[3], l[4], l[5], Int32.Parse(l[6]), Int32.Parse(l[7]),
                                             Int32.Parse(l[8]), Int32.Parse(l[9]), l[10], Double.Parse(l[11]), Double.Parse(l[12]),
                                             disponible);
                                            }


                                        }
                                        else //no imagen
                                        {
                                            p = new Producto(l[0], l[1], null, l[3], l[4], l[5], Int32.Parse(l[6]), Int32.Parse(l[7]),
                                            Int32.Parse(l[8]), Int32.Parse(l[9]), l[10], Double.Parse(l[11]), Double.Parse(l[12]),
                                            disponible);
                                            mensajeBoxImagen = (string)Application.Current.FindResource("productoCSVCorrecto");
                                        }
                                        arrayProductos.Add(p);

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
                            string mensajeBox2 = (string)Application.Current.FindResource("productoCSV");
                            MessageBox.Show(mensajeBox + " " + numLinea + "\n\n" + mensajeBox1 + "\n\n" + mensajeBox2
                                , cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            foreach (Producto p in arrayProductos)
                            {

                                if (MySQLDB.Instance.InsertarProducto(p))
                                {
                                    listaProductos.Add(p);
                                }
                            }
                            mensajeBox = (string)Application.Current.FindResource("importarCSVCorrecto");

                            MessageBox.Show(mensajeBox + mensajeBoxImagen, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                            UIGlobal.PageProducto.datagrid.Items.Refresh();
                        }
                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("importarCSVIncorrecto");
                        string mensajeBox2 = (string)Application.Current.FindResource("Productos");
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
