using Commando.GameScreens;
using Framework2D.Base;
using Framework2D.Base.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Commando
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CommandoGame : Game
    {
        public SpriteBatch SpriteBatch { get; set; }
        public int Width { get { return graphics.PreferredBackBufferWidth; } }
        public int Height { get { return graphics.PreferredBackBufferHeight; } }

        public DisclaimScreen DisclaimScreen { get; set; }
        public MenuScreen MenuScreen { get; set; }
        public GamePlayScreen GamePlayScreen { get; set; }
        public WinScreen WinScreen { get; set; }

        private ScreenManager screenManager;
        private GraphicsDeviceManager graphics;


        public Rectangle ScreenRectangle
        {
            get
            {
                return new Rectangle(
                    0,
                    0,
                    graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight);
            }
        }

        public CommandoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 464;

            Components.Add(new InputHandler(this));

            screenManager = new ScreenManager(this);

            DisclaimScreen = new DisclaimScreen(this, screenManager);
            MenuScreen = new MenuScreen(this, screenManager);
            GamePlayScreen = new GamePlayScreen(this, screenManager);
            WinScreen = new WinScreen(this, screenManager);

            screenManager.ChangeScreen(DisclaimScreen);
            //screenManager.ChangeScreen(GamePlayScreen);
            //screenManager.ChangeScreen(MenuScreen);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);                       
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
