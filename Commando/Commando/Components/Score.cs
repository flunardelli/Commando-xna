using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.Components
{
    public enum PlayerNum { Player1, Player2 };

    public class Score
    {
        static Texture2D texture;
        private PlayerNum currentPlayerNum;

        public Vector2 Position;
        static SpriteFont font;

        private Rectangle player1LiveRec;
        private Rectangle player2LiveRec;
        private Rectangle player1GrenadeRec;
        private Rectangle player2GrenadeRec;

        private CommandoGame commandoGame;

        private Color player1Color;
        private Color player2Color;

        public static void LoadContent(ContentManager content)
        {
            Score.texture = content.Load<Texture2D>(@"Sprites\8bitsScore");
            Score.font = content.Load<SpriteFont>(@"Fonts\8bitsScoreFontTexture");
        }

        public Score(CommandoGame game)
        {
            player1LiveRec = new Rectangle(0, 0, 20, 32);
            player2LiveRec = new Rectangle(20, 0, 20, 32);
            player1GrenadeRec = new Rectangle(0, 34, 20, 26);
            player2GrenadeRec = new Rectangle(20, 34, 20, 26);

            commandoGame = ((CommandoGame)game);

            player1Color = Color.White;
            player2Color = Color.Red;
        }

        private Vector2 upScorePosition;
        private Vector2 downScorePosition;

        public void Update(GameTime gameTime)
        {
           currentPlayerNum = commandoGame.GamePlayScreen.PlayerNum;
           downScorePosition = commandoGame.GamePlayScreen.Camera.Position + new Vector2(30, 420);
           upScorePosition = commandoGame.GamePlayScreen.Camera.Position + new Vector2(30, 20);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (currentPlayerNum == PlayerNum.Player1 && commandoGame.GamePlayScreen.player1 != null)
            {                
                drawText(spriteBatch, "P1", new Vector2(upScorePosition.X, upScorePosition.Y - 20), player1Color);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player1.Score.ToString("000000"), new Vector2(upScorePosition.X, upScorePosition.Y), player1Color);
                spriteBatch.Draw(Score.texture, new Vector2(downScorePosition.X, downScorePosition.Y), player1LiveRec, Color.White);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player1.Lives.ToString("00"), new Vector2(downScorePosition.X + 30, downScorePosition.Y), player1Color);
                spriteBatch.Draw(Score.texture, new Vector2(downScorePosition.X + 80, downScorePosition.Y), player1GrenadeRec, Color.White);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player1.Grenades.ToString("00"), new Vector2(downScorePosition.X + 110, downScorePosition.Y), player1Color);
            }
            else if (commandoGame.GamePlayScreen.player1 != null && commandoGame.GamePlayScreen.player2 != null)
            {
                drawText(spriteBatch, "P1", new Vector2(upScorePosition.X, upScorePosition.Y - 20), player1Color);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player1.Score.ToString("000000"), new Vector2(upScorePosition.X, upScorePosition.Y), player1Color);                   
                spriteBatch.Draw(Score.texture, new Vector2(downScorePosition.X, downScorePosition.Y), player1LiveRec, Color.White);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player1.Lives.ToString("00"), new Vector2(downScorePosition.X + 30, downScorePosition.Y), player1Color);
                spriteBatch.Draw(Score.texture, new Vector2(downScorePosition.X + 80, downScorePosition.Y), player1GrenadeRec, Color.White);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player1.Grenades.ToString("00"), new Vector2(downScorePosition.X + 110, downScorePosition.Y), player1Color);
               
                //p2
                drawText(spriteBatch, "P2", new Vector2(upScorePosition.X + 300, upScorePosition.Y - 20), player2Color);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player2.Score.ToString("000000"), new Vector2(upScorePosition.X + 300, upScorePosition.Y), player2Color);                   
                spriteBatch.Draw(Score.texture, new Vector2(downScorePosition.X + 300, downScorePosition.Y), player2LiveRec, Color.White);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player2.Lives.ToString("00"), new Vector2(downScorePosition.X + 300 + 30, downScorePosition.Y), player2Color);
                spriteBatch.Draw(Score.texture, new Vector2(downScorePosition.X + 300 + 80, downScorePosition.Y), player2GrenadeRec, Color.White);
                drawText(spriteBatch, commandoGame.GamePlayScreen.player2.Grenades.ToString("00"), new Vector2(downScorePosition.X + 300 + 110, downScorePosition.Y), player2Color);                                 
            }

        }

        private void drawText(SpriteBatch spriteBatch,string text, Vector2 position,Color color)
        {
           spriteBatch.DrawString(font, text, position, color);
        }
    }
}
