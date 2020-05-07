using Microsoft.Office.Interop.Excel;
using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para PageCategoria.xaml
    /// </summary>
    public partial class PageCategoria : System.Windows.Controls.Page
    {
        /// <summary>
        /// Inicializa una nueva instancia de PageCategoria.xaml
        /// </summary>
        public PageCategoria()
        {
            UIGlobal.PageCategoria = this;
            InitializeComponent();
            DataContext = new CategoriaVM();
        }

        
    }
}
