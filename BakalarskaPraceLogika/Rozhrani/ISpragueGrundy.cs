using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakalarkaDEMO
{
    interface ISpragueGrundy
    {

        int[] PNPositionSG { get; set; }
        int CurrentChipCount { get; set; }


        int GetCurrentSGValue();
        void FindPNSG();
        List<int> GetPossibleMoves();
        List<int> GetOptimalMoves();

    }
}
