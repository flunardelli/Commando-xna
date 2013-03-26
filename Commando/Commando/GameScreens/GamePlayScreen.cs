using System.Collections.Generic;
using System.Linq;
using Commando.Components;
using Commando.Singleton;
using Framework2D.Base;
using Framework2D.Base.Input;
using Framework2D.Base.Sprites;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Commando.GameScreens
{
    public class GamePlayScreen : BaseScreen
    {
        public PlayerNum PlayerNum { get; set; }

        Score gameScore;

        public Player player1;
        public Player player2;

        protected Song themeMusic;

        public Camera Camera { get; set; }

        public bool Debug = false;

        public int mapWidth = 512;
        public int mapHeight = 5760;
        bool firstRespawn;

        public GamePlayScreen(Game game, ScreenManager manager)
            : base(game, manager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            CommandoDebug.LoadContent(GraphicsDevice);
        }

        public void Restart()
        {
            CreateWorld();

            firstRespawn = true;
            gameScore = new Score(((CommandoGame)Game));
            themeMusic = Game.Content.Load<Song>(@"Music/CommandoTheme");

            MediaPlayer.Play(themeMusic);
            MediaPlayer.IsRepeating = true;

            Bullet.LoadContent(Game.Content);
            Grenade.LoadContent(Game.Content);
            Item.LoadContent(Game.Content);
            EnemyMachineGun.LoadContent(Game.Content);
            EnemyKnife.LoadContent(Game.Content);
            Score.LoadContent(Game.Content);

            SceneItems.Instance.Clear();

            MapObject obj = Map.Instance.Objects.Where(c => c.Type == "player1").FirstOrDefault();

            Camera = new Camera(((CommandoGame)Game).ScreenRectangle, new Vector2(mapWidth, mapHeight), mapWidth, mapHeight);

            if (InputHandler.GamepadConnected())
            {
                player1 = new Player(Game, CreatePlayer(@"Sprites\8bitsJoe"),
                    new PlayerControl(
                        Buttons.LeftThumbstickUp,
                        Buttons.LeftThumbstickDown,
                        Buttons.LeftThumbstickLeft,
                        Buttons.LeftThumbstickRight,
                        Buttons.A,
                        Buttons.X));
            }
            else
            {
                player1 = new Player(Game, CreatePlayer(@"Sprites\8bitsJoe"), new PlayerControl(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.C));
            }
            
            player1.Sprite.CurrentAnimation = AnimationKey.Up;
            player1.Checkpoint = new Vector2(obj.X, obj.Y); ;
            player1.Sprite.Position = new Vector2(obj.X, obj.Y);

            SceneItems.Instance.Add(player1);

            if (PlayerNum == PlayerNum.Player2)
            {
                obj = Map.Instance.Objects.Where(c => c.Type == "player2").FirstOrDefault();
                player2 = new Player(Game, CreatePlayer(@"Sprites\8bitsJack"), new PlayerControl(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.O, Keys.P));
                player2.Sprite.CurrentAnimation = AnimationKey.Up;
                player2.Checkpoint = new Vector2(obj.X, obj.Y);
                player2.Sprite.Position = new Vector2(obj.X, obj.Y);

                SceneItems.Instance.Add(player2);
            }

            foreach (MapObject item in Map.Instance.Objects)
            {
                if (item.Type == "4grenades")
                {
                    Item item4Grenades = new Item(Game, new Vector2(item.X, item.Y));
                    SceneItems.Instance.Add(item4Grenades);
                }
            }

            Camera.MaxOffSetY = (PlayerNum == PlayerNum.Player1) ? 100 : 200;
        }

        private void CreateWorld()
        {
            Map.Load(@"..\..\..\commandoTileMap.tmx", Game.Content);
        }

        private AnimatedSprite CreatePlayer(string texturePath)
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(2, 22, 44, 0, 0);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(2, 32, 44, 0, 44);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(2, 32, 44, 0, 88);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(2, 26, 44, 0, 132);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(2, 32, 44, 0, 176);
            animations.Add(AnimationKey.DownLeft, animation);

            animation = new Animation(2, 32, 44, 0, 220);
            animations.Add(AnimationKey.DownRight, animation);

            animation = new Animation(2, 32, 44, 0, 264);
            animations.Add(AnimationKey.UpLeft, animation);

            animation = new Animation(2, 32, 44, 0, 308);
            animations.Add(AnimationKey.UpRight, animation);

            animation = new Animation(1, 32, 44, 0, 352);
            animations.Add(AnimationKey.JumpLeft, animation);

            animation = new Animation(1, 32, 44, 32, 352);
            animations.Add(AnimationKey.JumpRight, animation);

            animation = new Animation(2, 32, 48, 0, 396);
            animations.Add(AnimationKey.LaunchGrenade, animation);

            animation = new Animation(2, 32, 44, 0, 444);
            animations.Add(AnimationKey.Bye, animation);
            animation.FramesPerSecond = 2;

            animation = new Animation(2, 32, 54, 0, 496);
            animations.Add(AnimationKey.Dead, animation);
            animation.FramesPerSecond = 2;

            Texture2D texture = Game.Content.Load<Texture2D>(texturePath);

            return new AnimatedSprite(texture, animations);
        }

        public AnimatedSprite CreateEnemySoldier()
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(2, 22, 42, 0, 0);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(2, 32, 44, 0, 44);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(2, 32, 44, 0, 88);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(2, 26, 44, 0, 132);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(2, 32, 44, 0, 176);
            animations.Add(AnimationKey.DownLeft, animation);

            animation = new Animation(2, 32, 44, 0, 220);
            animations.Add(AnimationKey.DownRight, animation);

            animation = new Animation(2, 32, 44, 0, 264);
            animations.Add(AnimationKey.UpLeft, animation);

            animation = new Animation(2, 32, 44, 0, 308);
            animations.Add(AnimationKey.UpRight, animation);

            animation = new Animation(1, 32, 44, 0, 352);
            animations.Add(AnimationKey.JumpLeft, animation);

            animation = new Animation(1, 32, 44, 32, 352);
            animations.Add(AnimationKey.JumpRight, animation);

            animation = new Animation(2, 32, 54, 0, 396);
            animations.Add(AnimationKey.Dead, animation);
            animation.FramesPerSecond = 2;

            Texture2D texture = Game.Content.Load<Texture2D>(@"Sprites\8bitsEnemy");

            return new AnimatedSprite(texture, animations);
        }

        private AnimatedSprite CreateEnemyGreenBoss()
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(3, 32, 44, 0, 0);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(3, 32, 44, 0, 44);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(3, 32, 40, 0, 88);
            animations.Add(AnimationKey.DownRight, animation);

            animation = new Animation(3, 32, 40, 0, 128);
            animations.Add(AnimationKey.DownLeft, animation);

            animation = new Animation(2, 32, 64, 0, 168);
            animations.Add(AnimationKey.Dead, animation);
            animation.FramesPerSecond = 1;

            Texture2D texture = Game.Content.Load<Texture2D>(@"Sprites\8bitsEnemyGreenBoss");

            return new AnimatedSprite(texture, animations);
        }

        private void UpdateSceneItems()
        {
            Rectangle rectExternal = Camera.ViewportRectangle;
            rectExternal.X += (int)Camera.Position.X - 100;
            rectExternal.Y += (int)Camera.Position.Y - 100;
            rectExternal.Height += 100;
            rectExternal.Width += 200;

            Rectangle rectInternal = Camera.ViewportRectangle;
            rectInternal.X += (int)Camera.Position.X;
            rectInternal.Y += (int)Camera.Position.Y;

            SceneItems.Instance.RemoveAll(c => c is Enemy && !rectExternal.Contains(((Enemy)c).Sprite.Rectangle));

            foreach (MapObject item in Map.Instance.Objects)
            {
                if (rectExternal.Contains(item.Rectangle) && !rectInternal.Contains(item.Rectangle))
                {
                    if (!SceneItems.Instance.Exists(c => c is Enemy && ((Enemy)c).MapObject == item))
                    {
                        if (item.Type == "soldier")
                        {
                            RespawnEnemySoldier(item);
                        }
                        else if (item.Type == "machineGun")
                        {
                            RespawnEnemyMachineGun(item);
                        }
                        else if (item.Type == "knife")
                        {
                            RespawnEnemyKnife(item);
                        }
                        else if (item.Type == "greenBoss" && firstRespawn)
                        {
                            RespawnEnemyGreenBoss(item);
                            firstRespawn = false;
                        }
                        else if (item.Type == "jumper")
                        {
                            RespawnEnemyJumper(item);
                        }
                        else if (item.Type == "respawn")
                        {
                            if (player1 != null)
                                player1.Checkpoint = new Vector2(item.X, item.Y);

                            if (player2 != null)
                                player2.Checkpoint = new Vector2(item.X, item.Y);
                        }
                    }
                }
            }
        }

        private void RespawnEnemyJumper(MapObject lobj)
        {
            EnemyJumper enemyJumper = new EnemyJumper(Game, CreateEnemySoldier());
            enemyJumper.Sprite.CurrentAnimation = (lobj.X < 0) ? AnimationKey.Right : AnimationKey.Left;
            enemyJumper.Sprite.Position = new Vector2(lobj.X, lobj.Y);
            enemyJumper.MapObject = lobj;
            SceneItems.Instance.Add(enemyJumper);
        }

        private void RespawnEnemyKnife(MapObject lobj)
        {
            EnemyKnife enemyKnife = new EnemyKnife(Game);
            enemyKnife.Sprite.Position = new Vector2(lobj.X, lobj.Y);
            enemyKnife.MapObject = lobj;
            SceneItems.Instance.Add(enemyKnife);
        }

        private void RespawnEnemyGreenBoss(MapObject lobj)
        {
            EnemyGreenBoss enemyGreenBoss = new EnemyGreenBoss(Game, CreateEnemyGreenBoss());
            enemyGreenBoss.Sprite.Position = new Vector2(lobj.X, lobj.Y);
            enemyGreenBoss.MapObject = lobj;
            enemyGreenBoss.Sprite.CurrentAnimation = AnimationKey.Left;
            SceneItems.Instance.Add(enemyGreenBoss);
        }

        private void RespawnEnemyMachineGun(MapObject lobj)
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(1, 32, 32, 0, 0);
            animations.Add(AnimationKey.Up, animation);

            AnimatedSprite animated = new AnimatedSprite(new Texture2D(Game.GraphicsDevice, 32, 32), animations);

            EnemyMachineGun enemyMachineGun = new EnemyMachineGun(Game, animated, new Vector2(lobj.X, lobj.Y));

            enemyMachineGun.MapObject = lobj;
            SceneItems.Instance.Add(enemyMachineGun);
        }

        private void RespawnEnemySoldier(MapObject lobj)
        {
            EnemySoldier enemySoldier = new EnemySoldier(Game, CreateEnemySoldier());

            if (lobj.X > 0 && lobj.X < 480)
            {
                enemySoldier.Sprite.Position = new Vector2(lobj.X, lobj.Y);
            }
            else
            {
                if (lobj.X >= (Camera.ViewportRectangle.Width / 2))
                {
                    enemySoldier.Sprite.CurrentAnimation = AnimationKey.Left;
                    enemySoldier.Sprite.Position = new Vector2(Camera.ViewportRectangle.Width, lobj.Y);
                }
                else
                {
                    enemySoldier.Sprite.CurrentAnimation = AnimationKey.Right;
                    enemySoldier.Sprite.Position = new Vector2(0, lobj.Y);
                }
            }

            enemySoldier.MapObject = lobj;
            SceneItems.Instance.Add(enemySoldier);
        }

        private void GameOver()
        {
            if ((PlayerNum == PlayerNum.Player1 && player1.Lives == 0) || (PlayerNum == PlayerNum.Player2 && player1.Lives == 0 && player2.Lives == 0))
            {

                if (player1 != null)
                {
                    ((CommandoGame)Game).MenuScreen.player1Score = player1.Score;
                    player1 = null;
                }

                if (player2 != null)
                {
                    ((CommandoGame)Game).MenuScreen.player2Score = player2.Score;
                    player2 = null;
                }

                ScreenManager.ChangeScreen(((CommandoGame)Game).MenuScreen);
            }
        }

        private void NextLevel()
        {

            if ((player1 != null && player1.Sprite.Position.Y < 64) || (player2 != null && player2.Sprite.Position.Y < 64))
            {
                if (player1 != null)
                {                  
                    ((CommandoGame)Game).WinScreen.player1Score = player1.Score;
                    ((CommandoGame)Game).MenuScreen.player1Score = player1.Score;
                    player1 = null;
                }

                if (player2 != null)
                {
                    ((CommandoGame)Game).WinScreen.player2Score = player2.Score;
                    ((CommandoGame)Game).MenuScreen.player2Score = player2.Score;
                    player2 = null;

                }

                ScreenManager.ChangeScreen(((CommandoGame)Game).WinScreen);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.T))
            {
                Debug = !Debug;
            }

            if (InputHandler.KeyReleased(Keys.M))
            {
                EnemyGreenBoss enemyGreenBoss = new EnemyGreenBoss(Game, CreateEnemyGreenBoss());
                enemyGreenBoss.Sprite.Position = new Vector2(mapWidth / 2 - enemyGreenBoss.Sprite.Width / 2, 64);
                enemyGreenBoss.Sprite.CurrentAnimation = AnimationKey.Left;
                SceneItems.Instance.Add(enemyGreenBoss);
            }

            LockCamera();

            Camera.Update(gameTime);

            UpdateSceneItems();

            gameScore.Update(gameTime);

            SceneItems.Instance.ForEach(c => c.Update(gameTime));

            GameOver();

            NextLevel();

            base.Update(gameTime);
        }

        private void LockCamera()
        {
            Player playerLock = player1;
            //Player 1 and 2 alive.
            if (player1 != null && player2 != null)
                playerLock = (player1.Sprite.Position.Y < player2.Sprite.Position.Y) ? player1 : player2;
            //Player 1 dead and 2 alive.
            else if (player1 == null && player2 != null)
                playerLock = player2;

            if (playerLock != null)
            {
                float bottomLimit = (Camera.Position.Y + Camera.ViewportRectangle.Height);
                Camera.OffSetY = (int)(bottomLimit - ((int)playerLock.Sprite.Position.Y + playerLock.Sprite.Height));

                Camera.LockToSprite(playerLock.Sprite, LockPosition.Bottom);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ((CommandoGame)this.Game).SpriteBatch;

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Camera.Transformation);

            Map.Instance.Tiles.Draw(spriteBatch, Camera, "ground");

            SceneItems.Instance.OfType<Item>().ToList().ForEach(c => c.Draw(gameTime, spriteBatch));
            SceneItems.Instance.OfType<Bullet>().ToList().ForEach(c => c.Draw(gameTime, spriteBatch));
            SceneItems.Instance.OfType<Grenade>().ToList().ForEach(c => c.Draw(gameTime, spriteBatch));
            SceneItems.Instance.OfType<Character>().ToList().ForEach(c => c.Draw(gameTime, spriteBatch));

            Map.Instance.Tiles.Draw(spriteBatch, Camera, "roof");

            gameScore.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
