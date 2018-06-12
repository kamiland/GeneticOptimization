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

    /**
    * \brief Window1.xaml - Okno wyświetlające przebiegi
    *
    * Logika interakcji dla klasy Window1.xaml
    * Wyświetlanie wykresów przy wykorzystaniu biblioteki OxyPlot
    *
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
    
    /**
     * \brief Klasa MainViewModel
     *
     * Klasa uruchamiająca symulacje sterowania silnikiem DC
     * oraz wizualizująca dane parametrów symulacji na wykresach.
     *
     *\version wersja alfa/beta - w trakcie testowania
     *
     */

    public class MainViewModel
    {
        public MainViewModel()
        {
            /**  Stworz wykres */
            var tmp = new PlotModel { Title = "Wykres wartości", Subtitle = "prądu i prędkości silnika" };

            /** Stworz dwie serie danych */
            var series1 = new LineSeries { Title = "Prąd", MarkerType = MarkerType.None }; /**< Seria danych wartości prądu silnika */
            var series2 = new LineSeries { Title = "Prędkość", MarkerType = MarkerType.None };/**< Seria danych wartości prędkości silnika */

            /** Otworz pliki z danymi   */
            System.IO.TextReader current = new System.IO.StreamReader("current.txt");  /**< Strumien otwarcia pliku z danymi prądu silnika */
            System.IO.TextReader angular = new System.IO.StreamReader("angular.txt");  /**< Strumień otwarcia pliku z danymi prędkości silnika*/

            /** Zmienne pomocnicze xout - wartosc, sout - string z danymi   */
            double x1out = 0;  /**< Zmienna pomocnicza double do danych prądu */
            String s1out;       /**< Zmienna pomocnicza string do danych prądu */
            double x2out = 0;  /**< Zmienna pomocnicza double do danych prędkości */
            String s2out;      /**< Zmienna pomocnicza string do danych prędkości */

            

            /**
            *\fn
            *
            * \brief Pętla "for" do zapisywania danych.
            *
            * Zapisywanie wartości danych prądu w pętli iteracyjnej
            *
            * 
            */
            for (int i = 0; i < 1000; i++)
            {
                s1out = current.ReadLine();
                Double.TryParse(s1out, out x1out);
                series1.Points.Add(new DataPoint(i, x1out));
            }

            /**
            *
            *\fn
            * \brief Pętla "for" do zapisywania danych.
            *
            * Zapisywanie wartości danych prędkości w pętli iteracyjnej
            *
            * 
            */

            for (int i = 0; i < 1000; i++)
            {
                s2out = angular.ReadLine();
                Double.TryParse(s2out, out x2out);
                series2.Points.Add(new DataPoint(i, x2out));
            }

            /**
            *
            * Dodanie wartości danych prędkości i prądu do generowanego wykresu.
            *
            * 
            */
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);
            this.Model = tmp;
        }
        
        // Rysuj
        public PlotModel Model { get; private set; }
    }
}
