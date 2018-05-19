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
        PLC SiemensS7 = new PLC();
        double[] objectParameters = { 0, 0, 0 };

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

        private void AutoIdentification_Click(object sender, RoutedEventArgs e)
        {
            objectParameters = SiemensS7.Auto_Identyfication();

            CurrentParam1.Text = objectParameters[0].ToString();
            CurrentParam2.Text = objectParameters[1].ToString();
            CurrentParam3.Text = objectParameters[2].ToString();

            MessageBox.Show("Identification done.");
        }

        private void BtnUserParamOK_Click(object sender, RoutedEventArgs e)
        {
            objectParameters[0] = Convert.ToDouble(UserParam1.Text);
            objectParameters[1] = Convert.ToDouble(UserParam2.Text);
            objectParameters[2] = Convert.ToDouble(UserParam3.Text);

            CurrentParam1.Text = UserParam1.Text;
            CurrentParam2.Text = UserParam2.Text;
            CurrentParam3.Text = UserParam3.Text;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            (this).Close();
        }
    }
}
