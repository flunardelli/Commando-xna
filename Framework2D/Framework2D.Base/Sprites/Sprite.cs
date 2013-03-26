using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.GameScreens;

namespace Framework2D.Base.Sprites
{
    public class Sprite : IObject
    {
        protected Texture2D Texture { get; set; }
        public Rectangle sourceRectangle;

        public virtual int Width { get { return Texture.Width; } }
        public virtual int Height { get { return Texture.Height; } }

        public virtual Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); } }

        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }

        private float Acceleration { get; set; }
        private float MaxVelocity { get; set; }

        public Sprite(Texture2D image, Rectangle? sourceRectangle)
        {
            this.Texture = image;

            if (sourceRectangle.HasValue)
            {
                this.sourceRectangle = sourceRectangle.Value;
            }
            else
            {
                this.sourceRectangle = new Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height);
            }

            this.Position = Vector2.Zero;
            this.Velocity = Vector2.Zero;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color.White);        
        }
    }
}
