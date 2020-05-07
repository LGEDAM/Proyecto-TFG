using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    /// <summary>
    /// Crea un objeto Almacen
    /// </summary>
    public class Almacen : NotifyBase
    {
        #region Atributos
        private string id;
        private string nombre;
        private string direccion;
        #endregion

        #region Propiedades
        /// <summary>
        /// Gets y sets del código del almacen
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        ///  Gets y sets del nombre del almacen
        /// </summary>
        public string Nombre { get => nombre; set => nombre = value; }
        /// <summary>
        ///  Gets y sets de la direccion del almacen
        /// </summary>
        public string Direccion { get => direccion; set => direccion = value; }

        #endregion

        #region Constructores
        /// <summary>
        /// Contructor vacio
        /// </summary>
        public Almacen()
        {
        }

        /// <summary>
        /// Crea un nuevo Almacen
        /// </summary>
        /// <param name="id">Código del almacen</param>
        /// <param name="nombre">Nombre del almacen</param>
        /// <param name="direccion">Direccion del almacen</param>
        public Almacen(string id, string nombre, string direccion)
        {
            this.id = id;
            this.nombre = nombre;
            this.direccion = direccion;
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
