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
    public partial class AddEditCategoria : Window
    {
        private CategoriaVM categoriaVM;
        private Categoria categoria;

        private Boolean add = true; // true añadir categoria, false modificar categoria


        /// <summary>
        /// Constructor para añadir una categoria
        /// </summary>
        /// <param name="cVM">Vista modelo de la categoria</param>
        public AddEditCategoria(CategoriaVM cVM)
        {
            UIGlobal.AddEditCategoria = this;
            InitializeComponent();
            categoriaVM = cVM;
            categoria = new Categoria();
            DataContext = categoria;

            add = true;
        }

        /// <summary>
        /// Constructor para modificar una categoria
        /// </summary>
        /// <param name="cVM">Vista modelo de la categoria</param>
        /// <param name="c">Categoria a modificar</param>
        public AddEditCategoria(CategoriaVM cVM, Categoria c)
        {
            UIGlobal.AddEditCategoria = this;
            InitializeComponent();

            id.IsEnabled = false; // el codigo de la categoria no se puede cambiar
            categoriaVM = cVM;

            categoria = (Categoria)c.Clone();
            DataContext = categoria;

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
        /// Método del boton Aceptar que añadira o modificara una categoria dependiendo de la opcion escogida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarClick(object sender, RoutedEventArgs e)
        {
            if (add) // añadir categoria
            {
                categoriaVM.AddCategoria(categoria);
            }
            else // modificar categoria
            {
                categoriaVM.EditCategoria(categoria);
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
