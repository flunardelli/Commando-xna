using System;
using Framework2D.Base;
using Framework2D.Base.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Framework2D.Base.Input;
using Microsoft.Xna.Framework.Input;

namespace Commando.GameScreens
{
    public class WinScreen : BaseScreen
    {
        Texture2D background;   

        protected Song menuMusic;        

        SpriteFont ScoreFont;

        public int highScore = 0;
        public int player1Score = 0;
        public int player2Score = 0;

        private Vector2 position; 

        public WinScreen(Game game, ScreenManager manager)
            : base(game, manager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            
            background = ((CommandoGame)Game).Content.Load<Texture2D>(@"Background\8bitsLevelComplete");
            ScoreFont = ((CommandoGame)Game).Content.Load<SpriteFont>(@"Fonts\8bitsScoreFontTexture");

            menuMusic = ((CommandoGame)Game).Content.Load<Song>(@"Music\TitleTheme");

            MediaPlayer.Play(menuMusic);

            position = new Vector2(((CommandoGame)Game).Width / 2, ((CommandoGame)Game).Height / 2);
            
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

            if (InputHandler.KeyDown(Keys.Enter))
            {
                ScreenManager.ChangeScreen(((CommandoGame)Game).MenuScreen);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ((CommandoGame)Game).SpriteBatch;

            spriteBatch.Begin();

            base.Draw(gameTime);
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
           

            spriteBatch.DrawString(ScoreFont, player1Score.ToString("000000"), new Vector2(32, 48), Color.White);
            spriteBatch.DrawString(ScoreFont, highScore.ToString("000000"), new Vector2(208, 48), Color.Pink);
            spriteBatch.DrawString(ScoreFont, player2Score.ToString("000000"), new Vector2(400, 48), Color.White);

            spriteBatch.DrawString(ScoreFont, "THANKS FOR PLAYING", new Vector2(95,400), Color.White);
            spriteBatch.End();
        }
    }
}
