using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinal_LGE.View
{
    /// <summary>
    /// Lógica de interacción para Ayuda.xaml
    /// </summary>
    public partial class Ayuda : Page
    {
        /// <summary>
        /// Inicializa una nueva instancia de Ayuda.xaml
        /// </summary>
        public Ayuda()
        {
            InitializeComponent();
        }

        private void Click1(object sender, RoutedEventArgs e)
        {
            Animacion(gestion);
        }

        private void Click2(object sender, RoutedEventArgs e)
        {
            Animacion(usuarios);
        }

        private void Animacion(TextBlock elemento)
        {
            elemento.Visibility = Visibility.Visible;

            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 40);

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1.0;
            animation.To = 0.0;
            animation.Duration = new Duration(duration);
            
            Storyboard.SetTargetName(animation, elemento.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(OpacityProperty));
            // Add the animation to the storyboard
            storyboard.Children.Add(animation);

            // Begin the storyboard
            storyboard.Begin(this);
        }
    }
}
