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
    /// Lógica de interacción para Ajustes.xaml
    /// </summary>
    public partial class Ajustes : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de Ajustes.xaml
        /// </summary>
        public Ajustes()
        {
            InitializeComponent();
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
        private void CancelarClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Método para cambiar del tema de material design a claro (Light)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModoLight(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries[1].Source =
               new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml", UriKind.RelativeOrAbsolute);
        }


        /// <summary>
        /// Método para cambiar del tema de material design a oscuro (Dark)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModoDark(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries[1].Source =
               new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
