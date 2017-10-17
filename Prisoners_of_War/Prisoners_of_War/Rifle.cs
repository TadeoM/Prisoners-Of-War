using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prisoners_of_War
{
    class Rifle : Weapon
    {
        // Constructor
        public Rifle(int x, int y, Direction d):base(x,y, 50, 20, d)
        {
            Damage = 1;
            TickCooldown = 20;
            Name = "Rifle";
        }
    }
}
