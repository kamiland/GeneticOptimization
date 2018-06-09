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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Genop
{

    /*
    * Logika interakcji dla klasy Window1.xaml
    * Wyświetlanie wykresów używając biblioteki OxyPlot
    */

    using System;
    using OxyPlot;
    using OxyPlot.Series;

    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
    }

    /*
     * Klasa uruchamiająca wizualizujące danych na wykresach
     */
    public class MainViewModel
    {
        public MainViewModel()
        {
            // Stworz wykres
            var tmp = new PlotModel { Title = "Wykres wartości", Subtitle = "prądu i prędkości silnika" };

            // Stworz dwie serie danych
            var series1 = new LineSeries { Title = "Prąd", MarkerType = MarkerType.None };
            var series2 = new LineSeries { Title = "Prędkość", MarkerType = MarkerType.None };

            // Otworz pliki z danymi
            System.IO.TextReader current = new System.IO.StreamReader("current.txt");
            System.IO.TextReader angular = new System.IO.StreamReader("angular.txt");

            // Zmienne pomocnicze xout - wartosc, sout - string z danymi
            double x1out = 0;
            String s1out;
            double x2out = 0;
            String s2out;

            // zapisz wartości w pętli
            for (int i = 0; i < 1000; i++)
            {
                s1out = current.ReadLine();
                Double.TryParse(s1out, out x1out);
                series1.Points.Add(new DataPoint(i, x1out));
            }

            for (int i = 0; i < 1000; i++)
            {
                s2out = angular.ReadLine();
                Double.TryParse(s2out, out x2out);
                series2.Points.Add(new DataPoint(i, x2out));
            }

            // Dodaj wartości do wykresu
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);
            this.Model = tmp;
        }
        
        // Rysuj
        public PlotModel Model { get; private set; }
    }
}
