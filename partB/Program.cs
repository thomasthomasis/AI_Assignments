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

            ArrayList students = new ArrayList();
            ArrayList preferences = new ArrayList();
            
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

            ArrayList supervisors = new ArrayList();
            ArrayList capacities = new ArrayList();

            using(StreamReader reader = new StreamReader("Supervisors.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    supervisors.Add(values[0]);
                    capacities.Add(values[1]);
                }
            }
            
            
            ArrayList initialPopulation = GeneratePopulation(students, preferences, supervisors, capacities);
            


        }

        static ArrayList GeneratePopulation(ArrayList students, ArrayList preferences, ArrayList supervisors, ArrayList capacities)
        {
            ArrayList population = new ArrayList();

            for(int i = 0; i < students.Count; i++)
            {
                String preferencesString = preferences[i] as String;

                String[] values = preferencesString.Split(',');
                Random rnd = new Random();
                int randomNumber = rnd.Next(values.Length);

                int randomChoice = int.Parse(values[randomNumber]);
                Console.WriteLine(randomChoice);
            }


            return population;
        }

    }

}