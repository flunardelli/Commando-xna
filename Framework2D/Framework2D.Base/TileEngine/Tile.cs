
namespace Framework2D.Base.TileEngine
{
    public enum CollisionType { None = 0, Block = 1, Fall = 2 }
    public class Tile
    {
        public int TileIndex { get; private set; }
        public int Tileset { get; private set; }
        public CollisionType CollisionType { get; private set; }

        public Tile(int tileIndex, int tileset)
        {
            TileIndex = tileIndex;
            Tileset = tileset;
            CollisionType = CollisionType.None;
        }

        public Tile(int tileIndex, int tileset, CollisionType collisionType)
        {
            TileIndex = tileIndex;
            Tileset = tileset;
            CollisionType = collisionType;
        }
    }
}
