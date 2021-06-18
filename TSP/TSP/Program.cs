using System;
using System.Collections.Generic;
using System.IO;

namespace TSP
{
    class Program
    {
       public static int[,] CrossingOX(int[,] populationTemp, int populationAmount, int distance, Random rnd)
        {
            int[,] populationOutcome = new int[populationAmount, distance];

            for (int i = 0; i < populationAmount; i += 2)
            {
                if (rnd.Next(0, 101) <= 75)
                {
                    //two random points are being choosen
                    int counter = 0;
                    int point1 = rnd.Next(0, distance - 1);
                    int point2 = rnd.Next(point1 + 1, distance);
                    int[] part1 = new int[distance];
                    int[] part2 = new int[distance]; //creating tables for the two parts that will be changed 
                    int temp1 = 0;
                    int temp2 = 0;
                    for (int j = point2; j < distance; j++) //writing the parts that will be changed based on the random points
                    {
                        part1[counter] = populationTemp[i, j];
                        part2[counter] = populationTemp[i + 1, j];
                        counter++;
                    }
                    for (int j = 0; j < point1; j++)
                    {
                        part1[counter] = populationTemp[i, j];
                        part2[counter] = populationTemp[i + 1, j];
                        counter++;
                    }
                    for (int j = point1; j < point2; j++)
                    {
                        part1[counter] = populationTemp[i, j];
                        part2[counter] = populationTemp[i + 1, j];

                        populationOutcome[i, j] = populationTemp[i, j];
                        populationOutcome[i + 1, j] = populationTemp[i + 1, j];

                        counter++;

                    }

                    for (int k = 0; k < distance; k++) //changing the parts
                    {
                        for (int j = point1; j < point2; j++)
                        {
                            if (part2[k] == populationOutcome[i, j])
                            {

                                part2[k] = -1;
                            }
                            if (part1[k] == populationOutcome[i + 1, j])
                            {
                                part1[k] = -1;
                            }
                        }
                    }

                    for (int j = 0; j < point1; j++)
                    {
                        while (part2[temp2] == -1)
                        {
                            temp2++;
                        }
                        populationOutcome[i, j] = part2[temp2];
                        temp2++;
                        while (part1[temp1] == -1)
                        {
                            temp1++;
                        }
                        populationOutcome[i + 1, j] = part1[temp1];
                        temp1++;

                    }
                    for (int j = point2; j < distance; j++)
                    {
                        while (part2[temp2] == -1)
                        {
                            temp2++;
                        }
                        while (part1[temp1] == -1)
                        {
                            temp1++;
                        }
                        populationOutcome[i, j] = part2[temp2];
                        temp2++;
                        populationOutcome[i + 1, j] = part1[temp1];
                        temp1++;

                    }

                }
                else
                {
                    for (int b = 0; b < distance; b++)
                    {
                        populationOutcome[i, b] = populationTemp[i, b];
                        populationOutcome[i + 1, b] = populationTemp[i + 1, b];

                    }
                }
            }

            return populationOutcome;
        }
        public static int[,] MutationInversion(int[,] populationTemp, int populationAmount, int distance, Random rnd)
        {


            for (int a = 0; a < populationAmount; a++)
            {
                if (rnd.Next(0, 101) <= 5)
                { //two random point are being chosen
                    int point1 = rnd.Next(0, distance - 1);
                    int point2 = rnd.Next(point1 + 1, distance);
                    int[] temp = new int[point2 - point1]; 
                    int counter = 0;
                    for (int i = point1; i < point2; i++)
                    {
                        temp[counter] = populationTemp[a, i];

                        counter++;
                    }

                    counter--;
                    for (int i = point1; i < point2; i++) //reversiong the part between two chosen points
                    {
                        populationTemp[a, i] = temp[counter];

                        counter--;
                    }

                }


            }

            return populationTemp;
        }

