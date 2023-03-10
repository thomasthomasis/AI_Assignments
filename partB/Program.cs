using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;

// Hello World! program
namespace PartB
{

    class Hello 
    {         
        static void Main(string[] args)
        {
            int populationSize = 100; //number of strings in each generation
            int numberOfGenerations = 100; //number of generations
            int mutationRate = 1; //how often a character in a string will mutate (1/100)

            List<String> students = new List<String>();
            List<String> preferences = new List<String>();
            
            using(StreamReader reader = new StreamReader("Student-choices.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    students.Add(values[0]);

                    String studentPreferences = "";
                    for(int i = 1; i < values.Length; i++)
                    {
                        if(i == values.Length - 1)
                        {
                            studentPreferences += values[i];
                        }
                        else
                        {
                            studentPreferences += values[i] + ","; 
                        }
                    
                        
                    }

                    preferences.Add(studentPreferences);
                    
                }
            }

            List<String> supervisors = new List<String>();
            List<int> capacities = new List<int>();

            using(StreamReader reader = new StreamReader("Supervisors.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    supervisors.Add(values[0]);
                    capacities.Add(int.Parse(values[1]));
                }
            }

            
            List<List<int[]>> initialPopulation = GeneratePopulation(populationSize, students.Count, supervisors.Count, capacities);

            List<float> averageFitness = CalculateAverageFitness(initialPopulation, numberOfGenerations, populationSize, mutationRate, preferences, supervisors.Count, students.Count, capacities); //calculate the average fitness of each generation
            for(int i = 0; i < averageFitness.Count; i++)
            {
                Console.Write(averageFitness[i] + ", "); //print out each average fitness
            }

        }

        static List<float> CalculateAverageFitness(List<List<int[]>> population, int numberOfGenerations, int populationSize, int mutationRate, List<String> preferences, int numSupervisors, int numStudents, List<int> capacities)
        {
            List<float> averageFitness = new List<float>(); //initiate empty array

            List<List<int[]>> newPopulation = population; //set newPopulation to one that is passed in

            for(int i = 0; i < numberOfGenerations; i++)
            {
                /*
                Console.WriteLine("----------------");
                Console.WriteLine("----------------");
                Console.WriteLine("Generation " + (i + 1));
                Console.WriteLine("----------------");
                Console.WriteLine("----------------");
                */

                List<int> fitness = EvaluateFitness(newPopulation, preferences);
                
                //find average fitness
                float totalFitness = 0;
                for(int j = 0; j < fitness.Count; j++)
                {
                    totalFitness += fitness[j];
                }
                averageFitness.Add(totalFitness/fitness.Count);

                List<List<int[]>> parents = SelectParents(newPopulation, fitness, numSupervisors, numStudents); //select parents from the population

                newPopulation.Clear();
                for(int j = 0; j < populationSize; j++)
                {
                    //Console.WriteLine("Child " + (j + 1));
                    newPopulation.Add(Breed(parents, mutationRate, preferences, capacities));
                }

                /*
                for(int j = 0; j < newPopulation.Count; j++)
                {
                    Console.WriteLine("Member " + (j+1));
                    for(int k = 0; k < newPopulation[j].Count; k++)
                    {
                        Console.Write("Student: " + newPopulation[j][k][0] + " | ");
                        Console.Write("Supervisor: " + newPopulation[j][k][1] + "");
                        Console.WriteLine();
                    }
                }
                */
            }
            
            return averageFitness;
        }

        static List<List<int[]>> GeneratePopulation(int populationSize, int solutionSize, int numSupervisors, List<int> capacities)
        {

            List<List<int[]>> population = new List<List<int[]>>();

            for(int i = 0; i < populationSize; i++)
            {
                List<int> initialCapacities = new List<int>();
                for(int j = 0; j < capacities.Count; j++)
                {
                    initialCapacities.Add(capacities[j]);
                }
                List<int[]> currentSolution = new List<int[]>();

                for(int j = 1; j <= solutionSize; j++)
                {
                    int[] matching = new int[2];
                    matching[0] = j;

                    while(1 == 1)
                    {
                        Random rnd = new Random();
                        int randomChoice = rnd.Next() % numSupervisors;
                        
                        if(initialCapacities[randomChoice] <= 0)
                        {
                            continue;
                        }
                        matching[1] = randomChoice + 1;

                        initialCapacities[randomChoice]--;
                        break;
                    }
                    
                    currentSolution.Add(matching);
                }

                population.Add(currentSolution);
            }
        

            return population;
        }

        static List<int> EvaluateFitness(List<List<int[]>> population, List<String> preferences)
        {
            List<int> fitness = new List<int>();

            for(int i = 0; i < population.Count; i++)
            {
                int currentFitness = 0;

                List<int[]> currentPopulationMember = population[i];
                
                for(int k = 0; k < currentPopulationMember.Count; k++)
                {
                    List<int> currentStudentPreferences = new List<int>();

                    var values = preferences[k].Split(',');

                    for(int j = 0; j < values.Length; j++)
                    {
                        currentStudentPreferences.Add(int.Parse(values[j]));
                    }

                    int supervisor = currentPopulationMember[k][1];

                    for(int m = 0; m < currentStudentPreferences.Count; m++)
                    {
                        if(supervisor == currentStudentPreferences[m])
                        {
                            currentFitness += m;
                            break;
                        }
                    }

                    currentStudentPreferences.Clear();
                }

                fitness.Add(currentFitness);
            }
            
                
            return fitness;
        }

