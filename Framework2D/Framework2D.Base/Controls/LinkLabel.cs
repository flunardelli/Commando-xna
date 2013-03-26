using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Framework2D.Base.Input;
using Microsoft.Xna.Framework.Input;

namespace Framework2D.Base.Controls
{
    public enum TypeEffect { Nome = 0, Blink = 1 };

    public class LinkLabel : FocusableControl
    {
        public TypeEffect TypeEffect { get; set; }
        int blinkTime;

        public override Vector2 Size
        {
            get
            {
                if (base.Size == Vector2.Zero && Text.Length > 0)
                    return SpriteFont.MeasureString(Text);

                return base.Size;
            }
            set { base.Size = value; }
        }

        public LinkLabel()
        {
            TypeEffect = TypeEffect.Nome;
            blinkTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!HasFocus)
                spriteBatch.DrawString(this.SpriteFont, this.Text, this.Position, this.Color);
            else
            {
                if (TypeEffect == TypeEffect.Blink)
                {
                    blinkTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds; 

                    if (((blinkTime / 500) % 2) == 1)
                    {
                        spriteBatch.DrawString(this.SpriteFont, this.Text, this.Position, this.Color);
                    }

                    if (blinkTime >= 1000) blinkTime = 0;
                }
                else
                {
                    spriteBatch.DrawString(this.SpriteFont, this.Text, this.Position, this.SelectedColor);
                }
            }
        }

        public override bool CanFocus()
        {
            return this.Enabled && this.Visible;
        }

        public override void HandleInput()
        {
            if (InputHandler.KeyReleased(Keys.Enter))
                base.OnSelected(null);
        }
    }
}
