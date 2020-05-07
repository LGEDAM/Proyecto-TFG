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

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para PageMovimientos.xaml
    /// </summary>
    public partial class PageMovimientos : Page
    {
        /// <summary>
        /// Inicializa una nueva instancia de PageMovimientos.xaml
        /// </summary>
        public PageMovimientos()
        {
            UIGlobal.PageMovimientos = this;
            InitializeComponent();
            DataContext = new MovimientosVM();
        }


    }
}
