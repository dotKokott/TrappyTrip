using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace TrappyTrip
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ExtendedSpriteBatch spriteBatch;

        Texture2D adventurerSheet;
        Adventurer player;
        public Light playerLight;

        public TileMap tileMap;
        public Camera camera;


        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;  
            this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            graphics.PreferredBackBufferWidth =  WindowWidth = 800;
            graphics.PreferredBackBufferHeight = WindowHeight =  480;

            DebugComponent debugComp = new DebugComponent(this);
            this.Components.Add(debugComp);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);
            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Content hier zu laden

            TileSet tileSet = new TileSet(64, 64, Content.Load<Texture2D>("tileset"));
            tileSet.AddTileMapping("ground_1", new Vector2(0, 0));
            tileSet.AddTileMapping("ground_2", new Vector2(64, 0));
            tileSet.AddTileMapping("ground_3", new Vector2(128, 0));
            tileSet.AddTileMapping("background", new Vector2(320, 0));

            tileSet.AddTileMapping("plattform_1", new Vector2(3 * 64, 2 * 64));
            tileSet.AddTileMapping("plattform_2", new Vector2(4 * 64, 2 * 64));
            tileSet.AddTileMapping("plattform_3", new Vector2(5 * 64, 2 * 64));

            tileMap = new TileMap(this,960, 512, 64, 64, tileSet);

            tileMap.ResetTiles("background");
            tileMap.tiles[0, 7].Name = "ground_1";
            tileMap.tiles[0, 7].IsSolid = true;
            for (int i = 1; i < 11; i++)
            {
                tileMap.tiles[i, 7].Name = "ground_2";
                tileMap.tiles[i, 7].IsSolid = true;
            }            
            tileMap.tiles[11, 7].Name = "ground_3";
            tileMap.tiles[11, 7].IsSolid = true;

            tileMap.tiles[5, 5].Name = "plattform_1";
            tileMap.tiles[5, 5].IsSolid = true;
            tileMap.tiles[6,5].Name = "plattform_2";
            tileMap.tiles[6, 5].IsSolid = true;
            tileMap.tiles[7,5].Name = "plattform_3";
            tileMap.tiles[7, 5].IsSolid = true;

            tileMap.tiles[8, 6].Name = "plattform_2";
            tileMap.tiles[8, 6].IsSolid = true;

            player = new Adventurer(this, 10, 300);
            playerLight = new Light(Color.White, 200, new Vector2(10, 300));
            playerLight.FollowEntity(player);
            
            tileMap.LightSources.Add(playerLight);
            tileMap.LightSources.Add(new Light(Color.White, 250, new Vector2(450, 100)));

            camera = new Camera(800, 480);
            camera.position = new Vector2(player.X, player.Y);
            camera.FollowEntity(player);
            camera.Zoom = 1f;
        }

        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Content hier
        }

        protected override void Update(GameTime gameTime)
        {
            // Ermöglicht ein Beenden des Spiels
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           
            tileMap.Update(gameTime);
            player.Update(gameTime);
            camera.Update(gameTime);
            //camera.Zoom += 0.001f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformation(GraphicsDevice));
            tileMap.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
