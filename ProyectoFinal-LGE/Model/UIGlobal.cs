using ProyectoFinal_LGE.View;
using ProyectoFinal_LGE.View.VentanasAddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    public static class UIGlobal
    {
        public static MainWindow MainWindow { get; set; }
        public static PageAlmacen PageAlmacen { get; set; }
        public static PageCategoria PageCategoria { get; set; }
        public static PageProveedor PageProveedor { get; set; }
        public static PageProducto PageProducto { get; set; }
        public static PageMovimientos PageMovimientos { get; set; }
        public static AddEditProducto AddEditProducto { get; set; }
        public static AddEditAlmacen AddEditAlmacen { get; set; }
        public static AddEditCategoria AddEditCategoria { get; set; }
        public static AddEditProveedor AddEditProveedor { get; set; }
        public static AddEditUsuario AddEditUsuario { get; set; }
        public static Login Login { get; set; }
        public static Usuarios Usuarios { get; set; }
        public static Inicio Inicio { get; set; }
        public static CambiarContraseña CambiarContraseña { get; set; }
    }
}
