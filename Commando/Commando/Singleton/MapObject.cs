using Microsoft.Xna.Framework;

namespace Framework2D.Base.TileEngine
{
    public class MapObject
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Type { get; private set; }

        public Rectangle Rectangle { get { return new Rectangle(X, Y, Width, Height); } }

        public MapObject(int x, int y, int width, int height, string type)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Type = type;
        }
    }
}
