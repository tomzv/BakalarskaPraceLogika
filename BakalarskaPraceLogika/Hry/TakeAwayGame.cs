using System;
using System.Collections.Generic;
using System.Linq;

namespace bakalarkaDEMO
{
    class TakeAwayGame : IAIPlayable, ISpragueGrundy
    {
        public int[] SubstractionSet { get; set; }
        public int NumberOfChips { get; set; }
        public string[] PNPosition { get; set; }
        public int[] PNPositionSG { get; set; }
        public int CurrentChipCount { get; set; }

             
        public TakeAwayGame() { }

        public TakeAwayGame(int[] substractionSet, int numberOfChips)
        {
            this.SubstractionSet = substractionSet;
            this.NumberOfChips = numberOfChips;
            this.CurrentChipCount = numberOfChips;
            FindPNSG();
        }

        public void FindPN()
        {
            PNPosition = new string[NumberOfChips + 1];
            int firstTerminal = 0;
            PNPosition[firstTerminal] = "P";

            for (int i = 1; i < PNPosition.Length; i++)
            {

                for (int j = 0; j < SubstractionSet.Length; j++)
                {
                                        
                    if (SubstractionSet[j] > i)
                    {
                        PNPosition[i] = "P";
                        continue;
                    }
                    if (PNPosition[i - SubstractionSet[j]] == "P")
                    {
                        PNPosition[i] = "N";
                        break;
                    }
                    else
                    {
                        PNPosition[i] = "P";
                    }

                
                }

            }

        }

        public void FindPNSG()
        {
            
            PNPositionSG = new int[NumberOfChips + 1];
            PNPosition = new string[NumberOfChips + 1];

            for (int i =0; i < PNPositionSG.Length; i++)
            {
                PNPositionSG[i] = -1;
            }
            PNPositionSG[0] = 0; 


            for(int i = 1; i < PNPositionSG.Length; i++)
            {
                
                List<int> verteces = new List<int>();
                
                for (int j = 0; j < SubstractionSet.Length; j++)
                {
                    if(i < SubstractionSet[j])
                    {
                        continue;
                    }
                    
                    verteces.Add(PNPositionSG[i - SubstractionSet[j]]);
                                        
                }
                PNPositionSG[i] = FindMinimum(verteces);

            }

            for(int i = 0; i < PNPositionSG.Length; i++)
            {
                if (PNPositionSG[i] == 0)
                {
                    PNPosition[i] = "P";

                }
                else PNPosition[i] = "N";
            }

        }

        public int FindMinimum(List<int> list)
        {
            for(int i = 0; i <= list.Count; i++)
            {
                if (!list.Contains(i)) return i;

            }
            return -1;
        }
   
        public bool AIMove(double probabilityForOptimal)
        {
            if(probabilityForOptimal > 1 || probabilityForOptimal < 0)
            {
                return false;                
            }

            List<int> optimalMoves = this.GetOptimalMoves();
            List<int> possibleMoves = this.GetPossibleMoves();

            //Pokud neexistuje mozny tah, vrat false
            if (possibleMoves.Count == 0) return false;


            int seed = DateTime.Now.Second;
            Random rnd = new Random(seed);
            double move = rnd.NextDouble();
            int randomMove = rnd.Next(possibleMoves.Count);



            try
            {
                //AI provede prvni optimalni tah, ktery nalezne
                if (move <= probabilityForOptimal)
                {
                    if(optimalMoves.Count == 0)
                    {
                        this.CurrentChipCount -= possibleMoves.ElementAt(randomMove);
                    }
                    //Pokud optimalni tah neexistuje, provede prvni mozny tah
                    else
                    {
                        this.CurrentChipCount -= optimalMoves.ElementAt(0);
                    }
                    return true;
                }
                //AI provede prvni mozny tah
                else
                {
                    

                    this.CurrentChipCount -= possibleMoves.ElementAt(randomMove);

                    return true;
                }
            }
            catch(IndexOutOfRangeException)
            {
                return false;
            }

                                   
        }

        public int GetCurrentSGValue()
        {
            return this.PNPositionSG[CurrentChipCount];
        }

        public List<int> GetPossibleMoves()
        {
            List<int> result = new List<int>();

            foreach(int x in this.SubstractionSet)
            {
                if( x <= this.CurrentChipCount)
                {
                    result.Add(x);
                }
            }
            return result;
        }

        public List<int> GetOptimalMoves()
        {
            List<int> possibleMoves = this.GetPossibleMoves();
            List<int> optimalMoves = new List<int>();

            for(int i = 0; i < possibleMoves.Count; i++)
            {
                if(this.PNPositionSG[this.CurrentChipCount - possibleMoves.ElementAt(i)] == 0)
                {
                    optimalMoves.Add(possibleMoves.ElementAt(i));
                }
            }

            return optimalMoves;
        }
    }
}
