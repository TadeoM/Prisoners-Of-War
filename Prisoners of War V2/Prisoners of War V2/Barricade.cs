using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_of_War_V2
{
    class Barricade : Powerup
    {
        public Barricade()
        {
            Health = 3;
            TimeOut = 60 * 60; //seconds times 60 ticks
        }
    }
}