        public static int[,] SelectionRoulette(int[,] population, int populationAmount, int[] milage, int distance, Random rnd)
        {

            int[] wheelSections = new int[populationAmount]; //% amounts of the wheel
            int[,] wheelPopulation = new int[populationAmount, distance];
            int max = 0;
            int amount = 0;
            for (int i = 0; i < populationAmount; i++)
            {
                if (max < milage[i])
                {
                    max = milage[i];
                }
            }
            for (int i = 0; i < populationAmount; i++)
            {

                wheelSections[i] = (max - milage[i] + 1);
                amount += wheelSections[i];

                if (i > 0)
                {
                    wheelSections[i] = wheelSections[i] + wheelSections[i - 1];

                }
            }

            for (int i = 0; i < populationAmount; i++)
            {
                int temp = rnd.Next(0, amount);
                for (int j = 0; j < populationAmount; j++)
                {

                    if (temp <= wheelSections[j])
                    {
                        for (int a = 0; a < distance; a++)
                        {
                            wheelPopulation[i, a] = population[j, a];

                        }
                        break;
                    }
                }

            }
            return wheelPopulation;
        }
        public static int[,] SelectionTournament(int[,] population, int populationAmount, int[] milage, int distance, Random rnd)
        {

            int maxSize = 9;
            int[] tournamentOutcome = new int[maxSize];
            int[,] tournament = new int[maxSize, distance];
            int[,] populationTournament = new int[populationAmount, distance];
            for (int i = 0; i < populationAmount; i++)
            {
                int min = 100000;
                int indexMin = 0;
                for (int j = 0; j < maxSize; j++)
                {
                    int num = rnd.Next(0, populationAmount);
                    for (int k = 0; k < distance; k++)
                    {
                        tournament[j, k] = population[num, k];
                    }
                    tournamentOutcome[j] = milage[num];
                }
                for (int j = 0; j < maxSize; j++)
                {
                    if (min > tournamentOutcome[j])
                    {
                        min = tournamentOutcome[j];
                        indexMin = j;
                    }
                }

                for (int j = 0; j < distance; j++)
                {
                    populationTournament[i, j] = tournament[indexMin, j];
                }
            }


            return populationTournament;
        }


        static void Main(string[] args)
        {
            int distance;
            int[,] distanceTable;
            Random rnd = new Random();
            int populationAmount = 40; //changing the size of population
            int iterations = 100000; //changing the amount of iterations

            //loading the file
            using (StreamReader sr = new StreamReader("berlin52.txt"))
            {
                distance = Int32.Parse(sr.ReadLine());
                distanceTable = new int[distance, distance];
                for (int i = 0; i < distance; i++)
                {
                    string[] temp = sr.ReadLine().Split(' ');
                    for (int j = 0; j < distance; j++)
                    {
                        if (i == j)
                        {
                            distanceTable[i, j] = 0;
                            j = distance;
                        }
                        else
                        {
                            distanceTable[i, j] = Int32.Parse(temp[j]);
                            distanceTable[j, i] = distanceTable[i, j];
                        }
                    }
                }
            }

            //creating population
            int[,] population = new int[populationAmount, distance];
            int[] milage = new int[populationAmount];
            int[,] populationTemp = new int[populationAmount, distance];

            int[] distanceBest = new int[distance];
            int bestMilage = 1000000;



            //first population
            for (int i = 0; i < populationAmount; i++)
            {
                List<int> temp = new List<int>();
                int randomTemp;
                for (int j = 0; j < distance; j++)
                {
                    do
                    {
                        randomTemp = rnd.Next(0, distance);
                    } while (temp.Contains(randomTemp));
                    population[i, j] = randomTemp;
                    temp.Add(randomTemp);

                }
            }
            for (int i = 0; i < populationAmount; i++)
            {
                for (int j = 0; j < distance; j++)
                {
                    if (j == 0)
                    {
                        milage[i] = 0;
                    }
                    else
                    {
                        milage[i] = milage[i] + distanceTable[population[i, j], population[i, j - 1]];
                    }

                }
                milage[i] = milage[i] + distanceTable[population[i, distance - 1], population[i, 0]];

                if (milage[i] < bestMilage)
                {
                    bestMilage = milage[i];
                    for (int a = 0; a < distance; a++)
                    {
                        distanceBest[a] = population[i, a];
                    }
                }
            }

            //main program loop
            for (int x = 0; x < iterations; x++)
            {
                //changing the selection here
                //populationTemp = SelectionRoulette(population, populationAmount, milage, distance, rnd);
                populationTemp = SelectionTournament(population, populationAmount, milage, distance, rnd);
                populationTemp = CrossingOX(populationTemp, populationAmount, distance, rnd);
                populationTemp = MutationInversion(populationTemp, populationAmount, distance, rnd);
                for (int i = 0; i < populationAmount; i++)
                {
                    for (int j = 0; j < distance; j++)
                    {
                        population[i, j] = populationTemp[i, j];
                    }
                }


                for (int i = 0; i < populationAmount; i++)
                {
                    for (int j = 0; j < distance; j++)
                    {
                        if (j == 0)
                        {
                            milage[i] = 0;
                        }
                        else
                        {
                            milage[i] = milage[i] + distanceTable[population[i, j], population[i, j - 1]];
                        }
                    }
                    milage[i] = milage[i] + distanceTable[population[i, distance - 1], population[i, 0]];
                    if (milage[i] < bestMilage)
                    {
                        bestMilage = milage[i];
                        for (int a = 0; a < distance; a++)
                        {
                            distanceBest[a] = population[i, a];
                        }
                    }
                }
            }
            //loop end

            for (int a = 0; a < distance; a++)
            {
                Console.Write(distanceBest[a] + "-");
            }
            Console.Write(" " + bestMilage);
            Console.ReadKey();
           
        }


    }


}