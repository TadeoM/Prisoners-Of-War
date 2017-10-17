using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prisoners_of_War_V2
{
    class Turret : Powerup
    {
        List<Bullet> listOfBullets = new List<Bullet>();
        public Direction Dir { get; set; }
        public Turret(Direction dir)
        {
            Health = 5;
            TimeOut = 30 * 60; //seconds times 60 ticks
            Dir = dir;
        }

        public void Fire()
        {
            if(TimeOut % 30 == 0)
            {
                listOfBullets.Add(new Bullet(Dir));
            }
        }
    }
}
