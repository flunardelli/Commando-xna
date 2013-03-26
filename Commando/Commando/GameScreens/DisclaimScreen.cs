using Framework2D.Base;
using Framework2D.Base.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Commando.GameScreens
{
    public class DisclaimScreen : BaseScreen
    {
        SpriteFont spriteFont;

        string message = "USE AND EXPORT OF THIS GAME\nOUTSIDE THE COUNTRY OF JAPAN\nIS IN VIOLATION OF\nCOPYRIGHT LAW\nAND COSTITUITES\nA CRIMINAL ACT";
        string credit = "CREDIT O1";

        Vector2 messagePosition;
        int currentWord = 0;

        public DisclaimScreen(Game game, ScreenManager manager)
            : base(game, manager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            //spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\ControlFont");
            spriteFont = ((CommandoGame)Game).Content.Load<SpriteFont>(@"Fonts\8bitsScoreFontTexture");
            float maxWidthMenssage = 0;
            foreach (string subString in message.Split('\n'))
            {
                if (maxWidthMenssage < spriteFont.MeasureString(subString).X)
                    maxWidthMenssage = spriteFont.MeasureString(subString).X;
            }

            messagePosition = new Vector2(
                (((CommandoGame)this.Game).Width / 2) - (maxWidthMenssage / 2),
                50);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.Enter))
                ScreenManager.ChangeScreen(((CommandoGame)Game).MenuScreen);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ((CommandoGame)this.Game).SpriteBatch;

            spriteBatch.Begin();

            base.Draw(gameTime);

            currentWord += (int)(gameTime.TotalGameTime.TotalSeconds / 3);
            currentWord = (currentWord > message.Length) ? message.Length : currentWord;

            string subString = message.Substring(0, (int)currentWord);

            spriteBatch.DrawString(spriteFont, subString, messagePosition, Color.White);

            if (currentWord == message.Length)
            {
                if (((int)gameTime.TotalGameTime.TotalSeconds % 2) == 1)
                {
                    spriteBatch.DrawString(spriteFont, credit,
                   new Vector2(((CommandoGame)this.Game).Width - 200, ((CommandoGame)this.Game).Height - 100),
                   Color.White);
                }
            }

            spriteBatch.End();
        }
    }
}
