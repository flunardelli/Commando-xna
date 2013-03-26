using Microsoft.Xna.Framework;

namespace Framework2D.Base.TileEngine
{
    public class Engine
    {
        public static int TileWidth { get; private set; }
        public static int TileHeight { get; private set; }

        public Engine(int tileWidth, int tileHeight)
        {
            Engine.TileWidth = tileWidth;
            Engine.TileHeight = tileHeight;
        }

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / TileWidth, (int)position.Y / TileHeight);
        }
    }
}
