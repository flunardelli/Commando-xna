using System;
using Microsoft.Xna.Framework;

namespace Framework2D.Base.Sprites
{
    public enum AnimationKey { Down, Left, Right, Up, UpLeft, UpRight, DownLeft, DownRight, Dead, JumpLeft, JumpRight, LaunchGrenade, Bye }

    public class Animation : ICloneable
    {
        public Rectangle CurrentFrameRect { get { return frames[CurrentFrame]; } }

        public int CurrentFrame { get; set; }

        private int framesPerSecond;
        public int FramesPerSecond 
        {
            get { return framesPerSecond; }
            set
            {
                framesPerSecond = (int)MathHelper.Clamp(value, 1f, 60f);
                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        private Rectangle[] frames;

        private TimeSpan frameTimer;
        private TimeSpan frameLength;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameCount">Number of frames in the animation</param>
        /// <param name="frameWidth">Frame Width</param>
        /// <param name="frameHeight">Frame Height</param>
        /// <param name="xOffset">Offset in axis X</param>
        /// <param name="yOffset">Offset in axis Y</param>
        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[frameCount];
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                        xOffset + (frameWidth * i),
                        yOffset,
                        frameWidth,
                        frameHeight);
            }
            FramesPerSecond = 5;
            Reset();
        }

        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            FramesPerSecond = 5;
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;

            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                CurrentFrame = (CurrentFrame + 1) % frames.Length;
            }
        }

        public void Reset()
        {
            this.CurrentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone.FrameWidth = this.FrameWidth;
            animationClone.FrameHeight = this.FrameHeight;
            animationClone.Reset();

            return animationClone;
        }
    }
}
