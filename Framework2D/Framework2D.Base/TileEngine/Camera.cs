using System;
using Framework2D.Base.Input;
using Framework2D.Base.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Framework2D.Base.TileEngine
{
    public enum LockPosition { Bottom = 0 }
    public class Camera
    {
        Rectangle viewportRectangle;

        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public float Zoom { get; private set; }
        public CameraMode CameraMode { get; private set; }

        private int offSetY;
        public int OffSetY
        {
            get
            {
                return offSetY;
            }
            set
            {
                offSetY = (int)MathHelper.Clamp(value, 0, MaxOffSetY);
            }
        }

        public int MaxOffSetY { get; set; }

        //TODO: usar tamano do TileMap
        public int mapWidth = 0;
        public int mapHeight = 0;

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        public Rectangle ViewportRectangle
        {
            get
            {
                return new Rectangle(
                    viewportRectangle.X,
                    viewportRectangle.Y,
                    viewportRectangle.Width,
                    viewportRectangle.Height);
            }
        }

        public Camera(Rectangle viewportRect, int mapWidth, int mapHeight)
        {
            Speed = 4f;
            Zoom = 1f;
            viewportRectangle = viewportRect;
            CameraMode = CameraMode.Follow;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            OffSetY = 0;
            MaxOffSetY = 100;
        }

        public Camera(Rectangle viewportRect, Vector2 position, int mapWidth, int mapHeight)
        {
            Speed = 4f;
            Zoom = 1f;
            viewportRectangle = viewportRect;
            Position = position;
            CameraMode = CameraMode.Follow;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            OffSetY = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (CameraMode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputHandler.KeyDown(Keys.Left))
                motion.X = -Speed;
            else if (InputHandler.KeyDown(Keys.Right))
                motion.X = Speed;

            if (InputHandler.KeyDown(Keys.Up))
                motion.Y = -Speed;
            else if (InputHandler.KeyDown(Keys.Down))
                motion.Y = Speed;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                Position += motion * Speed;
                LockCamera();
            }
        }

        public void ZoomIn()
        {
            Zoom += 0.25f;

            if (Zoom > 2.5f)
                Zoom = 2.5f;

            Vector2 newPosition = Position * Zoom;
            SnapToPosition(newPosition);
        }

        public void ZoomOut()
        {
            Zoom -= 0.25f;

            if (Zoom < .5f)
                Zoom = .5f;

            Vector2 newPosition = Position * Zoom;
            SnapToPosition(newPosition);
        }

        private void SnapToPosition(Vector2 newPosition)
        {
            Position = new Vector2(
                (newPosition.X - viewportRectangle.Width / 2),
                (newPosition.Y - viewportRectangle.Height / 2));

            LockCamera();
        }

        public void LockCamera()
        {
            Vector2 position = new Vector2();

            position.X = MathHelper.Clamp(Position.X,
                0,
                mapWidth * Zoom - viewportRectangle.Width);
            position.Y = MathHelper.Clamp(Position.Y,
                0,
                mapHeight * Zoom - viewportRectangle.Height);

            position.X = (float)Math.Floor(position.X);
            position.Y = (float)Math.Floor(position.Y);

            this.Position = position;
        }

        public void LockToSprite(AnimatedSprite sprite)
        {
            Position = new Vector2(
                ((sprite.Position.X + sprite.Width / 2) * Zoom - (viewportRectangle.Width / 2)),
                ((sprite.Position.Y + sprite.Height / 2) * Zoom - (viewportRectangle.Height / 2)));

            LockCamera();
        }

        public void LockToSprite(AnimatedSprite sprite, LockPosition lockPosition)
        {
            if (lockPosition == LockPosition.Bottom)
            {
                Position = new Vector2(
                    ((sprite.Position.X + sprite.Width / 2) * Zoom - (viewportRectangle.Width / 2)),
                    ((sprite.Position.Y + sprite.Height + OffSetY) * Zoom - viewportRectangle.Height));
            }

            LockCamera();
        }

        public void ToggleCameraMode()
        {
            if (CameraMode == CameraMode.Follow)
                CameraMode = CameraMode.Free;
            else if (CameraMode == CameraMode.Free)
                CameraMode = CameraMode.Follow;
        }
    }
}
