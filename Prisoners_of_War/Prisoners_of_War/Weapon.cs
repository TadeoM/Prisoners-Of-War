using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
    abstract class Weapon : GameObject
    {
        // Attributes
        public int Damage { get; set; }
        public Direction Dir { get; set; }
        public int currentX { get; set; }
        public int currentY { get; set; }
        public int TickCooldown { get; set; }
        public string Name { get; set; }

        // Constructor
        public Weapon(int x, int y, int width, int height, Direction d):base(x,y,width,height)//50, 20
        {
            currentX = x;
            currentY = y;
            Dir = d;
        }
        
        public Bullet fireWeapon()
        {
            if(this is MachineGun)
            {
                return new Bullet(HitBox.X, HitBox.Y + 35, Dir);
            }
            return new Bullet(HitBox.X+20, HitBox.Y+1, Dir);
        }

        public void Draw(SpriteBatch sb)
        {
            if(Dir == Direction.RIGHT)
            {
                sb.Draw(Sprite, HitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(Sprite, HitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
