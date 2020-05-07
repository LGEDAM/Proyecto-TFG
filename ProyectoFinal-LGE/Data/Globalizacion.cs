using ProyectoFinal_LGE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = System.Windows.Application;

namespace ProyectoFinal_LGE.Data
{
    /// <summary>
    /// Clase que cambia la localizacion de la aplicacion
    /// </summary>
    public sealed class Globalizacion
    {
        // Variable estatica de la clase         
        private readonly static Globalizacion instancia = new Globalizacion();

        // Constructor privado        
        private Globalizacion() { }

        /// <summary>
        /// Variable publica de la instancia de la clase
        /// </summary>
        public static Globalizacion Instance
        {
            get
            {
                return instancia;
            }
        }

        /// <summary>
        /// Método que cambia el diccionario de recursos que esta utilizando la aplicacion segun el idioma pasado
        /// </summary>
        /// <param name="idioma">Cadena que identifica el idioma</param>
        public void CambiarIdioma(String idioma)
        {
            // Creamos un nuevo diccionario de recursos 
            var dict = new ResourceDictionary();
            // Le pasamos la ruta correspondiente al diccionario del idioma pasado
            dict.Source = new Uri("..\\Resources\\Dictionary." + idioma + ".xaml", UriKind.Relative);

            // Añadimos el diccionario de recursos a los recursos de la aplicacion
            Application.Current.Resources.MergedDictionaries.Add(dict);

            // Cambiamos la cultura de la aplicacion al idioma pasado
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(idioma);

        }
    }
}
