using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prisoners_of_War_V2
{
    class Weapon
    {
        Rectangle HitBox;
        public Direction Dir { get; set; }
        Texture2D texture;
        public Weapon(int x, int y)
        {
            HitBox = new Rectangle(x, y, texture.Width / 6, texture.Height / 6);
        }

        public void Fire()
        {

        }
    }
}
