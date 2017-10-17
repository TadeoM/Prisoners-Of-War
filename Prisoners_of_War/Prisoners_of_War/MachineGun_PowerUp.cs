using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_of_War
{
    class MachineGun_PowerUp:PowerUp
    {
        public MachineGun_PowerUp(int x, int y):base(x,y,100,75)
        {
            Duration = 600;
        }
    }
}
