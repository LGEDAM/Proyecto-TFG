using Microsoft.Office.Interop.Excel;
using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;

namespace ProyectoFinal_LGE.ViewModel
{
    public class MainVM
    {
        #region Comandos MenuItems
        // Implementacion del comando Inicio
        private ICommand itemInicio;
        public ICommand ItemInicio
        {
            get
            {
                if (itemInicio == null)
                {
                    itemInicio = new RelayCommand(param => this.AccionInicio(), null);
                }
                return itemInicio;
            }
        }

        // Implementacion del comando Login
        private ICommand itemLogin;
        public ICommand ItemLogin
        {
            get
            {
                if (itemLogin == null)
                {
                    itemLogin = new RelayCommand(param => this.AccionLogin(), null);
                }
                return itemLogin;
            }
        }

        // Implementacion del comando Ayuda-Informacion
        private ICommand itemAyuda;
        public ICommand ItemAyuda
        {

            get
            {
                if (itemAyuda == null)
                {
                    itemAyuda = new RelayCommand(param => this.AccionAyuda(), null);
                }
                return itemAyuda;
            }
        }

        // Implementacion del comando Salir
        private ICommand itemSalir;
        public ICommand ItemSalir
        {

            get
            {
                if (itemSalir == null)
                {
                    itemSalir = new RelayCommand(param => this.AccionSalir(), null);
                }
                return itemSalir;
            }
        }

        // Implementacion del comando Minimizar Ventana
        private ICommand itemMinimizar;
        public ICommand ItemMinimizar
        {

            get
            {
                if (itemMinimizar == null)
                {
                    itemMinimizar = new RelayCommand(param => this.AccionMinimizarVentana(), null);
                }
                return itemMinimizar;
            }
        }

        // Implementacion del comando Maximizar Ventana
        private ICommand itemMaximizar;
        public ICommand ItemMaximizar
        {

            get
            {
                if (itemMaximizar == null)
                {
                    itemMaximizar = new RelayCommand(param => this.AccionMaximizarVentana(), null);
                }
                return itemMaximizar;
            }
        }



        // Implementacion del comando Home
        private ICommand itemHome;
        public ICommand ItemHome
        {

            get
            {
                if (itemHome == null)
                {
                    itemHome = new RelayCommand(param => this.AccionHome(), null);
                }
                return itemHome;
            }
        }

        // Implementacion del comando Ajustes
        private ICommand btnAjustes;
        public ICommand BtnAjustes
        {

            get
            {
                if (btnAjustes == null)
                {
                    btnAjustes = new RelayCommand(param => this.AccionAjustes(), null);
                }
                return btnAjustes;
            }
        }

        // Implementacion del comando Ajustes
        private ICommand itemIngles;
        public ICommand ItemIngles
        {

            get
            {
                if (itemIngles == null)
                {
                    itemIngles = new RelayCommand(param => this.AccionIdiomaIngles(), null);
                }
                return itemIngles;
            }
        }

        // Implementacion del comando Ajustes
        private ICommand itemEspañol;
        public ICommand ItemEspañol
        {

            get
            {
                if (itemEspañol == null)
                {
                    itemEspañol = new RelayCommand(param => this.AccionIdiomaEspañol(), null);
                }
                return itemEspañol;
            }
        }
        #endregion

        #region Acciones MenuItems

        private void AccionHome()
        {
            if (UIGlobal.MainWindow.MenuHome.IsVisible)
            {
                UIGlobal.MainWindow.MenuHome.Visibility = Visibility.Collapsed;
            }
            else
            {
                UIGlobal.MainWindow.MenuHome.Visibility = Visibility.Visible;
            }

        }

        private void AccionInicio()
        {
            Inicio page = new Inicio();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);
        }

