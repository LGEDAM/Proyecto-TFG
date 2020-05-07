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
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ProyectoFinal_LGE.ViewModel
{
    /// <summary>
    /// Vista modelo para usuarios
    /// </summary>
    public class UsuarioVM : ObservableCollection<Usuario>
    {

        private ObservableCollection<Usuario> listaUsuarios;
        /// <summary>
        /// Lista de usuarios.
        /// </summary>
        public ObservableCollection<Usuario> ListaUsuarios
        {
            get { return listaUsuarios; }
        }

        // Titulo de la cabecera del MessageBox
        string cabeceraInfo = (string)Application.Current.FindResource("informacion");
        // Mensaje del MessageBox
        string mensajeBox = null;

        /// <summary>
        /// Constructor de UsuarioVM.
        /// </summary>
        public UsuarioVM()
        {
            //Creo la lista de usuarios
            listaUsuarios = new ObservableCollection<Usuario>();

            // LLamo a la instancia  y la conecto a la DB
            MySQLDB.Instance.ConnectDB("Usuarios");

            // Recorrer el array de usuarios y coger los usuarios y asginarlos a la lista
            if (MySQLDB.Instance.ArrayUsuarios != null)
            {
                foreach (Usuario u in MySQLDB.Instance.ArrayUsuarios)
                {
                    listaUsuarios.Add(u);
                }
            }
        }

        #region Comandos
        private ICommand btnDeleteUsuario;
        /// <summary>
        /// Comando del botón eliminar usuario. Llama al método BorrarUsuario(Usuario)
        /// </summary>
        public ICommand BtnDeleteUsuario
        {
            get
            {
                if (btnDeleteUsuario == null)
                {
                    btnDeleteUsuario = new RelayCommand(param => this.BorrarUsuario((Usuario)param));
                }
                return btnDeleteUsuario;
            }
        }

        private ICommand btnAddUsuario;
        /// <summary>
        ///  Comando del botón añadir usuario. Llama al método AccionAdd().
        /// </summary>
        public ICommand BtnAddUsuario
        {
            get
            {
                if (btnAddUsuario == null)
                {
                    btnAddUsuario = new RelayCommand(param => this.AccionAdd(), null);
                }
                return btnAddUsuario;
            }
        }

        private ICommand btnEditUsuario;
        /// <summary>
        /// Comando del botón editar usuario. Llama al método AccionEdit(Usuario).
        /// </summary>
        public ICommand BtnEditUsuario
        {
            get
            {
                if (btnEditUsuario == null)
                {
                    btnEditUsuario = new RelayCommand(param => this.AccionEdit((Usuario)param));
                }
                return btnEditUsuario;
            }
        }

        private ICommand cambiarPwd;
        /// <summary>
        /// Comando del botón cambiar contraseña. Llama al método CambiarContraseña().
        /// </summary>
        public ICommand CambiarPwd
        {
            get
            {
                if (cambiarPwd == null)
                {
                    cambiarPwd = new RelayCommand(param => this.CambiarContraseña(), null);
                }
                return cambiarPwd;
            }
        }

        #endregion

        #region Acciones
        /// <summary>
        ///  Método que elimina el usuario pasado como parámetro.
        /// </summary>
        /// <param name="u">Usuario a eliminar</param>
        private void BorrarUsuario(Usuario u)
        {
            if (u != null) // debe seleccionarse un usuario
            {
                if (!u.User.Equals(Usuario.InstanciaUser.User)) // no se puede eliminar el usuario logeado
                {
                    if (!u.User.Equals("admin")) // no se puede eliminar el usuario admin
                    {
                        // preguntar si desea eliminar el usuario
                        mensajeBox = (string)Application.Current.FindResource("deseaEliminarUsuario");
                        if (MessageBox.Show(mensajeBox + "\n" + u.User, cabeceraInfo,
                                MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            //eliminar usuario de la BD
                            if ((MySQLDB.Instance.BorrarUsuario(u)))
                            {
                                listaUsuarios.Remove(u);

                                mensajeBox = (string)Application.Current.FindResource("usuarioEliminado");
                                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("usuarioAdmin");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("usuarioLogeado");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarUsuario");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Metodo que abre la ventana de añadir usuario (AddEditUsuario).
        /// </summary>
        private void AccionAdd()
        {
            AddEditUsuario page = new AddEditUsuario(this);
            page.ShowDialog();
        }

        /// <summary>
        /// Método que abre la ventana de modificar usuario (AddEditUsuario).
        /// </summary>
        /// <param name="u">Usuario a modificar</param>
        private void AccionEdit(Usuario u)
        {
            if (u != null) // debe estar seleccionado un usuario
            {
                AddEditUsuario page = new AddEditUsuario(this, u);
                page.ShowDialog();
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarUsuario");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Método que añade el usuario pasado como parámetro a la base de datos comprobando que ciertos campos
        /// no esten vacios y tengan el formato correcto.
        /// </summary>
        /// <param name="u">Usuario a añadir</param>
        public void AddUsuario(Usuario u)
        {
            String[] user = MySQLDB.Instance.ObtenerUsername(); //obtener nombres de usuarios de la BD

            // nombre de usuario no este vacio y ocupe mas de 40 caracteres
            if (u.User != null && !u.User.Trim().Equals("") && u.User.Length <= 40)
            {
                // contraseña de usuario no este vacia y este entre 5 y 15 caracteres
                if (u.Pwd != null && !u.Pwd.Trim().Equals("") && u.Pwd.Length <= 15 && u.Pwd.Length >= 5)
                {
                    if (!user.Contains(u.User)) // el nombre de usuario no este repetido
                    {
                        if (MySQLDB.Instance.InsertarUsuario(u)) // añadir usuario
                        {
                            listaUsuarios.Add(u);
                            // cerrar ventana
                            UIGlobal.AddEditUsuario.Close();
                            mensajeBox = (string)Application.Current.FindResource("usuarioAñadido");
                            cabeceraInfo = (string)Application.Current.FindResource("informacion");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("usuarioYaExiste");
                        cabeceraInfo = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                        UIGlobal.AddEditUsuario.user.Focus();
                    }

                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("usuarioPwdNoVacio");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditUsuario.pwd.Focus();
                }

            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("usuarioNombreNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditUsuario.user.Focus();
            }
        }


        /// <summary>
        /// Método que modifica el usuario pasado como parámetro en la base de datos comprobando que ciertos campos
        /// no esten vacios y tengan el formato correcto.
        /// </summary>
        /// <param name="u">Usuario a modificar</param>
        public void EditUsuario(Usuario u)
        {
            // contraseña de usuario no este vacia y este entre 5 y 15 caracteres
            if (UIGlobal.AddEditUsuario.pwd.Password != null && !UIGlobal.AddEditUsuario.pwd.Password.Trim().Equals("")
                && UIGlobal.AddEditUsuario.pwd.Password.Length <= 15 && UIGlobal.AddEditUsuario.pwd.Password.Length >= 5)
            {
                // La contraseña permanece igual
                if (UIGlobal.AddEditUsuario.contraseña.Equals(u.Pwd))
                {
                    // preguntar si quiere guardar sin cambiar la contraseña
                    string mensaje = (string)Application.Current.FindResource("usuarioPwdIgual");
                    string cabecera = (string)Application.Current.FindResource("informacion");
                    if (MessageBox.Show(mensaje, cabecera,
                                 MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        ActualizarUsuario(u); // actualizar usuario
                    }

                }
                else
                {
                    ActualizarUsuario(u); // actualizar usuario
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("usuarioPwdNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditUsuario.pwd.Focus();
            }

        }

        /// <summary>
        /// Método que modifica el usuario pasado como parámetro en la base de datos y actualiza su imagen de perfil.
        /// </summary>
        /// <param name="u">Usuario a modificar</param>
        public void ActualizarUsuario(Usuario u)
        {
            // modificar usuario en la base de datos
            if (MySQLDB.Instance.ModificarUsuario(u))
            {
                //Comprobar cual es el piloto de la lista que tenemos que modificar
                for (int i = 0; i < listaUsuarios.Count; i++)
                {
                    Usuario user = listaUsuarios[i];
                    if (user.User.Equals(u.User))
                    {
                        listaUsuarios[i] = u;
                    }
                }
                // cambiar imagen de perfil de usuario
                CambiarImagenPerfil(u);
                // cerrar ventana
                UIGlobal.AddEditUsuario.Close();
                mensajeBox = (string)Application.Current.FindResource("usuarioActualizado");
                cabeceraInfo = (string)Application.Current.FindResource("informacion");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Método que cambiar la imagen de perfil del usuario pasado como parámetro, si no tiene ninguna imagen
        /// pondrá la imagen por defecto.
        /// </summary>
        /// <param name="u">Usuario a modificar su imagen de usuario</param>
        public void CambiarImagenPerfil(Usuario u)
        {
            byte[] img = u.Imagen;
            if (img == null) // usuario tiene imagen
            {
                // imagen por defecto
                UIGlobal.MainWindow.imagenUsuario.Source = new BitmapImage(new Uri("/Media/avatarUsuario.png", UriKind.Relative));
            }
            else //usuario no tiene imagen
            {
                // transformar la imagen 
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
                // mostrar imagen
                UIGlobal.MainWindow.imagenUsuario.Source = imagen;
            }
        }

        /// <summary>
        /// Método que cambia la contraseña del usuario logeado comprobando que los campos de las contraseñas no
        /// esten vacios, la contraseña actual sea correcta, la contraseña nueva tenga entre 5 y 15 caracteres y que
        /// la nueva constraseña no sea igual a la actual.
        /// </summary>
        public void CambiarContraseña()
        {
            string pwdActual = UIGlobal.CambiarContraseña.pwdActual.Password; // contraseña actual
            string pwdNueva = UIGlobal.CambiarContraseña.pwdNueva.Password; // contraseña nueva

            if (pwdActual.Equals(Usuario.InstanciaUser.Pwd)) // contraseña actual correcta
            {
                if (!pwdNueva.Trim().Equals("")) // contraseña nueva no este vacia
                {
                    if (pwdNueva.Length <= 15 && pwdNueva.Length >= 5) //contraseña nueva tenga entre 5 y 15 caracteres
                    {

                        if (!pwdNueva.Equals(pwdActual)) // constraseña nueva no sea igual a la actual
                        {
                            // cambiar la contraseña del usuario en la base de datos
                            if (MySQLDB.Instance.CambiarConstraseña(Usuario.InstanciaUser.User, pwdNueva))
                            {
                                // cerrar ventana
                                UIGlobal.CambiarContraseña.Close();
                                string mensaje = (string)Application.Current.FindResource("exitoCambiarPwd");
                                string cabecera = (string)Application.Current.FindResource("informacion");
                                MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                        else
                        {
                            string mensaje = (string)Application.Current.FindResource("igualPwd");
                            string cabecera = (string)Application.Current.FindResource("error");
                            MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                            UIGlobal.CambiarContraseña.pwdNueva.Focus();
                        }
                    }
                    else
                    {
                        string mensaje = (string)Application.Current.FindResource("pwdMax");
                        string cabecera = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                        UIGlobal.CambiarContraseña.pwdNueva.Focus();
                    }
                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("pwdNoVacia");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.CambiarContraseña.pwdNueva.Focus();
                }
            }
            else
            {
                string mensaje = (string)Application.Current.FindResource("errorCambiarPwd");
                string cabecera = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.CambiarContraseña.pwdActual.Focus();
            }


        }

        #endregion
    }
}
