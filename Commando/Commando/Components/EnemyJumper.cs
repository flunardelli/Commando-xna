using System;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;

namespace Commando.Components
{
    public class EnemyJumper : Enemy
    {
        private TimeSpan jumpTimer;
        private TimeSpan jumpLength;

        private bool isJumping;

        public EnemyJumper(Game game, AnimatedSprite sprite)
            : base(game, sprite)
        {
            visionRadius = 500.0f;
            jumpLength = TimeSpan.FromSeconds(0.8f);
            isJumping = true;
            currentBehavior = BehaviorStates.FollowXY;
        }

        protected override Vector2 Walk(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            if (isJumping)
            {
                if (Sprite.CurrentAnimation == AnimationKey.Left)
                {
                    motion = new Vector2(-1f, 0);
                }
                else if (Sprite.CurrentAnimation == AnimationKey.Right)
                {
                    motion = new Vector2(1f, 0);
                }

                else if (Sprite.CurrentAnimation == AnimationKey.JumpLeft)
                {
                    motion = new Vector2(-1f, 0);
                }
                else if (Sprite.CurrentAnimation == AnimationKey.JumpRight)
                {
                    motion = new Vector2(1, 0);
                }

                Sprite.IsAnimating = true;

                Vector2 nextPosition = new Vector2();
                nextPosition = Sprite.Position;
                nextPosition += motion * Sprite.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                int prev = (motion.X > 0) ? 10 + this.Sprite.Width : -10;

                if (Map.Instance.Tiles.TestTileCollision((int)nextPosition.X + prev, (int)Sprite.Position.Y + prev, (int)this.Sprite.Width, (int)this.Sprite.Height) != CollisionType.Block &&
                    Map.Instance.Tiles.TestTileCollision((int)this.Sprite.Position.X, (int)this.Sprite.Position.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                {
                    if (Sprite.CurrentAnimation == AnimationKey.Left)
                    {
                        Sprite.CurrentAnimation = AnimationKey.JumpLeft;
                    }
                    else if (Sprite.CurrentAnimation == AnimationKey.Right)
                    {
                        Sprite.CurrentAnimation = AnimationKey.JumpRight;
                    }
                }

                if (isJumping && (Sprite.CurrentAnimation == AnimationKey.JumpLeft || Sprite.CurrentAnimation == AnimationKey.JumpRight))
                {
                    jumpTimer += gameTime.ElapsedGameTime;
                }

                if (jumpTimer > jumpLength)
                {
                    isJumping = false;
                }

                Sprite.Position = nextPosition;
            }
            else
            {
                base.Walk(gameTime);
            }

            return motion;
        }
    }
}
