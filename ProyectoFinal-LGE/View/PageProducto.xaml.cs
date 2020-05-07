using MaterialDesignThemes.Wpf;
using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Lógica de interacción para PageProducto.xaml
    /// </summary>
    public partial class PageProducto : Page
    {
        /// <summary>
        /// Inicializa una nueva instancia de PageProducto.xaml
        /// </summary>
        public PageProducto()
        {
            UIGlobal.PageProducto = this;
            InitializeComponent();
            DataContext = new ProductoVM();
        }

       
    }
}
