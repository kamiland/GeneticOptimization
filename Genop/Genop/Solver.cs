using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*!
\mainpage  STRONA GŁÓWNA DOKUMENTACJI PROJEKTU OPROGRAMOWANIA
\author  Maciej Zielonka \n Kamil Andrzejewski \n Paweł Zarembski \n Paweł Spławski
\date    01.06.2018
\version 1.0
\section etykieta-opis Ogólny Opis
Niniejsza dokumentacja dotyczy projektu oprogramowania systemu, którego
zadaniem jest automatyczne wyznaczanie nastaw dla regulatora silników DC oraz
ich optymalizacja względem określonego kryterium.
\section etykieta-wazne-cechy Najważniejsze cechy
Przedstawiony w specyfikacji funkcjonalnej system umożliwiać będzie realizację na-
stępujących zadań : \n
• wprowadzanie przez użytkownika wymaganych parametrów silnika prądu sta-
łego, \n
• automatyczną identyfikację parametrów silnika prądu stałego,
• wyznaczanie nastaw regulatora na podstawie zadanej trajektorii odpowiedzi
silnika, \n
• optymalizacja nastaw za pomocą algorytmu genetycznego i danych z rzeczy-
wistego obiektu, \n
• wizualizacja wartości wielkości pomiarowych podczas pracy silnika. \n

*/

/**
*
* Przestrzeń nazw związana z komponentami aplikacji optymalizującej.
*
*/

namespace Genop
{
    /**
     *
     * Klasa Solver implementuje algorytm Rungego-Kutty dla obiektu silnika DC:
     *
     * x1 - prąd wirnika, x2 - prędkość kątowa \n
     * x1' = -(Ra / La) * x1 - (Gaf / La) * x2 + (1 / La) * U \n
     * x2' =  (Gaf / J) * x1 - (B / J) * x2 + (1 / J) * Tl \n
     *   
     *\version Wersja alfa.
     */
    public class Solver
    {
        /** U - napięcie zasilania, E - siła elektromotoryczna */
        double U, E;
        /** La - indukcyjność własna wirnika, Ra - rezystancja uzwojenia wirnika, Ua - napięcie twornika    */
        public double La, Ra, Ua;
        /** Lf - indukcyjność własna obwodu wzbudzenia, Rf - rezystancja obwodu wzbudzenia, If - prąd obwodu wzbudzenia     */
        public double Lf, Rf, If, Ufn;
        /** Laf - indukcyjność wzajemna   */
        public double Laf;
        /**  T - moment napędowy, B - współczynnik tłumienia, p - pary biegunów, Tl - moment obciążenia, J - moment bezwłądności */
        public double T, B, p, Tl, J;
        /** Stała obwodu wzbudzenia (stałe magnesowanie) ifn = Ufn / Rf */
        double ifn;
        /** prędkość kątowa, prąd obwodu wirnika */
        double angularVelocity, rotorCurrent;

        double Gaf;

        public double[] x = new double[2];

        public Solver()
        {
            /** Parametry domyślne  */
            Ra = 0.4;   La = 0.02;
            Rf = 65;    Lf = 65;
            J = 0.11; B = 0.0053;
            p = 2;
            Laf = 0.363;
            Ufn = 110;

            /** Wartości początkowe dla obiektu silnika   */
            rotorCurrent = 0; angularVelocity = 0; 
            x[0] = rotorCurrent;
            x[1] = angularVelocity;

            ifn = Ufn / Rf;
            Gaf = p * Laf * ifn;
            E = Gaf * angularVelocity;
            T = Gaf * rotorCurrent;
        }

        /**
         * \brief Oblicza wartość prądu wirnika
         *
         * Pobiera trzy wartości typu double.
         *
         * \param[in] x1 prąd wirnika
         * \param[in] x2 prędkość kątowa
         * \param[in] U napięcie
         * \return wyznaczona na podstawie modelu, wartość prądu wirnika silnika DC

         * \attention wartości zmiennych wyskalowane są w podstawowych jednostkach układu SI
         *            
         */
        
        public double calculateRotorCurrent(double x1, double x2, double U)
        {
            return -(Ra / La) * x1 - (Gaf / La) * x2 + (1 / La) * U;
        }

        /**
         * \brief Oblicza wartość prędkości obrotowej silnika
         *
         * Pobiera trzy wartości typu double.
         *
         * \param[in] x1 prąd wirnika
         * \param[in] x2 prędkość kątowa
         * \param[in] Tl moment obciążenia
         * \return wyznaczona na podstawie modelu, wartość prędkości wirnika silnika DC

         * \attention wartości zmiennych wyskalowane są w podstawowych jednostkach układu SI \n
         *            domyślnie wartość Tl = 0
         *            
         */

        public double calculateAngularVelocity(double x1, double x2, double Tl = 0)
        {
            return (Gaf / J) * x1 - (B / J) * x2 + (1 / J) * Tl;
        }

        // implementacja algorytmu Rungego-Kutty dla obiektu silnika DC (https://pl.wikipedia.org/wiki/Algorytm_Rungego-Kutty)
        /**
         * \brief Wyznacza wartość kroku operacji numerycznej dla solvera w kolejnej iteracji.q
         *
         * Pobiera dwie wartości typu double.
         *
         * \param[in] U napięcie
         * \param[in] h krok całkowania
         * \return wyznaczona wartość kroku solvera w kolejnej iteracji
         *            
         */

        public double[] CalculateNextStep(double U, double h = 0.001)
        {
            double[,] k = new double[2, 4];

            /** Wyznaczanie prądu obwodu wirnika    */
            k[0, 0] = h * calculateRotorCurrent(x[0], x[1], U);
            k[0, 1] = h * calculateRotorCurrent(x[0] + k[0, 0] / 2, x[1] + k[0, 0] / 2, U);
            k[0, 2] = h * calculateRotorCurrent(x[0] + k[0, 1] / 2, x[1] + k[0, 1] / 2, U);
            k[0, 3] = h * calculateRotorCurrent(x[0] + k[0, 2], x[1] + k[0, 2], U);

            /** Wyznaczanie prędkości kątowej */
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
