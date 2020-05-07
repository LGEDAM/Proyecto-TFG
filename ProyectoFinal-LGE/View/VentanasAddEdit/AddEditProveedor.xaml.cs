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
    public partial class AddEditProveedor : Window
    {

        private ProveedorVM proveedorVM;
        private Proveedor proveedor;
      
        private Boolean add = true; // true añadir proveedor, false modificar proveedor

        /// <summary>
        /// Constructor para añadir un proveedor
        /// </summary>
        /// <param name="pVM">Vista modelo del proveedor</param>
        public AddEditProveedor(ProveedorVM pVM)
        {
            UIGlobal.AddEditProveedor = this;
            InitializeComponent();
            proveedorVM = pVM;
            proveedor = new Proveedor();
            DataContext = proveedor;

            add = true;
        }

        /// <summary>
        /// Constructor para modificar un proveedor
        /// </summary>
        /// <param name="pVM">Vista modelo del proveedor</param>
        /// <param name="p">Proveedor a modificar</param>
        public AddEditProveedor(ProveedorVM pVM, Proveedor p)
        {

            UIGlobal.AddEditProveedor = this;
            InitializeComponent();

            id.IsEnabled = false; // El codigo del proveedor no se puede modificar
            proveedorVM = pVM;

            proveedor = (Proveedor)p.Clone();
            DataContext = proveedor;

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
        /// Método del boton Aceptar que añadira o modificara un proveedor dependiendo de la opcion escogida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarClick(object sender, RoutedEventArgs e)
        {

            if (add) // Añadir proveedor
            {
                proveedorVM.addProveedor(proveedor);
            }
            else // Modificar proveedor
            {

                proveedorVM.editProveedor(proveedor);
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
            if (MessageBox.Show(mensaje, cabecera, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                Close();
            }
        }
    }
}
