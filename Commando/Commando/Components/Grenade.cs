using System;
using System.Collections.Generic;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.Components
{
    public sealed class Grenade : AnimatedSprite
    {
        static Texture2D texture;
        static Dictionary<AnimationKey, Animation> animations;

        public Character Shooter { get; set; }

        public bool IsStopUpdate { get; set; }
        public bool IsExploding { get; set; }

        static SoundEffect grenadeSound;

        private float range;

        private TimeSpan animationDeadTimer;
        private TimeSpan animationDeadLength;

        public override int Width
        {
            get
            {
                if (this.CurrentAnimation == AnimationKey.Dead)
                    return 62;

                return base.Width;
            }
        }

        public override int Height
        {
            get
            {
                if (this.CurrentAnimation == AnimationKey.Dead)
                    return 60;

                return base.Height;
            }
        }

        public static void LoadContent(ContentManager content)
        {
            Grenade.texture = content.Load<Texture2D>(@"Sprites\8bitsGrenade");

            Grenade.grenadeSound = content.Load<SoundEffect>(@"Sound\grenadeSound");

            animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(2, 10, 12, 2, 2);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(2, 10, 12, 2, 2);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(6, 62, 60, 0, 12);
            animations.Add(AnimationKey.Dead, animation);
        }

        public Grenade(Sprite sprite, Character shooter)
            : base(texture, animations)
        {
            this.Velocity = new Vector2(200, 200);

            this.Position = new Vector2(
                sprite.Position.X + (sprite.Width / 2 - this.Width / 2),
                sprite.Position.Y + (sprite.Height / 2 - this.Height / 2));

            this.Shooter = shooter;
            this.range = 200;

            this.IsStopUpdate = false;

            this.CurrentAnimation = AnimationKey.Up;
            animationDeadLength = TimeSpan.FromSeconds(1);
            this.IsAnimating = true;


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsStopUpdate)
            {
                if (this.CurrentAnimation == AnimationKey.Dead)
                {
                    animationDeadTimer += gameTime.ElapsedGameTime;

                    if (animationDeadTimer >= animationDeadLength)
                    {
                        SceneItems.Instance.Remove(this);
                    }
                }
                else
                {
                    Vector2 motion = (this.CurrentAnimation == AnimationKey.Up) ? new Vector2(0, -1) : new Vector2(0, 1);
                    Vector2 lastPosition = this.Position;

                    this.Position += motion * this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    range -= Vector2.Distance(this.Position, lastPosition);
                    if (this.range < 0)
                    {
                        this.CurrentAnimation = AnimationKey.Dead;

                        this.Position = new Vector2(
                            this.Position.X - (this.Width / 2),
                            this.Position.Y - (this.Height / 2));

                        IsExploding = true;
                        grenadeSound.Play();
                    }
                    else if (Map.Instance.Tiles.TestTileCollision((int)this.Position.X, (int)this.Position.Y, (int)this.Width, (int)this.Height) == CollisionType.Block)
                    {
                        this.CurrentAnimation = AnimationKey.Dead;

                        this.Position = new Vector2(
                            this.Position.X - (this.Width / 2),
                            this.Position.Y - this.Height);

                        IsExploding = true;
                        grenadeSound.Play();
                    }
                }

                base.Update(gameTime);
            }
        }
    }
}
