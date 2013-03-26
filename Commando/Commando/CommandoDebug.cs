using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    public class CommandoDebug
    {
        static Texture2D lineTexture;

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            lineTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            lineTexture.SetData<Color>(pixels);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 vSrc, Vector2 vTgt, Color color, float depth)
        {
            float distance = Vector2.Distance(vSrc, vTgt);
            float angle = (float)Math.Atan2((double)(vTgt.Y - vSrc.Y), (double)(vTgt.X - vSrc.X));
            spriteBatch.Draw(lineTexture, vSrc, null, color, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, depth);
        }

        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color, float depth)
        {
            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            float theta = 0.0f;

            for (int c = 0; c < sides; c++)
            {
                Vector2 vSrc = center + new Vector2(radius * (float)Math.Cos((double)theta), radius * (float)Math.Sin((double)theta));
                Vector2 vTgt = center + new Vector2(radius * (float)Math.Cos((double)theta + step), radius * (float)Math.Sin((double)theta + step));

                CommandoDebug.DrawLine(spriteBatch, vSrc, vTgt, color, depth);

                theta += step;
            }
        }

        public static void WriteLine(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void WriteLine(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }
    }
}
