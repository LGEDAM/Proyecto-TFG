using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_LGE.Model
{
    /// <summary>
    /// Crea un objeto Usuario
    /// </summary>
    public sealed class Usuario : NotifyBase
    {
        //Variable estatica de la  clase      
        private static Usuario instanciaUser = new Usuario();

        /// <summary>
        /// Variable publica de la instancia de la clase
        /// </summary>
        public static Usuario InstanciaUser { set; get; }
    

        #region Atributos
        private String user;
        private String pwd;
        private Boolean importar;
        private Boolean exportar;
        private Boolean productos;
        private Boolean categorias;
        private Boolean proveedores;
        private Boolean almacenes;
        private Boolean usuarios;
        private Boolean transacciones;
        private byte[] imagen;
        #endregion

        #region Propiedades
        /// <summary>
        /// Gets y sets del nombre de usuario
        /// </summary>
        public string User { get => user; set => user = value; }
        /// <summary>
        /// Gets y sets de la contraseña de usuario
        /// </summary>
        public string Pwd { get => pwd; set => pwd = value; }
        /// <summary>
        /// Gets y sets de la imagen de perfil de usuario
        /// </summary>
        public byte[] Imagen { get => imagen; set => imagen = value; }
        /// <summary>
        /// Gets y sets del permiso para importar CSV. True puede importar, false no.
        /// </summary>
        public bool Importar { get => importar; set => importar = value; }
        /// <summary>
        /// Gets y sets del permiso para expotar a CSV. True puede exportar, false no.
        /// </summary>
        public bool Exportar { get => exportar; set => exportar = value; }
        /// <summary>
        /// Gets y sets del permiso para trabajar con los productos. True puede trabajar, false no.
        /// </summary>
        public bool Productos { get => productos; set => productos = value; }
        /// <summary>
        /// Gets y sets del permiso para trabajar con las categorias. True puede trabajar, false no.
        /// </summary>
        public bool Categorias { get => categorias; set => categorias = value; }
        /// <summary>
        /// Gets y sets del permiso para trabajar con los proveedore. True puede trabajar, false no.
        /// </summary>
        public bool Proveedores { get => proveedores; set => proveedores = value; }
        /// <summary>
        /// Gets y sets del permiso para trabajar con los almacenes. True puede trabajar, false no.
        /// </summary>
        public bool Almacenes { get => almacenes; set => almacenes = value; }
        /// <summary>
        /// Gets y sets del permiso para trabajar con los usuarios. True puede trabajar, false no.
        /// </summary>
        public bool Usuarios { get => usuarios; set => usuarios = value; }
        /// <summary>
        /// Gets y sets del permiso para borrar transacciones. True puede trabajar, false no.
        /// </summary>
        public bool Transacciones { get => transacciones; set => transacciones = value; }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor vacio
        /// </summary>
        public Usuario()
        {
        }

        /// <summary>
        /// Crea un nuevo usuario con nombre y contraseña
        /// </summary>
        /// <param name="user">Nombre de usuario</param>
        /// <param name="pwd">Contraseña de usuario</param>
        public Usuario(string user, string pwd)
        {
            this.user = user;
            this.pwd = pwd;
        }

        /// <summary>
        ///  Crea un nuevo Usuario
        /// </summary>
        /// <param name="user">Nombre de usuario</param>
        /// <param name="pwd">Contraseña de usuario</param>
        /// <param name="importar">Permiso para importar CSV</param>
        /// <param name="exportar">Permiso para exportar CSV</param>
        /// <param name="productos">Permiso para trabajar con productos</param>
        /// <param name="categorias">Permiso para trabajar con categorias</param>
        /// <param name="proveedores">Permiso para trabajar con proveedores</param>
        /// <param name="almacenes">Permiso para trabajar con almacenes</param>
        /// <param name="usuarios">Permiso para trabajar con usuarios</param>
        /// <param name="transacciones">Permiso para borrar transacciones</param>
        /// <param name="imagen">Imagen de perfil de usuario</param>
        public Usuario(string user, string pwd, bool importar, bool exportar, bool productos, bool categorias, bool proveedores, bool almacenes, bool usuarios, 
            bool transacciones, byte[] imagen)
        {
            this.user = user;
            this.pwd = pwd;
            this.importar = importar;
            this.exportar = exportar;
            this.productos = productos;
            this.categorias = categorias;
            this.proveedores = proveedores;
            this.almacenes = almacenes;
            this.usuarios = usuarios;
            this.transacciones = transacciones;
            this.imagen = imagen;
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
