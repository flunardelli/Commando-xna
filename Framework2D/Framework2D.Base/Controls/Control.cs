using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework2D.Base.Controls
{
    public abstract class Control
    {
        public Color Color { get; set; }

        public Vector2 Position { get; set; }

        public SpriteFont SpriteFont { get; set; }
        public string Text { get; set; }

        public virtual Vector2 Size { get; set; }

        public bool Visible { get; set; }
        public bool Enabled { get; set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public Control()
        {
            this.Position = Vector2.Zero;
            this.Visible = true;
            this.Enabled = true;
            this.SpriteFont = ControlManager.SpriteFont;
            this.Color = Color.White;
        }
    }
}
