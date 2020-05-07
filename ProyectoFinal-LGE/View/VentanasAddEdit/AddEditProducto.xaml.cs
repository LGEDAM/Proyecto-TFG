using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoFinal_LGE.View.VentanasAddEdit
{
    /// <summary>
    /// Lógica de interacción para AddEditAlmacen.xaml
    /// </summary>
    public partial class AddEditProducto : Window
    {

        private ProductoVM productoVM;
        private Producto producto;

        private Boolean add = true; // true añadir producto, false modificar producto

        /// <summary>
        /// Almacen antes de modificar el producto
        /// </summary>
        string almacenViejo = null;
        /// <summary>
        /// Almacen despues de modificar el producto
        /// </summary>
        string almacenNuevo = null;

        /// <summary>
        /// Constructor para añadir un producto
        /// </summary>
        /// <param name="pVM">Vista modelo del producto</param>
        public AddEditProducto(ProductoVM pVM)
        {
            UIGlobal.AddEditProducto = this;
            InitializeComponent();
            productoVM = pVM;
            producto = new Producto();
            DataContext = producto;

            add = true;
        }

        /// <summary>
        ///  Constructor para modificar un producto
        /// </summary>
        /// <param name="pVM">Vista modelo del producto</param>
        /// <param name="p">Producto a modificar</param>
        public AddEditProducto(ProductoVM pVM, Producto p)
        {
            UIGlobal.AddEditProducto = this;
            InitializeComponent();

            id.IsEnabled = false; // El codigo del producto no se puede cambiar
            productoVM = pVM;

            producto = (Producto)p.Clone();
            DataContext = producto;

            if (producto.Imagen != null) // si el producto tiene una imagen se escribe en el texto box que hay imagen
            {
                imagen.Text = (string)Application.Current.FindResource("TieneImagen");

            }
            else  // si el producto no tiene imagen se escribe en el texto box que no hay imagen
            {
                imagen.Text = (string)Application.Current.FindResource("NoTieneImagen");
            }

            almacenViejo = p.Almacen.ToString(); // guardamos el valor del almacen actual
            add = false;

        }


        /// <summary>
        /// Método para poder mover la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        /// <summary>
        /// Método del boton Aceptar que añadira o modificara un producto dependiendo de la opcion escogida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarClick(object sender, RoutedEventArgs e)
        {
            if (add) // añadir producto
            {
                productoVM.addProducto(producto);
            }
            else // modificar producto
            {
                if (productoVM.editProducto(producto)) // si se modifica el producto de añade una nueva transaccion
                {
                    almacenNuevo = producto.Almacen.ToString(); // guardamos el valor del almacen despues de modificar
                    int stock = producto.Unidades;
                    if (!almacenNuevo.Equals(almacenViejo)) // para añadir la transaccion debe cambiar el almacen
                    {
                        DateTime date = DateTime.Now;
                        Transaccion t = new Transaccion(date, producto.Codigo.ToString(), almacenViejo, almacenNuevo, stock);
                        MySQLDB.Instance.InsertarTransaccion(t);  // Añadir transaccion
                    }

                }
            }
        }

        /// <summary>
        /// Método que pregunta si deseea cerrar la ventana al pulsar el boton cancelar o X(cerrar)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelarClick(object sender, RoutedEventArgs e)
        {
            string mensaje = (string)Application.Current.FindResource("deseaSalir");
            string cabecera = (string)Application.Current.FindResource("informacion");
            if (MessageBox.Show(mensaje, cabecera,
                         MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                Close();
            }
        }


        /// <summary>
        /// Método del boton subir imagen del producto que abre el explorador de archivos para obtener una imagen jpg o png.
        /// Si la imagen ocupa menos de 1 MB transforma la imagen del tipo System.Drawing a byte[] (soportado en WPF) y asigna la imagen 
        /// al producto mostrando la ruta en la interfaz; si la imagen es mayor de 1MB muestra un mensaje indicandolo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubirImagen(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image image = null;

            // Abrir explorador de archivos
            System.Windows.Forms.OpenFileDialog dialogoBuscarArchivo = new System.Windows.Forms.OpenFileDialog();

            BitmapDecoder bitdecoder;
            // Filtar imagen por jpg o png
            dialogoBuscarArchivo.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";
            dialogoBuscarArchivo.FilterIndex = 1;
            dialogoBuscarArchivo.Title = (string)Application.Current.FindResource("SubirImagen");
            dialogoBuscarArchivo.Multiselect = false;

            if (dialogoBuscarArchivo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Tamaño de la imagen no puede ser mayor de 1MB
                var size = new FileInfo(dialogoBuscarArchivo.FileName).Length;
                if (size < 1023000)
                {
                    // Transformar la imagen a System.Drawing
                    using (Stream stream = dialogoBuscarArchivo.OpenFile())
                    {
                        string ruta = dialogoBuscarArchivo.FileName;
                        imagen.Text = ruta; //nombre ruta en el textbox

                        image = System.Drawing.Image.FromFile(ruta);

                        bitdecoder = BitmapDecoder.Create(stream,
                            BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    }
                }
                else // imagen mayor a 1MB
                {
                    string mensaje = (string)Application.Current.FindResource("imagenMax");
                    string cabecera = (string)Application.Current.FindResource("informacion");
                    MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Si se elige una imagen
                if (image != null)
                {
                    // Transformar la imagen a byte[] y asignarla al producto
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, image.RawFormat);
                        producto.Imagen = ms.ToArray();
                    }
                }
            }

        }

        /// <summary>
        /// Metodo que elimina la imagen del producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EliminarImagen(object sender, RoutedEventArgs e)
        {
            producto.Imagen = null;
            imagen.Text = (string)Application.Current.FindResource("NoTieneImagen");
            UIGlobal.PageProducto.datagrid.Items.Refresh();
        }


    }
}
