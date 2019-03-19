using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakalarkaDEMO
{
    class SubstractionSumGame : IAIPlayable
    {
        public ISpragueGrundy[] games { get; set; }
        public int CurrentChipCount { get; set; }
        public int[] PNPositionSG { get; set; }

        public SubstractionSumGame()
        {
            
        }

        public List<int> GetOptimalMoves()
        {
            
            List<int> optimalMoves = new List<int>();
            for (int i = 0; i < games.Length; i++)
            {
                optimalMoves.Add(0);
            }

            int result = 0;
            try
            {
                
                 //Pro kazdou z her
                 for (int i = 0; i < games.Length; i++)
                 {
                     //Pro kazdy mozny tah 
                     for (int j = 0; j < games[i].GetPossibleMoves().Count; j++)
                     {
                         //Hodnota SG funkce po jednom z moznych tahu
                         result = games[i].PNPositionSG[games[i].CurrentChipCount - games[i].GetPossibleMoves()[j]];
                         //NIM sum te hodnoty XOR SG hodnoty ostatnich her
                         for (int k = 0; k < games.Length; k++)
                         {
                             if (k == i)
                             {
                                 continue;
                             }
                             result ^= games[k].GetCurrentSGValue();

                         }
                         //Pokud je NIM sum 0, najdi dalsi optimalni tah, ale v jine hre
                         if (result == 0)
                         {
                             optimalMoves.RemoveAt(i);
                             optimalMoves.Insert(i, games[i].GetPossibleMoves()[j]);
                             break;
                         }

                         optimalMoves.RemoveAt(i);
                         optimalMoves.Insert(i, 0);

                     }
                 }
                
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Chyba, index out of range");
                return new List<int>();
            }

            return optimalMoves;
        }
        public bool AIMove(double probabilityForOptimal)
        {
            if (probabilityForOptimal > 1 || probabilityForOptimal < 0)
            {
                return false;
            }
           
            GetOptimalMoves();

            int seed = DateTime.Now.Second;
            Random rnd = new Random(seed);
            double move = rnd.NextDouble();

            List<int> optimalMoves = GetOptimalMoves();

            try
            {
                if (probabilityForOptimal >= move)
                {
                    
                    for (int i = 0; i < games.Length; i++)
                    {
                        if(optimalMoves.ElementAt(i) != 0 && games[i].CurrentChipCount != 0)
                        {
                            games[i].CurrentChipCount -= optimalMoves.ElementAt(i);
                            return true;
                        }
                    }
                    for(int i = 0; i < games.Length; i++)
                    {
                        if (games[i].GetPossibleMoves().Count != 0)
                        {
                            int randomMove = rnd.Next(games[i].GetPossibleMoves().Count);
                            games[i].CurrentChipCount -= games[i].GetPossibleMoves()[randomMove];
                            return true;
                        }
                    }
                }
                else
                {
                    
                    for (int i = 0; i < games.Length; i++)
                    {
                        if (games[i].GetPossibleMoves().Count != 0)
                        {
                            int randomMove = rnd.Next(games[i].GetPossibleMoves().Count);
                            games[i].CurrentChipCount -= games[i].GetPossibleMoves()[randomMove];
                            return true;
                        }
                    }
                }
                    
            }
            catch(IndexOutOfRangeException)
            {
                Console.WriteLine("Chyba, index out of range");
                return false;
            }

            return false;
        }
              
        public int GetCurrentSGValue()
        {
            int result = 0;

            for(int i = 0; i < games.Length; i++)
            {
                games[i].FindPNSG();
                result ^= games[i].GetCurrentSGValue();
            }

            return result;
        }
              
       
    }
}
