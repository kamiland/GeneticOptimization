using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{   
    /*
    *   Klasa Symulator wykorzystuje algorytm Rungego-Kutty do wygenerowania przebiegu obiektu
    */
    class Simulator
    {
        Controller PID = new Controller();
        public Solver RK4 = new Solver();

        public void GetUserParameters(double[] objectParameters)
        {
            // ugly but works - to change
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
        }
        public double[] Simulate(int numberOfProbes, double timeStep = 0.001)
        {
            System.IO.TextWriter current = new System.IO.StreamWriter("current.txt");
            System.IO.TextWriter angular = new System.IO.StreamWriter("angular.txt");
            // initial state
            RK4.x[0] = 0;
            RK4.x[1] = 0;
            for (int i = 0; i < numberOfProbes; i++)
            {
                RK4.x = RK4.CalculateNextStep(220, timeStep);
                current.WriteLine(RK4.x[0]);
                angular.WriteLine(RK4.x[1]);
            }
            current.Close();
            angular.Close();

            return RK4.x;
        }

    }
}
