using Framework2D.Base.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework2D.Base.Controls
{
    public class Label : Control
    {
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

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.SpriteFont, this.Text, this.Position, this.Color);
        }
    }
}
