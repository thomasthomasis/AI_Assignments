// Hello World! program
namespace HelloWorld
{
    using System.Text;

    class Hello 
    {         
        static void Main(string[] args)
        {
            int populationSize = 100;
            int stringLength = 30;
            int numberOfGenerations = 50;
            int mutationRate = 1;

            string[] population = GeneratePopulation(populationSize, stringLength);

            float[] averageFitness = CalculateAverageFitness(population, numberOfGenerations, populationSize, mutationRate);
            for(int i = 0; i < averageFitness.Length; i++)
            {
                Console.Write(averageFitness[i] + ", ");
            }
        }

        static float[] CalculateAverageFitness(string[] population, int numberOfGenerations, int populationSize, int mutationRate)
        {
            float[] averageFitness = new float[numberOfGenerations];

            string[] newPopulation = population;

            for(int i = 0; i < numberOfGenerations; i++)
            {
                //int[] fitness = EvaluateFitnessOneMax(newPopulation);
                //int[] fitness = EvaluateFitnessMatchingPairs(newPopulation);
                //int[] fitness = EvaluateFitnessDeceptiveLandscape(newPopulation);
                int[] fitness = EvaluateFitnessLargerAlphabet(newPopulation);
                
                float totalFitness = 0;
                for(int j = 0; j < fitness.Length; j++)
                {
                    totalFitness += fitness[j];
                }
                averageFitness[i] = totalFitness/fitness.Length;

                string[] parents = SelectParents(newPopulation, fitness);

                Array.Clear(newPopulation, 0, newPopulation.Length);
                for(int j = 0; j < populationSize; j++)
                {
                    //newPopulation[j] = Breed(parents, mutationRate);
                    newPopulation[j] = BreedLargerAlphabet(parents, mutationRate);
                }

                Console.WriteLine(newPopulation[0]);
            }

            
            
            return averageFitness;
        }

        static string[] GeneratePopulation(int populationSize, int stringLength)
        {
            string[] population = new string[populationSize];
            Random rnd = new Random();
            for(int i = 0; i < populationSize; i++)
            {
                string stringValue = "";
                for(int j = 0; j < stringLength; j++)
                {
                    stringValue += rnd.Next() % 2;
                }

                population[i] = stringValue;
            }
            
            return population;
        }

        static int[] EvaluateFitnessOneMax(string[] population)
        {
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < population[0].Length; j++)
                {
                    if(population[i][j] == '1')
                    {
                        fitness[i]++;
                    }
                }
                
            }

            return fitness;
        }

        static int[] EvaluateFitnessMatchingPairs(string[] population)
        {
            string target = "101010101010101010101010101010";
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < population[0].Length; j++)
                {
                    if(population[i][j] == target[j])
                    {
                        fitness[i]++;
                    }
                }
                
            }

            return fitness;
        }

        static int[] EvaluateFitnessDeceptiveLandscape(string[] population)
        {
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < population[0].Length; j++)
                {
                    if(population[i][j] == '1')
                    {
                        fitness[i]++;
                    }
                }
                
                if(fitness[i] == 0)
                {
                    fitness[i] = 2 * population[0].Length;
                }
            }

            return fitness;
        }

        static int[] EvaluateFitnessLargerAlphabet(string[] population)
        {
            string target = "0123456789";
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < target.Length; j++)
                {
                    if(population[i][j] == target[j])
                    {
                        fitness[i]++;
                    }
                }
                
            }

            return fitness;
        }



        static string[] SelectParents(string[] population, int[] fitness)
        {
            string[] parents = new string[2];

            string[] newPopulation = new string[population.Length];
            int[] newFitness = new int[fitness.Length];

            for(int i = 0; i < 2; i++)
            {
                int largestIndex = 0;
                int largestFitness = 0;

                for(int j = 0; j < fitness.Length; j++)
                {
                    if(fitness[j] > largestFitness)
                    {
                        largestFitness = fitness[j];
                        largestIndex = j;
                    }
                }

                parents[i] = population[largestIndex];

                for(int j = 0; j < population.Length; j++)
                {
                    if(j == largestIndex)
                    {
                        continue;
                    }

                    newPopulation[j] = population[j];
                    newFitness[j] = fitness[j];
                }

                population = newPopulation;
                fitness = newFitness;
            }

            return parents;
        }

        static string Breed(string[] parents, int mutationRate)
        {
            string parentOne = parents[0];
            string parentTwo = parents[1];

            Random rnd = new Random();
            int crossoverPoint = rnd.Next(parents[0].Length);

            StringBuilder child = new StringBuilder(parents[0].Length);
            
            for(int i = 0; i < parents[0].Length; i++)
            {
                if(i < crossoverPoint)
                {

                    child.Append(parentOne[i]);
                }
                else
                {
                    child.Append(parentTwo[i]);
                }
                
                if(rnd.Next(1000) < mutationRate)
                {
                    child[i] = '1';
                }
            }

            return child.ToString();
        }

        static string BreedLargerAlphabet(string[] parents, int mutationRate)
        {
            string target = "0123456789";

            string parentOne = parents[0];
            string parentTwo = parents[1];

            Random rnd = new Random();
            int crossoverPoint = rnd.Next(parents[0].Length);

            StringBuilder child = new StringBuilder(parents[0].Length);
            
            for(int i = 0; i < target.Length; i++)
            {
                if(i < crossoverPoint)
                {

                    child.Append(parentOne[i]);
                }
                else
                {
                    child.Append(parentTwo[i]);
                }
                
                if(rnd.Next(1000) < mutationRate)
                {
                    child[i] = target[i];
                }
            }

            return child.ToString();
        }

    }
}


