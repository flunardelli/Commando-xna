using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework2D.Base.TileEngine
{
    public class TileMap
    {
        List<Tileset> tilesets;
        List<ILayer> mapLayers;

        public CollisionType[,] CollisionLayer { get; set; }

        static int mapWidth;
        static int mapHeight;

        public static int WidthInPixels { get { return mapWidth * Engine.TileWidth; } }
        public static int HeightInPixels { get { return mapHeight * Engine.TileHeight; } }

        public TileMap(Tileset tileset, MapLayer baseLayer)
        {
            tilesets = new List<Tileset>();
            tilesets.Add(tileset);

            mapLayers = new List<ILayer>();
            mapLayers.Add(baseLayer);

            mapWidth = baseLayer.Width;
            mapHeight = baseLayer.Height;

            CollisionLayer = new CollisionType[mapWidth, mapHeight];
        }

        public void AddLayer(ILayer layer)
        {
            if (layer is MapLayer)
            {
                if (!(((MapLayer)layer).Width == mapWidth && ((MapLayer)layer).Height == mapHeight))
                    throw new Exception("Map layer size exception");
            }

            mapLayers.Add(layer);
        }

        public void AddTileset(Tileset tileset)
        {
            tilesets.Add(tileset);
        }

        public void Update(GameTime gameTime)
        {
            mapLayers.ForEach(layer => layer.Update(gameTime));
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            mapLayers.ForEach(layer => layer.Draw(spriteBatch, camera, tilesets));
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, int layerIndex)
        {
            mapLayers[layerIndex].Draw(spriteBatch, camera, tilesets);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, string layerName)
        {
            List<MapLayer> layers = mapLayers.OfType<MapLayer>().Where(l =>
                l.Name.Equals(layerName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (layers.Count == 0)
                throw new Exception("Não existem layers com esse nome.");


            layers.ForEach(l => l.Draw(spriteBatch, camera, tilesets));
        }

        public CollisionType TestTileCollision(int positionX, int positionY, int width, int height)
        {
            Rectangle collisionBox = new Rectangle(
                positionX,
                positionY,
                width,
                height);

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (CollisionLayer[i, j] == CollisionType.None) continue;

                    Rectangle tileBox = new Rectangle(
                        i * Engine.TileWidth,
                        j * Engine.TileHeight,
                        Engine.TileWidth,
                        Engine.TileHeight);

                    if (collisionBox.Intersects(tileBox))
                    {
                        return CollisionLayer[i, j];
                    }
                }
            }

            return CollisionType.None;
        }
    }
}
