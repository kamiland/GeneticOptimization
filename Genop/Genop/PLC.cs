using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genop
{
    /**
     * Klasa PLC odpowiada za komunikację sterownika PLC z główną aplikacją.
     */
    public class PLC
    {
        Random rnd = new Random();
      /**
         * \brief Dokonuje pomiaru parametrów rzeczywistego silnika DC a następnie przeprowadza jego indentyfikację.
         *
         * Zwraca tablicę elementów double, zawierającą parametry silnika DC.
         *
         *            
         */
        public double[] AutoIdentyfication()
        {
            Console.WriteLine("Identyfication running...");

            double[] objectParameters = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

             /**
             * \fn Funkcja przypisująca parametry silnika.
             *
             * Pseudolosowe wyznaczenie parametrów silnika
             *           
             */
             
            for (int i = 0; i < objectParameters.Length; i++)
            {
                objectParameters[i] = (double)rnd.Next(10000) / 1000;
            }

            return objectParameters;
        }
    }
}
