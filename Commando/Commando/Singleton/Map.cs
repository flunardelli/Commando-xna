using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Framework2D.Base.TileEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.Singleton
{
    public sealed class Map
    {
        private static readonly Map instance = new Map();

        public List<MapObject> Objects;
        public TileMap Tiles;

        private Map() { }

        public static Map Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Load(string filename, ContentManager content)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            //MAPA
            XmlNode mapNode = document["map"];
            int width = int.Parse(mapNode.Attributes["width"].Value, CultureInfo.InvariantCulture);
            int height = int.Parse(mapNode.Attributes["height"].Value, CultureInfo.InvariantCulture);
            int tileWidth = int.Parse(mapNode.Attributes["tilewidth"].Value, CultureInfo.InvariantCulture);
            int tileHeight = int.Parse(mapNode.Attributes["tileheight"].Value, CultureInfo.InvariantCulture);

            Engine engine = new Engine(tileWidth, tileHeight);

            //TILESETS
            Tileset tileset = null;
            Dictionary<int, CollisionType> tilesCollisions = new Dictionary<int, CollisionType>();
            foreach (XmlNode tilesetNode in mapNode.SelectNodes("tileset"))
            {
                tileset = CreateTileset(content, tilesetNode);

                //TILES
                foreach (XmlNode tileNode in tilesetNode.SelectNodes("tile"))
                {
                    int id = int.Parse(tileNode.Attributes["id"].Value, CultureInfo.InvariantCulture);

                    //TODO: Suporta apenas 1 property.
                    XmlNode property = tileNode.SelectNodes("properties/property").Item(0);
                    if ((string)property.Attributes["name"].Value == "collisionType")
                    {
                        string collisionType = (string)property.Attributes["value"].Value;
                        tilesCollisions.Add(id, (CollisionType)Enum.Parse(typeof(CollisionType), collisionType, true));
                    }
                }

                //TODO: Suporta apenas 1 tileset;
                break;
            }

            //LAYERS                                               
            List<MapLayer> layers = new List<MapLayer>();
            CollisionType[,] collisionLayer = new CollisionType[width, height];
            foreach (XmlNode layerNode in mapNode.SelectNodes("layer"))
            {
                string layerName = (string)layerNode.Attributes["name"].Value;
                MapLayer layer = new MapLayer(layerName, width, height);

                int x = 0;
                int y = 0;
                foreach (XmlNode tileNode in layerNode.SelectNodes("data/tile"))
                {
                    int gid = int.Parse(tileNode.Attributes["gid"].Value, CultureInfo.InvariantCulture) - 1;

                    Tile tile = new Tile(gid, 0);

                    if (tilesCollisions.Keys.Contains(gid))
                    {
                        collisionLayer[y, x] = tilesCollisions[gid];
                    }

                    layer.SetTile(y++, x, tile);

                    if (!(y < width))
                    {
                        y = 0;
                        x++;
                    }
                }

                layers.Add(layer);
            }

            //OBJECTS
            Map.Instance.Objects = new List<MapObject>();
            foreach (XmlNode mapObject in mapNode.SelectNodes("objectgroup"))
            {
                foreach (XmlNode mapObjectNode in mapObject.SelectNodes("object"))
                {
                    int loX = int.Parse(mapObjectNode.Attributes["x"].Value, CultureInfo.InvariantCulture);
                    int loY = int.Parse(mapObjectNode.Attributes["y"].Value, CultureInfo.InvariantCulture);
                    int loWidth = int.Parse(mapObjectNode.Attributes["width"].Value, CultureInfo.InvariantCulture);
                    int loHeight = int.Parse(mapObjectNode.Attributes["height"].Value, CultureInfo.InvariantCulture);

                    XmlNode property = mapObjectNode.SelectNodes("properties/property").Item(0);

                    if (property != null && (string)property.Attributes["name"].Value == "type")
                    {
                        string loType = (string)property.Attributes["value"].Value;
                        instance.Objects.Add(new MapObject(loX, loY, loWidth, loHeight, loType));
                    }
                }

                break;
            }

            //TILEMAP
            TileMap tileMap = new TileMap(tileset, layers[0]);
            tileMap.CollisionLayer = collisionLayer;
            for (int i = 1; i < layers.Count; i++)
            {
                tileMap.AddLayer(layers[i]);
            }

            Map.Instance.Tiles = tileMap;
        }

        private static Tileset CreateTileset(ContentManager content, XmlNode node)
        {
            XmlNode imageNode = node["image"];
            string image = imageNode.Attributes["source"].Value;
            int width = int.Parse(imageNode.Attributes["width"].Value, CultureInfo.InvariantCulture);
            int height = int.Parse(imageNode.Attributes["height"].Value, CultureInfo.InvariantCulture);

            string name = node.Attributes["name"].Value;
            int tileWidth = int.Parse(node.Attributes["tilewidth"].Value, CultureInfo.InvariantCulture);
            int tileHeight = int.Parse(node.Attributes["tileheight"].Value, CultureInfo.InvariantCulture);

            Texture2D tilesetTexture = content.Load<Texture2D>(@"Tilesets\tileset");
            return new Tileset(tilesetTexture, name, width / tileWidth, height / tileHeight, tileWidth, tileHeight);
        }
    }
}
