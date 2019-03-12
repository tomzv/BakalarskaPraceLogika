using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakalarkaDEMO
{
    class EvenOddGame : IAIPlayable, ISpragueGrundy
    {
        public int[] PNPositionSG { get; set; }
        public int CurrentChipCount { get; set; }

        public EvenOddGame(int numberOfChips)
        {
            this.CurrentChipCount = numberOfChips;
            FindPNSG();
        }

        public void FindPNSG()
        {
            if (CurrentChipCount < 1) return;

            PNPositionSG = new int[CurrentChipCount + 1];


            PNPositionSG[0] = 0;
            PNPositionSG[1] = 1;
            PNPositionSG[2] = 0;


            for (int i = 3; i < PNPositionSG.Length; i++)
            {

                List<int> verteces = new List<int>();

                for (int j = 2; j <= i; j++)
                {
                    if (j % 2 == 0 && j != i)
                    {
                        verteces.Add(PNPositionSG[i - j]);
                        continue;
                    }
                    if(j % 2 == 1 && j == i)
                    {
                        verteces.Add(PNPositionSG[i - j]);
                    }
                        

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

            

            try
            {
                //AI provede prvni optimalni tah, ktery nalezne
                if (move <= probabilityForOptimal)
                {
                    if(optimalMoves.Count == 0)
                    {
                        CurrentChipCount -= possibleMoves.ElementAt(0);
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

                    CurrentChipCount -= possibleMoves.ElementAt(0);
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
                if(i % 2 == 0 && i != CurrentChipCount)
                {
                    result.Add(i);
                }
                else if(i % 2 == 1 && i == CurrentChipCount)
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
