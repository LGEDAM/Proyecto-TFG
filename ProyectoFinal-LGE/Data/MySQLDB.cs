using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using MySql.Data.MySqlClient;
using ProyectoFinal_LGE.Model;

namespace ProyectoFinal_LGE.Data
{
    /// <summary>
    /// Clase que se conecta a la base de datos para leer, añadir, modificar o eliminar información
    /// </summary>
    public sealed class MySQLDB
    {
        // Variable estatica de la clase      
        private readonly static MySQLDB instanciaBD = new MySQLDB();

        // Arrays de los diferentes tipos de datos
        public Usuario[] ArrayUsuarios;
        public Producto[] ArrayProductos;
        public Categoria[] ArrayCategorias;
        public Proveedor[] ArrayProveedores;
        public Transaccion[] ArrayTransacciones;
        public Almacen[] ArrayAlmacenes;
        public Usuario usuario;

        // Arrays de los codigos de los distintos tipos de datos
        public String[] categorias;
        public String[] proveedores;
        public String[] almacenes;
        public String[] productos;
        public String[] usuarios;

        /// <summary>
        /// Atributo de tipo dataset donde se almacena resultado de queries
        /// </summary>
        public DataSet dsResult { get; set; }
        private DataTable dt; // Atributo de tipo datatable donde se alamcena la informacion de una tabla
        private int intEffected; // Atributo donde se almancena el numero de filas afectadas
        private string query; //Consulta

        // Datos para la conexion
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;
        private string connectionString;

        // Constructor privado        
        private MySQLDB() { }

        /// <summary>
        /// Variable publica de la instancia de la clase
        /// </summary>
        public static MySQLDB Instance
        {
            get
            {
                return instanciaBD;
            }
        }

        /// <summary>
        ///  Crear conexion a la base de datos
        /// </summary>
        private void InitParamsDB()
        {
            uid = "root";
            password = "root";
            server = "localhost";
            database = "db_inventory";
            port = "3311"; //
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" +
                                "PORT=" + port + ";" + "UID=" + uid + ";" + "PWD=" + password +
                                ";Convert Zero Datetime=true; CharSet=utf8";
            connection = new MySqlConnection(connectionString);

        }

        /// <summary>
        /// Método que inicializa las listas y las carga a los atributos
        /// </summary>
        /// <param name="opcion">Nombre de la opción a obtener información</param>
        public void ConnectDB(String opcion)
        {
            try
            {
                InitParamsDB();
                // Si se puede establecer la conexion se cargan los datos en los arrays 
                if (OpenConnection())
                {
                    switch (opcion)
                    {

                        case "Productos":
                            LeerProductos();
                            break;
                        case "Categorias":
                            LeerCategorias();
                            break;
                        case "Proveedores":
                            LeerProveedores();
                            break;
                        case "Almacenes":
                            LeerAlmacenes();
                            break;
                        case "Usuarios":
                            LeerUsuarios();
                            break;
                        case "Transacciones":
                            LeerTransacciones();
                            break;
                        case "IdCategorias":
                            ObtenerCategoriasId();
                            break;
                        case "IdProveedor":
                            ObtenerProveedoresId();
                            break;
                        case "IdAlmacenes":
                            ObtenerAlmacenesId();
                            break;
                        case "IdProductos":
                            ObtenerProductosId();
                            break;
                    }

                }
                //Cerramos la conexion
                CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        //Abrir conexion a la base de datos
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
                return false;
            }
        }

