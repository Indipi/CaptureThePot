using System;

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

            int A_pos = 0; // position of A, number between 0 - 50 (Symmetrical game so only need to test half)
            int B_pos = 0; // position of B, number between 0 - 100
            int C_pos = 0; // position of C, number between 0 - 100
            int D_pos = 0; // position of D, number between 0 - 100

            // Positions of each player for the current best capture for A
            int bestAPos = 0;
            int bestBPos = 0;
            int bestCPos = 0;
            int bestDPos = 0;

            // The area captured by each player for the optimal position of A
            int areaCapturedA = 0;
            int areaCapturedB = 0;
            int areaCapturedC = 0;
            int areaCapturedD = 0;


            // Positions of each later player for the current best capture for B ( for a given A)
            int bestCPosLevel2 = 0;
            int bestDPosLevel2 = 0;

            // The area captured by each later player for the optimal position of B (for a given A)
            int areaCapturedCLevel2 = 0;
            int areaCapturedDLevel2 = 0;

            // Positions of each later player for the current best capture for C ( for a given A, B)
            int bestDPosLevel3 = 0;

            // The area captured by each later player for the optimal position of C (for a given A, B)
            int areaCapturedDLevel3 = 0;


            // Best positions for each player for given current positions of previous players
            int LocalBestBPos = 0;
            int LocalBestCPos = 0;
            int LocalBestDPos = 0;

            // The area captured for the optimal position of current player given previous player positions
            int LocalAreaCapturedB = 0;
            int LocalAreaCapturedC = 0;
            int LocalAreaCapturedD = 0;


            // FindArea function return temporary variable
            int temporaryArea = 0;

            // Brute force method. Test every value including for final player. We could caculate the best position for the final player but, while it would 
            // be faster, it is also less simple to write (takes longer, more complex, and thus more likely I, the programmer will make a mistake.) 
            // As a rule the first iteration of any completely new code (especially for tricky problems) should be kept as simple as possible and once it works then 
            // it can be made faster. We are only doing 4 interations deep here at the moment anyway so speed is not an issue here.

            // For each player keep track of the best value
            // Compare new value after position assignment and overwrite if better.

            // So for example ... for a given value of A we find the best values for B, C, D
            // then after their loops (before end of A loop) we check the captured area for A
            // If it's better than the current best area for A then we make it the new best area
            for (A_pos = 0; A_pos <= 50; A_pos++)
            {
                LocalBestBPos = 0;
                LocalAreaCapturedB = 0;
                bestCPosLevel2 = 0;
                areaCapturedCLevel2 = 0;
                bestDPosLevel2 = 0;
                areaCapturedDLevel2 = 0;
                //The following nested loops select the best B, C, D for this A
                for (B_pos = 0; B_pos <= 100; B_pos++)
                {
                    if (B_pos == A_pos)
                    {
                        continue;
                    }

                    LocalBestCPos = 0;
                    LocalAreaCapturedC = 0;
                    areaCapturedDLevel3 = 0;
                    bestDPosLevel3 = 0;
                    for (C_pos = 0; C_pos <=100; C_pos++)
                    {
                        if (C_pos == A_pos)
                        {
                            continue;
                        }
                        if (C_pos == B_pos)
                        {
                            continue;
                        }

                        LocalBestDPos = 0;
                        LocalAreaCapturedD = 0;
                        for (D_pos = 0; D_pos <= 100; D_pos++)
                        {
                            if (D_pos == A_pos)
                            {
                                continue;
                            }
                            if (D_pos == B_pos)
                            {
                                continue;
                            }
                            if (D_pos == C_pos)
                            {
                                continue;
                            }

                            // Given the A, B, C, D, positions find the captured area for D
                            temporaryArea = FindArea(4, A_pos, B_pos, C_pos, D_pos);

                            if(temporaryArea > LocalAreaCapturedD)
                            {
                                LocalAreaCapturedD = temporaryArea;
                                LocalBestDPos = D_pos;
                            }
                            else if(temporaryArea == LocalAreaCapturedD)
                            {
                                //multiple best positions
                                // AAAAAAHHHHH
                                //TODO
                            }

                        }

                        // Given the A, B, C, D, positions find the captured area for C
                        temporaryArea = FindArea(3, A_pos, B_pos, C_pos, LocalBestDPos);

                        if (temporaryArea > LocalAreaCapturedC)
                        {
                            LocalAreaCapturedC = temporaryArea;
                            LocalBestCPos = C_pos;

                            areaCapturedDLevel3 = LocalAreaCapturedD;
                            bestDPosLevel3 = LocalBestDPos;

                        }
                        else if (temporaryArea == LocalAreaCapturedC)
                        {
                            //multiple best positions
                            // AAAAAAHHHHH
                            //TODO
                        }


                    }

                    // Given the A, B, C, D, positions find the captured area for B
                    temporaryArea = FindArea(2, A_pos, B_pos, LocalBestCPos, bestDPosLevel3);

                    if (temporaryArea > LocalAreaCapturedB)
                    {
                        LocalAreaCapturedB = temporaryArea;
                        LocalBestBPos = B_pos;

                        areaCapturedCLevel2 = LocalAreaCapturedC;
                        bestCPosLevel2 = LocalBestCPos;

                        areaCapturedDLevel2 = areaCapturedDLevel3;
                        bestDPosLevel2 = bestDPosLevel3;

                    }
                    else if (temporaryArea == LocalAreaCapturedC)
                    {
                        //multiple best positions
                        // AAAAAAHHHHH
                        //TODO
                    }

                }

                // Given the A, B, C, D, positions find the captured area for A
                temporaryArea = FindArea(1, A_pos, LocalBestBPos, bestCPosLevel2, bestDPosLevel2);

                if (temporaryArea > areaCapturedA)
                {
                    areaCapturedA = temporaryArea;
                    bestAPos = A_pos;

                    areaCapturedB = LocalAreaCapturedB;
                    bestBPos = LocalBestBPos;

                    areaCapturedC = areaCapturedCLevel2;
                    bestCPos = bestCPosLevel2;

                    areaCapturedD = areaCapturedDLevel2;
                    bestDPos = bestDPosLevel2;

                }
                else if (temporaryArea == LocalAreaCapturedC)
                {
                    // multiple best positions
                    // For A we don't really care as there are no higher levels to impact
                    // Let's just keep the current one as best
                }

            }

            Console.WriteLine("Got it...\n");
            Console.WriteLine("Best position is: ");
            Console.WriteLine(bestAPos);
            Console.WriteLine("\nWith captured area of: ");
            Console.WriteLine(areaCapturedA);
            Console.WriteLine("\nPositions of others are respectively (B-D): ");
            Console.WriteLine(bestBPos);
            Console.WriteLine(bestCPos);
            Console.WriteLine(bestDPos);
            Console.WriteLine("\nWith captured area of: ");
            Console.WriteLine(areaCapturedB);
            Console.WriteLine(areaCapturedC);
            Console.WriteLine(areaCapturedD);
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
