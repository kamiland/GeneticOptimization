using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
*	Klasa GeneticAlgorithm tworzy osobniki populacji a nastepnie przeprowadza na nich optymalizację poprzez selekcję oraz mutację.
*	Ostatecznie tworzona jest nowa generacja i cały cykl się powtarza aż do znalezienia zadowającego rozwiązania
*/
namespace Genop
{
    public class GeneticAlgorithm
    {
        static int populationSize; //ilość osobników w populacji
        Simulator[] engines;
        Simulator[] next_generation_engines;
        Simulator best = new Simulator();
        double[] objectParameters;
        double best_fit = 0;
        int generationCounter = 0;

        Random rand_gen = new Random();
        double rand;

        double P_range = 10;
        double I_range = 10;
        double D_range = 10;

        long number_of_probes; //ilość próbek symulacji

        public GeneticAlgorithm(long _numberOfProbes, int _pupulationSize, double[] _objectParameters)
        {
            number_of_probes = _numberOfProbes;
            populationSize = _pupulationSize;
            objectParameters = _objectParameters;
            engines = new Simulator[populationSize];
            next_generation_engines = new Simulator[populationSize];

            for(int i = 0; i<populationSize; i++)
            {
                engines[i] = new Simulator();
                engines[i].GetUserParameters(objectParameters);
                engines[i].PID.Kp = ((rand_gen.NextDouble() * 2.0) - 1.0) * P_range; //randomize a P value between -P_range and P_range
                engines[i].PID.Ki = ((rand_gen.NextDouble() * 2.0) - 1.0) * I_range;
                engines[i].PID.Kd = ((rand_gen.NextDouble() * 2.0) - 1.0) * D_range;
            }
        }

        //przeprowadzenie jednej generacji aż do momentu powstania nowej
        public void do_one_generation()
        {
            for (int i = 0; i < populationSize; i++)
                engines[i].Simulate( number_of_probes, saveToFile: false);
            normalize_fitness();
            next_generation();
            generationCounter++;
        }

        public double[] show_best()
        {
            best.Simulate(number_of_probes, saveToFile: true);
            double[] PID = { best.PID.Kp, best.PID.Ki, best.PID.Kd };
            Window1 graphWindow = new Window1();
            graphWindow.Show();

            return PID;
        }


        //utworzenie nowej generacji z bieżącej generacji
        void next_generation()
        {
            normalize_fitness();
            
            for (int i = 0; i < populationSize; i++)
            {
                rand = rand_gen.NextDouble();
                if (rand >= 0.5)
                    pick_tweak(i);  //50% chance of tweaking

                else if (rand >= 0.1)
                    pick_and_cross(i); //40% chance of crossing

                else
                    mutatant(i); //10% chance of new random PID
            }

            for (int i = 0; i < populationSize; i++)
                engines[i] = next_generation_engines[i];
        }

        //wybór jednego osobnika z populacji oraz delikatna modyfikacja jego nastaw
        void pick_tweak(int i)
        {
            Simulator parent = new Simulator();
            int x;
            bool picked;

            if (rand_gen.NextDouble() > 0.01)
            {
                picked = false;
                do
                {
                    x = (int)(rand_gen.NextDouble() * populationSize); //i can do it like this because rand_gen.NextDouble() will never return value = 1, so i will never refer to engines[populationSize]
                    if (rand_gen.NextDouble() <= engines[x].fitness)
                    {
                        parent = engines[x];
                        picked = true;
                    }
                } while (!picked);
            }
            else
                parent = best; // 1% chance of picking "best" to tweak
            next_generation_engines[i] = tweak(parent);
        }

        //wybieranie dwóch osobników z populacji oraz krzyżowanie ich
        void pick_and_cross(int i)
        {
            Simulator parent_a = new Simulator();
            int a;
            Simulator parent_b = new Simulator();
            int b;
            bool picked;

            picked = false;
            do
            {
                a = (int)(rand_gen.NextDouble() * populationSize);
                if (rand_gen.NextDouble() <= engines[a].fitness)
                {
                    parent_a = engines[a];
                    picked = true;
                }
            } while (!picked);
            if (rand_gen.NextDouble() > 0.01)
            {
                picked = false;
                do
                {
                    b = (int)(rand_gen.NextDouble() * populationSize);
                    if (a != b && (rand_gen.NextDouble() <= engines[b].fitness))
                    {
                        parent_b = engines[b];
                        picked = true;
                    }
                } while (!picked);
            }
            else
                parent_b = best; // 1% chance of picking "best" as a pair to cross with
            next_generation_engines[i] = cross(parent_a, parent_b);
        }

        //normalizacja wartości fitness wszystkich osobników tak aby były wartościami od 0 do 1
        void normalize_fitness()
        {
            double max_fit = 0;
            double min_fit;

            //evaluating maximum fitness value
            for (int i = 0; i < populationSize; i++)
            {
                if (engines[i].fitness > max_fit)
                {
                    max_fit = engines[i].fitness;
                    if (max_fit > best_fit)
                    {
                        best_fit = max_fit;
                        best = engines[i];
                    }
                }
            }

            //evaluating minimum fitness value
            min_fit = max_fit;
            for (int i = 0; i < populationSize; i++)
            {
                min_fit = engines[i].fitness < min_fit ? engines[i].fitness : min_fit;
            }

            if (max_fit != 0)
                for (int i = 0; i < populationSize; i++)
                {
                    engines[i].fitness -= min_fit;
                    engines[i].fitness /= (max_fit - min_fit);
                }
        }

        //funkcja tworząca nowy losowy organizm
        void mutatant(int i)
        {
            Simulator child = new Simulator();
            child.GetUserParameters(objectParameters);
            child.PID.Kp = ((rand_gen.NextDouble() * 2.0) - 1.0) * P_range; //randomize a P value between -P_range and P_range
            child.PID.Ki = ((rand_gen.NextDouble() * 2.0) - 1.0) * I_range;
            child.PID.Kd = ((rand_gen.NextDouble() * 2.0) - 1.0) * D_range;
            if (rand_gen.NextDouble() > 0.01) next_generation_engines[i] = child;
            else next_generation_engines[i] = best; // 1% chance of picking "best" as a mutant
        }

        //funkcja delikatnie modyfikująca jeden organizm
        Simulator tweak(Simulator parent) 
        {
            Simulator child = new Simulator();
            child.GetUserParameters(objectParameters);
            child.PID.Kp = parent.PID.Kp + (rand_gen.NextDouble() * 2.0) - 1.0; // value of P +- random between 0 and 1
            child.PID.Ki = parent.PID.Ki + (rand_gen.NextDouble() * 2.0) - 1.0;
            child.PID.Kd = parent.PID.Kd + (rand_gen.NextDouble() * 2.0) - 1.0;
            return child;
        }

        //funkcja krzyrzująca dwa organizmy
        Simulator cross(Simulator parent_a, Simulator parent_b)
        {
            Simulator child = new Simulator();
            child.GetUserParameters(objectParameters);
            double crossing_point;
            crossing_point = rand_gen.NextDouble();
            child.PID.Kp = parent_a.PID.Kp * crossing_point + parent_b.PID.Kp * (1.0 - crossing_point); // picking a new P value in random spot between parent_a.P and parent_b.P
            crossing_point = rand_gen.NextDouble();
            child.PID.Ki = parent_a.PID.Ki * crossing_point + parent_b.PID.Ki * (1.0 - crossing_point);
            crossing_point = rand_gen.NextDouble();
            child.PID.Kd = parent_a.PID.Kd * crossing_point + parent_b.PID.Kd * (1.0 - crossing_point);
            return child;
        }
    }
}