        //Cerrar conexion a la base de datos
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
                return false;
            }
        }


        #region Productos
        /// <summary>
        /// Método que obtiene los datos todos los productos de la base de datos y los almacena en ArrayProductos
        /// </summary>
        public void LeerProductos()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT * FROM productos ORDER BY codigo";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTAble
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    ArrayProductos = new Producto[intEffected];
                else
                    ArrayProductos = new Producto[0];
                // Contadores de cada fila
                int i = 0;
                string _row;
                Producto myData;
                // Por cada fila se crear un nuevo producto y se almacena en el ArrayProductos
                foreach (DataRow row in dt.Rows)
                {
                    string codigo = Convert.ToString(row["codigo"]);
                    string nombre = Convert.ToString(row["nombre"]);
                    byte[] imagen = null;
                    // Comprobar si existe la imagen del producto
                    if (Convert.IsDBNull(row["imagen"])) // Si no existe toma el valor null
                    {
                        imagen = null;
                    }
                    else
                    {
                        imagen = (byte[])(row["imagen"]);// Si existe te transforma a un array de bytes
                    }
                    string descripcion = Convert.ToString(row["descripcion"]);
                    string categoria = Convert.ToString(row["categoria"]);
                    string almacen = Convert.ToString(row["almacen"]);
                    int unidades = Convert.ToInt32(row["unidades"]);
                    int ancho = Convert.ToInt32(row["ancho"]);
                    int largo = Convert.ToInt32(row["largo"]);
                    int alto = Convert.ToInt32(row["alto"]);
                    string proveedor = Convert.ToString(row["proveedor"]);
                    double precioCompra = Convert.ToDouble(row["precioCompra"]);
                    double precioVenta = Convert.ToDouble(row["precioVenta"]);
                    Boolean disponible = Convert.ToBoolean(row["disponible"]);

                    _row = codigo + ";" + nombre + ";" + imagen + ";" + descripcion + ";" + categoria + ";" +
                        almacen + ";" + unidades + ";" + ancho + ";" + largo + ";" + alto + ";" + proveedor + ";" + precioCompra + ";" +
                        precioVenta + ";" + disponible;
                    Console.WriteLine(_row);
                    myData = new Producto(codigo, nombre, imagen, descripcion, categoria, almacen, unidades, ancho, largo, alto,
                        proveedor, precioCompra, precioVenta, disponible);
                    ArrayProductos[i] = myData;
                    i++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }


        }

        /// <summary>
        /// Método que obtiene los códigos de todos los productos de la base de datos y los almacena en el array productos
        /// </summary>
        public void ObtenerProductosId()
        {
            //Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query 
                    query = "SELECT c.codigo FROM productos c;";
                    // Ejecuto la query y recojo el resultado en un adapter
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    // Creo un nuevo DataTable
                    dt = new DataTable();
                    // Asigno al datatable el valor devuelto por el adapter 
                    adapter.Fill(dt);
                    // Obtengo el numero de filas devueltas por la query
                    intEffected = dt.Rows.Count;
                    // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                    if (intEffected > 0)
                        productos = new String[intEffected];
                    else
                        productos = new String[0];
                    int i = 0; //Numero de fila
                               //Por cada fila obtengo los datos
                    foreach (DataRow row in dt.Rows)
                    {
                        productos[i] = Convert.ToString(row["codigo"]);
                        i++;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(ex.Message);
                }
            }
            CloseConnection();
        }

        /// <summary>
        /// Método que añade el producto pasado como parametro a la base de datos
        /// </summary>
        /// <param name="prod">Producto a añadir</param>
        /// <returns>True si se ha añadido correctamente,o false en caso contrario</returns>
        public Boolean InsertarProducto(Producto prod)
        {
            Boolean añadir = false;
            //Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query 
                    query = "INSERT INTO productos VALUES(?codigo,?nombre,?imagen,?descripcion,?categoria,?almacen,?unidades," +
                        "?ancho,?largo,?alto,?proveedor,?precioCompra,?precioVenta,?disponible);";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?codigo", prod.Codigo);
                    comando.Parameters.AddWithValue("?nombre", prod.Nombre);
                    comando.Parameters.AddWithValue("?imagen", prod.Imagen);
                    comando.Parameters.AddWithValue("?descripcion", prod.Descripcion);
                    comando.Parameters.AddWithValue("?categoria", prod.Categoria);
                    comando.Parameters.AddWithValue("?almacen", prod.Almacen);
                    comando.Parameters.AddWithValue("?unidades", prod.Unidades);
                    comando.Parameters.AddWithValue("?ancho", prod.Ancho);
                    comando.Parameters.AddWithValue("?largo", prod.Largo);
                    comando.Parameters.AddWithValue("?alto", prod.Alto);
                    comando.Parameters.AddWithValue("?proveedor", prod.Proveedor);
                    comando.Parameters.AddWithValue("?precioCompra", prod.PrecioCompra);
                    comando.Parameters.AddWithValue("?precioVenta", prod.PrecioVenta);
                    comando.Parameters.AddWithValue("?disponible", prod.Disponible);
                    comando.ExecuteNonQuery();
                    añadir = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }
            //Cerrar conexion
            CloseConnection();

            return añadir;
        }

        /// <summary>
        ///  Método que modifica el producto pasado como parametro en la base de datos
        /// </summary>
        /// <param name="prod">Producto a modificar</param>
        /// <returns>True si se ha modificado correctamente,o false en caso contrario</returns>
        public Boolean ModificarProducto(Producto prod)
        {
            Boolean exito = false;
            //Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query 
                    query = "UPDATE productos SET imagen=?imagen,descripcion=?descripcion,almacen=?almacen," +
                        "categoria=?categoria,unidades=?unidades,precioCompra=?precioCompra," +
                        "precioVenta=?precioVenta,disponible=?disponible,proveedor=?proveedor" +
                        " WHERE codigo=?codigo";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?codigo", prod.Codigo);
                    comando.Parameters.AddWithValue("?nombre", prod.Nombre);
                    comando.Parameters.AddWithValue("?imagen", prod.Imagen);
                    comando.Parameters.AddWithValue("?descripcion", prod.Descripcion);
                    comando.Parameters.AddWithValue("?categoria", prod.Categoria);
                    comando.Parameters.AddWithValue("?almacen", prod.Almacen);
                    comando.Parameters.AddWithValue("?unidades", prod.Unidades);
                    comando.Parameters.AddWithValue("?proveedor", prod.Proveedor);
                    comando.Parameters.AddWithValue("?precioCompra", prod.PrecioCompra);
                    comando.Parameters.AddWithValue("?precioVenta", prod.PrecioVenta);
                    comando.Parameters.AddWithValue("?disponible", prod.Disponible);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }
            CloseConnection();
            return exito;
        }

        /// <summary>
        ///  Método que elimina el producto pasado como parametro en la base de datos
        /// </summary>
        /// <param name="p">Producto a eliminar</param>
        /// <returns>True si se ha eliminado correctamente,o false en caso contrario</returns>
        public Boolean BorrarProducto(Producto p)
        {
            Boolean exito = false;
            //Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "DELETE FROM productos WHERE codigo=?id AND almacen=?a";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", p.Codigo);
                    comando.Parameters.AddWithValue("?a", p.Almacen);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }
        #endregion

        #region Proveedores
        /// <summary>
        /// Método que obtiene los datos todos los proveedores de la base de datos y los almacena en ArrayProveedores
        /// </summary>
        private void LeerProveedores()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT * FROM proveedores ORDER BY id";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTAble
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    ArrayProveedores = new Proveedor[intEffected];
                else
                    ArrayProveedores = new Proveedor[0];
                // Contadores de cada fila
                int i = 0;
                string _row;
                Proveedor myData;
                // Por cada fila se crear un nuevo proveedor y se almacena en el ArrayProveedores
                foreach (DataRow row in dt.Rows)
                {
                    string id = Convert.ToString(row["id"]);
                    string compania = Convert.ToString(row["nombreCompania"]);
                    string contacto1 = Convert.ToString(row["nombreContacto1"]);
                    string contacto2 = Convert.ToString(row["nombreContacto2"]);
                    string dir1 = Convert.ToString(row["direccion"]);
                    string dir2 = Convert.ToString(row["direccion2"]);
                    string ciudad = Convert.ToString(row["ciudad"]);
                    string pais = Convert.ToString(row["pais"]);
                    string tlno = Convert.ToString(row["tlno"]);
                    string email = Convert.ToString(row["email"]);

                    _row = id + ";" + compania + ";" + contacto1 + ";" + contacto2 + ";" +
                        dir1 + ";" + dir2 + ";" + ciudad + ";" + pais + ";" + tlno + ";" + email;
                    Console.WriteLine(_row);
                    myData = new Proveedor(id, compania, contacto1, contacto2, dir1, dir2,
                        ciudad, pais, tlno, email);
                    ArrayProveedores[i] = myData;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        /// Método que obtiene los id de todos las categorías de la base de datos y las almacena en el array proveedores
        /// </summary>
        public void ObtenerProveedoresId()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT c.id FROM proveedores c;";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTable
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    proveedores = new String[intEffected];
                else
                    proveedores = new String[0];
                int i = 0; //Numero de fila
                //Por cada fila obtengo los id y los almaceno en el array proveedores
                foreach (DataRow row in dt.Rows)
                {
                    proveedores[i] = Convert.ToString(row["id"]);
                    i++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        /// Método que añade el proveedor pasado como parametro a la base de datos
        /// </summary>
        /// <param name="p">Proveedor a añadir</param>
        /// <returns>True si se ha añadido correctamente,o false en caso contrario</returns>
        public Boolean InsertarProveedor(Proveedor p)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    //Introduzco el texto de la query
                    query = "INSERT INTO proveedores VALUES(?id, ?nombreCompania,?nombreContacto1," +
                        "?nombreContacto2,?direccion,?direccion2,?ciudad,?pais,?tlno,?email)";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", p.Id);
                    comando.Parameters.AddWithValue("?nombreCompania", p.NombreCompania);
                    comando.Parameters.AddWithValue("?nombreContacto1", p.NombreContacto1);
                    comando.Parameters.AddWithValue("?nombreContacto2", p.NombreContacto2);
                    comando.Parameters.AddWithValue("?direccion", p.Direccion1);
                    comando.Parameters.AddWithValue("?direccion2", p.Direccion2);
                    comando.Parameters.AddWithValue("?ciudad", p.Ciudad);
                    comando.Parameters.AddWithValue("?pais", p.Pais);
                    comando.Parameters.AddWithValue("?tlno", p.Tlno);
                    comando.Parameters.AddWithValue("?email", p.Email);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que modifica el proveedor pasado como parametro en la base de datos
        /// </summary>
        /// <param name="p">Proveedor a modificar</param>
        /// <returns>True si se ha modificado correctamente,o false en caso contrario</returns>
        public Boolean ModificarProveedor(Proveedor p)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    //Introduzco el texto de la query
                    query = "UPDATE proveedores SET nombreCompania=?nombreCompania,nombreContacto1=?nombreContacto1," +
                        "nombreContacto2=?nombreContacto2,direccion=?direccion,direccion2=?direccion2," +
                        "ciudad=?ciudad,pais=?pais,tlno=?tlno,email=?email" +
                        " WHERE id=?id";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", p.Id);
                    comando.Parameters.AddWithValue("?nombreCompania", p.NombreCompania);
                    comando.Parameters.AddWithValue("?nombreContacto1", p.NombreContacto1);
                    comando.Parameters.AddWithValue("?nombreContacto2", p.NombreContacto2);
                    comando.Parameters.AddWithValue("?direccion", p.Direccion1);
                    comando.Parameters.AddWithValue("?direccion2", p.Direccion2);
                    comando.Parameters.AddWithValue("?ciudad", p.Ciudad);
                    comando.Parameters.AddWithValue("?pais", p.Pais);
                    comando.Parameters.AddWithValue("?tlno", p.Tlno);
                    comando.Parameters.AddWithValue("?email", p.Email);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que elimina el proveedor pasado como parametro en la base de datos
        /// </summary>
        /// <param name="p">Proveedor a eliminar</param>
        /// <returns>True si se ha eliminado correctamente,o false en caso contrario</returns>
        public Boolean BorrarProveedor(Proveedor p)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "DELETE FROM proveedores WHERE id=?id";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", p.Id);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }
        #endregion

        #region Almacenes
        /// <summary>
        /// Método que obtiene los datos todos los almacenes de la base de datos y los almacena en ArrayAlmacenes
        /// </summary>
        public void LeerAlmacenes()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT * FROM almacenes ORDER BY id";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTAble
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    ArrayAlmacenes = new Almacen[intEffected];
                else
                    ArrayAlmacenes = new Almacen[0];
                // Contadores de cada fila
                int i = 0;
                string _row;
                Almacen myData;
                // Por cada fila creo un nuevo almacenes y lo almaceno en el ArrayAlmacenes
                foreach (DataRow row in dt.Rows)
                {
                    string id = Convert.ToString(row["id"]);
                    string nombre = Convert.ToString(row["nombre"]);
                    string direccion = Convert.ToString(row["direccion"]);
                    _row = id + ";" + nombre + ";" + direccion;
                    Console.WriteLine(_row);
                    myData = new Almacen(id, nombre, direccion);
                    ArrayAlmacenes[i] = myData;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        /// Método que obtiene los id de todos los almacenes de la base de datos y los almacena en el array almacenes
        /// </summary>
        public void ObtenerAlmacenesId()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT c.id FROM almacenes c;";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTable
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    almacenes = new String[intEffected];
                else
                    almacenes = new String[0];
                int i = 0; //Numero de fila
                //Por cada fila obtengo los id y los almaceno en el array almacenes
                foreach (DataRow row in dt.Rows)
                {
                    almacenes[i] = Convert.ToString(row["id"]);
                    i++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        /// Método que añade el almacén pasado como parametro a la base de datos
        /// </summary>
        /// <param name="a">Almacén a añadir</param>
        /// <returns>True si se ha añadido correctamente,o false en caso contrario</returns>
        public Boolean InsertarAlmacen(Almacen a)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "INSERT INTO almacenes VALUES(?id,?nombre,?direccion)";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", a.Id);
                    comando.Parameters.AddWithValue("?nombre", a.Nombre);
                    comando.Parameters.AddWithValue("?direccion", a.Direccion);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que modifica el almacén pasado como parametro en la base de datos
        /// </summary>
        /// <param name="a">Almacén a modificar</param>
        /// <returns>True si se ha modificado correctamente,o false en caso contrario</returns>
        public Boolean ModificarAlmacen(Almacen a)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "UPDATE almacenes SET nombre=?nombre, direccion=?direccion " +
                        " WHERE id=?id";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", a.Id);
                    comando.Parameters.AddWithValue("?nombre", a.Nombre);
                    comando.Parameters.AddWithValue("?direccion", a.Direccion);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que elimina el almacén pasado como parametro en la base de datos siempre que el almacén no contenga productos
        /// </summary>
        /// <param name="a">Almacén a eliminar</param>
        /// <returns>True si se ha eliminado correctamente,o false en caso contrario</returns>
        public Boolean BorrarAlmacen(Almacen a)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Seleccionamos los productos situados en el almacén pasado
                    query = "SELECT*FROM Productos WHERE almacen=?id";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", a.Id);

                    // Ejecuto la query y recojo el resultado en un adapter
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                    // Creo un nuevo DataTAble
                    dt = new DataTable();
                    // Asigno al datatable el valor devuelto por el adapter 
                    adapter.Fill(dt);
                    // Obtengo el numero de filas devueltas por la query
                    intEffected = dt.Rows.Count;

                    // Si no devuelve ninguna fila borramos el almacén
                    if (intEffected == 0)
                    {
                        query = "DELETE FROM almacenes WHERE id=?id";
                        // Ejecuto la query pasandole los parametros
                        MySqlCommand comando2 = new MySqlCommand(query, connection);
                        comando2.Parameters.AddWithValue("?id", a.Id);
                        comando2.ExecuteNonQuery();
                        exito = true;
                    }
                    else // Si devuelve filas mostramos un mensaje de error
                    {
                        string cabeceraInfo = (string)Application.Current.FindResource("error");
                        string mensajeBox = (string)Application.Current.FindResource("eliminarAlmacen");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        #endregion

        #region Categorias
        /// <summary>
        /// Método que obtiene los datos todos las categorías de la base de datos y las almacena en ArrayCategorias
        /// </summary>
        public void LeerCategorias()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT * FROM categorias ORDER BY id";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTAble
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    ArrayCategorias = new Categoria[intEffected];
                else
                    ArrayCategorias = new Categoria[0];
                // Contadores de cada fila
                int i = 0;
                string _row;
                Categoria myData;
                // Por cada fila devuelta se crea una categorias y se almacena en el ArrayCategorias
                foreach (DataRow row in dt.Rows)
                {
                    string id = Convert.ToString(row["id"]);
                    string nombre = Convert.ToString(row["nombre"]);
                    string descripcion = Convert.ToString(row["descripcion"]);
                    _row = id + ";" + nombre + ";" + descripcion;
                    Console.WriteLine(_row);
                    myData = new Categoria(id, nombre, descripcion);
                    ArrayCategorias[i] = myData;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        ///  Método que obtiene los id de todos las categorias de la base de datos y las almacena en el array categorias
        /// </summary>
        public void ObtenerCategoriasId()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT c.id FROM categorias c;";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTable
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    categorias = new String[intEffected];
                else
                    categorias = new String[0];
                int i = 0; //Numero de fila
                //Por cada fila obtengo los id de las categorias y se almacenan en el array categorias
                foreach (DataRow row in dt.Rows)
                {
                    categorias[i] = Convert.ToString(row["id"]);
                    i++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        /// Método que añade la categoría pasada como parametro a la base de datos
        /// </summary>
        /// <param name="c">Categoría a añadir</param>
        /// <returns>True si se ha añadido correctamente,o false en caso contrario</returns>
        public Boolean InsertarCategoria(Categoria c)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "INSERT INTO categorias VALUES(?id,?nombre,?descripcion)";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?nombre", c.Nombre);
                    comando.Parameters.AddWithValue("?descripcion", c.Descripcion);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que modifica la categoría pasada como parametro en la base de datos
        /// </summary>
        /// <param name="c">Categoría a modificar</param>
        /// <returns>True si se ha modificado correctamente,o false en caso contrario</returns>
        public Boolean ModificarCategoria(Categoria c)
        {
            Boolean exito = false;
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "UPDATE categorias SET nombre=?nombre, descripcion=?d " +
                        " WHERE id=?id";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.Parameters.AddWithValue("?nombre", c.Nombre);
                    comando.Parameters.AddWithValue("?d", c.Descripcion);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que elimina la categoría pasada como parametro en la base de datos
        /// </summary>
        /// <param name="c">Categoría a eliminar</param>
        /// <returns>True si se ha eliminado correctamente,o false en caso contrario</returns>
        public Boolean BorrarCategoria(Categoria c)
        {
            Boolean exito = false;
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "DELETE FROM categorias WHERE id=?id";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?id", c.Id);
                    comando.ExecuteNonQuery();

                    // Una vez eliminada la categoria actualizamos los productos que tenian la categoria con el valor '-'
                    query = "UPDATE Productos SET categoria='-' WHERE categoria=?c";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando2 = new MySqlCommand(query, connection);
                    comando2.Parameters.AddWithValue("?c", c.Id);
                    comando2.ExecuteNonQuery();

                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }
        #endregion

        #region Usuarios

        /// <summary>
        /// Método que comprueba si existe un usuario en la tabla Usuarios
        /// </summary>
        /// <param name="name">Nombre de usuario</param>
        /// <param name="pwd">Contraseña de usuario</param>
        /// <returns>True si existe el usuario,o false en caso contrario</returns>
        public Boolean Logear(String name, String pwd)
        {
            bool existe = false;
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Seleccionamos el usuario con el nombre y contraseña pasados
                    query = "SELECT * FROM usuarios WHERE user=?u and password=?p";
                    // Ejecuto la query pasandole los parametos
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?u", name);
                    comando.Parameters.AddWithValue("?p", pwd);
                    // Ejecuto la query y recojo el resultado en un adapter
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                    // Creo un nuevo DataTAble
                    dt = new DataTable();
                    // Asigno al datatable el valor devuelto por el adapter 
                    adapter.Fill(dt);
                    // Obtengo el numero de filas devueltas por la query
                    intEffected = dt.Rows.Count;

                    // Si devuelve una resultado se crear el usuario como instancia de la clase Usuario
                    if (intEffected == 1)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Boolean importar = Convert.ToBoolean(row["importar"]);
                            Boolean exportar = Convert.ToBoolean(row["exportar"]);
                            Boolean productos = Convert.ToBoolean(row["productos"]);
                            Boolean categorias = Convert.ToBoolean(row["categorias"]);
                            Boolean proveedores = Convert.ToBoolean(row["proveedores"]);
                            Boolean almacenes = Convert.ToBoolean(row["almacenes"]);
                            Boolean usuarios = Convert.ToBoolean(row["usuarios"]);
                            Boolean transacciones = Convert.ToBoolean(row["transacciones"]);
                            // Comprobar si existe la imagen del producto
                            byte[] imagen = null;
                            if (Convert.IsDBNull(row["imagen"])) // Si no existe toma el valor null
                            {
                                imagen = null;
                            }
                            else
                            {
                                imagen = (byte[])(row["imagen"]); ;// Si existe te transforma a un array de bytes
                            }

                            usuario = new Usuario(name, pwd, importar, exportar, productos, categorias, proveedores,
                        almacenes, usuarios, transacciones, imagen);
                            Usuario.InstanciaUser = usuario;
                            existe = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(ex.Message);
                }

            }

            return existe;
        }

        /// <summary>
        /// Método que obtiene los datos todos los usuarios de la base de datos y los almacena en ArrayUsuarios
        /// </summary>
        public void LeerUsuarios()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT * FROM usuarios ORDER BY user";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTAble
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    ArrayUsuarios = new Usuario[intEffected];
                else
                    ArrayUsuarios = new Usuario[0];
                // Contadores de cada fila
                int i = 0;
                string _row;
                Usuario myData;
                // Por cada fila se crea un usuario y se almacena en ArrayUsuarios
                foreach (DataRow row in dt.Rows)
                {
                    string u = Convert.ToString(row["user"]);
                    string pwd = Convert.ToString(row["password"]);
                    Boolean importar = Convert.ToBoolean(row["importar"]);
                    Boolean exportar = Convert.ToBoolean(row["exportar"]);
                    Boolean productos = Convert.ToBoolean(row["productos"]);
                    Boolean categorias = Convert.ToBoolean(row["categorias"]);
                    Boolean proveedores = Convert.ToBoolean(row["proveedores"]);
                    Boolean almacenes = Convert.ToBoolean(row["almacenes"]);
                    Boolean usuarios = Convert.ToBoolean(row["usuarios"]);
                    Boolean transacciones = Convert.ToBoolean(row["transacciones"]);
                    byte[] imagen = null;
                    // Comprobar si existe la imagen del producto
                    if (Convert.IsDBNull(row["imagen"])) // Si no existe toma el valor null
                    {
                        imagen = null;
                    }
                    else
                    {
                        imagen = (byte[])(row["imagen"]);// Si existe te transforma a un array de bytes
                    }

                    _row = u + ";" + pwd + ";" + importar + ";" + exportar + ";" + productos + ";" + categorias
                        + ";" + proveedores + ";" + almacenes + ";" + usuarios + ";" + transacciones + ";" + imagen + ";";
                    Console.WriteLine(_row);
                    myData = new Usuario(u, pwd, importar, exportar, productos, categorias, proveedores,
                        almacenes, usuarios, transacciones, imagen);
                    ArrayUsuarios[i] = myData;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }
        }

        /// <summary>
        /// Método que añade el usaurio pasado como parametro a la base de datos
        /// </summary>
        /// <param name="u">Usuario a añadir</param>
        /// <returns>True si se ha añadido correctamente,o false en caso contrario</returns>
        public Boolean InsertarUsuario(Usuario u)
        {
            Boolean exito = false;
            //Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "INSERT INTO usuarios VALUES(?user,?pwd,?im,?i,?e,?p,?c,?pr,?a,?u,?t)";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?user", u.User);
                    comando.Parameters.AddWithValue("?pwd", u.Pwd);
                    comando.Parameters.AddWithValue("?i", u.Importar);
                    comando.Parameters.AddWithValue("?im", u.Imagen);
                    comando.Parameters.AddWithValue("?e", u.Exportar);
                    comando.Parameters.AddWithValue("?p", u.Productos);
                    comando.Parameters.AddWithValue("?c", u.Categorias);
                    comando.Parameters.AddWithValue("?pr", u.Proveedores);
                    comando.Parameters.AddWithValue("?a", u.Almacenes);
                    comando.Parameters.AddWithValue("?u", u.Usuarios);
                    comando.Parameters.AddWithValue("?t", u.Transacciones);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que modifica el usaurio pasado como parametro en la base de datos
        /// </summary>
        /// <param name="u">Usuario a modificar</param>
        /// <returns>True si se ha modificado correctamente,o false en caso contrario</returns>
        public Boolean ModificarUsuario(Usuario u)
        {
            Boolean exito = false;
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "UPDATE usuarios SET password=?pwd, importar=?i, exportar=?e, productos=?p," +
                        "categorias=?c,proveedores=?pr,almacenes=?a,usuarios=?u , imagen=?im" +
                        " WHERE user=?user";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?user", u.User);
                    comando.Parameters.AddWithValue("?pwd", u.Pwd);
                    comando.Parameters.AddWithValue("?im", u.Imagen);
                    comando.Parameters.AddWithValue("?i", u.Importar);
                    comando.Parameters.AddWithValue("?e", u.Exportar);
                    comando.Parameters.AddWithValue("?p", u.Productos);
                    comando.Parameters.AddWithValue("?c", u.Categorias);
                    comando.Parameters.AddWithValue("?pr", u.Proveedores);
                    comando.Parameters.AddWithValue("?a", u.Almacenes);
                    comando.Parameters.AddWithValue("?u", u.Usuarios);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que elimina el usaurio pasado como parametro en la base de datos
        /// </summary>
        /// <param name="u">Usuario a eliminar</param>
        /// <returns>True si se ha eliminado correctamente,o false en caso contrario</returns>
        public Boolean BorrarUsuario(Usuario u)
        {
            Boolean exito = false;
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "DELETE FROM usuarios WHERE user=?u";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?u", u.User);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que cambia la contraseña de un usuario existente
        /// </summary>
        /// <param name="name">Nombre de usuario</param>
        /// <param name="pwd">Contraseña de usuario</param>
        /// <returns>True si se cambia correctamente la contraseña,o false en caso contrario</returns>
        public Boolean CambiarConstraseña(String name, String pwd)
        {
            Boolean exito = false;
            // cargar daroa iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "UPDATE usuarios SET password=?pwd WHERE user=?user";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?user", name);
                    comando.Parameters.AddWithValue("?pwd", pwd);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que obtiene los nombre de todos los usuarios de la base de datos y los almacena en el array de usaurios
        /// </summary>
        /// <returns>Array con los nombre de usuarios</returns>
        public String[] ObtenerUsername()
        {
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query 
                    query = "SELECT u.user FROM usuarios u;";
                    // Ejecuto la query y recojo el resultado en un adapter
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    // Creo un nuevo DataTable
                    dt = new DataTable();
                    // Asigno al datatable el valor devuelto por el adapter 
                    adapter.Fill(dt);
                    // Obtengo el numero de filas devueltas por la query
                    intEffected = dt.Rows.Count;
                    // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                    if (intEffected > 0)
                        usuarios = new String[intEffected];
                    else
                        usuarios = new String[0];
                    int i = 0; //Numero de fila
                    //Por cada fila obtengo los datos y los almaceno en el array usuarios
                    foreach (DataRow row in dt.Rows)
                    {
                        usuarios[i] = Convert.ToString(row["user"]);
                        i++;
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(ex.Message);
                }
            }
            CloseConnection();
            //Devuelve el array con los nombres de usuario
            return usuarios;
        }

        #endregion

        #region Transacciones

        /// <summary>
        /// Método que obtiene los datos todos las transacciones de la base de datos y las almacena en ArrayTransacciones
        /// </summary>
        public void LeerTransacciones()
        {
            try
            {
                // Introduzco el texto de la query 
                query = "SELECT * FROM transacciones ORDER BY fecha_transaccion desc";
                // Ejecuto la query y recojo el resultado en un adapter
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                // Creo un nuevo DataTAble
                dt = new DataTable();
                // Asigno al datatable el valor devuelto por el adapter 
                adapter.Fill(dt);
                // Obtengo el numero de filas devueltas por la query
                intEffected = dt.Rows.Count;
                // Si es mayor de cero, creo un array con las filas devueltas, sino un array de 0
                if (intEffected > 0)
                    ArrayTransacciones = new Transaccion[intEffected];
                else
                    ArrayTransacciones = new Transaccion[0];
                // Contadores de cada fila
                int i = 0;
                string _row;
                Transaccion myData;
                // Por cada fila se crea una nueva Transaccion y se almacena en ArrayTransacciones
                foreach (DataRow row in dt.Rows)
                {
                    DateTime fecha = Convert.ToDateTime(row["fecha_transaccion"]);
                    string producto = Convert.ToString(row["producto"]);
                    string almacenV = Convert.ToString(row["almacenViejo"]);
                    string almacenN = Convert.ToString(row["almacenNuevo"]);
                    int unidades = Convert.ToInt32(row["unidades"]);


                    _row = fecha + ";" + producto + ";" + almacenV + ";" + almacenN + ";" + unidades;
                    Console.WriteLine(_row);
                    myData = new Transaccion(fecha, producto, almacenV, almacenN, unidades);
                    ArrayTransacciones[i] = myData;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                FicheroLog.Instance.EscribirFichero(ex.Message);
            }


        }

        /// <summary>
        /// Método que elimina la transaccion pasada como parametro en la base de datos
        /// </summary>
        /// <param name="t">Transaccion a eliminar</param>
        /// <returns>True si se ha eliminado correctamente,o false en caso contrario</returns>
        public Boolean BorrarTransaccion(Transaccion t)
        {
            Boolean exito = false;
            // Cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "DELETE FROM Transacciones WHERE fecha_transaccion=?f ";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?f", t.Fecha);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        /// <summary>
        /// Método que añade la transaccion pasada como parametro a la base de datos
        /// </summary>
        /// <param name="t">Transaccion a añadir</param>
        /// <returns>True si se ha añadido correctamente,o false en caso contrario</returns>
        public Boolean InsertarTransaccion(Transaccion t)
        {
            Boolean exito = false;
            // cargar datos iniciales
            InitParamsDB();
            if (OpenConnection())
            {
                try
                {
                    // Introduzco el texto de la query
                    query = "INSERT INTO transacciones(fecha_transaccion,producto,almacenViejo,almacenNuevo,unidades) " +
                        " VALUES(?fecha,?prod,?aV,?aN,?s)";
                    // Ejecuto la query pasandole los parametros
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("?fecha", t.Fecha);
                    comando.Parameters.AddWithValue("?prod", t.Producto);
                    comando.Parameters.AddWithValue("?aV", t.AlmacenV);
                    comando.Parameters.AddWithValue("?aN", t.AlmacenN);
                    comando.Parameters.AddWithValue("?s", t.Unidades);
                    comando.ExecuteNonQuery();
                    exito = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    FicheroLog.Instance.EscribirFichero(e.Message);
                }
            }

            CloseConnection();

            return exito;
        }

        #endregion

    }

}