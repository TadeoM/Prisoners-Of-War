using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_of_War
{
    class Turret : PowerUp
    {
        // Properties & Attributes
        public Weapon Gun { get; set; }
        int ammo = 100;

        // Constructor
        public Turret(int x, int y, Direction d) : base(x,y, 100,100)
        {
            Health = 5;
            MaxHealth = 5;
            Duration = 1800;
            Weapon = new Rifle(x+35, y+25, d);
        }
    }
}
