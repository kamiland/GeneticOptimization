using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{
    /*
     * Klasa PLC odpowiada za komunikację sterownika PLC z aplikacją
     */
    public class PLC
    {
        Random rnd = new Random();
        public double[] AutoIdentyfication()
        {
            Console.WriteLine("Identyfication running...");

            double[] objectParameters = { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < objectParameters.Length; i++)
            {
                objectParameters[i] = (double)rnd.Next(10000) / 1000;
            }

            return objectParameters;
        }
    }
}
