using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Genop
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    
        
    public partial class MainWindow : Window
    {
        PLC siemensS7 = new PLC();
        Simulator simulatorDC = new Simulator();
        double[] objectParameters = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        long numberOfProbes = 1000;
        double timeStep = 0.001;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Genop";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Title = e.GetPosition(this).ToString();
        }

        // po kliknięciu [auto identif.] losuje ("identyfikuje") parametry i wpisuje je do ramek w okienku
        private void AutoIdentification_Click(object sender, RoutedEventArgs e)
        {
            objectParameters = siemensS7.AutoIdentyfication();

            Current_Ra.Text = objectParameters[0].ToString();
            Current_La.Text = objectParameters[1].ToString();
            Current_Rf.Text = objectParameters[2].ToString();
            Current_Lf.Text = objectParameters[3].ToString();
            Current_J.Text = objectParameters[4].ToString();
            Current_B.Text = objectParameters[5].ToString();
            Current_p.Text = objectParameters[6].ToString();
            Current_Laf.Text = objectParameters[7].ToString();

            MessageBox.Show("Identification done.");
        }

        // po kliknięciu  [OK] pobiera dane z ramek usera i przypisuje do obiektu oraz do ramek wyświetlania
        private void BtnUserParamOK_Click(object sender, RoutedEventArgs e)
        {
            objectParameters[0] = Convert.ToDouble(User_Ra.Text);
            objectParameters[1] = Convert.ToDouble(User_La.Text);
            objectParameters[2] = Convert.ToDouble(User_Rf.Text);
            objectParameters[3] = Convert.ToDouble(User_Lf.Text);
            objectParameters[4] = Convert.ToDouble(User_J.Text);
            objectParameters[5] = Convert.ToDouble(User_B.Text);
            objectParameters[6] = Convert.ToDouble(User_p.Text);
            objectParameters[7] = Convert.ToDouble(User_Laf.Text);
            objectParameters[8] = Convert.ToDouble(User_Setpoint.Text);

            simulatorDC.GetUserParameters(objectParameters);

            Current_Ra.Text = Convert.ToString(simulatorDC.RK4.Ra);
            Current_La.Text = Convert.ToString(simulatorDC.RK4.La);
            Current_Rf.Text = Convert.ToString(simulatorDC.RK4.Rf);
            Current_Lf.Text = Convert.ToString(simulatorDC.RK4.Lf);
            Current_J.Text = Convert.ToString(simulatorDC.RK4.J);
            Current_B.Text = Convert.ToString(simulatorDC.RK4.B);
            Current_p.Text = Convert.ToString(simulatorDC.RK4.p);
            Current_Laf.Text = Convert.ToString(simulatorDC.RK4.Laf);
        }

        // wywołuje symulację i otwiera nowe okno z wykresem
        private void BtnSimulate_Click(object sender, RoutedEventArgs e)
        {
            //simulatorDC.Simulate(numberOfProbes, timeStep);

            //Window1 graphWindow = new Window1();
            //graphWindow.Show();
            double[] PID;
            GeneticAlgorithm Optimization = new GeneticAlgorithm(numberOfProbes, 100, objectParameters);
            for (int i = 0; i < 30; i++)
                Optimization.do_one_generation();
            PID = Optimization.show_best();
            Kp.Text = Convert.ToString(Math.Round(PID[0], 3));
            Ki.Text = Convert.ToString(Math.Round(PID[1], 3));
            Kd.Text = Convert.ToString(Math.Round(PID[2], 3));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            (this).Close();
        }
    }
}
