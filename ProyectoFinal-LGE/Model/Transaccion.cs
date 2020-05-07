using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    /// <summary>
    /// Crea un objeto Transaccion
    /// </summary>
    public class Transaccion : NotifyBase
    {
        #region Atributos
        private DateTime fecha;
        private string producto;
        private string almacenV;
        private string almacenN;
        private int unidades;
        #endregion

        #region Propiedades
        /// <summary>
        /// Gets y sets de la fecha en que se realiza la transaccion
        /// </summary>
        public DateTime Fecha { get => fecha; set => fecha = value; }
        /// <summary>
        /// Gets y sets del producto a transladar
        /// </summary>
        public string Producto { get => producto; set => producto = value; }
        /// <summary>
        /// Gets y sets del almacen de origen del producto
        /// </summary>
        public string AlmacenV { get => almacenV; set => almacenV = value; }
        /// <summary>
        /// Gets y sets del almacen de destino del producto
        /// </summary>
        public string AlmacenN { get => almacenN; set => almacenN = value; }
        /// <summary>
        /// Gets y sets del numero de unidades del producto a transladar
        /// </summary>
        public int Unidades { get => unidades; set => unidades = value; }
        #endregion

        #region Constructores
        /// <summary>
        /// Crea un nuevo objeto Transaccion
        /// </summary>
        /// <param name="fecha">Fecha de realizacion de la transaccion</param>
        /// <param name="producto">Producto a transaladar</param>
        /// <param name="almacenV">Almacen de origen del producto</param>
        /// <param name="almacenN">Almacen de destino del producto</param>
        /// <param name="unidades">Numero de unidades del producto a transladar</param>
        public Transaccion( DateTime fecha, string producto, string almacenV, string almacenN,int unidades)
        {
            
            this.fecha = fecha;
            this.producto = producto;
            this.almacenV = almacenV;
            this.almacenN = almacenN;
            this.unidades = unidades;
        }

        /// <summary>
        ///  Constructor vacio
        /// </summary>
        public Transaccion()
        {
        }
        #endregion
    }
}
