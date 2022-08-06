using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.GUI;

/// <summary>
/// Welcome to Crab Thief
/// </summary>

namespace CrabThief {
    public class GameMain : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Input
        private GameKeyboard keyboard;
        private GameMouse mouse; 

        private Player player;

        private GameCamera camera;

        //Handles map generation
        private GameHandler gameHandler;
        private MapGenerator mapGenerator;
        private WorldMap worldMap;

        private CollisionEngine collisionEngine;

        private OverlayHandler overlayHandler;

        //Screen scale
        private Matrix scale;

        //Background image
        private Texture2D background;

        public GameMain() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            //Divide the full screen size by the preferred: actual size / virtual size
            //Actual size (fullscreen)
            float scaleX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            float scaleY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //Virtual size (Default size: 800 x 480)
            float virtualX = _graphics.PreferredBackBufferWidth; 
            float virtualY = _graphics.PreferredBackBufferHeight; 
            //Set the scale
            scale = Matrix.CreateScale(new Vector3(scaleX / virtualX, scaleY / virtualY, 1));
            
            //Set the game to fullscreen
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            
            //Initialize game components
            keyboard = new GameKeyboard();

            mouse = new GameMouse(scale); 

            player = new Player(GraphicsDevice.Viewport);

            camera = new GameCamera(GraphicsDevice.Viewport, new Vector2(scaleX / virtualX, scaleY / virtualY));

            mapGenerator = new MapGenerator();

            worldMap = new WorldMap(); 

            gameHandler = new GameHandler(mapGenerator, worldMap);

            collisionEngine = new CollisionEngine();

            overlayHandler = new OverlayHandler(new Vector2(virtualX, virtualY));

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load game content
            player.LoadContent(Content);

            gameHandler.CreateWorld();

            worldMap.LoadContent(Content);

            background = Content.Load<Texture2D>("Assets/Textures/Backgrounds/gameBackground");

            overlayHandler.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Update input devices
            keyboard.Update();
            mouse.Update();

            //Update the gameplay state while playing
            if(overlayHandler.GetCurrentOverlay() == OverlayHandler.Overlays.gameplay) {
                //Update player
                player.Update(gameTime, keyboard, mouse);

                //Handle collisions
                collisionEngine.Update(player, worldMap.GetWallTiles(), worldMap.GetFoodTiles(), worldMap.GetChangeTiles(), worldMap.GetTreasureMap(), worldMap.GetShells(), worldMap);

                //Update game
                gameHandler.Update(gameTime, player, collisionEngine, mouse, camera, overlayHandler);

                //Set the camera to look at player
                camera.LookAt(player.GetPosition(), player.GetSize(), GraphicsDevice.Viewport);
                //Handle camera zoom (testing only)
                //camera.Zoom(keyboard);
            }

            //Handle overlays and menu switching
            overlayHandler.Update(collisionEngine, mouse, player, worldMap, keyboard, worldMap.GetTimerTile().IsTimerZero(), worldMap.GetTimerTile().GetWantToExit()); 

            //Reload the game when at main menu
            if(overlayHandler.GetDoReload()) {
                //Reset the map and world generator
                mapGenerator = new MapGenerator();
                worldMap = new WorldMap();
                gameHandler = new GameHandler(mapGenerator, worldMap);
                overlayHandler.ResetHealth(); 
                //Reset the player
                player = new Player(GraphicsDevice.Viewport); 
                //Reload content
                LoadContent();
                //complete reload
                overlayHandler.SetDoReload(false); 
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.SteelBlue);

            //Draw background image
            Vector2 parallax = new Vector2(0, 0);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, scale);
            _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White * 0.5f);
            _spriteBatch.End();
            

            //Draw Game
            parallax = new Vector2(1, 1);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix(parallax)); 

            worldMap.Draw(_spriteBatch, camera); 
            player.Draw(_spriteBatch);

            _spriteBatch.End();


            //Menu and GUI drawing
            parallax = new Vector2(0, 0);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, scale);

            overlayHandler.Draw(_spriteBatch, gameTime); 

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
