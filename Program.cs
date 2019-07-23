using System;
using System.Collections.Generic;

namespace CaptureThePot
{

    public class Position
    {
        public double A_Position { get; set; }
        public double B_Position { get; set; }
        public double C_Position { get; set; }
        public double D_Position { get; set; }
        public double A_Area { get; set; }
        public double B_Area { get; set; }
        public double C_Area { get; set; }
        public double D_Area { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            /* This program solves the following problem...
             * 
             * Three players A, B, C play the following game. First, A picks a real number between 0 and 1 (both inclusive), then B picks a number in the same 
             * range (different from A’s choice) and finally C picks a number, also in the same range, (different from the two chosen numbers). We then pick a 
             * number in the range uniformly randomly. Whoever’s number is closest to this random number wins the game. Assume that A, B and C all play optimally 
             * and their sole goal is to maximise their chances of winning. Also assume that if one of them has several optimal choices, then that player will 
             * randomly pick one of the optimal choices.
             * If A chooses 0, then what is the best choice for B?
             * What is the best choice for A?
             * Can you write a program to figure out the best choice for the first player when the game is played among four players?
             * 
             * */

            Console.WriteLine("Thinking...");

            // for 4 players using a resolution of 0 to 100 should be sufficient

            // Problem specifies any real number between 0 and 1. This program selects an integer for each player (saved as type double) between 0 and the RESOLUTION
            // The higher the resolution the more accurate the output however the longer the program will take.
            // Before printing the program converts the answer back so it lies at the correct position between 0 and 1.
            const int RESOLUTION = 100;

            double A_position; // position of A, number between 0 - RESOLUTION / 2 (Symmetrical game so only need to test half - note at end specifies mirrored positions also work)
            double B_position; // position of B, number between 0 - RESOLUTION
            double C_position; // position of C, number between 0 - RESOLUTION
            double D_position; // position of D, number between 0 - RESOLUTION

            double bestAArea = 0;
            List<Position> Best_A_Position_List = new List<Position>();
            int NumberOfBestAPositions = 1;

            for (A_position = 0; A_position <= (RESOLUTION / 2); A_position++)
            {
                double bestBArea = 0;
                List<Position> Best_B_Position_List = new List<Position>();
                int NumberOfBestBPositions = 1;

                Console.Clear();
                Console.WriteLine("Thinking...");
                Console.WriteLine((RESOLUTION / 2) - A_position);

                for (B_position = 0; B_position <= RESOLUTION; B_position++)
                {
                    if (B_position == A_position)
                    {
                        continue;
                    }

                    double bestCArea = 0;
                    List<Position> Best_C_Position_List = new List<Position>();
                    int NumberOfBestCPositions = 1;

                    for (C_position = 0; C_position <= RESOLUTION; C_position++)
                    {
                        if (C_position == A_position)
                        {
                            continue;
                        }
                        if (C_position == B_position)
                        {
                            continue;
                        }

                        double bestDArea = 0;
                        List<Position> Best_D_Position_List = new List<Position>();

                        int NumberOfBestDPositions = 1;

                        for (D_position = 0; D_position <= RESOLUTION; D_position++)
                        {
                            if (D_position == A_position)
                            {
                                continue;
                            }
                            if (D_position == B_position)
                            {
                                continue;
                            }
                            if (D_position == C_position)
                            {
                                continue;
                            }

                            double areaD = FindArea(4, A_position, B_position, C_position, D_position, RESOLUTION);

                            Position position = new Position();
                            position.A_Position = A_position;
                            position.B_Position = B_position;
                            position.C_Position = C_position;
                            position.D_Position = D_position;
                            position.A_Area = FindArea(1, A_position, B_position, C_position, D_position, RESOLUTION);
                            position.B_Area = FindArea(2, A_position, B_position, C_position, D_position, RESOLUTION);
                            position.C_Area = FindArea(3, A_position, B_position, C_position, D_position, RESOLUTION);
                            position.D_Area = areaD;

                            if (areaD > bestDArea)
                            {
                                NumberOfBestDPositions = 1;

                                // Clear D Position List
                                // Then add the current position to the list
                                Best_D_Position_List.Clear();
                                Best_D_Position_List.Add(position);
                                bestDArea = areaD;
                            }
                            else if (areaD == bestDArea)
                            {
                                // Multiple Best Positions
                                // Add current position to the D Position List
                                Best_D_Position_List.Add(position);
                                NumberOfBestDPositions = NumberOfBestDPositions + 1;
                            }
                        }

                        double areaC = 0;
                        foreach(Position p in Best_D_Position_List)
                        {
                            areaC = areaC + p.C_Area;
                        }

                        areaC = areaC / NumberOfBestDPositions;

                        if (areaC > bestCArea)
                        {
                            // Clear the C Position List
                            // Then set entire Cposition List equal to D position List
                            Best_C_Position_List.Clear();
                            Best_C_Position_List.AddRange(Best_D_Position_List);
                            NumberOfBestCPositions = NumberOfBestDPositions;
                            bestCArea = areaC;
                        }
                        else if (areaC == bestCArea)
                        {
                            // Multiple Best Positions
                            // Add entire D position list to end of C position List
                            Best_C_Position_List.AddRange(Best_D_Position_List);
                            NumberOfBestCPositions = NumberOfBestCPositions + NumberOfBestDPositions;
                        }
                    }

                    double areaB = 0;
                    foreach (Position p in Best_C_Position_List)
                    {
                        areaB = areaB + p.B_Area;
                    }

                    areaB = areaB / NumberOfBestCPositions;

                    if (areaB > bestBArea)
                    {
                        // Clear the B Position List
                        // Then set entire B position List equal to C position List
                        Best_B_Position_List.Clear();
                        Best_B_Position_List.AddRange(Best_C_Position_List);
                        NumberOfBestBPositions = NumberOfBestCPositions;
                        bestBArea = areaB;

                    }
                    else if (areaB == bestBArea)
                    {
                        // Multiple Best Positions
                        // Add entire C position list to end of B position List
                        Best_B_Position_List.AddRange(Best_C_Position_List);
                        NumberOfBestBPositions = NumberOfBestBPositions + NumberOfBestCPositions;
                    }
                }

                double areaA = 0;
                foreach (Position p in Best_B_Position_List)
                {
                    areaA = areaA + p.A_Area;
                }

                areaA = areaA / NumberOfBestBPositions;

                if (areaA > bestAArea)
                {
                    // Clear the A Position List
                    // Then set entire A position List equal to B position List
                    Best_A_Position_List.Clear();
                    Best_A_Position_List.AddRange(Best_B_Position_List);
                    NumberOfBestAPositions = NumberOfBestBPositions;
                    bestAArea = areaA;

                }
                else if (areaA == bestAArea)
                {
                    // Multiple Best Positions
                    // Add entire B position list to end of A position List
                    Best_A_Position_List.AddRange(Best_B_Position_List);
                    NumberOfBestAPositions = NumberOfBestAPositions + NumberOfBestBPositions;
                }

            }

            double uniqueAPos = 0;
            int countPosA = 0;
            foreach (Position p in Best_A_Position_List)
            {
                if (uniqueAPos != p.A_Position)
                {
                    countPosA++;
                    uniqueAPos = p.A_Position;
                }
            }

            Console.Write("Number of possible positions: ");
            Console.WriteLine(countPosA);
            Console.WriteLine("Printing all possible best A positions...");
            Console.WriteLine("Best position(s) are: ");

            uniqueAPos = 0;
            foreach (Position p in Best_A_Position_List)
            {
                if (uniqueAPos != p.A_Position)
                {
                    // Convert back to a value between 0 and 1 based on RESOLUTION and print
                    Console.WriteLine((p.A_Position) / RESOLUTION);
                    uniqueAPos = p.A_Position;
                }
            }

            Console.Write("With an expected captured area of: ");
            Console.Write((bestAArea) / RESOLUTION);
            Console.WriteLine("\n");

            Console.WriteLine("Note: Mirrored positions also work equally well");

            // Have left the below code in for debugging purposes
            /*Console.Write("Printing all positions... ");
            foreach (Position p in Best_A_Position_List)
            {
                Console.Write("\nPosition A: ");
                Console.Write(p.A_Position / RESOLUTION);
                Console.Write("\nPosition B: ");
                Console.Write(p.B_Position / RESOLUTION);
                Console.Write("\nPosition C: ");
                Console.Write(p.C_Position / RESOLUTION);
                Console.Write("\nPosition D: ");
                Console.Write(p.D_Position / RESOLUTION);
                Console.Write("\nArea A: ");
                Console.Write(p.A_Area / RESOLUTION);
                Console.Write("\nArea B: ");
                Console.Write(p.B_Area / RESOLUTION);
                Console.Write("\nArea C: ");
                Console.Write(p.C_Area / RESOLUTION);
                Console.Write("\nArea D: ");
                Console.Write(p.D_Area / RESOLUTION);
            }
            */

        }


        //This function takes a player and 4 positions (non-ordered) and returns the captured area for that player
        static double FindArea(int player, double pos1, double pos2, double pos3, double pos4, int RESOLUTION)
        {
            // player is 1, 2, 3, or 4 and refers to the player whose area we want to know
            // Player n is at posn

            double area = 0;

            double[] arr = new double[] {pos1, pos2, pos3, pos4};

            double position = arr[player - 1];

            // Sort array in ascending order. 
            Array.Sort(arr);

            for(int i = 0; i < 4; i++)
            {
                if(position == arr[i])
                {
                    if(i == 0)
                    {
                        area = arr[i] + ((arr[i + 1] - arr[i]) / 2);
                    }
                    else if(i == 3)
                    {
                        area = (RESOLUTION - arr[i]) + ((arr[i] - arr[i - 1]) / 2);
                    }
                    else
                    {
                        area = ((arr[i] - arr[i - 1]) / 2) + ((arr[i + 1] - arr[i]) / 2);
                    }
                }
            }

            return area;

        }


    }
}
