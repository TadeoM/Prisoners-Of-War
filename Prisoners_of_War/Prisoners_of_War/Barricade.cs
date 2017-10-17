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
    class Barricade : PowerUp
    {
        //Constructor
        public Barricade(int x, int y) : base(x, y, 130, 130)
        {
            Health = 3;
            MaxHealth = 3;
            Duration = -1;
        }
    }
}
