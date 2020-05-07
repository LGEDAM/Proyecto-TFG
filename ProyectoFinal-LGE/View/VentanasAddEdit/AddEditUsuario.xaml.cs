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
    public partial class AddEditUsuario : Window
    {
        /// <summary>
        /// Valor de la contraseña de usuario
        /// </summary>
        public string contraseña;

        private UsuarioVM usuarioVM;
        private Usuario usuario;
        private Boolean add = true; // true añadir usuario, false modificar usuario

        /// <summary>
        /// Constructor para añadir un usuario
        /// </summary>
        /// <param name="uVM">Vista modelo del usuario</param>
        public AddEditUsuario(UsuarioVM uVM)
        {

            UIGlobal.AddEditUsuario = this;
            InitializeComponent();
            usuarioVM = uVM;
            usuario = new Usuario();
            DataContext = usuario;

            add = true;
        }

        /// <summary>
        /// Constructor para modificar un usuario
        /// </summary>
        /// <param name="uVM">Vista modelo del usuario</param>
        /// <param name="u">Usuario a modificar</param>
        public AddEditUsuario(UsuarioVM uVM, Usuario u)
        {

            UIGlobal.AddEditUsuario = this;
            InitializeComponent();

            user.IsEnabled = false; // El nombre de usuario no se puede cambiar
            pwd.Password = u.Pwd; // Le pasamos el valor de la contraseña que no puede hacerse mediante binding
            contraseña = u.Pwd;
            if (u.User.Equals("admin")) // El usuario "admin" no puede modificar sus permisos
            {
                gridPermisos.IsEnabled = false;
            }
            usuarioVM = uVM;
            usuario = (Usuario)u.Clone();
            DataContext = usuario;

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
        /// Método del boton Aceptar que añadira o modificara un usuario dependiendo de la opcion escogida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarClick(object sender, RoutedEventArgs e)
        {
            if (add) // Añadir usuario
            {
                try
                {
                    usuario.Pwd = pwd.Password; // Pasamos el valor de la contraseña que no se puede hacer binding
                    usuarioVM.AddUsuario(usuario);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(ex.Message);
                }
            }
            else // Modificar usuario
            {
                usuario.Pwd = pwd.Password; // Pasamos el valor de la contraseña que no se puede hacer binding
                usuarioVM.EditUsuario(usuario);
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
        /// Método del boton subir imagen para el perfil de usuario que abre el explorador de archivos para obtener una imagen jpg o png.
        /// Si la imagen ocupa menos de 1 MB transforma la imagen del tipo System.Drawing a byte[] (soportado en WPF) y asigna la imagen 
        /// al usuario mostrandola en la interfaz; si la imagen es mayor de 1MB muestra un mensaje indicandolo.
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
            }
            // Si se elige imagen
            if (image != null)
            {
                // Transformar la imagen a byte[] y asignarla al usuario
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    usuario.Imagen = ms.ToArray();
                }

                // Trnasformar la imagen para mostrarla en la interfaz
                byte[] img = usuario.Imagen;
                var imagen = new BitmapImage();
                using (var mem = new MemoryStream(img))
                {
                    mem.Position = 0;
                    imagen.BeginInit();
                    imagen.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    imagen.CacheOption = BitmapCacheOption.OnLoad;
                    imagen.UriSource = null;
                    imagen.StreamSource = mem;
                    imagen.EndInit();
                }
                usuarioImagen.Source = imagen;

            }
        }

        /// <summary>
        /// Metodo que elimina la imagen de perfil de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EliminarImagen(object sender, RoutedEventArgs e)
        {

            usuario.Imagen = null;
            usuarioImagen.Source = null;

        }

    }
}
