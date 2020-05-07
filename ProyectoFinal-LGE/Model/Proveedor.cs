using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    /// <summary>
    /// Crea un objeto Proveedor
    /// </summary>
    public class Proveedor : NotifyBase
    {
        #region Atributos
        private string id;
        private string nombreCompania;
        private string nombreContacto1;
        private string nombreContacto2;
        private string direccion1;
        private string direccion2;
        private string ciudad;
        private string pais;
        private string tlno;
        private string email;
        #endregion

        #region Propiedades
        /// <summary>
        /// Gets y sets del código del proveedor
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// Gets y sets del nombre del proveedor
        /// </summary>
        public string NombreCompania { get => nombreCompania; set => nombreCompania = value; }
        /// <summary>
        /// Gets y sets del nombre del contacto del proveedor
        /// </summary>
        public string NombreContacto1 { get => nombreContacto1; set => nombreContacto1 = value; }
        /// <summary>
        /// Gets y sets de los apellidos del contacto del proveedor
        /// </summary>
        public string NombreContacto2 { get => nombreContacto2; set => nombreContacto2 = value; }
        /// <summary>
        /// Gets y sets de la direccion del proveedor
        /// </summary>
        public string Direccion1 { get => direccion1; set => direccion1 = value; }
        /// <summary>
        /// Gets y sets de la segunda direccion del proveedor
        /// </summary>
        public string Direccion2 { get => direccion2; set => direccion2 = value; }
        /// <summary>
        /// Gets y sets de la ciudad del proveedor
        /// </summary>
        public string Ciudad { get => ciudad; set => ciudad = value; }
        /// <summary>
        /// Gets y sets del pais del proveedor
        /// </summary>
        public string Pais { get => pais; set => pais = value; }
        /// <summary>
        /// Gets y sets del numero de telefono del proveedor
        /// </summary>
        public string Tlno { get => tlno; set => tlno = value; }
        /// <summary>
        /// Gets y sets del email del proveedor
        /// </summary>
        public string Email { get => email; set => email = value; }
        #endregion

        #region Constructores
        /// <summary>
        ///  Crea un nuevo Proveedor
        /// </summary>
        /// <param name="id">Codigo del proveedor</param>
        /// <param name="nombreCompania">Nombre del proveedor</param>
        /// <param name="nombreContacto1">Nombre del contacto del proveedor</param>
        /// <param name="nombreContacto2">Apellidos del contacto del proveedor</param>
        /// <param name="direccion1">Direccion 1 del proveedor</param>
        /// <param name="direccion2">Direccion 2 del proveedor</param>
        /// <param name="ciudad">Ciudad del proveedor</param>
        /// <param name="pais">Pais del proveedor</param>
        /// <param name="tlno">Numero de telefono del proveedor</param>
        /// <param name="email">Email del proveedor</param>
        public Proveedor(string id, string nombreCompania, string nombreContacto1, string nombreContacto2, string direccion1, 
            string direccion2, string ciudad, string pais, string tlno, string email)
        {
            this.id = id;
            this.nombreCompania = nombreCompania;
            this.nombreContacto1 = nombreContacto1;
            this.nombreContacto2 = nombreContacto2;
            this.direccion1 = direccion1;
            this.direccion2 = direccion2;
            this.ciudad = ciudad;
            this.pais = pais;
            this.tlno = tlno;
            this.email = email;
        }

        /// <summary>
        /// Constructor vacio
        /// </summary>
        public Proveedor()
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
