using System;
using System.Collections.Generic;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Commando.Components
{
    public sealed class Bullet : Sprite
    {
        private enum RectangleKey { Normal, Explode }

        static Texture2D texture;
        Dictionary<RectangleKey, Rectangle> animations;

        RectangleKey currentRectangle;

        public Character Shooter { get; set; }
        public bool IsStopUpdate { get; set; }     

        private Vector2 motion;

        static SoundEffect shootSound;

        private float range;

        private TimeSpan animationDeadTimer;
        private TimeSpan animationDeadLength;

        public static void LoadContent(ContentManager content)
        {
            Bullet.texture = content.Load<Texture2D>(@"Sprites\8bitsBullet");

            Bullet.shootSound = content.Load<SoundEffect>(@"Sound\gunshot");
        }

        public Bullet(Sprite sprite, Vector2 motion, Character shooter)
            : base(texture, new Rectangle(2, 2, 10, 10))
        {
            animations = new Dictionary<RectangleKey, Rectangle>();

            Rectangle bulletRect = new Rectangle(2, 2, 10, 10);
            animations.Add(RectangleKey.Normal, bulletRect);

            bulletRect = new Rectangle(14, 0, 14, 14);
            animations.Add(RectangleKey.Explode, bulletRect);

            currentRectangle = RectangleKey.Normal ;

            this.Velocity = new Vector2(100, 100);

            this.Position = new Vector2(
                sprite.Position.X + (sprite.Width / 2 - animations[currentRectangle].Width / 2),
                sprite.Position.Y + (sprite.Height / 2 - animations[currentRectangle].Height / 2));
            this.motion = motion;

            this.Shooter = shooter;
            range = 200;
            this.IsStopUpdate = false;

            animationDeadLength = TimeSpan.FromSeconds(0.1f);

            shootSound.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Bullet.texture, this.Position, animations[currentRectangle], Color.White);
        }

        public override void Update(GameTime gameTime)
        {   
            if (!IsStopUpdate)
            {               
                if (this.currentRectangle == RectangleKey.Explode)
                {
                    animationDeadTimer += gameTime.ElapsedGameTime;

                    if (animationDeadTimer >= animationDeadLength)
                    {
                        SceneItems.Instance.Remove(this);
                    }
                }
                else
                {
                    Vector2 lastPosition = this.Position;

                    this.Position += this.motion * this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    range -= Vector2.Distance(this.Position, lastPosition);
                                        
                    if (this.range < 10)
                    {
                        ChangeExplode();
                    }
                    else if (!(this.Shooter is EnemyMachineGun) && Map.Instance.Tiles.TestTileCollision((int)this.Position.X, (int)this.Position.Y, (int)this.Width, (int)this.Height) == CollisionType.Block)
                    {
                        ChangeExplode();
                    }
                }
            }
        }

        private void ChangeExplode()
        {
            currentRectangle = RectangleKey.Explode;

            float diffWidth = this.animations[RectangleKey.Normal].Width - this.animations[RectangleKey.Explode].Width;
            float diffHeight = this.animations[RectangleKey.Normal].Height - this.animations[RectangleKey.Explode].Height;

            this.Position = new Vector2(
                this.Position.X - (diffWidth / 2),
                this.Position.Y - (diffHeight / 2));
        }
    }
}
