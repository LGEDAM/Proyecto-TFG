using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para PageAlmacen.xaml
    /// </summary>
    public partial class PageAlmacen : Page
    {
        /// <summary>
        /// Inicializa una nueva instancia de PageAlmacen.xaml
        /// </summary>
        public PageAlmacen()
        {
            UIGlobal.PageAlmacen = this;
            InitializeComponent();
            DataContext = new AlmacenVM();
        }

    }
}
