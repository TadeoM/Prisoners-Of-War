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
    class Bullet
    {
        const int BULLET_SPEED = 30;
        public Rectangle HitBox;
        public Direction Dir { get; set; }

        public Bullet(Direction dir)
        {
            Dir = dir;
        }

        public void Move()
        {
            if(Dir == Direction.RIGHT) HitBox.X += BULLET_SPEED;
            else if (Dir == Direction.LEFT) HitBox.X -= BULLET_SPEED;
        }
    }
}
