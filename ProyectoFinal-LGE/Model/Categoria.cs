using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    /// <summary>
    /// Crea un objeto Categoria
    /// </summary>
    public class Categoria : NotifyBase
    {
        #region Atributos
        private string id;
        private string nombre;
        private string descripcion;
        #endregion

        #region Propiedades
        /// <summary>
        /// Gets y sets del código de la categoria
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        ///  Gets y sets de la descripción de la categoria
        /// </summary>
        public string Descripcion { get => descripcion; set => descripcion = value; }
        /// <summary>
        /// Gets y sets del nombre de la categoria
        /// </summary>
        public string Nombre { get => nombre; set => nombre = value; }
        #endregion

        #region Constructores

        /// <summary>
        /// Crea una nueva Categoria
        /// </summary>
        /// <param name="id">Código de la categoria.</param>
        /// <param name="nombre">Nombre de la categoria.</param>
        /// <param name="descripcion">Descripción de la categoria.</param>
        public Categoria(string id, string nombre,string descripcion)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
        }
        
        /// <summary>
        /// Constructor vacio
        /// </summary>
        public Categoria()
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
