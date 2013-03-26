using System.Linq;
using Commando.GameScreens;
using Commando.Singleton;
using Framework2D.Base.Input;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Commando.Components
{
    public class PlayerControl
    {
        public Keys Up;
        public Keys Down;
        public Keys Left;
        public Keys Right;
        public Keys Shoot;
        public Keys LaunchGrenade;

        public Buttons bUp;
        public Buttons bDown;
        public Buttons bLeft;
        public Buttons bRight;
        public Buttons bShoot;
        public Buttons bLaunchGrenade;
        public bool usingGamePad = false;

        public PlayerControl(Keys up, Keys down, Keys left, Keys right, Keys shoot, Keys launchGrenade)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            Shoot = shoot;
            LaunchGrenade = launchGrenade;
            usingGamePad = false;
        }

        public PlayerControl(Buttons up, Buttons down, Buttons left, Buttons right, Buttons shoot, Buttons launchGrenade)
        {
            bUp = up;
            bDown = down;
            bLeft = left;
            bRight = right;
            bShoot = shoot;
            bLaunchGrenade = launchGrenade;
            usingGamePad = true;
        }

        public PlayerControl()
        {
        }
    }

    public class Player : Character
    {        
        private PlayerControl playerControl;
        private bool prevFire = true;

        public int Score { get; set; }
        public int Lives { get; set; }
        public int Grenades { get; set; }
        public Vector2 Checkpoint { get; set; }

        public Player(Game game, AnimatedSprite sprite, PlayerControl control)
            : base(game, sprite)
        {
            Sprite.Velocity = new Vector2(100, 100);
            shootVelocity = new Vector2(700, 700);

            playerControl = control;

            this.Score = 0;
            this.Lives = 3;
            this.Grenades = 5;
        }

        protected override void Collisions()
        {            
            Rectangle boxPlayer = this.Sprite.Rectangle;

            foreach (IObject item in SceneItems.Instance)
            {
                Bullet bullet = item as Bullet;
                if (bullet != null && !(bullet.Shooter is Player) && !this.IsRespawning)
                {
                    if (boxPlayer.Intersects(bullet.Rectangle))
                    {
                        this.Die();
                        SceneItems.Instance.Remove(bullet);
                        return;
                    }
                }

                Grenade grenade = item as Grenade;
                if (grenade != null && !(grenade.Shooter is Player) && grenade.IsExploding && !this.IsRespawning)
                {
                    if (boxPlayer.Intersects(grenade.Rectangle))
                    {
                        this.Die();
                        return;
                    }
                }

                Enemy enemy = item as Enemy;
                if (enemy != null && (!enemy.IsDying && !enemy.IsDead && !this.IsRespawning))
                {
                    if (boxPlayer.Intersects(enemy.Sprite.Rectangle))
                    {
                        this.Die();
                        enemy.currentBehavior = BehaviorStates.Evade;
                        return;
                    }
                }

                Item item4Grenades = item as Item;
                if (item4Grenades != null)
                {
                    if (boxPlayer.Intersects(item4Grenades.Rectangle))
                    {
                        UpdateGrenade(4);
                        SceneItems.Instance.Remove(item4Grenades);
                        return;
                    }
                }

            }
        }

        protected override Vector2 Walk(GameTime gameTime)
        {
            Vector2 motion = this.CreateNormalizeMotion();

            if (motion != Vector2.Zero)
            {
                Sprite.IsAnimating = true;

                Vector2 nextPosition = new Vector2();
                nextPosition = Sprite.Position;
                nextPosition += motion * Sprite.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Map.Instance.Tiles.TestTileCollision((int)nextPosition.X, (int)nextPosition.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Fall)
                {
                    this.Die();
                }
                else
                {
                    if (Map.Instance.Tiles.TestTileCollision((int)nextPosition.X, (int)Sprite.Position.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                    {
                        nextPosition.X = Sprite.Position.X;
                    }
                    if (Map.Instance.Tiles.TestTileCollision((int)Sprite.Position.X, (int)nextPosition.Y, (int)this.Sprite.Width, (int)this.Sprite.Height) == CollisionType.Block)
                    {
                        nextPosition.Y = Sprite.Position.Y;
                    }

                    Player otherPlayer = SceneItems.Instance.Where(c => c is Player && c != this).Cast<Player>().FirstOrDefault();

                    if (otherPlayer == null || (otherPlayer.Sprite.Position.Y - nextPosition.Y) < CommandoGame.GamePlayScreen.Camera.MaxOffSetY)
                    {
                        float bottomLimit = (CommandoGame.GamePlayScreen.Camera.Position.Y + CommandoGame.GamePlayScreen.Camera.ViewportRectangle.Height);
                        Sprite.Position = new Vector2(
                            MathHelper.Clamp(nextPosition.X, 0, mapWidth - Sprite.Width),
                            MathHelper.Clamp(nextPosition.Y, 0, bottomLimit - Sprite.Height));
                    }
                }
            }
            else
            {
                Sprite.IsAnimating = false;
            }

            return motion;
        }

        protected override void Shoot(Vector2 velocity, Vector2 motion)
        {
            if (!prevFire && (InputHandler.KeyDown(playerControl.Shoot) || ( playerControl.usingGamePad && InputHandler.KeyDown(playerControl.bShoot))))
                base.Shoot(shootVelocity, motion);

            if (!prevFire && (InputHandler.KeyDown(playerControl.LaunchGrenade) || ( playerControl.usingGamePad && InputHandler.KeyDown(playerControl.bLaunchGrenade))))
            {
                if (UpdateGrenade(-1))
                {
                    Grenade bullet = new Grenade(this.Sprite, this);
                    SceneItems.Instance.Add(bullet);
                }
            }

            prevFire = InputHandler.KeyDown(playerControl.Shoot) || InputHandler.KeyDown(playerControl.LaunchGrenade) || ( playerControl.usingGamePad && (InputHandler.KeyDown(playerControl.bShoot) || InputHandler.KeyDown(playerControl.bLaunchGrenade)) );
        }

        protected override void Die()
        {
            if (!debug)
                base.Die();
        }

        protected override void Dead()
        {
            if (UpdateLives(-1))
                Respawn();
            else
                SceneItems.Instance.Remove(this);
        }

        private Vector2 CreateNormalizeMotion()
        {
            Vector2 motion = Vector2.Zero;

            if ((InputHandler.KeyDown(playerControl.Up) && InputHandler.KeyDown(playerControl.Left)) || ( playerControl.usingGamePad && (InputHandler.KeyDown(playerControl.bUp) && InputHandler.KeyDown(playerControl.bLeft))))
            {
                Sprite.CurrentAnimation = AnimationKey.UpLeft;
                motion.Y = -1;
                motion.X = -1;
            }
            else if ((InputHandler.KeyDown(playerControl.Up) && InputHandler.KeyDown(playerControl.Right)) || (playerControl.usingGamePad && (InputHandler.KeyDown(playerControl.bUp) && InputHandler.KeyDown(playerControl.bRight))))
            {
                Sprite.CurrentAnimation = AnimationKey.UpRight;
                motion.Y = -1;
                motion.X = 1;
            }
            else if ((InputHandler.KeyDown(playerControl.Down) && InputHandler.KeyDown(playerControl.Left)) || (playerControl.usingGamePad && (InputHandler.KeyDown(playerControl.bDown) && InputHandler.KeyDown(playerControl.bLeft))))
            {
                Sprite.CurrentAnimation = AnimationKey.DownLeft;
                motion.Y = 1;
                motion.X = -1;
            }
            else if ((InputHandler.KeyDown(playerControl.Down) && InputHandler.KeyDown(playerControl.Right)) || (playerControl.usingGamePad && (InputHandler.KeyDown(playerControl.bDown) && InputHandler.KeyDown(playerControl.bRight))))
            {
                Sprite.CurrentAnimation = AnimationKey.DownRight;
                motion.Y = 1;
                motion.X = 1;
            }
            else if ((InputHandler.KeyDown(playerControl.Up)) || (playerControl.usingGamePad && (InputHandler.KeyDown(playerControl.bUp))))
            {
                Sprite.CurrentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }
            else if ((InputHandler.KeyDown(playerControl.Down)) || ( playerControl.usingGamePad &&(InputHandler.KeyDown(playerControl.bDown))))
            {
                Sprite.CurrentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }
            else if ((InputHandler.KeyDown(playerControl.Left)) || ( playerControl.usingGamePad &&(InputHandler.KeyDown(playerControl.bLeft))))
            {
                Sprite.CurrentAnimation = AnimationKey.Left;
                motion.X = -1;
            }
            else if ((InputHandler.KeyDown(playerControl.Right)) || ( playerControl.usingGamePad &&(InputHandler.KeyDown(playerControl.bRight))))
            {
                Sprite.CurrentAnimation = AnimationKey.Right;
                motion.X = 1;
            }

            if (motion != Vector2.Zero)
                motion.Normalize();

            return motion;
        }

        private bool UpdateLives(int num)
        {
            this.Lives += num;

            if (this.Lives == 0)
                return false;

            return true;
        }

        private bool UpdateGrenade(int num)
        {
            if (this.Grenades == 0)
                return false;

            this.Grenades += num;

            return true;
        }

        protected void Respawn()
        {
            Player otherPlayer = null;

            if (CommandoGame.GamePlayScreen.PlayerNum == PlayerNum.Player2)
                otherPlayer = SceneItems.Instance.Where(c => c is Player && c != this).Cast<Player>().FirstOrDefault();

            if (otherPlayer == null || otherPlayer.IsDead)
            {
                this.Sprite.Position = this.Checkpoint;
                CommandoGame.GamePlayScreen.Camera.Position += new Vector2(0, this.Checkpoint.Y);
            }

            this.Sprite.CurrentAnimation = AnimationKey.Up;
            this.IsDead = false;
            this.IsRespawning = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
