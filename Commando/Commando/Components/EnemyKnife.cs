using System.Collections.Generic;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.Components
{
    public class EnemyKnife : Enemy
    {
        static Texture2D texture;
        static Dictionary<AnimationKey, Animation> animations;

        private Player playerDetected;

        public static void LoadContent(ContentManager content)
        {
            EnemyKnife.texture = content.Load<Texture2D>(@"Sprites\8bitsEnemyKnife");
            EnemyKnife.animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(2, 22, 42, 0, 0);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(2, 32, 44, 0, 44);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(2, 32, 44, 0, 88);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(2, 26, 44, 0, 132);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(2, 32, 54, 0, 176);
            animations.Add(AnimationKey.Dead, animation);
            animation.FramesPerSecond = 2;
        }

        public EnemyKnife(Game game)
            : base(game, new AnimatedSprite(texture, animations))
        {
            visionRadius = 300.0f;
            this.Sprite.Velocity = new Vector2(110, 110);
        }

        protected override Vector2 Walk(GameTime gameTime)
        {
            Vector2 motion = createMotion();

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
                        nextPosition += motion * Sprite.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (Map.Instance.Tiles.TestTileCollision((int)Sprite.Position.X, (int)nextPosition.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                    {
                        nextPosition += new Vector2(-1f, 1f) * Sprite.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }                

                this.Sprite.Position = nextPosition;
            }
            else
            {
                Sprite.IsAnimating = false;
            }

            return motion;
        }

        private Vector2 createMotion()
        {
            Vector2 motion = Vector2.Zero;

            playerDetected = EnemyAI.PlayerClosest(this, visionRadius);

            if (playerDetected != null)
            {
                motion.X = playerDetected.Sprite.Position.X - Sprite.Position.X;

                if (motion.X >= -1 && motion.X <= 1)
                {
                    motion.X = 0;
                    motion.Y = playerDetected.Sprite.Position.Y - Sprite.Position.Y;
                }
                else
                {
                    motion.Y = 0;
                }
            }

            return NormalizeMotion(motion);
        }

        private Vector2 NormalizeMotion(Vector2 motion)
        {
            if (motion != Vector2.Zero)
            {
                motion.Normalize();

                if (motion.Y < 0)
                {
                    Sprite.CurrentAnimation = AnimationKey.Up;
                }
                else if (motion.Y > 0)
                {
                    Sprite.CurrentAnimation = AnimationKey.Down;
                }
                else if (motion.X < 0)
                {
                    Sprite.CurrentAnimation = AnimationKey.Left;
                }
                else if (motion.X > 0)
                {
                    Sprite.CurrentAnimation = AnimationKey.Right;
                }
            }

            return motion;
        }

        protected override void Shoot(Vector2 velocity, Vector2 motion)
        {
        }
    }
}
