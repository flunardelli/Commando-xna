using System;
using Commando.GameScreens;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.Components
{
    public abstract class Character : IObject
    {
        public AnimatedSprite Sprite { get; protected set; }

        protected CommandoGame CommandoGame { get; set; }

        protected int mapWidth = 512;
        protected int mapHeight = 5760;

        public bool IsDead;
        public bool IsDying;
        public bool IsRespawning;

        private TimeSpan animationDeadTimer;
        private TimeSpan animationDeadLength;

        private TimeSpan animationRespawnTimer;
        private TimeSpan animationRespawnLength;

        private int blinkLenght;

        protected Vector2 shootVelocity;

        public bool debug;

        public Character(Game game, AnimatedSprite sprite)
        {
            this.CommandoGame = (CommandoGame)game;
            this.Sprite = sprite;

            this.IsDead = false;
            this.IsDying = false;
            this.IsRespawning = false;

            animationDeadLength = TimeSpan.FromSeconds(1);
            animationRespawnLength = TimeSpan.FromSeconds(2);
            blinkLenght = 100;

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.IsDying && !this.IsRespawning)
                this.Sprite.Draw(gameTime, spriteBatch);

            if (this.IsDying && ((((int)animationDeadTimer.TotalMilliseconds / blinkLenght) % 2) == 1))
                this.Sprite.Draw(gameTime, spriteBatch);

            if (this.IsRespawning && ((((int)animationRespawnTimer.TotalMilliseconds / blinkLenght) % 2) == 1))
                this.Sprite.Draw(gameTime, spriteBatch);
        }

        public virtual void Update(GameTime gameTime)
        {

            debug = CommandoGame.GamePlayScreen.Debug;

            if (this.IsRespawning && !this.IsDying)
            {
                animationRespawnTimer += gameTime.ElapsedGameTime;

                if (animationRespawnTimer >= animationRespawnLength)
                {
                    animationRespawnTimer = TimeSpan.Zero;
                    this.IsRespawning = false;
                }
            }

            if (this.IsDead)
            {
                this.Dead();
            }
            else
            {
                if (this.IsDying)
                {
                    animationDeadTimer += gameTime.ElapsedGameTime;

                    if (animationDeadTimer >= animationDeadLength)
                    {

                        animationDeadTimer = TimeSpan.Zero;
                        this.IsDead = true;
                        this.IsDying = false;
                    }

                    this.Sprite.IsAnimating = true;
                }
                else
                {
                    Vector2 motion = this.Walk(gameTime);

                    this.Collisions();

                    this.Shoot(Vector2.Zero, motion);
                }

                this.Sprite.Update(gameTime);
            }
        }

        protected virtual void Shoot(Vector2 velocity, Vector2 motion)
        {
            Vector2 motionBullet = motion;

            if (motionBullet == Vector2.Zero)
            {
                motionBullet = AnimationToMove(this.Sprite.CurrentAnimation);
                motionBullet.Normalize();
            }

            Bullet bullet = new Bullet(this.Sprite, motionBullet, this);
            bullet.Velocity = velocity;

            SceneItems.Instance.Add(bullet);
        }

        protected virtual void Collisions()
        {
        }

        protected virtual Vector2 Walk(GameTime gameTime)
        {
            return Vector2.Zero;
        }

        protected virtual void Die()
        {
            this.IsDying = true;
            this.Sprite.CurrentAnimation = AnimationKey.Dead;
            this.Sprite.IsAnimating = true;
        }

        protected virtual void Dead() { }

        public virtual Vector2 AnimationToMove(AnimationKey animation)
        {
            Vector2 motion = Vector2.Zero;

            switch (animation)
            {
                case AnimationKey.Up:
                    motion = new Vector2(0, -1f);
                    break;
                case AnimationKey.Down:
                    motion = new Vector2(0, 1f);
                    break;
                case AnimationKey.Left:
                    motion = new Vector2(-1f, 0);
                    break;
                case AnimationKey.Right:
                    motion = new Vector2(1f, 0);
                    break;
                case AnimationKey.UpLeft:
                    motion = new Vector2(-1f, -1f);
                    break;
                case AnimationKey.DownLeft:
                    motion = new Vector2(-1f, 1f);
                    break;
                case AnimationKey.UpRight:
                    motion = new Vector2(1f, -1f);
                    break;
                case AnimationKey.DownRight:
                    motion = new Vector2(1f, 1f);
                    break;
                case AnimationKey.JumpLeft:
                    motion = new Vector2(-1f, 1f);
                    break;
                case AnimationKey.JumpRight:
                    motion = new Vector2(1f, 1f);
                    break;
            }

            return motion;
        }
    }
}