        static List<List<int[]>> SelectParents(List<List<int[]>> population, List<int> fitness, int numSupervisors, int numStudents)
        {
            List<List<int[]>> parents = new List<List<int[]>>();

            for(int i = 0; i < 2; i++)
            {
                int smallestIndex = 0;
                int smallestFitness = (numSupervisors * numStudents + 1);

                for(int j = 0; j < fitness.Count; j++)
                {
                    if(fitness[j] < smallestFitness)
                    {
                        smallestFitness = fitness[j];
                        smallestIndex = j;
                    }
                }

                parents.Add(population[smallestIndex]);

                fitness.RemoveAt(smallestIndex);
                population.RemoveAt(smallestIndex);

            }

            return parents;
        }

        static List<int[]> Breed(List<List<int[]>> parents, int mutationRate, List<String> preferences, List<int> capacities) //breeding method to create new children from parents
        {
            List<int[]> parentOne = parents[0];
            List<int[]> parentTwo = parents[1];

            int numMutations = 0;

            Random rnd = new Random();
            int crossoverPoint = rnd.Next(parents[0].Count); //set the crossover point of the breeding function

            List<int[]> child = new List<int[]>();

            for(int i = 0; i < parents[0].Count; i++)
            {
                if(i < crossoverPoint)
                {
                    child.Add(parentOne[i]); //if i is less than the crossover take from the first parent else take from the second parent
                }
                else
                {
                    child.Add(parentTwo[i]);
                }
                
                if(rnd.Next(1000) < mutationRate) //if we generate a number which is less than our mutation rate number, mutate the character to 1
                {
                    numMutations++;
                }

                //Console.Write(child[i][1] + "|");
            }

            //Console.WriteLine();


            
            List<int> newCapacities = new List<int>();

            /*
            Console.WriteLine("Before");
            for(int i = 0; i < capacities.Count; i++)
            {
                newCapacities.Add(capacities[i]);
                Console.Write(newCapacities[i] + "|");
            }
            Console.WriteLine();
            */
            
            for(int i = 0; i < newCapacities.Count; i++)
            {
                for(int j = 0; j < child.Count; j++)
                {
                    if(child[j][1] == (i + 1))
                    {
                        newCapacities[i]--;
                    }
                }
            }
            
            /*
            Console.WriteLine("During");
            for(int j = 0; j < newCapacities.Count; j++)
                {
                    Console.Write(newCapacities[j] + "|");
                }
            Console.WriteLine();
            */

            for(int i = 0; i < newCapacities.Count; i++)
            {
                if(newCapacities[i] == 0)
                {
                    //Console.WriteLine("Given Out Enough " + newCapacities[i]);
                }

                else if(newCapacities[i] > 0)
                {
                    //Console.WriteLine("Not Given Out Enough " + newCapacities[i]);
                }
                
                else if(newCapacities[i] < 0)
                {
                    //Console.WriteLine("Given Out Too Much " + newCapacities[i]);
                    int amount = (newCapacities[i] * -1);

                    for(int k = 0; k < amount; k++)
                    {
                        //Console.WriteLine("Reducing " + (k + 1));
                        for(int j = 0; j < child.Count; j++)
                        {
                            if(child[j][1] == (i + 1))
                            {
                                //Console.WriteLine("Reduced ");
                                child[j][1] = 0;
                                newCapacities[i]++;
                                break;
                            }
                        }
                    }
                    
                }
            }

            /*
            Console.WriteLine("After Reducing Under Zero");

            for(int j = 0; j < newCapacities.Count; j++)
            {
                Console.Write(newCapacities[j] + "|");
            }
            Console.WriteLine();
            */

            for(int i = 0; i < newCapacities.Count; i++)
            {
                if(newCapacities[i] > 0)
                {
                    for(int j = 0; j < child.Count; j++)
                    {
                        if(child[j][1] == 0)
                        {
                            child[j][1] = (i + 1);
                            newCapacities[i]--;
                            if(newCapacities[i] == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            

            /*
            Console.WriteLine("After Reducing Over Zero");

            for(int j = 0; j < newCapacities.Count; j++)
            {
                Console.Write(newCapacities[j] + "|");
            }
            Console.WriteLine();
            */

            for(int i = 0; i < numMutations; i++)
            {
                List<int> currentStudentPreferences = new List<int>();

                int randomChoice = rnd.Next(child.Count);

                int selectedStudent = (child[randomChoice][0] - 1);

                var values = preferences[selectedStudent].Split(',');

                for(int j = 0; j < values.Length; j++)
                {
                    currentStudentPreferences.Add(int.Parse(values[j]));
                }

                int previousSupervisor = child[randomChoice][1];

                int previousSupervisorIndex = 0;

                for(int j = 0; j < values.Length; j++)
                {
                    if(currentStudentPreferences[j] == previousSupervisor)
                    {
                        previousSupervisorIndex = j;
                        break;
                    }
                }

                int newSupervisorIndex = rnd.Next(previousSupervisorIndex);
                int newSupervisor = currentStudentPreferences[newSupervisorIndex];

                child[i][1] = currentStudentPreferences[newSupervisorIndex];

                for(int j = 0; j < child.Count; j++)
                {
                    if(child[j][1] == newSupervisor)
                    {
                        child[j][1] = previousSupervisor;
                        break;
                    }
                }
            }

            /*
            Console.WriteLine("After Mutation");

            for(int j = 0; j < newCapacities.Count; j++)
            {
                Console.Write(newCapacities[j] + "|");
            }
            Console.WriteLine();
            */

            return child;
        }

    }

}