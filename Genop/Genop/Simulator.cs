using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{
    class Simulator
    {
        Controller PID = new Controller();
        Solver RK4 = new Solver();

        public double[] Simulate(int numberOfProbes, double timeStep = 0.001)
        {
            System.IO.TextWriter current = new System.IO.StreamWriter("current.txt");
            System.IO.TextWriter angular = new System.IO.StreamWriter("angular.txt");

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
