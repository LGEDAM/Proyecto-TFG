using ProyectoFinal_LGE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    /// <summary>
    /// Crea un objeto Producto
    /// </summary>
    public class Producto : NotifyBase
    {
        #region Atributos
        private string codigo;
        private string nombre;
        private byte[] imagen;
        private string descripcion;
        private string categoria;
        private string almacen;
        private int unidades;
        private string proveedor;
        private double precioCompra;
        private double precioVenta;
        private Boolean disponible;
        private int ancho;
        private int largo;
        private int alto;
        #endregion

        #region Propiedades
        /// <summary>
        /// Gets y sets del código del producto
        /// </summary>
        public string Codigo { get => codigo; set => codigo = value; }
        /// <summary>
        /// Gets y sets del nombre del producto
        /// </summary>
        public string Nombre { get => nombre; set => nombre = value; }
        /// <summary>
        /// Gets y sets de la descripcion del producto
        /// </summary>
        public string Descripcion { get => descripcion; set => descripcion = value; }
        /// <summary>
        /// Gets y sets del numero de unidades del producto
        /// </summary>
        public int Unidades { get => unidades; set => unidades = value; }
        /// <summary>
        /// Gets y sets del almacen donde se encuentra el producto
        /// </summary>
        public string Almacen {get => almacen; set => almacen = value; }
        /// <summary>
        /// Gets y sets del precio de venta del producto
        /// </summary>
        public double PrecioVenta { get => precioVenta; set => precioVenta = value; }
        /// <summary>
        /// Gets y sets de la categoria del producto
        /// </summary>
        public string Categoria { get => categoria; set => categoria = value; }
        /// <summary>
        /// Gets y sets del proveedor del producto
        /// </summary>
        public string Proveedor { get => proveedor; set => proveedor = value; }
        /// <summary>
        /// Gets y sets del precio de compra del producto
        /// </summary>
        public double PrecioCompra { get => precioCompra; set => precioCompra = value; }
        /// <summary>
        /// Gets y sets de la disponibilidad del producto
        /// </summary>
        public Boolean Disponible { get => disponible; set => disponible = value; }
        /// <summary>
        /// Gets y sets de ancho del producto
        /// </summary>
        public int Ancho { get => ancho; set => ancho = value; }
        /// <summary>
        /// Gets y sets del largo del producto
        /// </summary>
        public int Largo { get => largo; set => largo = value; }
        /// <summary>
        /// Gets y sets de la altura del producto
        /// </summary>
        public int Alto { get => alto; set => alto = value; }
        /// <summary>
        /// Gets y sets de la imagen del producto
        /// </summary>
        public byte[] Imagen { get => imagen; set => imagen = value; }
        /// <summary>
        /// Lista de los codigos de las categorias
        /// </summary>
        public String[] IdCategorias
        {
            get
            {
                MySQLDB.Instance.ConnectDB("IdCategorias");
                return MySQLDB.Instance.categorias;
            }
        }
        /// <summary>
        /// Lista de los codigos de los proveedores
        /// </summary>
        public String[] IdProveedor
        {
            get
            {

                MySQLDB.Instance.ConnectDB("IdProveedor");
                return MySQLDB.Instance.proveedores;
            }
        }
        /// <summary>
        /// Lista de los codigos de los almacenes
        /// </summary>
        public String[] IdAlmacenes
        {
            get
            {

                MySQLDB.Instance.ConnectDB("IdAlmacenes");
                return MySQLDB.Instance.almacenes;
            }
        }
        #endregion

        #region Constructor

        //public Producto(string codigo,string almacen)
        //{
        //    this.codigo = codigo;
        //    this.almacen = almacen;
        //}

        /// <summary>
        ///  Crea un nuevo Producto
        /// </summary>
        /// <param name="codigo">Codigo del producto</param>
        /// <param name="nombre">Nombre del producto</param>
        /// <param name="imagen">Imagen del producto</param>
        /// <param name="descripcion">Descripcion del producto</param>
        /// <param name="categoria">Categoria del prodcuto</param>
        /// <param name="almacen">Almacen del producto</param>
        /// <param name="unidades">Unidades del producto</param>
        /// <param name="ancho">Ancho del producto</param>
        /// <param name="largo">Largo del producto</param>
        /// <param name="alto">Alto del producto</param>
        /// <param name="proveedor">Proveedor del producto</param>
        /// <param name="precioCompra">Precio de compra del producto</param>
        /// <param name="precioVenta">Precio de venta del producto</param>
        /// <param name="disponible">Disponibilidad del producto</param>
        public Producto(string codigo, string nombre, byte[] imagen, string descripcion, string categoria, 
            string almacen, int unidades, int ancho, int largo, int alto, string proveedor, double precioCompra, double precioVenta, bool disponible) 
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.imagen = imagen;
            this.almacen = almacen;
            this.descripcion = descripcion;
            this.categoria = categoria;
            this.unidades = unidades;
            this.proveedor = proveedor;
            this.precioCompra = precioCompra;
            this.precioVenta = precioVenta;
            this.disponible = disponible;
            this.ancho = ancho;
            this.largo = largo;
            this.alto = alto;
        }

        /// <summary>
        /// Constructor vacio
        /// </summary>
        public Producto()
        {
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Crea una copia del objeto actual
        /// </summary>
        /// <returns>El objeto copiado</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

    }
}
