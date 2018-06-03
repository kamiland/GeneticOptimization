using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Mathematical model of DC motor:
 * x1 - rotor current, x2 - angular velocity
 * x1' = -(Ra / La) * x1 - (Gaf / La) * x2 + (1 / La) * U;
 * x2' =  (Gaf / J) * x1 - (B / J) * x2 + (1 / J) * Tl;
*/

namespace Genop
{
    class Solver
    {
        // U - supply voltage, E - electromotive force
        double U, E;
        // La - rotor inductance, Ra - rotor resistance, Ua - rotor voltage
        public double La, Ra, Ua;
        // Lf - excitation induction, Rf - excitation resistance, If - excitation current 
        public double Lf, Rf, If, Ufn;
        // Ltf - mutual inductance
        public double Laf;
        //  T - drive torque, B - damping constant, p - number of pole pairs, Tl - moment load, J - moment of inertia
        public double T, B, p, Tl, J;
        // fixed excitation current ifn = Ufn / Rf
        double ifn;
        // angular velocity, rotor current
        double angularVelocity, rotorCurrent;

        double Gaf;

        public double[] x = new double[2];

        public Solver()
        {
            // default parameters
            Ra = 0.4;   La = 0.02;
            Rf = 65;    Lf = 65;
            J = 0.11; B = 0.0053;
            p = 2;
            Laf = 0.363;
            Ufn = 110;

            // initial object values
            rotorCurrent = 0; angularVelocity = 0; 
            x[0] = rotorCurrent;
            x[1] = angularVelocity;

            ifn = Ufn / Rf;
            Gaf = p * Laf * ifn;
            E = Gaf * angularVelocity;
            T = Gaf * rotorCurrent;
        }

        public double calculateRotorCurrent(double x1, double x2, double U)
        {
            return -(Ra / La) * x1 - (Gaf / La) * x2 + (1 / La) * U;
        }

        public double calculateAngularVelocity(double x1, double x2, double Tl = 0)
        {
            return (Gaf / J) * x1 - (B / J) * x2 + (1 / J) * Tl;
        }

        public double[] CalculateNextStep(double U, double h = 0.001)
        {
            double[,] k = new double[2, 4];

            // rotorCurrent
            k[0, 0] = h * calculateRotorCurrent(x[0], x[1], U);
            k[0, 1] = h * calculateRotorCurrent(x[0] + k[0, 0] / 2, x[1] + k[0, 0] / 2, U);
            k[0, 2] = h * calculateRotorCurrent(x[0] + k[0, 1] / 2, x[1] + k[0, 1] / 2, U);
            k[0, 3] = h * calculateRotorCurrent(x[0] + k[0, 2], x[1] + k[0, 2], U);

            // angular velocity
            k[1, 0] = h * calculateAngularVelocity(x[0], x[1], 0);
            k[1, 1] = h * calculateAngularVelocity(x[0] + k[1, 0] / 2, x[1] + k[1, 0] / 2, 0);
            k[1, 2] = h * calculateAngularVelocity(x[0] + k[1, 1] / 2, x[1] + k[1, 1] / 2, 0);
            k[1, 3] = h * calculateAngularVelocity(x[0] + k[1, 2], x[1] + k[1, 2], 0);

            for (int i = 0; i < 2; i++)
            {
                x[i] = x[i] + (k[i, 0] + 2 * k[i, 1] + 2 * k[i, 2] + k[i, 3]) / 6;
            }
            return x;
        }
    }
}
