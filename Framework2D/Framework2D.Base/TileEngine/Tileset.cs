using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework2D.Base.TileEngine
{
    public class Tileset
    {
        public Texture2D Texture { get; private set; }

        public int TileWidth  { get; private set; }
        public int TileHeight  { get; private set; }

        public int TilesWide  { get; private set; }
        public int TilesHigh  { get; private set; }

        public string Name { get; private set; }

        Rectangle[] sourceRectangles;
        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        public Tileset(Texture2D image, string name, int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
            : this(image, tilesWide, tilesHigh, tileWidth, tileHeight)
        {
            this.Name = name;
        }

        public Tileset(Texture2D image, int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            Texture = image;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;

            sourceRectangles = new Rectangle[tilesWide * tilesHigh];

            int tile = 0;
            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight);
                    tile++;
                }
            }
        }
    }
}
