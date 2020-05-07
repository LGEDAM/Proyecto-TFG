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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de Login.xaml
        /// </summary>
        public Login()
        {
            UIGlobal.Login = this;
            InitializeComponent();
            DataContext = new MainVM();
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
        /// Método para cerrar la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CerrarClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
