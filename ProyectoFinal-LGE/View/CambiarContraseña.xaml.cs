using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Collections.Generic;
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

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para CambiarContraseña.xaml
    /// </summary>
    public partial class CambiarContraseña : Window
    {

        /// <summary>
        /// Inicializa una nueva instancia de CambiarContraseña.xaml
        /// </summary>
        public CambiarContraseña()
        {
            UIGlobal.CambiarContraseña = this;
            InitializeComponent();
            DataContext = new UsuarioVM();
        }

        /// <summary>
        /// Método para poder mover la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
    }
}
