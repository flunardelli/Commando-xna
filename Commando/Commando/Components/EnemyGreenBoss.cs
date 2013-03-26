using System;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;

namespace Commando.Components
{
    public class EnemyGreenBoss : Enemy
    {
        private Vector2 motion;
        private Random random;

        private TimeSpan changeMoveTimer;
        private TimeSpan changeMoveLength;

        private TimeSpan shootTimer;
        private TimeSpan shootLength;

        public EnemyGreenBoss(Game game, AnimatedSprite sprite)
            : base(game, sprite)
        {
            motion = new Vector2(1, 0);
            random = new Random(DateTime.Now.Millisecond);

            changeMoveLength = TimeSpan.FromSeconds(1);
            shootLength = TimeSpan.FromSeconds(2);

            this.Sprite.Velocity = new Vector2(50, 50);
        }

        protected override Vector2 Walk(GameTime gameTime)
        {
            if (motion != Vector2.Zero)
            {
                Sprite.IsAnimating = true;

                Vector2 nextPosition = new Vector2();
                nextPosition = Sprite.Position;
                nextPosition += motion * Sprite.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Map.Instance.Tiles.TestTileCollision((int)nextPosition.X, (int)nextPosition.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Fall)
                {
                    nextPosition = Sprite.Position;
                }
                else
                {
                    if (Map.Instance.Tiles.TestTileCollision((int)nextPosition.X, (int)Sprite.Position.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                    {
                        nextPosition.X = this.Sprite.Position.X;
                        motion *= -1;
                        this.Sprite.CurrentAnimation = (motion.X > 0) ? AnimationKey.Right : AnimationKey.Left;
                    }
                    if (Map.Instance.Tiles.TestTileCollision((int)Sprite.Position.X, (int)nextPosition.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                    {
                        nextPosition.Y = this.Sprite.Position.Y;
                    }
                }

                Sprite.Position = new Vector2(
                    MathHelper.Clamp(nextPosition.X, -100, (mapWidth - Sprite.Width) + 100),
                    MathHelper.Clamp(nextPosition.Y, -100, (mapHeight - Sprite.Height) + 100));
            }
            else
            {
                Sprite.IsAnimating = false;
            }

            return motion;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            changeMoveTimer += gameTime.ElapsedGameTime;

            if (changeMoveTimer > changeMoveLength)
            {
                if (random.Next() % 2 == 0)
                {
                    motion *= -1;
                    this.Sprite.CurrentAnimation = (motion.X > 0) ? AnimationKey.Right : AnimationKey.Left;
                    this.Sprite.Velocity = new Vector2(50, 50);
                }

                if (random.Next() % 2 == 0)
                {
                    motion *= -1;
                    this.Sprite.CurrentAnimation = (motion.X > 0) ? AnimationKey.DownRight : AnimationKey.DownLeft;
                    this.Sprite.Velocity = new Vector2(0, 0);
                }

                changeMoveTimer = TimeSpan.Zero;
            }

            shootTimer += gameTime.ElapsedGameTime;
        }

        protected override void Shoot(Vector2 velocity, Vector2 motion)
        {
            if (shootTimer > shootLength && (this.Sprite.CurrentAnimation == AnimationKey.DownLeft || this.Sprite.CurrentAnimation == AnimationKey.DownRight))
            {
                Grenade bullet = new Grenade(this.Sprite, this);
                bullet.CurrentAnimation = AnimationKey.Down;
                SceneItems.Instance.Add(bullet);

                shootTimer = TimeSpan.Zero;
            }
        }

        protected override void Die()
        {
            base.Die();
            this.Sprite.Position = new Vector2(this.Sprite.Position.X, this.Sprite.Position.Y - 20);
        }
    }
}
