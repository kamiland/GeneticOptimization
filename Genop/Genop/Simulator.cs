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

        public double[] Simulate()
        {
            
            RK4.x =  RK4.CalculateNextStep(220);

            return RK4.x;
        }

    }
}
