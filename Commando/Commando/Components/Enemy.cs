using System;
using Commando.GameScreens;
using Commando.Singleton;
using Framework2D.Base.Input;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Commando.Components
{
    public enum BehaviorStates { Follow = 0, FollowXY = 1, Evade = 2, Random = 3 };

    public abstract class Enemy : Character
    {
        public MapObject MapObject = null;
        private static int seed = 0;
        public bool IsStopUpdate { get; set; }

        private Player playerDetected;
        public float visionRadius;

        private float shootIntervalSeconds;
        private float shootIntervalCount;

        private float changeBehaviorSeconds;
        private float changeBehaviorCount;

        private float changeMovementSeconds;
        private float changeMovementCount;

        private Random random;

        public BehaviorStates currentBehavior { get; set; }

        private bool prevFire = true;

        private AnimationKey prevAnimmation;

        private Vector2 prevMotion;

        public Enemy(Game game, AnimatedSprite sprite)
            : base(game, sprite)
        {
            Sprite.Velocity = new Vector2(70, 70);

            shootIntervalSeconds = 4f;
            shootIntervalCount = 0f;

            changeBehaviorSeconds = 2f;
            changeBehaviorCount = 0f;

            changeMovementSeconds = 2f;
            changeMovementCount = 0f;

            visionRadius = 600.0f;

            seed = (seed > 1000) ? 0 : seed + 10;
            random = new Random(DateTime.Now.Millisecond + seed);

            currentBehavior = (BehaviorStates)random.Next(0, 4);
            shootVelocity = new Vector2(120, 120);

            IsStopUpdate = false;
        }


        public override void Update(GameTime gameTime)
        {
            if (!IsStopUpdate)
            {
                if (shootIntervalCount < shootIntervalSeconds)
                    shootIntervalCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (changeBehaviorCount < changeBehaviorSeconds)
                    changeBehaviorCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (changeMovementCount < changeMovementSeconds)
                    changeMovementCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

                random = new System.Random(DateTime.Now.Millisecond + (int)Sprite.Position.Y);

                if (changeBehaviorCount > changeBehaviorSeconds)
                {
                    prevAnimmation = Sprite.CurrentAnimation;

                    currentBehavior = (BehaviorStates)random.Next(0, 4);

                    changeBehaviorCount = 0;
                }

                prevFire = InputHandler.KeyDown(Keys.T);

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (debug)
                DrawDebug(spriteBatch);

            base.Draw(gameTime, spriteBatch);
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
                    if (Map.Instance.Tiles.TestTileCollision((int)nextPosition.X, (int)nextPosition.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                    {
                        nextPosition = Sprite.Position;
                        currentBehavior = BehaviorStates.Follow;
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

        protected override void Shoot(Vector2 velocity, Vector2 motion)
        {
            if (playerDetected != null && shootIntervalCount > shootIntervalSeconds)
            {

                if (random.Next(1, 3) % 2 == 0)
                {
                    motion = NormalizeMotion(playerDetected.Sprite.Position - Sprite.Position);
                    base.Shoot(shootVelocity, motion);
                }

                shootIntervalCount = 0;
            }
        }

        protected override void Collisions()
        {
            Rectangle boxEnemy = this.Sprite.Rectangle;

            foreach (IObject item in SceneItems.Instance)
            {
                Bullet bullet = item as Bullet;
                if (bullet != null && !(bullet.Shooter is Enemy))
                {
                    if (boxEnemy.Intersects(bullet.Rectangle))
                    {
                        SceneItems.Instance.Remove(bullet);
                        this.Die();
                        UpdateScore((Player)bullet.Shooter);
                        return;
                    }
                }

                Grenade grenade = item as Grenade;
                if (grenade != null && !(grenade.Shooter is Enemy) && grenade.IsExploding)
                {
                    if (boxEnemy.Intersects(grenade.Rectangle))
                    {
                        UpdateScore((Player)grenade.Shooter);
                        this.Die();
                        return;
                    }
                }
            }
        }

        protected override void Dead()
        {
            base.Dead();

            SceneItems.Instance.Remove(this);
        }

        private void DrawDebug(SpriteBatch spriteBatch)
        {
            Color debugColor;

            if (playerDetected != null)
            {
                CommandoDebug.DrawLine(
                    spriteBatch,
                    Sprite.Position,
                    playerDetected.Sprite.Position,
                    Color.Red, 0.0f);

                debugColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            }
            else
                debugColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);

            CommandoDebug.DrawCircle(
                spriteBatch,
                Sprite.Position,
                visionRadius, 32,
                debugColor, 0.0f);
        }

        private Vector2 createMotion()
        {
            playerDetected = EnemyAI.PlayerClosest(this, visionRadius);

            Vector2 motion = Vector2.Zero;

            if (currentBehavior == BehaviorStates.Follow && playerDetected != null)
            {
                motion = playerDetected.Sprite.Position - Sprite.Position;
            }
            else if (currentBehavior == BehaviorStates.FollowXY && playerDetected != null)
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
            else if (currentBehavior == BehaviorStates.Evade && playerDetected != null)
            {

                motion = new Vector2(playerDetected.Sprite.Position.X != 0 ? -playerDetected.Sprite.Position.X : 0, playerDetected.Sprite.Position.Y != 0 ? -playerDetected.Sprite.Position.Y : 0);

            }
            else if (currentBehavior == BehaviorStates.Random)
            {
                Vector2 randomMotion = Vector2.Zero;

                if (changeMovementCount > changeMovementSeconds)
                {
                    float rx = (float)random.Next(-1, 2);
                    float ry = (float)random.Next(-1, 2);
                    randomMotion = new Vector2(rx, ry);
                    changeMovementCount = 0;
                }

                motion += randomMotion;
            }
            else
            {
                motion = AnimationToMove(Sprite.CurrentAnimation);
            }

            return NormalizeMotion(motion);
        }

        private void UpdateScore(Player player)
        {
            player.Score += 150;
        }

        private Vector2 NormalizeMotion(Vector2 motion)
        {
            if (motion != Vector2.Zero)
            {
                motion.Normalize();

                if (motion.Y < -0.5f && motion.X < -0.5f)
                {
                    Sprite.CurrentAnimation = AnimationKey.UpLeft;
                }
                else if (motion.Y < -0.5f && motion.X > 0.5f)
                {
                    Sprite.CurrentAnimation = AnimationKey.UpRight;
                }
                else if (motion.Y > 0.5f && motion.X < -0.5f)
                {
                    Sprite.CurrentAnimation = AnimationKey.DownLeft;
                }
                else if (motion.Y > 0.5f && motion.X > 0.5f)
                {
                    Sprite.CurrentAnimation = AnimationKey.DownRight;
                }
                else if (motion.Y < 0)
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

            if (motion == Vector2.Zero)
            {
                motion = prevMotion;
            }
            else
            {
                prevMotion = motion;
            }

            return motion;
        }
    }
}
