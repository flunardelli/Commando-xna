using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework2D.Base.Sprites
{
    public class AnimatedSprite : Sprite
    {
        Dictionary<AnimationKey, Animation> animations;

        public AnimationKey CurrentAnimation { get; set; }
        public int CurrentFrame { get { return animations[CurrentAnimation].CurrentFrame; } }

        public bool IsAnimating { get; set; }

        private int width;
        public override int Width { get { return width; } }

        private int height;
        public override int Height { get { return height; } }

        public AnimatedSprite(Texture2D image, Dictionary<AnimationKey, Animation> animation)
            : base(image, null)
        {
            animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());

            width = Int32.MaxValue;
            height = Int32.MaxValue;
            foreach (AnimationKey key in animation.Keys)
            {
                width = (width > animation[key].FrameWidth) ? animation[key].FrameWidth : width;
                height = (height > animation[key].FrameHeight) ? animation[key].FrameHeight : height;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAnimating)
                animations[CurrentAnimation].Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.Texture,
                this.Position,
                animations[CurrentAnimation].CurrentFrameRect,
                Color.White);
        }
    }
}
