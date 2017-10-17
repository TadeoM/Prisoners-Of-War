using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prisoners_of_War_V2
{
    enum GameState
    {
        MENU,
        LEVELEDITOR,
        GAME,
        GAMEOVER
    }

    enum Direction
    {
        LEFT,
        RIGHT
    }

    enum AnimationState
    {
        STANDING,
        RUNNING,
        SIDESTEPPING
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameLoop : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //attributes
        Texture2D bulletTexture;
        Texture2D usWeaponTexture;
        Texture2D ruWeaponTexture;
        Texture2D background;
        Texture2D privateCursor;
        SpriteFont font;
        Texture2D spritesheet;

        Player[] players = new Player[2];

        Vector2 player1Score;
        Vector2 player2Score;

        KeyboardState kbState;
        KeyboardState prevKbState;

        LevelEditor editor = new LevelEditor();
        Texture2D buttonTexture;
        MouseState previousMouseState;
        MouseState currentMouseState;
        MouseState msState;
        Vector2 mousePos;
        Vector2 windowScale;

        Vector2 menuTextLength;

        int[,] tileValues = new int[9, 16];

        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
