using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{   
   /**
    *   Klasa Symulator wykorzystuje algorytm Rungego-Kutty do wygenerowania przebiegu obiektu
    *	przechowuje ona również cały przebieg wraz z parametrami symulacji np. takimi jak krok solvera.
    *
    * \version Wersja Alfa/Beta - w trakcie testowania oprogramowania.
    */
    public class Simulator
    {
        public Controller PID = new Controller(5, 0.2, 0.001);
        public Solver RK4 = new Solver();
        double setpoint = 100;
        public double fitness = 0;
        double error_int = 0;

        /**
         * \brief Pobiera parametry silnika DC od użytkownika.
         *
         * Pobiera trzy wartości typu double.
         *
         * \param[in] objectParameters tablica parametrow silnika podana przez użytkownika lub odczytana z obiektu.
         * 
         * 
         * 
         *
         * \attention wartości zmiennych wyskalowane są w podstawowych jednostkach układu SI
         *            
         */ 

        public void GetUserParameters(double[] objectParameters)
        {
            if(0 != objectParameters[0])
                RK4.Ra = objectParameters[0];
            if (0 != objectParameters[1])
                RK4.La = objectParameters[1];
            if (0 != objectParameters[2])
                RK4.Rf = objectParameters[2];
            if (0 != objectParameters[3])
                RK4.Lf = objectParameters[3];
            if (0 != objectParameters[4])
                RK4.J = objectParameters[4];
            if (0 != objectParameters[5])
                RK4.B = objectParameters[5];
            if (0 != objectParameters[6])
                RK4.p = objectParameters[6];
            if (0 != objectParameters[7])
                RK4.Laf = objectParameters[7];
            if (0 != objectParameters[8])
                setpoint = objectParameters[8];
        }

         /**
         * \brief 
         *
         * 
         *
         * \param[in] numberOfProbes ilość próbek danych wykorzystana do symulacji.
         * \param[in] timeStep krok wykonywania symulacji.
         * 
         * \return wyznaczona na podstawie modelu, wartość prądu wirnika silnika DC

         * \attention wartości zmiennych wyskalowane są w podstawowych jednostkach układu SI
         *            
         */

        public double[] Simulate(long numberOfProbes, double timeStep = 0.001, bool saveToFile = true)
        {
            System.IO.TextWriter current = null;
            System.IO.TextWriter angular = null;
            if (saveToFile)
            {
                current = new System.IO.StreamWriter("current.txt");
                angular = new System.IO.StreamWriter("angular.txt");
            }
                
   
            /** initial state   */
            RK4.x[0] = 0;
            RK4.x[1] = 0;

         /**
         *\fn
         *
         * \brief Kalkuluje wartość prądu i prędkości silnika DC.
         *  Wykonaj liczbe kroków określoną w numberOfProbes, zapisz pomiary do plików, oblicz całkę uchybu 
         *
         */
             wykonaj liczbe kroków określoną w numberOfProbes, zapisz pomiary do plików, oblicz całkę uchybu 
            for (int i = 0; i < numberOfProbes; i++)
            {
                RK4.x = RK4.CalculateNextStep(PID.CalculateOutput(setpoint, RK4.x[1]), timeStep);
                if(saveToFile)
                {
                    current.WriteLine(RK4.x[0]);
                    angular.WriteLine(RK4.x[1]);
                }
                error_int += (Math.Abs(setpoint - RK4.x[1])) * timeStep;
            }
            if (saveToFile)
            {
                current.Close();
                angular.Close();
            }

            fitness = 1.0 / (error_int + 1.0);
            return RK4.x;
        }
    }
}
