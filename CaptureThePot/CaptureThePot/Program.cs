﻿using System;

namespace CaptureThePot
{
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

            int A_position = 0; // position of A, number between 0 - 50 (Symmetrical game so only need to test half)
            int B_position = 0; // position of B, number between 0 - 100
            int C_position = 0; // position of C, number between 0 - 100
            int D_position = 0; // position of D, number between 0 - 100

            int areaA = 0;
            int areaB = 0;
            int areaC = 0;
            int areaD = 0;

            const int INDEX_Row = 0;
            const int INDEX_APos = 1;
            const int INDEX_BPos = 2;
            const int INDEX_CPos = 3;
            const int INDEX_DPos = 4;
            const int INDEX_AreaA = 5;
            const int INDEX_AreaB = 6;
            const int INDEX_AreaC = 7;
            const int INDEX_AreaD = 8;

            int bestAArea = 0;
            int[,] BestAPositionArray = new int[1000, 9];
            int NumberOfBestAPositions = 1;
            int RowsA = 1;

            for (A_position = 0; A_position <= 100; A_position++)
            {
                int bestBArea = 0;
                int[,] BestBPositionArray = new int[10000, 9];
                int NumberOfBestBPositions = 1;
                int RowsB = 1;

                for (B_position = 0; B_position <= 100; B_position++)
                {
                    if (B_position == A_position)
                    {
                        continue;
                    }

                    int bestCArea = 0;
                    int[,] BestCPositionArray = new int[1000, 9];
                    int NumberOfBestCPositions = 1;
                    int RowsC = 1;

                    for (C_position = 0; C_position <= 100; C_position++)
                    {
                        if (C_position == A_position)
                        {
                            continue;
                        }
                        if (C_position == B_position)
                        {
                            continue;
                        }

                        int bestDArea = 0;
                        int[,] BestDPositionArray = new int[101, 9];
                        int NumberOfBestDPositions = 1;

                        for (D_position = 0; D_position <= 100; D_position++)
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

                            areaA = FindArea(1, A_position, B_position, C_position, D_position);
                            areaB = FindArea(2, A_position, B_position, C_position, D_position);
                            areaC = FindArea(3, A_position, B_position, C_position, D_position);
                            areaD = FindArea(4, A_position, B_position, C_position, D_position);

                            if (areaD > bestDArea)
                            {
                                Array.Clear(BestDPositionArray, 0, BestDPositionArray.Length);
                                NumberOfBestDPositions = 1;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_Row] = NumberOfBestDPositions;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_APos] = A_position;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_BPos] = B_position;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_CPos] = C_position;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_DPos] = D_position;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_AreaA] = areaA;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_AreaB] = areaB;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_AreaC] = areaC;
                                BestDPositionArray[NumberOfBestDPositions - 1, INDEX_AreaD] = areaD;

                                bestDArea = areaD;
                            }
                            else if (areaD == bestDArea)
                            {
                                // Multiple Best Positions
                                BestDPositionArray[NumberOfBestDPositions, INDEX_Row] = NumberOfBestDPositions;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_APos] = A_position;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_BPos] = B_position;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_CPos] = C_position;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_DPos] = D_position;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_AreaA] = areaA;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_AreaB] = areaB;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_AreaC] = areaC;
                                BestDPositionArray[NumberOfBestDPositions, INDEX_AreaD] = areaD;
                                NumberOfBestDPositions = NumberOfBestDPositions + 1;
                            }
                        }

                        areaC = 0;
                        for (int i = 0; i < NumberOfBestDPositions; i++)
                        {
                            areaC = areaC + BestDPositionArray[i, INDEX_AreaC];
                        }

                        areaC = areaC / NumberOfBestDPositions;

                        if (areaC > bestCArea)
                        {
                            Array.Clear(BestCPositionArray, 0, BestCPositionArray.Length);
                            NumberOfBestCPositions = 1;

                            for (int i = 0; i < NumberOfBestDPositions; i++)
                            {
                                BestCPositionArray[i, INDEX_Row] = i + 1;
                                BestCPositionArray[i, INDEX_APos] = BestDPositionArray[i, INDEX_APos];
                                BestCPositionArray[i, INDEX_BPos] = BestDPositionArray[i, INDEX_BPos];
                                BestCPositionArray[i, INDEX_CPos] = BestDPositionArray[i, INDEX_CPos];
                                BestCPositionArray[i, INDEX_DPos] = BestDPositionArray[i, INDEX_DPos];
                                BestCPositionArray[i, INDEX_AreaA] = BestDPositionArray[i, INDEX_AreaA];
                                BestCPositionArray[i, INDEX_AreaB] = BestDPositionArray[i, INDEX_AreaB];
                                BestCPositionArray[i, INDEX_AreaC] = BestDPositionArray[i, INDEX_AreaC];
                                BestCPositionArray[i, INDEX_AreaD] = BestDPositionArray[i, INDEX_AreaD];
                                RowsC = i + 1;
                            }

                            bestCArea = areaC;

                        }
                        else if (areaC == bestCArea)
                        {
                            // Multiple Best Positions
                            for (int i = RowsC; i < NumberOfBestCPositions; i++)
                            {
                                for(int j = 0; j < NumberOfBestDPositions; j++)
                                {
                                    BestCPositionArray[i + j, INDEX_Row] = i + 1;
                                    BestCPositionArray[i + j, INDEX_APos] = BestDPositionArray[j, INDEX_APos];
                                    BestCPositionArray[i + j, INDEX_BPos] = BestDPositionArray[j, INDEX_BPos];
                                    BestCPositionArray[i + j, INDEX_CPos] = BestDPositionArray[j, INDEX_CPos];
                                    BestCPositionArray[i + j, INDEX_DPos] = BestDPositionArray[j, INDEX_DPos];
                                    BestCPositionArray[i + j, INDEX_AreaA] = BestDPositionArray[j, INDEX_AreaA];
                                    BestCPositionArray[i + j, INDEX_AreaB] = BestDPositionArray[j, INDEX_AreaB];
                                    BestCPositionArray[i + j, INDEX_AreaC] = BestDPositionArray[j, INDEX_AreaC];
                                    BestCPositionArray[i + j, INDEX_AreaD] = BestDPositionArray[j, INDEX_AreaD];
                                    RowsC = RowsC + 1;
                                }
                                
                            }
                            NumberOfBestCPositions = NumberOfBestCPositions + 1;
                        }
                    }

                    NumberOfBestCPositions = RowsC;

                    areaB = 0;
                    for (int i = 0; i < NumberOfBestCPositions; i++)
                    {
                        areaB = areaB + BestCPositionArray[i, INDEX_AreaB];
                    }

                    areaB = areaB / NumberOfBestCPositions;

                    if (areaB > bestBArea)
                    {
                        Array.Clear(BestBPositionArray, 0, BestBPositionArray.Length);
                        NumberOfBestBPositions = 1;

                        for (int i = 0; i < NumberOfBestCPositions; i++)
                        {
                            BestBPositionArray[i, INDEX_Row] = i;
                            BestBPositionArray[i, INDEX_APos] = BestCPositionArray[i, INDEX_APos];
                            BestBPositionArray[i, INDEX_BPos] = BestCPositionArray[i, INDEX_BPos];
                            BestBPositionArray[i, INDEX_CPos] = BestCPositionArray[i, INDEX_CPos];
                            BestBPositionArray[i, INDEX_DPos] = BestCPositionArray[i, INDEX_DPos];
                            BestBPositionArray[i, INDEX_AreaA] = BestCPositionArray[i, INDEX_AreaA];
                            BestBPositionArray[i, INDEX_AreaB] = BestCPositionArray[i, INDEX_AreaB];
                            BestBPositionArray[i, INDEX_AreaC] = BestCPositionArray[i, INDEX_AreaC];
                            BestBPositionArray[i, INDEX_AreaD] = BestCPositionArray[i, INDEX_AreaD];
                            RowsB = i + 1;
                        }

                        bestBArea = areaB;

                    }
                    else if (areaB == bestBArea)
                    {
                        // Multiple Best Positions
                        for (int i = RowsB; i < NumberOfBestBPositions; i++)
                        {
                            for (int j = 0; j < NumberOfBestCPositions; j++)
                            {
                                BestBPositionArray[i + j, INDEX_Row] = i + 1;
                                BestBPositionArray[i + j, INDEX_APos] = BestCPositionArray[j, INDEX_APos];
                                BestBPositionArray[i + j, INDEX_BPos] = BestCPositionArray[j, INDEX_BPos];
                                BestBPositionArray[i + j, INDEX_CPos] = BestCPositionArray[j, INDEX_CPos];
                                BestBPositionArray[i + j, INDEX_DPos] = BestCPositionArray[j, INDEX_DPos];
                                BestBPositionArray[i + j, INDEX_AreaA] = BestCPositionArray[j, INDEX_AreaA];
                                BestBPositionArray[i + j, INDEX_AreaB] = BestCPositionArray[j, INDEX_AreaB];
                                BestBPositionArray[i + j, INDEX_AreaC] = BestCPositionArray[j, INDEX_AreaC];
                                BestBPositionArray[i + j, INDEX_AreaD] = BestCPositionArray[j, INDEX_AreaD];
                                RowsB = RowsB + 1;
                            }

                        }
                        NumberOfBestBPositions = NumberOfBestBPositions + 1;
                    }
                }

                NumberOfBestBPositions = RowsB;

                areaA = 0;
                for (int i = 0; i < NumberOfBestBPositions; i++)
                {
                    areaA = areaA + BestBPositionArray[i, INDEX_AreaA];
                }

                areaA = areaA / NumberOfBestBPositions;

                if (areaA > bestAArea)
                {
                    Array.Clear(BestAPositionArray, 0, BestAPositionArray.Length);
                    NumberOfBestAPositions = 1;

                    for (int i = 0; i < NumberOfBestBPositions; i++)
                    {
                        BestAPositionArray[i, INDEX_Row] = i;
                        BestAPositionArray[i, INDEX_APos] = BestBPositionArray[i, INDEX_APos];
                        BestAPositionArray[i, INDEX_BPos] = BestBPositionArray[i, INDEX_BPos];
                        BestAPositionArray[i, INDEX_CPos] = BestBPositionArray[i, INDEX_CPos];
                        BestAPositionArray[i, INDEX_DPos] = BestBPositionArray[i, INDEX_DPos];
                        BestAPositionArray[i, INDEX_AreaA] = BestBPositionArray[i, INDEX_AreaA];
                        BestAPositionArray[i, INDEX_AreaB] = BestBPositionArray[i, INDEX_AreaB];
                        BestAPositionArray[i, INDEX_AreaC] = BestBPositionArray[i, INDEX_AreaC];
                        BestAPositionArray[i, INDEX_AreaD] = BestBPositionArray[i, INDEX_AreaD];
                        RowsA = i + 1;
                    }

                    bestAArea = areaA;

                }
                else if (areaA == bestAArea)
                {
                    // Multiple Best Positions
                    for (int i = RowsA; i < NumberOfBestAPositions; i++)
                    {
                        for (int j = 0; j < NumberOfBestBPositions; j++)
                        {
                            BestAPositionArray[i + j, INDEX_Row] = i + 1;
                            BestAPositionArray[i + j, INDEX_APos] = BestBPositionArray[j, INDEX_APos];
                            BestAPositionArray[i + j, INDEX_BPos] = BestBPositionArray[j, INDEX_BPos];
                            BestAPositionArray[i + j, INDEX_CPos] = BestBPositionArray[j, INDEX_CPos];
                            BestAPositionArray[i + j, INDEX_DPos] = BestBPositionArray[j, INDEX_DPos];
                            BestAPositionArray[i + j, INDEX_AreaA] = BestBPositionArray[j, INDEX_AreaA];
                            BestAPositionArray[i + j, INDEX_AreaB] = BestBPositionArray[j, INDEX_AreaB];
                            BestAPositionArray[i + j, INDEX_AreaC] = BestBPositionArray[j, INDEX_AreaC];
                            BestAPositionArray[i + j, INDEX_AreaD] = BestBPositionArray[j, INDEX_AreaD];
                            RowsA = RowsA + 1;
                        }

                    }
                    NumberOfBestAPositions = NumberOfBestAPositions + 1;
                }

            }

            if(NumberOfBestAPositions == 1)
            {
                Console.WriteLine("One best position for Player A...");
                Console.Write("Best position is: ");
                Console.WriteLine(BestAPositionArray[0, INDEX_APos]);
                Console.Write("\n");
                Console.Write("With an expected captured area of: ");
                Console.WriteLine(BestAPositionArray[0, INDEX_AreaA]);
            }
            //else
            //{
                Console.Write("Printing all possible best positions...");
                Console.WriteLine(RowsA);
                for (int i = 0; i < RowsA; i++)
                {
                    Console.Write("Index:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_Row]);
                    Console.Write("APos:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_APos]);
                    Console.Write("BPos:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_BPos]);
                    Console.Write("CPos:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_CPos]);
                    Console.Write("DPos:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_DPos]);
                    Console.Write("AreaA:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_AreaA]);
                    Console.Write("AreaB:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_AreaB]);
                    Console.Write("AreaC:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_AreaC]);
                    Console.Write("AreaD:");
                    Console.WriteLine(BestAPositionArray[i, INDEX_AreaD]);

                    Console.WriteLine("\n");
                }
            //}

        }


        //This function takes a player and 4 positions (non-ordered) and returns the captured area for that player
        static int FindArea(int player, int pos1, int pos2, int pos3, int pos4)
        {
            // player is 1, 2, 3, or 4 and refers to the player whose area we want to know
            // Player n is at posn

            int area = 0;

            int[] arr = new int[] {pos1, pos2, pos3, pos4};

            int position = arr[player - 1];

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
                        area = (100 - arr[i]) + ((arr[i] - arr[i - 1]) / 2);
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
