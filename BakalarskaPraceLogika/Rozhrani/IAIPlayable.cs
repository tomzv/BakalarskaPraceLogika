using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bakalarkaDEMO
{
    interface IAIPlayable
    {
        bool AIMove(double probabilityForOptimal);
    }
}
