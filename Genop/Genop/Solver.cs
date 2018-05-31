using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{
    class Solver
    {
        // U - supply voltage, E - electromotive force
        double U, E;
        // La - rotor inductance, Ra - rotor resistance, Ua - rotor voltage
        double La, Ra, Ua;
        // Lf - excitation induction, Rf - excitation resistance, If - excitation current 
        double Lf, Rf, If, Ufn;
        // Ltf - mutual inductance
        double Laf;
        //  T - drive torque, p - number of pole pairs, B - damping constant, Tl - moment load, J - moment of inertia
        double T, p, B, Tl, J;
        // fixed excitation current ifn = Ufn / Rf
        double ifn;
        // angular velocity, rotor current
        double angularV, current;
        double[][] state;

        public Solver()
        {
            state[0][0] = current;
            state[0][1] = angularV;
            ifn = Ufn / Rf;
            double Gaf = p * Laf * ifn;
            E = Gaf * angularV;
            T = Gaf * current;

            // Mathematical Model
            // [0][x] - prev step, [1][x] - next step
            state[1][0] = -(Ra / La) * state[0][0] - (Gaf / La) * state[0][1] + (1 / La) * U;
            state[1][1] = (Gaf / J) * state[0][0] - (B / J) * state[0][1] + (1 / J) * Tl;
        }


        public double[][] CalculateNextStep()
        {

            
            return state;
        }
    }
}
