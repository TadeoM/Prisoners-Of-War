using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Prisoners_of_War
{

    enum AnimationState
    {
        FORWARD,
        BACK,
        UP,
        DOWN,
        IDLE
    }
    class Player : GameObject
    {
        // Attributes
        private int tempScoreTurret = 0;
        private int tempScoreBarricade = 0;

        public List<Bullet> spawnedBullets = new List<Bullet>();
        public List<PowerUp> spawnedPowerUps = new List<PowerUp>();
        public int Name { get; set; }
        public int Health { get; set; }
        public Weapon DefaultWeapon { get; set; }
        public Weapon Weapon { get; set; }
        public int Score { get; set; }
        public AnimationState AnimState { get; set; }
        public AnimationState prevAnimState { get; set; }
        public Texture2D walkSpritesheet;
        public Texture2D sideStepSpritesheet;
        int animTicks = 0;
        int currentColumn = 0;
        int lastColumn = 0;
        public int gunCooldownTicks = 0;
        public int bulletsShot = 0;
        SpriteEffects spriteEffect;


        // Acting as inventory Properties
        public int NumOfBarricades { get; set; }
        public int NumOfTurrets { get; set; }

        // player sound effects
        public SoundEffect Reload { get; set; }
        public SoundEffect Death { get; set; }
        public SoundEffect shoot;
        
        
        public Player(int hp, int pn, int x, int y, int spd, Weapon wp):base(x,y,120,120)
        {
            Name = pn;
            DefaultWeapon = wp;
            Health = hp;
            Speed = spd;
            Weapon = DefaultWeapon;
            Score = 0;
            NumOfBarricades = 0;
            NumOfTurrets = 0;
            if(pn == 1)
            {
                spriteEffect = SpriteEffects.None;
            }
            if (pn == 2)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
        }

        public void Reset()
        {
            Score = 0;
            NumOfBarricades = 0;
            NumOfTurrets = 0;
            Weapon = DefaultWeapon;
        }

        // Checks for movement in a single method
        public void InputCheck(byte playerName, KeyboardState input, bool singleFireKeyPress, bool singlePlaceKeyPress)
        {
            // Player 1 movement (WASD)
            if(playerName == 1)
            {
                if (input.IsKeyDown(Keys.W))
                {
                    HitBox.Y -= Speed;
                    AnimState = AnimationState.UP;
                }
                if (input.IsKeyDown(Keys.S))
                {
                    HitBox.Y += Speed;
                    AnimState = AnimationState.DOWN;
                }
                if (input.IsKeyDown(Keys.D))
                {
                    HitBox.X += Speed;
                    AnimState = AnimationState.BACK;
                }
                if (input.IsKeyDown(Keys.A))
                {
                    HitBox.X -= Speed;
                    AnimState = AnimationState.FORWARD;
                }
                if(input.IsKeyUp(Keys.W) && input.IsKeyUp(Keys.S) && input.IsKeyUp(Keys.D) && input.IsKeyUp(Keys.A))
                {
                    AnimState = AnimationState.IDLE;
                }
                if ((input.IsKeyDown(Keys.Space)) && singleFireKeyPress && gunCooldownTicks <= 0)
                {
                    spawnedBullets.Add(Weapon.fireWeapon());
                    bulletsShot++;
                    gunCooldownTicks = Weapon.TickCooldown;
                    shoot.Play();
                }
                // Barricade 
                if(NumOfBarricades > 0)
                {
                    if (input.IsKeyDown(Keys.E) && singlePlaceKeyPress)
                    {
                        //Checking for overlap to limit Player placement of items
                        bool collision = false;
                        for (int i = 0; i < spawnedPowerUps.Count; i++)
                        {
                            if (spawnedPowerUps[i].HitBox.Intersects(new Rectangle(HitBox.X + HitBox.Width + 50, HitBox.Y, 75, 140)))
                            {
                                collision = true;
                            }
                        }
                        if (collision == false)
                        {
                            spawnedPowerUps.Add(new Barricade(HitBox.X + HitBox.Width + 50, HitBox.Y));
                            NumOfBarricades--;
                        }
                    }
                }
                // Turret
                if(NumOfTurrets > 0)
                {
                    if(input.IsKeyDown(Keys.R) && singlePlaceKeyPress)
                    {
                        //Checking for overlap to limit Player placement of items
                        bool turretCollision = false;
                        for (int i = 0; i < spawnedPowerUps.Count; i++)
                        {
                            if(spawnedPowerUps[i].HitBox.Intersects(new Rectangle(HitBox.X + HitBox.Width + 50, HitBox.Y, 35, 60)))
                            {
                                turretCollision = true;
                            }
                        }
                        if (turretCollision == false)
                        {
                            spawnedPowerUps.Add(new Turret(HitBox.X + HitBox.Width + 25, HitBox.Y + 25, Direction.RIGHT));
                            NumOfTurrets--;
                        }
                    }
                }
            }

            // Player 2 movement (Arrow keys ^v<>)
            else if (playerName == 2)
            {
                if (input.IsKeyDown(Keys.Up))
                {
                    HitBox.Y -= Speed;
                    AnimState = AnimationState.UP;
                }
                if (input.IsKeyDown(Keys.Down))
                {
                    HitBox.Y += Speed;
                    AnimState = AnimationState.DOWN;
                }
                if (input.IsKeyDown(Keys.Right))
                {
                    HitBox.X += Speed;
                    AnimState = AnimationState.FORWARD;
                }
                if (input.IsKeyDown(Keys.Left))
                {
                    HitBox.X -= Speed;
                    AnimState = AnimationState.BACK;
                }
                if (input.IsKeyUp(Keys.Up) && input.IsKeyUp(Keys.Down) && input.IsKeyUp(Keys.Left) && input.IsKeyUp(Keys.Right))
                {
                    AnimState = AnimationState.IDLE;
                }
                if ((input.IsKeyDown(Keys.Enter)) && singleFireKeyPress && gunCooldownTicks <= 0)
                {
                    spawnedBullets.Add(Weapon.fireWeapon());
                    bulletsShot++;
                    gunCooldownTicks = Weapon.TickCooldown;
                    shoot.Play();
                }
                // Barricade
                if(NumOfBarricades > 0)
                {
                    if (input.IsKeyDown(Keys.RightShift) && singlePlaceKeyPress)
                    {
                        bool collision = false;
                        for (int i = 0; i < spawnedPowerUps.Count; i++)
                        {
                            //Checking for overlap to limit Player placement of items
                            if (spawnedPowerUps[i].HitBox.Intersects(new Rectangle(HitBox.X - HitBox.Width - 50, HitBox.Y, 75, 140)))
                            {
                                collision = true;
                            }
                        }

                        if (collision == false)
                        {
                            spawnedPowerUps.Add(new Barricade(HitBox.X - HitBox.Width - 50, HitBox.Y));
                            NumOfBarricades--;
                        }
                    }
                }
                // Turret
                if (NumOfTurrets > 0)
                {
                    if (input.IsKeyDown(Keys.RightControl) && singlePlaceKeyPress)
                    {
                        //Checking for overlap to limit Player placement of items
                        bool turretCollision = false;
                        for (int i = 0; i < spawnedPowerUps.Count; i++)
                        {
                            if (spawnedPowerUps[i].HitBox.Intersects(new Rectangle(HitBox.X - 50, HitBox.Y, 35, 60)))
                            {
                                turretCollision = true;
                            }
                        }
                        if (turretCollision == false)
                        {
                            spawnedPowerUps.Add(new Turret(HitBox.X - HitBox.Width - 25, HitBox.Y + 25, Direction.LEFT));
                            NumOfTurrets--;
                        }
                    }
                }
            }
            if(bulletsShot != 0)
            {
                switch (Weapon.Name)
                {
                    case "Rifle":
                        if (bulletsShot % 6 == 0)
                        {
                            gunCooldownTicks = 80;
                            bulletsShot = 0;
                            Reload.Play();
                        }
                        break;
                    case "MachineGun":
                        if (bulletsShot % 20 == 0)
                        {
                            gunCooldownTicks = 300;
                            bulletsShot = 0;
                        }
                        break;
                    default:
                        break;
                }
            }
            
            gunCooldownTicks--;
            animTicks++;
            
            if (AnimState != prevAnimState) currentColumn = 0;
            if (animTicks >= 7)
            {
                if (AnimState == AnimationState.IDLE)
                {
                    currentColumn = 0;
                }
                else if (AnimState == AnimationState.UP)
                {
                    currentColumn++;
                    if (currentColumn > 4) currentColumn = 1;
                }
                else if (AnimState == AnimationState.DOWN)
                {
                    currentColumn--;
                    if (currentColumn < 1) currentColumn = 4;
                }
                else if (AnimState == AnimationState.FORWARD)
                {
                    currentColumn++;
                    if (currentColumn > 5) currentColumn = 1;
                }
                else if (AnimState == AnimationState.BACK)
                {
                    currentColumn--;
                    if (currentColumn < 1) currentColumn = 5;
                }
                animTicks = 0;
            }
            prevAnimState = AnimState;
        }

        // Handle player inventory via score (Scorestreaks)
        public void itemCheck()
        {
            if (Score == tempScoreBarricade + 5)
            {
                //NumOfBarricades++;
                tempScoreBarricade += 5;
            }
            if(Score == tempScoreTurret + 10)
            {
                //NumOfTurrets++;
                tempScoreTurret += 10;
            }
        }

        // Handle firing the turret
        public void fireTurret()
        {
            if (spawnedPowerUps.Count != 0)
            {
                for (int i = 0; i < spawnedPowerUps.Count; i++)
                {
                    if (spawnedPowerUps[i] is Turret)
                    {
                        if (spawnedPowerUps[i].TickCount % 60 == 0)
                        {
                            spawnedBullets.Add(spawnedPowerUps[i].Weapon.fireWeapon());
                            shoot.Play();
                        }
                        spawnedPowerUps[i].TickCount++;
                    }
                }
            }
        }

        // Restrict player's movement while colliding with a barricade
        public void barricadeMovement(Texture2D machineGunSprite)
        {
            bool collide = false;
            for (int i = 0; i < spawnedPowerUps.Count; i++)
            {
                if (HitBox.Intersects(spawnedPowerUps[i].HitBox))
                {
                    if(spawnedPowerUps[i] is MachineGun_PowerUp)
                    {
                        if (this.Name == 1)
                            this.Weapon = new MachineGun(this.HitBox.X, this.HitBox.Y, Direction.RIGHT) { Sprite = machineGunSprite };
                        else
                            this.Weapon = new MachineGun(this.HitBox.X, this.HitBox.Y, Direction.LEFT) { Sprite = machineGunSprite };
                        spawnedPowerUps.RemoveAt(i);
                    }
                    else
                    {
                        collide = true;
                        break;
                    }
                }           
            }
            if (collide)
            {
                Speed = 2;
            }
            else
            {
                Speed = 5;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if(AnimState == AnimationState.FORWARD || AnimState == AnimationState.BACK || AnimState == AnimationState.IDLE)
                sb.Draw
                    (
                    walkSpritesheet,
                    HitBox,
                    new Rectangle(240 * currentColumn, 0, 240, 240),
                    Color.White,
                    0,
                    Vector2.Zero,
                    spriteEffect,
                    0
                    );
            else if (AnimState == AnimationState.UP || AnimState == AnimationState.DOWN)
                sb.Draw
                    (
                    sideStepSpritesheet,
                    HitBox,
                    new Rectangle(240 * currentColumn, 0, 240, 240),
                    Color.White,
                    0,
                    Vector2.Zero,
                    spriteEffect,
                    0
                    );
        }
    }
}
