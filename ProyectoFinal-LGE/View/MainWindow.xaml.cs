using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.View;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            UIGlobal.MainWindow = this;
            InitializeComponent();
            DataContext = new MainVM();

            // Mostramos la pagina de inicio
            Inicio i = new Inicio();
            framePrincipal.Navigate(i);

            // texto de los botones del MessageBox
            System.Windows.Forms.MessageBoxManager.OK = (string)Application.Current.FindResource("Aceptar");
            System.Windows.Forms.MessageBoxManager.Cancel = (string)Application.Current.FindResource("Cancelar");
            System.Windows.Forms.MessageBoxManager.Register();

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

    }
}
