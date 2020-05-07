using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProyectoFinal_LGE.Data
{
    /// <summary>
    /// Clase que crea y escribe ficheros log
    /// </summary>
    public sealed class FicheroLog
    {
        // Variable estatica de la clase      
        private readonly static FicheroLog instancia = new FicheroLog();

        // Constructor privado        
        private FicheroLog() { }

        /// <summary>
        /// Variable publica de la instancia de la clase
        /// </summary>
        public static FicheroLog Instance
        {
            get
            {
                return instancia;
            }
        }

        /// <summary>
        ///  Método que escribe en un fichero log del dia actual el mensaje pasado
        /// </summary>
        /// <param name="mensaje">Mensaje a escribir en el fichero</param>
        public void EscribirFichero(String mensaje)
        {
            // Obtenemos la ruta donde guardar los ficheros -> C:\ProgramData
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // Creamos el directorio \Karma\Logs si no existe
            string target = @"\Karma\Logs";
            if (!Directory.Exists(path+ @"\Karma"))
            {
                Directory.CreateDirectory(path + @"\Karma");
            }
            if (!Directory.Exists(path + target))
            {
                Directory.CreateDirectory(path + target);
            }

            // Creamos ficheros log con el siguiente formato 'logFecha'
            String fecha = DateTime.Now.ToString("dd -MM-yyyy");
            using (StreamWriter file = new StreamWriter(path+@"\Karma\Logs\log" + fecha + ".txt", true))
            {
                // Escribimos el fichero log del dia actual el mensaje y su hora exacta 
                file.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")+" -> "+mensaje);
                file.Close();
            }
        }
    }
}
