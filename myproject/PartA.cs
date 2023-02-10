// Hello World! program
namespace HelloWorld
{
    using System.Text;

    class Hello 
    {         
        static void Main(string[] args)
        {
            int populationSize = 100; //number of strings in each generation
            int stringLength = 30; //length of each string
            int numberOfGenerations = 25; //number of generations
            int mutationRate = 1; //how often a character in a string will mutate (1/100)

            string[] population = GeneratePopulation(populationSize, stringLength); //generate the initial population

            float[] averageFitness = CalculateAverageFitness(population, numberOfGenerations, populationSize, mutationRate); //calculate the average fitness of each generation
            for(int i = 0; i < averageFitness.Length; i++)
            {
                Console.Write(averageFitness[i] + ", "); //print out each average fitness
            }

        }

        static float[] CalculateAverageFitness(string[] population, int numberOfGenerations, int populationSize, int mutationRate)
        {
            float[] averageFitness = new float[numberOfGenerations]; //initiate empty array

            string[] newPopulation = population; //set newPopulation to one that is passed in

            for(int i = 0; i < numberOfGenerations; i++)
            {
                //each part of the assignment has its own fitness function
                int[] fitness = EvaluateFitnessOneMax(newPopulation);
                //int[] fitness = EvaluateFitnessMatchingPairs(newPopulation);
                //int[] fitness = EvaluateFitnessDeceptiveLandscape(newPopulation);
                //int[] fitness = EvaluateFitnessLargerAlphabet(newPopulation);
                
                //find average fitness
                float totalFitness = 0;
                for(int j = 0; j < fitness.Length; j++)
                {
                    totalFitness += fitness[j];
                }
                averageFitness[i] = totalFitness/fitness.Length;

                string[] parents = SelectParents(newPopulation, fitness); //select parents from the population

                Array.Clear(newPopulation, 0, newPopulation.Length); //empty population array
                for(int j = 0; j < populationSize; j++)
                {
                    newPopulation[j] = Breed(parents, mutationRate);
                    //newPopulation[j] = BreedLargerAlphabet(parents, mutationRate); //select this Breeding method if using the Larger Alphabet fitness function
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
                    stringValue += rnd.Next() % 2; //construct a string of length "stringLength" where each character is either a 0 or a 1
                }

                population[i] = stringValue;
            }
            
            return population; //return a string array of random strings
        }

        static int[] EvaluateFitnessOneMax(string[] population) //evaluate fitness for one max problem
        {
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < population[0].Length; j++)
                {
                    if(population[i][j] == '1')
                    {
                        fitness[i]++; //if character of the string is a zero increment fitness variable
                    }
                }
                
            }

            return fitness;
        }

        static int[] EvaluateFitnessMatchingPairs(string[] population) //fitness function for Matching Pairs
        {
            string target = "101010101010101010101010101010"; //target string
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < population[0].Length; j++)
                {
                    if(population[i][j] == target[j])
                    {
                        fitness[i]++; //if character in this position is equal to the character in the same position of the target string increment the fitness of this population member
                    }
                }
                
            }

            return fitness;
        }

        static int[] EvaluateFitnessDeceptiveLandscape(string[] population) //fitness function for the deceptive landscape part of the assignment
        {
            int[] fitness = new int[population.Length];

            for(int i = 0; i < population.Length; i++)
            {
                for(int j = 0; j < population[0].Length; j++)
                {
                    if(population[i][j] == '1')
                    {
                        fitness[i]++; //if character is a 1 increment fitness
                    }
                }
                
                if(fitness[i] == 0)
                {
                    fitness[i] = 2 * population[0].Length; //if there are no ones make the fitness double the length of the strings in the population
                }
            }

            return fitness;
        }

        static int[] EvaluateFitnessLargerAlphabet(string[] population) //fitness function for sequence of numbers similar to second part of the assignment althought the breeding function needs to change
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



        static string[] SelectParents(string[] population, int[] fitness) //function to select two parents with the highest fitness in the population
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
                        largestFitness = fitness[j]; //find the position in the fitness array with the largest integer
                        largestIndex = j;
                    }
                }

                parents[i] = population[largestIndex]; //make the parent the largest member in the population

                for(int j = 0; j < population.Length; j++)
                {
                    if(j == largestIndex)
                    {
                        continue;
                    }

                    newPopulation[j] = population[j]; //input all population members back into array except the one that was picked as a parent
                    newFitness[j] = fitness[j]; //input all fitness integers back into the array except if it was the largest was picked before
                }

                population = newPopulation;
                fitness = newFitness;
            }

            return parents;
        }

        static string Breed(string[] parents, int mutationRate) //breeding method to create new children from parents
        {
            string parentOne = parents[0];
            string parentTwo = parents[1];

            Random rnd = new Random();
            int crossoverPoint = rnd.Next(parents[0].Length); //set the crossover point of the breeding function

            StringBuilder child = new StringBuilder(parents[0].Length);
            
            for(int i = 0; i < parents[0].Length; i++)
            {
                if(i < crossoverPoint)
                {
                    child.Append(parentOne[i]); //if i is less than the crossover take from the first parent else take from the second parent
                }
                else
                {
                    child.Append(parentTwo[i]);
                }
                
                if(rnd.Next(1000) < mutationRate) //if we generate a number which is less than our mutation rate number, mutate the character to 1
                {
                    if(child[i] == '1')
                    {
                        child[i] == '0';
                    }
                    else 
                    {
                        child[i] = '1';
                    }
                }
            }

            return child.ToString();
        }

        static string BreedLargerAlphabet(string[] parents, int mutationRate) //breeding method for the larger alphabet part
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

                    child.Append(parentOne[i]); // if i is less than the crossover point take from the first parent else take from the second parent
                }
                else
                {
                    child.Append(parentTwo[i]);
                }
                
                if(rnd.Next(1000) < mutationRate)
                {
                    child[i] = target[i]; //if we generate a number which is less than our mutation rate number, mutate the character to what it should be
                }
            }

            return child.ToString();
        }

    }
}


