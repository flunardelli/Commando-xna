using System;
using Framework2D.Base;
using Framework2D.Base.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Commando.GameScreens
{
    public class MenuScreen : BaseScreen
    {
        Texture2D background;

        LinkLabel onePlayer;
        LinkLabel twoPlayer;

        Texture2D grenadeSelector;

        protected Song menuMusic;

        string currentPlayerIndex;

        SpriteFont ScoreFont;

        public int highScore = 0;
        public int player1Score = 0;
        public int player2Score = 0;

        protected ControlManager ControlManager { get; set; }

        public MenuScreen(Game game, ScreenManager manager)
            : base(game, manager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ControlManager = new ControlManager(((CommandoGame)Game).Content.Load<SpriteFont>(@"Fonts\ControlFont"));
            background = ((CommandoGame)Game).Content.Load<Texture2D>(@"Background\8bitsMenu");
            grenadeSelector = ((CommandoGame)Game).Content.Load<Texture2D>(@"Sprites\8bitsMisc");
            ScoreFont = ((CommandoGame)Game).Content.Load<SpriteFont>(@"Fonts\8bitsScoreFontTexture");

            menuMusic = ((CommandoGame)Game).Content.Load<Song>(@"Music\TitleTheme");

            MediaPlayer.Play(menuMusic);

            Vector2 position = new Vector2(((CommandoGame)Game).Width / 2, ((CommandoGame)Game).Height / 2);

            onePlayer = new LinkLabel();
            onePlayer.HasFocus = true;
            onePlayer.Text = "Player1";

            position.X -= onePlayer.Size.X / 2;
            onePlayer.Position = position;
            onePlayer.Selected += new EventHandler(menu_Selected);

            position.Y += onePlayer.Size.Y + 5f;

            ControlManager.Add(onePlayer);

            twoPlayer = new LinkLabel();
            twoPlayer.Text = "Player2";
            twoPlayer.Position = position;
            twoPlayer.Selected += new EventHandler(menu_Selected);

            ControlManager.Add(twoPlayer);


            currentPlayerIndex = "Player1";
            
        }

        void menu_Selected(object sender, System.EventArgs e)
        {
            GamePlayScreen gamePlayerScreen = ((CommandoGame)Game).GamePlayScreen;
            if (sender == onePlayer)
            {
                gamePlayerScreen.PlayerNum = Commando.Components.PlayerNum.Player1;
            }
            else if (sender == twoPlayer)
            {
                gamePlayerScreen.PlayerNum = Commando.Components.PlayerNum.Player2;
            }

            gamePlayerScreen.Restart();

            ScreenManager.ChangeScreen(gamePlayerScreen);
        }

        public override void Update(GameTime gameTime)
        {

            if (player1Score > highScore)
            {
                highScore = player1Score;
            }

            if (player2Score > highScore)
            {
                highScore = player2Score;
            }

            ControlManager.Update(gameTime);
            foreach (Control c in ControlManager)
            {
                FocusableControl focusableControl = c as FocusableControl;
                if (focusableControl != null && focusableControl.HasFocus)
                {
                    focusableControl.HandleInput();
                    currentPlayerIndex = c.Text;
                    break;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ((CommandoGame)Game).SpriteBatch;

            spriteBatch.Begin();

            base.Draw(gameTime);
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            if (currentPlayerIndex == "Player1")
            {
                spriteBatch.Draw(grenadeSelector, new Vector2(146, 236), new Rectangle(0, 66, 26, 28), Color.White);
            }
            else
            {
                spriteBatch.Draw(grenadeSelector, new Vector2(146, 265), new Rectangle(0, 66, 26, 28), Color.White);
            }

            spriteBatch.DrawString(ScoreFont, player1Score.ToString("000000"), new Vector2(32, 48), Color.White);
            spriteBatch.DrawString(ScoreFont, highScore.ToString("000000"), new Vector2(208, 48), Color.Pink);
            spriteBatch.DrawString(ScoreFont, player2Score.ToString("000000"), new Vector2(400, 48), Color.White);
            spriteBatch.End();
        }
    }
}
