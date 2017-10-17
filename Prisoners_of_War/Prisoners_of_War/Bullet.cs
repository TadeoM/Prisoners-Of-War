using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_of_War 
{
    class Bullet : GameObject
    {
        // Attributes
        const int BULLET_SPEED = 15;
        public Direction Dir { get; set; }
        int previousX;
        int previousY; 

        // Constructor
        public Bullet(int x, int y, Direction d):base(x,y,10,5)
        {
            Dir = d;
        }

        // Handles bullet movement
        public void Move()
        {
            previousX = HitBox.X;
            previousY = HitBox.Y;

            if(Dir == Direction.LEFT)
            {
                HitBox.X -= BULLET_SPEED;
            }
            if(Dir == Direction.RIGHT)
            {
                HitBox.X += BULLET_SPEED;
            }
        }
    }
}
