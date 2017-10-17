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
    class PowerUp : GameObject
    {
        // Attributes & Properties
        public int Duration { get; set; }
        public Weapon Weapon { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int TickCount { get; set; }

        // Constructor
        public PowerUp(int x, int y, int width, int height): base(x,y,width,height)
        {
            TickCount = 0;
        }
    }
}
