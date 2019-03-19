using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakalarkaDEMO
{
    class DimGame : IAIPlayable, ISpragueGrundy
    {
        public int CurrentChipCount { get; set; }
        public int[] PNPositionSG { get; set; }

        public DimGame(int numberOfChips)
        {
            this.CurrentChipCount = numberOfChips;
            FindPNSG();
        }

        public void FindPNSG()
        {
            
            PNPositionSG = new int[CurrentChipCount + 1];

            PNPositionSG[0] = 0;
            
            for (int i = 1; i < PNPositionSG.Length; i++)
            {

                List<int> verteces = new List<int>();

                for (int j = 1; j <= i; j++)
                {
                    if (i < j)
                    {
                        continue;
                    }
                    if (i % j == 0) verteces.Add(PNPositionSG[i - j]);

                }
                PNPositionSG[i] = FindMinimum(verteces);

            }
        }

        public int FindMinimum(List<int> list)
        {
            for (int i = 0; i <= list.Count; i++)
            {
                if (!list.Contains(i)) return i;

            }
            return -1;
        }
        public bool AIMove(double probabilityForOptimal)
        {
            if (probabilityForOptimal > 1 || probabilityForOptimal < 0)
            {
                return false;
            }

            List<int> optimalMoves = this.GetOptimalMoves();
            List<int> possibleMoves = this.GetPossibleMoves();

            //If there are no possible moves, terminate
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
                        CurrentChipCount -= possibleMoves.ElementAt(randomMove);
                    }
                    //Pokud optimalni tah neexistuje, provede prvni mozny tah
                    else
                    {
                        CurrentChipCount -= optimalMoves.ElementAt(0);
                    }
                }
                //AI provede prvni mozny tah
                else
                {
                    CurrentChipCount -= possibleMoves.ElementAt(randomMove);
                }
            }
                    
                
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            
            return true;
        }

        public int GetCurrentSGValue()
        {
            return this.PNPositionSG[CurrentChipCount];
        }

        public List<int> GetPossibleMoves()
        {
            List<int> result = new List<int>();

            for(int i = 1; i <= CurrentChipCount; i++)
            {
                if(CurrentChipCount % i == 0)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        public List<int> GetOptimalMoves()
        {
            List<int> possibleMoves = GetPossibleMoves();
            List<int> optimalMoves = new List<int>();

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (PNPositionSG[CurrentChipCount - possibleMoves.ElementAt(i)] == 0)
                {
                    optimalMoves.Add(possibleMoves.ElementAt(i));
                }
            }

            return optimalMoves;
        }
    }
}
