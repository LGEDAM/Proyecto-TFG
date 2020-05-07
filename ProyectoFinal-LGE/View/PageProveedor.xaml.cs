using Microsoft.Office.Interop.Excel;
using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.ViewModel;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para PageProveedor.xaml
    /// </summary>
    public partial class PageProveedor : System.Windows.Controls.Page
    {

        /// <summary>
        /// Inicializa una nueva instancia de PageProveedor.xaml
        /// </summary>
        public PageProveedor()
        {
            UIGlobal.PageProveedor = this;
            InitializeComponent();
            DataContext = new ProveedorVM();          
        }
        
        
    }
}
