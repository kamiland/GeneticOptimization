﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{
    class Controller
    {
        double Kp, Ti, Td;
        double controllerOutput = 0;

        public double CalculateOutput(double error, double dt)
        {

            return controllerOutput;
        }
    }
}