using System;
using System.Collections.Generic;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.Components
{
    public sealed class Item : AnimatedSprite
    {
        static Texture2D texture;
        static Dictionary<AnimationKey, Animation> animations;        

        public static void LoadContent(ContentManager content)
        {
            Item.texture = content.Load<Texture2D>(@"Sprites\8bitsMisc");
            animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(4, 54, 31, 0, 2);
            animation.FramesPerSecond = 2;
            animations.Add(AnimationKey.Up, animation);            

        }

        public Item(Game game, Vector2 position)
            : base(texture, animations)
        {
            this.Velocity = new Vector2(0, 0);
            this.Position = position;
            this.CurrentAnimation = AnimationKey.Up;
            this.IsAnimating = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
