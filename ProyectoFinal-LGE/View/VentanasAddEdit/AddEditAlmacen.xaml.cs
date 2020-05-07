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

namespace ProyectoFinal_LGE.View.VentanasAddEdit
{
    /// <summary>
    /// Lógica de interacción para AddEditAlmacen.xaml
    /// </summary>
    public partial class AddEditAlmacen : Window
    {

        private AlmacenVM almacenVM;
        private Almacen almacen;

        private Boolean add = true; // true añadir almacen, false modificar almacen

        /// <summary>
        /// Constructor para añadir un almacen
        /// </summary>
        /// <param name="aVM">Vista modelo del almacen</param>
        public AddEditAlmacen(AlmacenVM aVM)
        {
            UIGlobal.AddEditAlmacen = this;
            InitializeComponent();
            almacenVM = aVM;
            almacen = new Almacen();
            DataContext = almacen;

            add = true;
        }

        /// <summary>
        /// Constructor para modificar un almacen
        /// </summary>
        /// <param name="aVM">Vista modelo del almacen</param>
        /// <param name="a">Almacen a modificar</param>
        public AddEditAlmacen(AlmacenVM aVM, Almacen a)
        {
            UIGlobal.AddEditAlmacen = this;
            InitializeComponent();

            id.IsEnabled = false; // el codigo del almacen no se puede cambiar
            almacenVM = aVM;

            almacen = (Almacen)a.Clone();
            DataContext = almacen;
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
        /// Método del boton Aceptar que añadira o modificara un almacen dependiendo de la opcion escogida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarClick(object sender, RoutedEventArgs e)
        {
            if (add) // añadir almacen
            {
                almacenVM.AddAlmacen(almacen);

            }
            else // modificar almacen
            {
                almacenVM.EditAlmacen(almacen);
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
    }
}
