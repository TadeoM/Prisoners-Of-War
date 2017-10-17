using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_of_War
{
    public class GameObject
    {
        
        // Attributes & Properties
        public Rectangle HitBox;
        public Rectangle Collision;

        // Sprite attributes
        private Texture2D sprite;

        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        
        // Position attributes
        private int speed = 5;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // Constructor
        public GameObject(int x, int y, int width, int height)
        {
            if(this is Barricade)
            {
                HitBox = new Rectangle(x, y, width, height);
                Collision = new Rectangle(x+((width*3)/5) - 4, y, width/10, height/2);
            }
            else
            {
                HitBox = new Rectangle(x, y, width, height);
            }
        }

        /// <summary>
        /// Draws itself
        /// </summary>
        /// <param name="sb"></param>
        /*virtual public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, HitBox, Color.White);
        }
        */
    }
}