        // Método que implementa el comando BtnAyuda
        private void AccionAyuda()
        {
            Ayuda page = new Ayuda();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);
        }

        private void AccionSalir()
        {
            UIGlobal.MainWindow.Close();
        }

        private void AccionLogin()
        {
            Login page = new Login();
            page.ShowDialog();
        }

        private void AccionMaximizarVentana()
        {
            if (UIGlobal.MainWindow.WindowState == System.Windows.WindowState.Maximized)
            {
                UIGlobal.MainWindow.WindowState = System.Windows.WindowState.Normal;
                UIGlobal.MainWindow.max.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
            else
            {
                UIGlobal.MainWindow.max.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                UIGlobal.MainWindow.WindowState = System.Windows.WindowState.Maximized;
            }

        }


        private void AccionMinimizarVentana()
        {
            UIGlobal.MainWindow.WindowState = WindowState.Minimized;
        }



        private void AccionIdiomaIngles()
        {
            Globalizacion.Instance.CambiarIdioma("en");
            System.Windows.Forms.MessageBoxManager.OK = (string)Application.Current.FindResource("Aceptar");
            System.Windows.Forms.MessageBoxManager.Cancel = (string)Application.Current.FindResource("Cancelar");

        }


        private void AccionIdiomaEspañol()
        {
            Globalizacion.Instance.CambiarIdioma("es");
            System.Windows.Forms.MessageBoxManager.OK = (string)Application.Current.FindResource("Aceptar");
            System.Windows.Forms.MessageBoxManager.Cancel = (string)Application.Current.FindResource("Cancelar");
        }

        #endregion

        #region Comandos Botones


        private ICommand btnProductos;
        public ICommand BtnProductos
        {
            get
            {
                if (btnProductos == null)
                {
                    btnProductos = new RelayCommand(param => this.AccionPageProductos(), null);
                }
                return btnProductos;
            }
        }
        // Implementacion del comando Proveedores 
        private ICommand btnProveedores;
        public ICommand BtnProveedores
        {

            get
            {
                if (btnProveedores == null)
                {
                    btnProveedores = new RelayCommand(param => this.AccionPageProveedores(), null);
                }
                return btnProveedores;
            }
        }
        // Implementacion del comando Categorias
        private ICommand btnCategorias;
        public ICommand BtnCategorias
        {
            get
            {
                if (btnCategorias == null)
                {
                    btnCategorias = new RelayCommand(param => this.AccionPageCategorias(), null);
                }
                return btnCategorias;
            }
        }
        // Implementacion del comando Categorias
        private ICommand btnAlmacenes;
        public ICommand BtnAlmacenes
        {
            get
            {
                if (btnAlmacenes == null)
                {
                    btnAlmacenes = new RelayCommand(param => this.AccionPageAlmacenes(), null);
                }
                return btnAlmacenes;
            }
        }
        // Implementacion del comando Categorias
        private ICommand btnMovimientos;
        public ICommand BtnMovimientos
        {
            get
            {
                if (btnMovimientos == null)
                {
                    btnMovimientos = new RelayCommand(param => this.AccionPageMovimientos(), null);
                }
                return btnMovimientos;
            }
        }
        #endregion

        #region Acciones Botones

        private void AccionAjustes()
        {
            Ajustes page = new Ajustes();
            page.ShowDialog();
        }

        private void AccionPageMovimientos()
        {
            //Cambiar pagina
            PageMovimientos page = new PageMovimientos();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);

            if (Usuario.InstanciaUser.Exportar)
            {
                UIGlobal.PageMovimientos.exportarTransaccion.IsEnabled = true;
                //UIGlobal.PageMovimientos.eliminarRecepcion.Visibility = Visibility.Visible;
                //UIGlobal.PageMovimientos.exportarRecepcion.IsEnabled = true;

            }
            if (Usuario.InstanciaUser.Transacciones)
            {
                UIGlobal.PageMovimientos.eliminarTransaccion.Visibility = Visibility.Visible;
            }
            //if (Usuario.InstanciaUser.Importar)
            //{
            //    UIGlobal.PageMovimientos.importarRecepcion.IsEnabled = true;
            //}

        }

        private void AccionPageAlmacenes()
        {
            //Cambiar pagina
            PageAlmacen page = new PageAlmacen();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);

            //Usuario usuario = MySQLDB.Instance.usuario;
            if (Usuario.InstanciaUser.Almacenes)
            {
                UIGlobal.PageAlmacen.add.IsEnabled = true;
                UIGlobal.PageAlmacen.edit.IsEnabled = true;
                UIGlobal.PageAlmacen.delete.IsEnabled = true;
            }
            if (Usuario.InstanciaUser.Exportar)
            {
                UIGlobal.PageAlmacen.exportar.IsEnabled = true;
                UIGlobal.PageAlmacen.reporte.IsEnabled = true;
            }

            if (Usuario.InstanciaUser.Importar)
            {
                UIGlobal.PageAlmacen.importar.IsEnabled = true;
            }

        }

        private void AccionPageProductos()
        {
            //Cambiar pagina
            PageProducto page = new PageProducto();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);

            //Usuario u = MySQLDB.Instance.usuario;
            if (Usuario.InstanciaUser.Productos)
            {
                UIGlobal.PageProducto.add.IsEnabled = true;
                UIGlobal.PageProducto.edit.IsEnabled = true;
                UIGlobal.PageProducto.delete.IsEnabled = true;
            }
            if (Usuario.InstanciaUser.Exportar)
            {
                UIGlobal.PageProducto.exportar.IsEnabled = true;
            }

            if (Usuario.InstanciaUser.Importar)
            {
                UIGlobal.PageProducto.importar.IsEnabled = true;
            }
        }

        // Método que implementa el comando BtnCategorias
        // Muestra la pagina de categorias
        private void AccionPageCategorias()
        {
            //Cambiar pagina
            PageCategoria page = new PageCategoria();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);

            if (Usuario.InstanciaUser.Categorias)
            {
                UIGlobal.PageCategoria.add.IsEnabled = true;
                UIGlobal.PageCategoria.edit.IsEnabled = true;
                UIGlobal.PageCategoria.delete.IsEnabled = true;
            }
            if (Usuario.InstanciaUser.Exportar)
            {
                UIGlobal.PageCategoria.exportar.IsEnabled = true;
            }

            if (Usuario.InstanciaUser.Importar)
            {
                UIGlobal.PageCategoria.importar.IsEnabled = true;
            }

        }

        // Método que implementa el comando BtnProveedores
        // Muestra la pagina de proveedores
        private void AccionPageProveedores()
        {
            //Cambiar pagina
            PageProveedor page = new PageProveedor();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);

            if (Usuario.InstanciaUser.Proveedores)
            {
                UIGlobal.PageProveedor.add.IsEnabled = true;
                UIGlobal.PageProveedor.edit.IsEnabled = true;
                UIGlobal.PageProveedor.delete.IsEnabled = true;
            }
            if (Usuario.InstanciaUser.Exportar)
            {
                UIGlobal.PageProveedor.exportar.IsEnabled = true;
            }

            if (Usuario.InstanciaUser.Importar)
            {
                UIGlobal.PageProveedor.importar.IsEnabled = true;
            }

        }
        #endregion

        #region Comandos Usuarios

        private ICommand btnLogin;
        public ICommand BtnLogin
        {

            get
            {
                if (btnLogin == null)
                {
                    btnLogin = new RelayCommand(param => this.AccionComprobarLogin(), null);
                }
                return btnLogin;
            }
        }

        private ICommand btnUsuarioLogeado;
        public ICommand BtnUsuarioLogeado
        {
            get
            {
                if (btnUsuarioLogeado == null)
                {
                    btnUsuarioLogeado = new RelayCommand(param => this.AccionInformacionUsuario(), null);
                }
                return btnUsuarioLogeado;
            }
        }

        private ICommand cerrarSesion;
        public ICommand CerrarSesion
        {
            get
            {
                if (cerrarSesion == null)
                {
                    cerrarSesion = new RelayCommand(param => this.AccionCerrarSesion(), null);
                }
                return cerrarSesion;
            }
        }

        private ICommand gestionarUsuarios;
        public ICommand GestionarUsuarios
        {
            get
            {
                if (gestionarUsuarios == null)
                {
                    gestionarUsuarios = new RelayCommand(param => this.AccionGestionarUsuarios(), null);
                }
                return gestionarUsuarios;
            }
        }

        private ICommand cambiarContraseña;
        public ICommand CambiarContraseña
        {
            get
            {
                if (cambiarContraseña == null)
                {
                    cambiarContraseña = new RelayCommand(param => this.AccionCambiarContraseña(), null);
                }
                return cambiarContraseña;
            }
        }



        #endregion

        #region Acciones Usuarios

        private void AccionComprobarLogin()
        {

            String u = UIGlobal.Login.usuario.Text;
            String p = UIGlobal.Login.pwd.Password;

            String[] user = MySQLDB.Instance.ObtenerUsername();

            if (MySQLDB.Instance.Logear(u, p))
            {

                UIGlobal.Login.Close();
                UIGlobal.MainWindow.btnUsuarioLogeado.IsEnabled = true;
                UIGlobal.MainWindow.Home.IsEnabled = true;
                UIGlobal.MainWindow.menuItemLogin.Visibility = Visibility.Hidden;
                UIGlobal.MainWindow.MenuHome.Visibility = Visibility.Visible;
                UIGlobal.Inicio.ComenzarInicioSesion.Visibility = Visibility.Hidden;

                //Nombre de usuario
                UIGlobal.MainWindow.nombreUsuario.Text = Usuario.InstanciaUser.User;
                UIGlobal.MainWindow.iconUsuario.Visibility = Visibility.Visible;

            }
            else
            {
                if (u != null && !u.Trim().Equals(""))
                {

                    if (!user.Contains(u))
                    {
                        string mensaje = (string)Application.Current.FindResource("noExisteUsuario");
                        string cabecera = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (p != null && !p.Trim().Equals(""))
                        {
                            string mensaje = (string)Application.Current.FindResource("errorCambiarPwd");
                            string cabecera = (string)Application.Current.FindResource("error");
                            MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            string mensaje = (string)Application.Current.FindResource("usuarioPwdNoVacio");
                            string cabecera = (string)Application.Current.FindResource("informacion");
                            MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    string mensaje = (string)Application.Current.FindResource("usuarioNombreNoVacio");
                    string cabecera = (string)Application.Current.FindResource("informacion");
                    MessageBox.Show(mensaje, cabecera, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AccionInformacionUsuario()
        {
            UIGlobal.MainWindow.usuarioLogeado.IsOpen = true;

            UIGlobal.MainWindow.textNombre.Text = Usuario.InstanciaUser.User;

            byte[] img = Usuario.InstanciaUser.Imagen;
            if (img != null)
            {
                var image = new BitmapImage();
                using (var mem = new MemoryStream(img))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                UIGlobal.MainWindow.imagenUsuario.Source = image;
            }
            else
            {
                UIGlobal.MainWindow.imagenUsuario.Source = new BitmapImage(new Uri("/Media/avatarUsuario.png", UriKind.Relative));
            }


            if (Usuario.InstanciaUser.Usuarios)
            {
                UIGlobal.MainWindow.stackGestionUsuarios.Visibility = Visibility.Visible;
                UIGlobal.MainWindow.gestionarUsaurios.IsEnabled = true;
            }
            else
            {
                UIGlobal.MainWindow.stackGestionUsuarios.Visibility = Visibility.Collapsed;
                UIGlobal.MainWindow.gestionarUsaurios.IsEnabled = false;
            }
        }

        private void AccionCerrarSesion()
        {
            UIGlobal.MainWindow.usuarioLogeado.IsOpen = false;
            UIGlobal.MainWindow.menuItemLogin.Visibility = Visibility.Visible;
            UIGlobal.MainWindow.MenuHome.Visibility = Visibility.Collapsed;
            UIGlobal.MainWindow.Home.IsEnabled = false;
            UIGlobal.MainWindow.nombreUsuario.Text = null;
            UIGlobal.MainWindow.iconUsuario.Visibility = Visibility.Hidden;
            Inicio page = new Inicio();
            UIGlobal.MainWindow.framePrincipal.Navigate(page);
        }

        private void AccionGestionarUsuarios()
        {
            Usuarios page = new Usuarios();
            page.ShowDialog();
        }

        private void AccionCambiarContraseña()
        {

            CambiarContraseña page = new CambiarContraseña();
            page.ShowDialog();

        }

        #endregion
    }
}
