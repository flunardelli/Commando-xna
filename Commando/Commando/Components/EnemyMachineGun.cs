using System.Collections.Generic;
using Commando.Singleton;
using Framework2D.Base.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Commando.GameScreens;
using Framework2D.Base.TileEngine;

namespace Commando.Components
{
    public sealed class EnemyMachineGun : Enemy
    {
        static Texture2D texture;
        Dictionary<AnimationKey, Rectangle> animations;
        AnimationKey currentAnimation;

        private Player playerDetected;

        private float shootIntervalSeconds;
        private float shootIntervalCount;

        public static void LoadContent(ContentManager content)
        {
            EnemyMachineGun.texture = content.Load<Texture2D>(@"Sprites\8bitsEnemyMachineGun");
        }

        public EnemyMachineGun(Game game, AnimatedSprite animation, Vector2 position)
            : base(game, animation)
        {
            this.Sprite.Position = position;
            visionRadius = 300.0f;
            

            animations = new Dictionary<AnimationKey, Rectangle>();

            Rectangle machineGunRect = new Rectangle(0, 0, 32, 32);
            animations.Add(AnimationKey.Left, machineGunRect);

            machineGunRect = new Rectangle(32, 0, 32, 32);
            animations.Add(AnimationKey.DownLeft, machineGunRect);

            machineGunRect = new Rectangle(64, 0, 32, 32);
            animations.Add(AnimationKey.Down, machineGunRect);

            machineGunRect = new Rectangle(96, 0, 32, 32);
            animations.Add(AnimationKey.DownRight, machineGunRect);

            machineGunRect = new Rectangle(128, 0, 32, 32);
            animations.Add(AnimationKey.Right, machineGunRect);

            shootIntervalSeconds = 4f;
            shootIntervalCount = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (shootIntervalCount < shootIntervalSeconds)
                shootIntervalCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

            CreateMotion();

            if (playerDetected != null && shootIntervalCount > shootIntervalSeconds)
            {
                Vector2 motion = NormalizeMotion(playerDetected.Sprite.Position - this.Sprite.Position);
                this.Shoot(this.Sprite.Velocity, motion);
                shootIntervalCount = 0;
            }

            if (currentAnimation == AnimationKey.Up || currentAnimation == AnimationKey.UpLeft || currentAnimation == AnimationKey.UpRight)
            {
                SceneItems.Instance.Remove(this);

                EnemySoldier enemySoldier = new EnemySoldier(CommandoGame, CommandoGame.GamePlayScreen.CreateEnemySoldier());
                enemySoldier.Sprite.Position = new Vector2(this.Sprite.Position.X, this.Sprite.Position.Y - 15);

                if (Map.Instance.Tiles.TestTileCollision((int)enemySoldier.Sprite.Position.X,
                    (int)enemySoldier.Sprite.Position.Y, (int)enemySoldier.Sprite.Width, (int)enemySoldier.Sprite.Height) == CollisionType.Block)
                {
                    enemySoldier.Sprite.Position = new Vector2(this.Sprite.Position.X, this.Sprite.Position.Y - 25);
                }

                enemySoldier.Sprite.CurrentAnimation = AnimationKey.Up;
                enemySoldier.currentBehavior = BehaviorStates.Evade;

                SceneItems.Instance.Add(enemySoldier);
            }

            Collisions();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EnemyMachineGun.texture, this.Sprite.Position, animations[currentAnimation], Color.White);
        }

        private Vector2 CreateMotion()
        {
            Vector2 motion = Vector2.Zero;

            playerDetected = EnemyAI.PlayerClosest(this, visionRadius);

            if (playerDetected != null)
                motion = playerDetected.Sprite.Position - this.Sprite.Position;

            return NormalizeMotion(motion);
        }

        private Vector2 NormalizeMotion(Vector2 motion)
        {
            if (motion != Vector2.Zero)
            {
                motion.Normalize();

                if (motion.Y < -0.5f && motion.X < -0.5f)
                {
                    this.currentAnimation = AnimationKey.UpLeft;
                }
                else if (motion.Y < -0.5f && motion.X > 0.5f)
                {
                    this.currentAnimation = AnimationKey.UpRight;
                }
                else if (motion.Y > 0.5f && motion.X < -0.5f)
                {
                    this.currentAnimation = AnimationKey.DownLeft;
                }
                else if (motion.Y > 0.5f && motion.X > 0.5f)
                {
                    this.currentAnimation = AnimationKey.DownRight;
                }
                else if (motion.Y < 0f)
                {
                    this.currentAnimation = AnimationKey.Up;
                }
                else if (motion.Y > 0.5f)
                {
                    this.currentAnimation = AnimationKey.Down;
                }
                else if (motion.X < -0.5f)
                {
                    this.currentAnimation = AnimationKey.Left;
                }
                else if (motion.X > 0.5f)
                {
                    this.currentAnimation = AnimationKey.Right;
                }
            }

            return motion;
        }

        protected override void Shoot(Vector2 velocity, Vector2 motion)
        {
            Vector2 motionBullet = motion;

            if (motionBullet == Vector2.Zero)
            {
                switch (currentAnimation)
                {
                    case AnimationKey.Up:
                        motionBullet = new Vector2(0, -1);
                        break;
                    case AnimationKey.Down:
                        motionBullet = new Vector2(0, 1);
                        break;
                    case AnimationKey.Left:
                        motionBullet = new Vector2(-1, 0);
                        break;
                    case AnimationKey.Right:
                        motionBullet = new Vector2(1, 0);
                        break;
                    case AnimationKey.UpLeft:
                        motionBullet = new Vector2(-1, -1);
                        break;
                    case AnimationKey.UpRight:
                        motionBullet = new Vector2(1, -1);
                        break;
                    case AnimationKey.DownLeft:
                        motionBullet = new Vector2(-1, 1);
                        break;
                    case AnimationKey.DownRight:
                        motionBullet = new Vector2(1, 1);
                        break;
                }

                motionBullet.Normalize();
            }

            Bullet bullet = new Bullet(this.Sprite, motionBullet, this);
            bullet.Velocity = new Vector2(130, 130);

            SceneItems.Instance.Add(bullet);
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
                        return;
                    }
                }

                Grenade grenade = item as Grenade;
                if (grenade != null && !(grenade.Shooter is Enemy) && grenade.IsExploding)
                {
                    if (boxEnemy.Intersects(grenade.Rectangle))
                    {
                        this.Die();
                        return;
                    }
                }
            }
        }

        protected override void Die()
        {
            SceneItems.Instance.Remove(this);

            EnemySoldier enemySoldier = new EnemySoldier(CommandoGame, CommandoGame.GamePlayScreen.CreateEnemySoldier());
            enemySoldier.Sprite.Position = new Vector2(this.Sprite.Position.X, this.Sprite.Position.Y - 15);

            if (Map.Instance.Tiles.TestTileCollision((int)enemySoldier.Sprite.Position.X,
                (int)enemySoldier.Sprite.Position.Y, (int)enemySoldier.Sprite.Width, (int)enemySoldier.Sprite.Height) == CollisionType.Block)
            {
                enemySoldier.Sprite.Position = new Vector2(this.Sprite.Position.X, this.Sprite.Position.Y - 25);
            }

            enemySoldier.Sprite.CurrentAnimation = AnimationKey.Dead;
            enemySoldier.IsDying = true;

            SceneItems.Instance.Add(enemySoldier);
        }
    }
}
