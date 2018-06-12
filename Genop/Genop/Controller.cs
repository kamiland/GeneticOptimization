using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{
    /*
     *  Klasa Controller implementuje algorytm sterujący typu PID do sterowania obiektem
     */ 
    public class Controller
    {
        public double Kp, Ki, Kd;
        double P, I, D;
        double integral, derivative;
        double error, pre_error = 0, controllerOutput = 0;

        public Controller(double initialKp, double initialKi, double initialKd)
        {
            Kp = initialKp;
            Ki = initialKi;
            Kd = initialKd;
        }
        // Metoda wykonująca algorytm PID
        public double CalculateOutput(double setpoint, double pv, double dt = 0.001)
        {
            error = setpoint - pv;

            P = Kp * error;

            integral += error * dt;
            I = Ki * integral;

            derivative = (error - pre_error) / dt;
            D = Kd * derivative;

            pre_error = error;

            controllerOutput = P + I + D;
            if (controllerOutput > 230) controllerOutput = 230;
            if (controllerOutput < 0) controllerOutput = 0;

            return controllerOutput;
        }
    }
}
