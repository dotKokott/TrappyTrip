using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrappyTrip
{
    public class TileMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public int TileCountX { get; set; }
        public int TileCountY { get; set; }

        public TileSet TileSet { get; set; }
        public Tile[,] tiles;

        public List<Light> LightSources = new List<Light>(); //Ist es richtig LightSources in der TileMap zu speichern?

        public Game1 Game { get; set; }

        public TileMap(Game1 game, int mapWidth, int mapHeight, int tileWidth, int tileHeight, TileSet tileSet)
        {
            Width = mapWidth;
            Height = mapHeight;

            TileWidth = tileWidth;
            TileHeight = tileHeight;

            TileCountX = mapWidth / tileWidth;
            TileCountY = mapHeight / tileHeight;

            tiles = new Tile[TileCountX, TileCountY];
            TileSet = tileSet;

            Game = game;

            ResetTiles("empty");
        }

        public Tile GetTileByCoordinates(int X, int Y)
        {
            int tileX = (int)(X / TileWidth);
            int tileY = (int)(Y / TileHeight);

            return tiles[tileX, tileY];            
        }

        public void ResetTiles(string tileName)
        {
            for(int x = 0; x < TileCountX; x ++)
            {
                for(int y = 0; y < TileCountY; y++)
                {
                    tiles[x, y] = new Tile(TileWidth, TileHeight, x, y, this, tileName);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Light l in LightSources)
                l.update(gameTime);

            for(int x = 0; x < TileCountX; x++)
                for(int y = 0; y < TileCountY; y++)
                    tiles[x,y].Update(gameTime);
    
        }

        public void Draw(ExtendedSpriteBatch spriteBatch)
        {
            for(int x = 0; x < TileCountX; x++)
                for(int y = 0; y < TileCountY; y++)
                    tiles[x,y].Draw(spriteBatch, GetNearestLight(tiles[x,y].Center));
        }

        public Light GetNearestLight(Vector2 position) //Optimierungsmöglichkeit: Ich berechne 2 mal die Entfernung zwischen Tile und Lichquelle. Einmal hier und noch einmal beim wirklich zeichnen
        {
            if (LightSources.Count > 0)
            {
                Light nearestLight = LightSources[0];
                foreach (Light l in LightSources)
                {
                    if ((position - nearestLight.Position).Length() > (position - l.Position).Length()) //TODO: BUG! Nicht nach Position sortieren, sondern nach Intensität!
                    {
                        nearestLight = l;
                    }
                }

                return nearestLight;
            }

            return null;
        }
    }

    public class Tile
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsSolid { get; set; }
        
        public TileMap Map { get; set; }
        public string Name { get; set; }


        public Tile(int width, int height, int x, int y, TileMap map, string tileName)
        {
            Width = width;
            Height = height;

            X = x;
            Y = y;

            IsSolid = false;

            Map = map;
            Name = tileName;        
        }

        public virtual bool IsPassable()
        {
            return !IsSolid;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(X * Width, Y * Height, Width, Height);
        }

        public Vector2 Center 
        {
            get { return new Vector2(GetRectangle().Center.X, GetRectangle().Center.Y); }
        }

        public void Draw(ExtendedSpriteBatch spriteBatch, Light nearestLight)
        {
            if (Map.TileSet.Tiles.ContainsKey(Name))
            {
                Color tileColor = Color.White;

                if (Config.LIGHTS_ENABLED)
                {
                    tileColor = Color.Black;
                    if (nearestLight != null)
                        tileColor = nearestLight.GetColor(Center);
                }

                spriteBatch.Draw(Map.TileSet.TileSheet, GetRectangle(), Map.TileSet.GetTileRectangle(Name), tileColor);           
            }
            else
            {
                spriteBatch.DrawRectangle(new Rectangle(X * Width, Y * Height, Width, Height), Color.White);
            }
            
        }
    }

    public class TileSet
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public Texture2D TileSheet { get; set; }

        public Dictionary<String, Vector2> Tiles = new Dictionary<string, Vector2>();

        public TileSet(int tileWidth, int tileHeight, Texture2D tileSheet)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TileSheet = tileSheet;
        }

        public void AddTileMapping(string tileName, Vector2 position)
        {
            if (!Tiles.ContainsKey(tileName))
            {
                Tiles.Add(tileName, position);
            }
            else
            {
                throw new ArgumentException(String.Format("tileName: {0} is already being used!", tileName));
            }
        }

        public Rectangle GetTileRectangle(string tileName)
        {
            if(Tiles.ContainsKey(tileName))
            {
                return new Rectangle((int)Tiles[tileName].X, (int)Tiles[tileName].Y, TileWidth, TileHeight);
            }

            return new Rectangle(0,0,0,0);
        }
    }
}
