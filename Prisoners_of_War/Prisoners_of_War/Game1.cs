using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Prisoners_of_War
{
    // Unimplemented Finite State Machine
    enum GameState
    {
        MENU,
        LEVELEDITOR,
        INSTRUCTIONS,
        PAUSE,
        GAME,
        GAMEOVER
    }

    enum Direction
    {
        LEFT,
        RIGHT
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {      
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Texture attributes 
        Texture2D bulletTexture;
        Texture2D usWeaponTexture;
        Texture2D ruWeaponTexture;
        Texture2D turret;
        Texture2D machineGun;

        Texture2D background;
        Texture2D homeMenu;
        Texture2D pauseScreen;
        Texture2D instructions;
        Texture2D p1Win;
        Texture2D p2Win;

        Texture2D sign;
        Texture2D middleLine;
        Texture2D barricade;
        Texture2D barricadeIcon;
        Texture2D turretIcon;

        // Colors
        Color scoreColor;
        Color lightScoreColor;
        Color levelEditorBG;
        Color darkTitleColor;
        Color titleColor;

        // Fonts
        SpriteFont font;
        SpriteFont scoreFont;

        // Mouse attributes
        MouseState msState;
        Vector2 mousePos;
        Vector2 windowScale;

        // Gun stats
        const int P1_RIFLE_X_OFFSET = 55;
        const int P1_RIFLE_Y_OFFSET = 55;

        const int P2_RIFLE_X_OFFSET = 20;
        const int P2_RIFLE_Y_OFFSET = 50;

        const int MG_Y_OFFSET = 33;

        const int LEVEL_EDITOR_ROWS = 4;
        const int LEVEL_EDITOR_COLUMNS = 6;

        // Player attributes
        Player player1;
        Vector2 player1Score;
        Player player2;
        Vector2 player2Score;
        int player1InvisTimer;
        int player2InvisTimer;
        int winner;
        int maxY;
        int minY;

        // Keypress states
        KeyboardState kbState;
        KeyboardState prevKbState;

        // Gamestate tracking
        GameState gState;
        GameState prevGState;

        // game attributes
        Song gameAmbience;
        Song gameRadio;
        Song menu;
        Song player1Wins;
        Song player2Wins;
        bool setFalse = true;


        // Level editor attributes
        LevelEditor editor = new LevelEditor();
        Texture2D buttonTexture;
        Texture2D darkButtonTexture;
        Texture2D privateCursor;
        MouseState previousMouseState;
        MouseState currentMouseState;

        Vector2 menuTextLength;
        int[,] tileValues = new int[LEVEL_EDITOR_ROWS, LEVEL_EDITOR_COLUMNS];

        GameState gameState;
        Random rng = new Random();
        int middleLocation;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1600;  // Width of the game window
            graphics.PreferredBackBufferHeight = 900;   // Height of the game window
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player1 = new Player(100, 1, 200, GraphicsDevice.Viewport.Height / 2, 20, new Rifle(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 2, Direction.RIGHT));
            player1Score = new Vector2(120, GraphicsDevice.Viewport.Height - 125);
            player2 = new Player(100, 2, GraphicsDevice.Viewport.Width-250, GraphicsDevice.Viewport.Height / 2, 20, new Rifle(GraphicsDevice.Viewport.Width - 250, GraphicsDevice.Viewport.Height / 2, Direction.LEFT));
            player2Score = new Vector2(GraphicsDevice.Viewport.Width - 300, GraphicsDevice.Viewport.Height - 125);

            // Sprite Colors
            scoreColor = new Color(39, 38, 38);
            titleColor = new Color(254, 202, 1);
            darkTitleColor = new Color(42, 40, 40);
            lightScoreColor = new Color(54, 52, 52);
            levelEditorBG = new Color(237, 237, 237);

            middleLocation = (GraphicsDevice.Viewport.Width / 2) - 25;

            gameState = GameState.MENU;

            gState = gameState;

            // Reads in info from the level editor
            StreamReader r = new StreamReader("level.json");
            string levelData = r.ReadToEnd();
            r.Close();
            tileValues = JsonConvert.DeserializeObject<int[,]>(levelData);

            GenerateMap();

            // Boundries of the game map
            maxY = GraphicsDevice.Viewport.Height - 110;
            minY = 75;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fonts
            font = Content.Load<SpriteFont>("Font");
            scoreFont = Content.Load<SpriteFont>("AmericanCaptain");

            // LevelEditor Sprites
            darkButtonTexture = Content.Load<Texture2D>("Button");
            buttonTexture = Content.Load<Texture2D>("Button2");
            privateCursor = Content.Load<Texture2D>("PrivateCursor");

            // Player 1 Sprites
            player1.Sprite = Content.Load<Texture2D>("Player1");
            player1.Weapon.Sprite = usWeaponTexture = Content.Load<Texture2D>("Player1Gun");
            player1.walkSpritesheet = Content.Load<Texture2D>("USWalk");
            player1.sideStepSpritesheet = Content.Load<Texture2D>("USSidestep");

            // Player 2 Sprites 
            player2.Sprite = Content.Load<Texture2D>("Player2");
            player2.Weapon.Sprite = ruWeaponTexture = Content.Load<Texture2D>("Player2Gun");
            player2.walkSpritesheet = Content.Load<Texture2D>("USSRWalk");
            player2.sideStepSpritesheet = Content.Load<Texture2D>("USSRSidestep");

            // Game Sprites
            bulletTexture = Content.Load<Texture2D>("Bullet");
            barricade = Content.Load<Texture2D>("Barrier");
            middleLine = Content.Load<Texture2D>("Line");
            sign = Content.Load<Texture2D>("Sign");
            turret = Content.Load<Texture2D>("Turret");
            machineGun = Content.Load<Texture2D>("MachineGun");
            player1.shoot = Content.Load<SoundEffect>("player1GunShot");
            player2.shoot = Content.Load<SoundEffect>("player2GunShot");
            player1.Reload = Content.Load<SoundEffect>("reload");
            player2.Reload = Content.Load<SoundEffect>("reload");
            player1.Death = Content.Load<SoundEffect>("death");
            player2.Death = Content.Load<SoundEffect>("death");
            gameAmbience = Content.Load<Song>("GameAmbience");
            gameRadio = Content.Load<Song>("GameRadio");
            menu = Content.Load<Song>("Game");
            player1Wins = Content.Load<Song>("americanAnthem");
            player2Wins = Content.Load<Song>("russianAnthem");
            turretIcon = Content.Load<Texture2D>("TurretIcon");
            barricadeIcon = Content.Load<Texture2D>("BarrierIcon");

            // Screen Sprites
            background = Content.Load<Texture2D>("Background");
            homeMenu = Content.Load<Texture2D>("MenuScreen");
            instructions = Content.Load<Texture2D>("InstructionsScreen");
            pauseScreen = Content.Load<Texture2D>("PauseScreen");
            p1Win = Content.Load<Texture2D>("Player1Win");
            p2Win = Content.Load<Texture2D>("Player2Win");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            // Runs any input through InputCheck (This will probably need to be changed in the longrun, but I wasn't sure of an easier way)
            var input = Keyboard.GetState();
            kbState = Keyboard.GetState();

            // Fullscreen mode
            if (SingleKeyPress(Keys.F11))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            // Manages gamestates
            switch (gameState)
            {
                case GameState.MENU:
                                     
                    if (setFalse)
                    {
                        setFalse = false;
                        MediaPlayer.Play(menu);
                        MediaPlayer.IsRepeating = true;
                    }
                    

                    // Sets up a new game
                    if(SingleKeyPress(Keys.P))
                    {
                        NewGame();
                        setFalse = true;
                        player1.Reset();
                        player2.Reset();
                        SwitchGameState(GameState.GAME);
                    }
                    if (SingleKeyPress(Keys.E))
                    {
                        SwitchGameState(GameState.LEVELEDITOR);
                    }
                    if (SingleKeyPress(Keys.I))
                    {
                        SwitchGameState(GameState.INSTRUCTIONS);
                    }
                    break;
                case GameState.LEVELEDITOR:
                    if (setFalse)
                    {
                        setFalse = false;
                        MediaPlayer.Play(menu);
                        MediaPlayer.IsRepeating = true;
                    }

                    msState = Mouse.GetState();
                    mousePos = new Vector2(msState.X, msState.Y);
                    windowScale = new Vector2(1600 / GraphicsDevice.Viewport.Width, 900 / GraphicsDevice.Viewport.Height);
                    mousePos = Vector2.Multiply(mousePos, windowScale);

                    if (SingleMouseInput())
                    {
                        SwitchGameState(editor.CheckButtonsForClick(previousMouseState));
                    }
                    else
                    {
                        SwitchGameState(editor.CheckButtonsForClick(kbState));
                    }
                        
                    if (gameState != GameState.LEVELEDITOR) GenerateMap();
                    break;
                case GameState.INSTRUCTIONS:
                    if (setFalse)
                    {
                        setFalse = false;
                        MediaPlayer.Play(menu);
                        MediaPlayer.IsRepeating = true;
                    }

                    if (SingleKeyPress(Keys.R))
                    {
                        if (prevGState == GameState.MENU)
                        {
                            SwitchGameState(GameState.MENU);
                        }
                        else if (prevGState == GameState.PAUSE)
                        {
                            SwitchGameState(GameState.PAUSE);
                        }
                    }
                    break;
                case GameState.GAME:
                    if (setFalse)
                    {
                        setFalse = false;
                        MediaPlayer.Play(gameAmbience);
                        MediaPlayer.Play(gameRadio);
                        MediaPlayer.Volume = .3f;
                        MediaPlayer.IsRepeating = true;
                    }

                    // bool means shots fire and other actions happen once at a time
                    bool spaceCheck = SingleKeyPress(Keys.Space);
                    bool enterCheck = SingleKeyPress(Keys.Enter);
                    bool p1itemCheck = SingleKeyPress(Keys.E) || SingleKeyPress(Keys.R);
                    bool p2itemCheck = SingleKeyPress(Keys.RightShift) || SingleKeyPress(Keys.RightControl);
                    bool pauseCheck = SingleKeyPress(Keys.Escape);

                    // Detect if player is intersecting with barricades, slow the player down if they are
                    if (player1InvisTimer > 0)
                        player1InvisTimer--;
                    if (player2InvisTimer > 0)
                        player2InvisTimer--;

                    // Checks if bullets are colliding with anything
                    CheckBulletCollisions();

                    // Check player inputs
                    player1.InputCheck(1, input, spaceCheck, p1itemCheck);
                    player1.Weapon.HitBox.X = player1.HitBox.X + P1_RIFLE_X_OFFSET;
                    if (player1.Weapon is Rifle)
                        player1.Weapon.HitBox.Y = player1.HitBox.Y + P1_RIFLE_Y_OFFSET;
                    else
                        player1.Weapon.HitBox.Y = player1.HitBox.Y + MG_Y_OFFSET;
                    player2.InputCheck(2, input, enterCheck, p2itemCheck);

                    if (player2.Weapon is Rifle)
                    {
                        player2.Weapon.HitBox.X = player2.HitBox.X + P2_RIFLE_X_OFFSET;
                        player2.Weapon.HitBox.Y = player2.HitBox.Y + P2_RIFLE_Y_OFFSET;
                    }
                    else
                    {
                        player2.Weapon.HitBox.X = player2.HitBox.X - 30;
                        player2.Weapon.HitBox.Y = player2.HitBox.Y + MG_Y_OFFSET;
                    }

                    // Handle Player containment
                    PlayerContainment(player1, 1);
                    PlayerContainment(player2, 2);

                    // Check Player items
                    player1.itemCheck();
                    player2.itemCheck();
                    player1.fireTurret();
                    player2.fireTurret();
                    UpdateDurations();

                    // Declares winner
                    winner = winnerDeclarationOfIndependence();

                    prevKbState = kbState;

                    player1.barricadeMovement(machineGun);
                    player2.barricadeMovement(machineGun);

                    // Enter pause state
                    if (pauseCheck)
                    {
                        SwitchGameState(GameState.PAUSE);
                    }
                    break;
                case GameState.PAUSE:
                    // Exit Pause state to game
                    if (SingleKeyPress(Keys.G))
                    {
                        SwitchGameState(GameState.GAME);
                        return;
                    }
                    // Exit Pause state to menu
                    if (SingleKeyPress(Keys.M))
                    {
                        setFalse = true;
                        SwitchGameState(GameState.MENU);
                        return;
                    }
                    // Exit Pause state to instructions
                    if (SingleKeyPress(Keys.I))
                    {
                        setFalse = true;
                        SwitchGameState(GameState.INSTRUCTIONS);
                        return;
                    }
                    break;
                case GameState.GAMEOVER:
                    //if (Keyboard.GetState().GetPressedKeys().Length > 0)

                    MediaPlayer.Volume = 5f;
                    if (setFalse == true)
                    {
                        if (winner == 1)
                        {
                            MediaPlayer.Volume = 6f;
                            MediaPlayer.Play(player1Wins);
                        }
                        else if(winner == 2)
                        {
                            MediaPlayer.Volume = 10f;
                            MediaPlayer.Play(player2Wins);
                        }
                        setFalse = false;
                    }

                    if (SingleKeyPress(Keys.M))
                    {
                        setFalse = true;
                        SwitchGameState(GameState.MENU);
                        return;
                    }
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.MENU:
                    spriteBatch.Draw(homeMenu, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.LEVELEDITOR:
                    GraphicsDevice.Clear(levelEditorBG);
                    editor.Draw(spriteBatch, buttonTexture, barricade, turret, scoreFont);
                    spriteBatch.Draw(privateCursor, new Rectangle((int)(mousePos.X - 15), (int)mousePos.Y, 30, 30), Color.White);
                    break;

                case GameState.INSTRUCTIONS:
                    spriteBatch.Draw(instructions, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.GAME:
                    // Background sprite
                    spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    // Middle line sprite
                    spriteBatch.Draw(middleLine, new Rectangle(middleLocation, 298, 100, 485), Color.White);
                    
                    // Both player's bullets
                    foreach (var item in player1.spawnedBullets)
                    {
                        spriteBatch.Draw(bulletTexture, item.HitBox, Color.White);
                    }
                    foreach (var item in player2.spawnedBullets)
                    {
                        spriteBatch.Draw(bulletTexture, item.HitBox, Color.White);
                    }
                    // Player 1 items
                    foreach(var item in player1.spawnedPowerUps)
                    {
                        if (item is Barricade)
                        {
                            spriteBatch.Draw(barricade, item.HitBox, Color.ForestGreen);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, item.HitBox.Width / 2, 5), Color.IndianRed);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, ((item.HitBox.Width / 2) / item.MaxHealth) * item.Health + 1, 5), Color.ForestGreen);
                        }
                        if (item is Turret)
                        {
                            spriteBatch.Draw(turret, item.HitBox, Color.LightGray);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, item.HitBox.Width / 2, 5), Color.IndianRed);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, ((item.HitBox.Width / 2) / item.MaxHealth) * item.Health + 1, 5), Color.ForestGreen);
                        }
                        if(item is MachineGun_PowerUp)
                        {
                            spriteBatch.Draw(machineGun, item.HitBox, Color.LightGray);
                        }
                        
                    }
                    // Player 2 items
                    foreach (var item in player2.spawnedPowerUps)
                    {
                        if (item is Barricade)
                        {
                            spriteBatch.Draw(barricade, item.HitBox, Color.IndianRed);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, item.HitBox.Width/2, 5), Color.IndianRed);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, ((item.HitBox.Width/2) / item.MaxHealth) * item.Health + 1, 5), Color.ForestGreen);
                        }
                        
                        // Flipped turret sprite    
                        if (item is Turret)
                        {
                            spriteBatch.Draw
                                (
                                turret,
                                item.HitBox,
                                null,
                                Color.LightGray,
                                0,
                                Vector2.Zero,
                                SpriteEffects.FlipHorizontally,
                                0
                                );
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, item.HitBox.Width / 2, 5), Color.IndianRed);
                            spriteBatch.Draw(buttonTexture, new Rectangle(item.HitBox.X + 30, item.HitBox.Y - 10, ((item.HitBox.Width / 2) / item.MaxHealth) * item.Health + 1, 5), Color.ForestGreen);
                        }
                        if (item is MachineGun_PowerUp)
                        {
                            spriteBatch.Draw(machineGun, item.HitBox, Color.LightGray);
                        }

                    }
                    // Player 1 invincibility blinking
                    if ((player1InvisTimer >= 0 && player1InvisTimer <= 15)
                        || (player1InvisTimer >= 30 && player1InvisTimer <= 45)
                        || (player1InvisTimer >= 60 && player1InvisTimer <= 75)
                        || (player1InvisTimer >= 90 && player1InvisTimer <= 105))
                    {
                        //spriteBatch.Draw(player1.Sprite, player1.HitBox, Color.White);
                        player1.Draw(spriteBatch);
                        player1.Weapon.Draw(spriteBatch);
                    }
                    // Player 2 invincibility blinking
                    if ((player2InvisTimer >= 0 && player2InvisTimer <= 15)
                        || (player2InvisTimer >= 30 && player2InvisTimer <= 45)
                        || (player2InvisTimer >= 60 && player2InvisTimer <= 75)
                        || (player2InvisTimer >= 90 && player2InvisTimer <= 105))
                    {
                        //spriteBatch.Draw(player2.Sprite, player2.HitBox, Color.White);
                        player2.Draw(spriteBatch);
                        player2.Weapon.Draw(spriteBatch);
                    }

                    // Score Signs
                    spriteBatch.Draw(sign, new Rectangle(5, GraphicsDevice.Viewport.Height - 205, 350, 250), Color.White);
                    spriteBatch.Draw(sign, new Rectangle(GraphicsDevice.Viewport.Width - 415, GraphicsDevice.Viewport.Height - 205, 350, 250), Color.White);

                    spriteBatch.DrawString(scoreFont, "  Player 1\nScore: ", player1Score, lightScoreColor);
                    spriteBatch.DrawString(scoreFont, "  Player 2\nScore: ", player2Score, lightScoreColor);

                    spriteBatch.DrawString(scoreFont, (player1.Score * 100).ToString(), new Vector2(player1Score.X + (scoreFont.MeasureString("Score: ").X), player1Score.Y + (scoreFont.MeasureString("Score: ").Y)), scoreColor);
                    spriteBatch.DrawString(scoreFont, (player2.Score * 100).ToString(), new Vector2(player2Score.X + (scoreFont.MeasureString("Score: ").X), player2Score.Y + (scoreFont.MeasureString("Score: ").Y)), scoreColor);


                    // Player 1 item icons
                    for (int i = 0; i < player1.NumOfBarricades; i++)
                    {
                        spriteBatch.Draw(barricadeIcon, new Rectangle(250 + (i * 35), GraphicsDevice.Viewport.Height - 135, 100, 100), Color.White);
                    }

                    for (int i = 0; i < player1.NumOfTurrets; i++)
                    {
                        spriteBatch.Draw(turretIcon, new Rectangle(250 + (i * 35), GraphicsDevice.Viewport.Height - 100, 100, 100), Color.White);
                    }

                    // Player 2 item icons
                    for (int i = 0; i < player2.NumOfBarricades; i++)
                    {
                        spriteBatch.Draw(barricadeIcon, new Rectangle(GraphicsDevice.Viewport.Width - (410 + (i * 35)), GraphicsDevice.Viewport.Height - 135, 100, 100), Color.White);
                    }

                    for (int i = 0; i < player2.NumOfTurrets; i++)
                    {
                        spriteBatch.Draw(turretIcon, new Rectangle(GraphicsDevice.Viewport.Width - (410 + (i * 35)), GraphicsDevice.Viewport.Height - 100, 100, 100), Color.White);
                    }

                    break;

                case GameState.PAUSE:
                    spriteBatch.Draw(pauseScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    break;

                case GameState.GAMEOVER:
                    if (winner == 1)
                    {
                        spriteBatch.Draw(p1Win, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    }
                    else if (winner == 2)
                    {
                        spriteBatch.Draw(p2Win, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    }
                    break;
                default:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Handles case despawning of bullets when colliding with objects
        public void CheckBulletCollisions()
        {
            // Player1 Bullets check
            for (int i = 0; i < player1.spawnedBullets.Count; i++)
            {
                player1.spawnedBullets[i].Move();
                // Hits Player 2 case
                if (player1.spawnedBullets[i].HitBox.Intersects(player2.HitBox) && player2InvisTimer <= 0)
                {
                    player1.Score++;
                    if(player1.Score % 3 == 0)
                    {
                        player1.NumOfBarricades++;
                    }
                    if(player1.Score % 5 == 0)
                    {
                        player1.NumOfTurrets++;
                    }
                    player2.HitBox.X = GraphicsDevice.Viewport.Width - 250;
                    player2.HitBox.Y = GraphicsDevice.Viewport.Height / 2;
                    player2.Weapon = player2.DefaultWeapon;
                    player2.Weapon.HitBox.X = player2.HitBox.X + P2_RIFLE_X_OFFSET;
                    player2.Weapon.HitBox.Y = player2.HitBox.Y + P2_RIFLE_Y_OFFSET;
                    player2.gunCooldownTicks = 0;
                    player2.bulletsShot = 0;
                    player1.spawnedBullets.RemoveAt(i);
                    player2InvisTimer = 90;
                    player2.Death.Play();

                    if (player1.Score > player2.Score)
                    {
                        middleLocation += (player1.Score - player2.Score) * 50;
                    }
                    if(player1.Score < player2.Score)
                    {
                        middleLocation += (player2.Score - player1.Score) * 50;
                    }
                    
                }
                // Exceeds edge of map case
                else if (player1.spawnedBullets[i].HitBox.X > GraphicsDevice.Viewport.Width ||
                    player1.spawnedBullets[i].HitBox.X < 0 ||
                    player1.spawnedBullets[i].HitBox.Y > GraphicsDevice.Viewport.Height ||
                    player1.spawnedBullets[i].HitBox.Y < 0)
                {
                    player1.spawnedBullets.RemoveAt(i);
                }
                else
                {
                    // Handle Barricades on the opposite side
                    for (int j = 0; j < player2.spawnedPowerUps.Count; j++)
                    {
                        if (player1.spawnedBullets[i].HitBox.Intersects(player2.spawnedPowerUps[j].HitBox))
                        {
                            player1.spawnedBullets.RemoveAt(i);
                            player2.spawnedPowerUps[j].Health--;
                            if (player2.spawnedPowerUps[j].Health == 0)
                            {
                                player2.spawnedPowerUps.RemoveAt(j);
                            }
                            break;
                        }
                    }
                }
            }

            // Player2 Bullets check
            for (int i = 0; i < player2.spawnedBullets.Count; i++)
            {
                player2.spawnedBullets[i].Move();
                // Hits Player 1 case
                if (player2.spawnedBullets[i].HitBox.Intersects(player1.HitBox) && player1InvisTimer <= 0)
                {
                    player2.Score++;
                    if (player2.Score % 3 == 0)
                    {
                        player2.NumOfBarricades++;
                    }
                    if (player2.Score % 5 == 0)
                    {
                        player2.NumOfTurrets++;
                    }
                    player1.HitBox.X = 200;
                    player1.HitBox.Y = GraphicsDevice.Viewport.Height / 2;
                    player1.Weapon = player1.DefaultWeapon;
                    player1.Weapon.HitBox.X = player1.HitBox.X + P1_RIFLE_X_OFFSET;
                    player1.Weapon.HitBox.Y = player1.HitBox.Y + P1_RIFLE_Y_OFFSET;
                    player1.gunCooldownTicks = 0;
                    player1.bulletsShot = 0;
                    player2.spawnedBullets.RemoveAt(i);
                    player1InvisTimer = 90;
                    player1.Death.Play();

                    if (player1.Score > player2.Score)
                    {
                        middleLocation += (player2.Score - player1.Score) * 50;
                    }
                    if (player1.Score < player2.Score)
                    {
                        middleLocation += (player1.Score - player2.Score) * 50;
                    }
                }
                // Exceeds edge of map case
                else if (player2.spawnedBullets[i].HitBox.X > GraphicsDevice.Viewport.Width ||
                    player2.spawnedBullets[i].HitBox.X < 0 ||
                    player2.spawnedBullets[i].HitBox.Y > GraphicsDevice.Viewport.Height ||
                    player2.spawnedBullets[i].HitBox.Y < 0)
                {
                    player2.spawnedBullets.RemoveAt(i);
                }
                else
                {
                    // Handle Barricades on the opposite side
                    for (int j = 0; j < player1.spawnedPowerUps.Count; j++)
                    {
                        if (player2.spawnedBullets[i].HitBox.Intersects(player1.spawnedPowerUps[j].HitBox))
                        {
                            player2.spawnedBullets.RemoveAt(i);
                            player1.spawnedPowerUps[j].Health--;
                            if (player1.spawnedPowerUps[j].Health == 0)
                            {
                                player1.spawnedPowerUps.RemoveAt(j);
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether it was the first frame that a key was pressed or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SingleKeyPress(Keys key)
        {
            bool firstFrame;
            if ((kbState.IsKeyDown(key)) && (prevKbState.IsKeyUp(key)))
            {
                firstFrame = true;
                return firstFrame;
            }
            else
            {
                firstFrame = false;
                return firstFrame;
            }
        }

        /// <summary>
        /// Checks to see if the object is trying to move out of the screen, keeps them on screen if they are
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pn"></param>
        
        // Handles the containment of the player to a respective box.
        public void PlayerContainment(GameObject obj, byte pn)
        {
            // Player1 Box
            if ((obj is Player) && pn == 1)
            {
                // Check if player 1 is trying to go off the screen in the Y direction
                if (obj.HitBox.Y >= maxY)
                {
                    obj.HitBox.Y = maxY ;
                }
                else if (obj.HitBox.Y <= minY)
                {
                    obj.HitBox.Y = minY;
                }

                // Check if player 1 is trying to go off the screen in the X direction
                if (obj.HitBox.X >= middleLocation -50)
                {
                    obj.HitBox.X = middleLocation - 50;
                }
                else if (obj.HitBox.X <= -32)
                {
                    obj.HitBox.X = - 32;
                }                
            }
            //Player2 Box
            if ((obj is Player) && pn == 2)
            {
                // Check if player 2 is trying to go off the screen in the Y direction
                if (obj.HitBox.Y >= maxY)
                {
                    obj.HitBox.Y = maxY;
                }
                else if (obj.HitBox.Y <= minY)
                {
                    obj.HitBox.Y = minY;
                }

                // Check if player 2 is trying to go off the screen in the X direction
                if (obj.HitBox.X <= middleLocation + 25)
                {
                    obj.HitBox.X = middleLocation + 25;
                }
                else if (obj.HitBox.X >= GraphicsDevice.Viewport.Width - 85)
                {
                    obj.HitBox.X = GraphicsDevice.Viewport.Width - 85;
                }
            }
        }

        /// <summary>
        /// Returns whether or not the left mouse button is being clicked
        /// </summary>
        /// <returns></returns>
        public bool SingleMouseInput()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the number of the player who wins the game based on where the center line (middleLocation) is
        /// </summary>
        /// <returns></returns>
        public int winnerDeclarationOfIndependence()
        {
            if (middleLocation <= 0)
            {
                setFalse = true;
                SwitchGameState(GameState.GAMEOVER);
                return 2;
            }

            if(middleLocation >= GraphicsDevice.Viewport.Width)
            {
                setFalse = true;
                SwitchGameState(GameState.GAMEOVER);
                return 1;
            }

            return 0;
        }

        void SwitchGameState(GameState gs)
        {
            prevGState = gameState;
            gameState = gs;
            gState = gameState;
        }


        // Sets up a new game
        public void NewGame()
        {
            // Regenerates map
            GenerateMap();

            // Resets invincibility
            player1InvisTimer = 0;
            player2InvisTimer = 0;

            // Resets turrets and barricades
            player1.NumOfBarricades = 0;
            player2.NumOfBarricades = 0;
            player1.NumOfTurrets = 0;
            player2.NumOfTurrets = 0;

            // Clears bullets and cooldowns
            player1.spawnedBullets.Clear();
            player2.spawnedBullets.Clear();
            player1.bulletsShot = 0;
            player2.bulletsShot = 0;
            player1.gunCooldownTicks = 0;
            player2.gunCooldownTicks = 0;

            // Resets Player 1 location
            player1.HitBox.X = 200;
            player1.HitBox.Y = GraphicsDevice.Viewport.Height / 2;
            player1.Weapon = player1.DefaultWeapon;
            player1.Weapon.HitBox.X = player1.HitBox.X + P1_RIFLE_X_OFFSET;
            player1.Weapon.HitBox.Y = player1.HitBox.Y + P1_RIFLE_Y_OFFSET;

            // Resets Player 2 location
            player2.HitBox.X = GraphicsDevice.Viewport.Width - 250;
            player2.HitBox.Y = GraphicsDevice.Viewport.Height / 2;
            player2.Weapon = player2.DefaultWeapon;
            player2.Weapon.HitBox.X = player2.HitBox.X + P2_RIFLE_X_OFFSET;
            player2.Weapon.HitBox.Y = player2.HitBox.Y + P2_RIFLE_Y_OFFSET;

            // Resets score
            player1.Score = 0;
            player2.Score = 0;

            // Resets middle line
            middleLocation = (GraphicsDevice.Viewport.Width / 2) - 25;
        }


        // Reads in the saved level from the level editor
        public void GenerateMap()
        {
            player1.spawnedPowerUps.Clear();
            player2.spawnedPowerUps.Clear();
            StreamReader r = new StreamReader("level.json");
            string levelData = r.ReadToEnd();
            r.Close();
            tileValues = JsonConvert.DeserializeObject<int[,]>(levelData);
            for (int i = 0; i < LEVEL_EDITOR_ROWS; i++)
            {
                for (int j = 0; j < LEVEL_EDITOR_COLUMNS; j++)
                {
                    if (j < LEVEL_EDITOR_COLUMNS / 2)
                    {
                        if (tileValues[i, j] == 2)
                        {
                            player1.spawnedPowerUps.Add(new Barricade(j * 160 + 20 + 340, i * 150 + 20 + 200));
                        }
                        else if (tileValues[i,j] == 3)
                        {
                            player1.spawnedPowerUps.Add(new Turret(j * 160 + 20 + 340, i * 150 + 20 + 200, Direction.RIGHT));
                        }
                    }
                    else
                    {
                        if (tileValues[i, j] == 2)
                        {
                            player2.spawnedPowerUps.Add(new Barricade(j * 160 + 20 + 340, i * 150 + 20 + 200));
                        }
                        else if (tileValues[i, j] == 3)
                        {
                            player2.spawnedPowerUps.Add(new Turret(j * 160 + 20 + 340, i * 150 + 20 + 200, Direction.LEFT));
                        }
                    }
                }
            }
        }

        // Sets durrations for turrets and machineguns
        public void UpdateDurations()
        {
            for (int i = 0; i < player1.spawnedPowerUps.Count; i++)
            {
                if (player1.spawnedPowerUps[i].Duration > 0)
                {
                    player1.spawnedPowerUps[i].Duration--;
                    if (player1.spawnedPowerUps[i].Duration == 0)
                    {
                        if (player1.spawnedPowerUps[i] is Turret)
                        {
                            int num = rng.Next(1, 5);
                            if(num == 1)
                                player1.spawnedPowerUps.Add(new MachineGun_PowerUp(player1.spawnedPowerUps[i].HitBox.X, player1.spawnedPowerUps[i].HitBox.Y));
                            player1.spawnedPowerUps.RemoveAt(i);
                            break;
                        }
                        if (player1.spawnedPowerUps[i] is MachineGun_PowerUp)
                        {
                            player1.spawnedPowerUps.RemoveAt(i);
                        }
                    }
                }
            }
            for (int i = 0; i < player2.spawnedPowerUps.Count; i++)
            {
                if (player2.spawnedPowerUps[i].Duration > 0)
                {
                    player2.spawnedPowerUps[i].Duration--;
                    if (player2.spawnedPowerUps[i].Duration == 0)
                    {
                        if (player2.spawnedPowerUps[i] is Turret)
                        {
                            int num = rng.Next(1, 5);
                            if(num == 1)
                                player2.spawnedPowerUps.Add(new MachineGun_PowerUp(player2.spawnedPowerUps[i].HitBox.X, player2.spawnedPowerUps[i].HitBox.Y));
                            player2.spawnedPowerUps.RemoveAt(i);
                            break;
                        }
                        if (player2.spawnedPowerUps[i] is MachineGun_PowerUp)
                        {
                            player2.spawnedPowerUps.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}