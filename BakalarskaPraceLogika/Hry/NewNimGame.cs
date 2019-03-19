using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakalarkaDEMO
{
    class NewNimGame : IAIPlayable, ISpragueGrundy
    {
        public int[] PNPositionSG { get; set; }
        public int CurrentChipCount { get; set; }


        public NewNimGame(int numberOfChips)
        {
            this.CurrentChipCount = numberOfChips;
            FindPNSG();
        }
        public bool AIMove(double probabilityForOptimal)
        {
            if (probabilityForOptimal > 1 || probabilityForOptimal < 0)
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
                    CurrentChipCount -= CurrentChipCount;
                    
                }
                //AI odebere nahodny pocet chipu
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

        public void FindPNSG()
        {
            PNPositionSG = new int[CurrentChipCount];
            for (int i = 0; i < PNPositionSG.Length; i++)
            {
                PNPositionSG[i] = i;
            }
        }

        public int GetCurrentSGValue()
        {
            return CurrentChipCount;
        }

        public List<int> GetPossibleMoves()
        {
            List<int> result = new List<int>();

            for(int i = 1; i <= CurrentChipCount; i++)
            {
                result.Add(i);
            }

            return result;
        }

        public List<int> GetOptimalMoves()
        {

            List<int> optimalMoves = new List<int>();
            optimalMoves.Add(this.CurrentChipCount);

            return optimalMoves;
        }
    }
}
