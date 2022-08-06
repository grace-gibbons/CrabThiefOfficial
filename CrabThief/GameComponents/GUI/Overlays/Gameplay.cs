using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using CrabThief.GameComponents.Audio;
using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.Graphics;
using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Map.Tiles.Collectibles;
using CrabThief.GameComponents.Map.Tiles.WorldTiles;
using CrabThief.GameComponents.Util;

//Gameplay overlay
namespace CrabThief.GameComponents.GUI.Overlays {
    class Gameplay : Overlay {

        //Pause button
        private Button pauseButton;
        private static readonly string pauseButtonPath = "Assets/Textures/GUI/Buttons/pauseButton";

        //Health bar (3 hearts)
        private BooleanTexture health2;
        private BooleanTexture health1;
        private BooleanTexture health0;
        //private static readonly string health0Path = "Assets/Textures/TestTextures/GUI/health0";
        private static readonly string health0Path = "Assets/Textures/GUI/Health/fullHeart";
        private static readonly string health1Path = "Assets/Textures/GUI/Health/noHeart";
        private static readonly string healthAnimationPath = "Assets/Textures/GUI/Health/loseHealthAnimation";
        //Position for each health image
        private readonly Vector2 healthPosition2 = new Vector2(700, 0); 
        private readonly Vector2 healthPosition1 = new Vector2(720, 0); 
        private readonly Vector2 healthPosition0 = new Vector2(740, 0);

        //Map Compass textures
        private Texture2D currentMap; 
        private Texture2D northMap;
        private Texture2D southMap;
        private Texture2D eastMap;
        private Texture2D westMap;
        private Texture2D northEastMap;
        private Texture2D northWestMap;
        private Texture2D southEastMap;
        private Texture2D southWestMap;
        private Texture2D foundMap;

        //Shells boolean texture
        private BooleanTexture orangeShell;
        private BooleanTexture blueShell;
        private BooleanTexture purpleShell;
        private BooleanTexture pinkShell;
        //Paths 
        private static readonly string shellPath = "Assets/Textures/Tiles/Collectibles/";
        private static readonly string shellAnimationPath = "Assets/Textures/GUI/Gameplay/";
        //Buttons that go overtop the collected shells in the gui and detect mouse clicks
        private Button orangeButton;
        private Button blueButton;
        private Button purpleButton;
        private Button pinkButton;

        //Player should not attack when clicking a ui component, this variable will be ture when they have just clicked a ui component
        private bool clickedUI;

        //Stores the shell that has been pick up and selected with the mouse
        private Shell.ShellTypes currentShell;

        //Forn used for gui display
        private SpriteFont font;
        //The total number of coins collected
        private int numSilver;
        private int numGold;
        private Vector2 changeLocation; 

        //Displays the total points from the food gathered 
        private int foodPoints;
        private Vector2 foodLocation;

        //Store the current amount of time on the timer, in seconds
        private int time;
        private Vector2 timeLocation; 

        /// <summary>
        /// Create overlay
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public Gameplay(Vector2 virtualScreenSize) : base() {
            pauseButton = new Button(new Vector2(virtualScreenSize.X - (96/2) - 5, virtualScreenSize.Y - (32/2) - 5), new Vector2(96, 32), true);

            health2 = new BooleanTexture(healthPosition2, new Vector2(16, 16), health0Path, health1Path, new Animation(healthPosition2, new SpriteSheet(healthAnimationPath, 6), 120)); 
            health1 = new BooleanTexture(healthPosition1, new Vector2(16, 16), health0Path, health1Path, new Animation(healthPosition1, new SpriteSheet(healthAnimationPath, 6), 120)); 
            health0 = new BooleanTexture(healthPosition0, new Vector2(16, 16), health0Path, health1Path, new Animation(healthPosition0, new SpriteSheet(healthAnimationPath, 6), 120));

            currentMap = null;

            orangeShell = new BooleanTexture(new Vector2((virtualScreenSize.X / 2) - 8 - 5, 5), new Vector2(16, 16), "NULL_TEXTURE", shellPath + "orangeShell", new Animation(new Vector2((virtualScreenSize.X / 2) - 8 - 5, 5), new SpriteSheet(shellAnimationPath + "orangeShellGUIAnimation", 4), 120)); 
            blueShell = new BooleanTexture(new Vector2((virtualScreenSize.X / 2) + 8 + 5, 5), new Vector2(16, 16), "NULL_TEXTURE", shellPath + "blueShell", new Animation(new Vector2((virtualScreenSize.X / 2) + 8 + 5, 5), new SpriteSheet(shellAnimationPath + "blueShellGUIAnimation", 4), 120)); 
            pinkShell = new BooleanTexture(new Vector2((virtualScreenSize.X / 2) - 22 - 15, 5), new Vector2(16, 16), "NULL_TEXTURE", shellPath + "pinkShell", new Animation(new Vector2((virtualScreenSize.X / 2) - 22 - 15, 5), new SpriteSheet(shellAnimationPath + "pinkShellGUIAnimation", 4), 120)); 
            purpleShell = new BooleanTexture(new Vector2((virtualScreenSize.X / 2) + 22 + 15, 5), new Vector2(16, 16), "NULL_TEXTURE", shellPath + "purpleShell", new Animation(new Vector2((virtualScreenSize.X / 2) + 22 + 15, 5), new SpriteSheet(shellAnimationPath + "purpleShellGUIAnimation", 4), 120));
            //Buttons that go over the shell graphic
            orangeButton = new Button(new Vector2((virtualScreenSize.X / 2) - 8 - 5, 5), new Vector2(16, 16));
            blueButton = new Button(new Vector2((virtualScreenSize.X / 2) + 8 + 5, 5), new Vector2(16, 16));
            pinkButton = new Button(new Vector2((virtualScreenSize.X / 2) - 22 - 15, 5), new Vector2(16, 16));
            purpleButton = new Button(new Vector2((virtualScreenSize.X / 2) + 22 + 15, 5), new Vector2(16, 16));

            clickedUI = false;

            currentShell = Shell.ShellTypes.None;

            numSilver = 0;
            numGold = 0; 

            foodPoints = 0;
            timeLocation = new Vector2((virtualScreenSize.X / 2), (virtualScreenSize.Y) - 20);
            foodLocation = new Vector2(virtualScreenSize.X - 110, 40);
            changeLocation = new Vector2(virtualScreenSize.X - 110, 25);
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            pauseButton.LoadContent(content, pauseButtonPath);

            health2.LoadContent(content); 
            health1.LoadContent(content); 
            health0.LoadContent(content);

            northMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/northMap");
            southMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/southMap");
            eastMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/eastMap");
            westMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/westMap");
            northEastMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/northEastMap");
            northWestMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/northWestMap");
            southEastMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/southEastMap");
            southWestMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/southWestMap");
            foundMap = content.Load<Texture2D>("Assets/Textures/GUI/TreasureMap/foundMap");

            orangeShell.LoadContent(content);
            blueShell.LoadContent(content);
            pinkShell.LoadContent(content);
            purpleShell.LoadContent(content);

            font = content.Load<SpriteFont>("Assets/Fonts/gameOverlayFont");
        }

        /// <summary>
        /// Update GUI components
        /// </summary>
        /// <param name="player"></param>
        /// <param name="worldMap"></param>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="keyboard"></param>
        public void Update(Player player, WorldMap worldMap, CollisionEngine collisionEngine, GameMouse mouse, GameKeyboard keyboard) {
            health0.Update();
            health1.Update();
            health2.Update();

            //Handle first heart
            if(player.GetHealth() == 3) {
                health0.SetShowTexture0(true);
            } else if(player.GetPreviousHealth() == 3 && player.GetHealth() == 2) {
                health0.SetShowTexture0(false);
                health0.SetShowAnimation(true);
            } else if(health0.GetAnimation().GetCurrentFrame() == health0.GetAnimation().GetMaxFrames() - 1 && !health0.GetAnimation().GetIsInMotion()) {
                health0.SetShowAnimation(false);
                health0.SetShowTexture1(true);
            }

            //Handle second heart
            if (player.GetHealth() == 2) {
                health1.SetShowTexture0(true);
            } else if (player.GetPreviousHealth() == 2 && player.GetHealth() == 1) {
                health1.SetShowTexture0(false);
                health1.SetShowAnimation(true);
            } else if (health1.GetAnimation().GetCurrentFrame() == health1.GetAnimation().GetMaxFrames() - 1 && !health1.GetAnimation().GetIsInMotion()) {
                health1.SetShowAnimation(false);
                health1.SetShowTexture1(true);
            }


            //Handle third heart
            if (player.GetHealth() == 1) {
                health2.SetShowTexture0(true);
            } else if (player.GetPreviousHealth() == 1 && player.GetHealth() == 0) {
                health2.SetShowTexture0(false);
                health2.SetShowAnimation(true);
            } else if (health2.GetAnimation().GetCurrentFrame() == health2.GetAnimation().GetMaxFrames() - 1 && !health2.GetAnimation().GetIsInMotion()) {
                health2.SetShowAnimation(false);
                health2.SetShowTexture1(true);
            }


            //If the map has been collected but not used, set the image states
            if(worldMap.GetTreasureMap().GetIsCollected() && !worldMap.GetTreasureMap().GetIsUsed()) {
                HandleMapOverlay(worldMap, player, keyboard); 
            }

            UpdateShells(worldMap);

            HandleButtons(collisionEngine, mouse);

            HandleCoinOverlay(worldMap);

            HandleFoodOverlay(worldMap);

            time = worldMap.GetTimerTile().GetTime(); 
        }

        /// <summary>
        /// Update shell GUI
        /// </summary>
        /// <param name="worldMap"></param>
        public void UpdateShells(WorldMap worldMap) {
            //Orange shell
            if (!worldMap.GetOrangeShell().GetIsCollected()) {
                //Shell is not collected
                orangeShell.SetShowTexture0(true);
            } else if (orangeShell.GetAnimation().GetCurrentFrame() == orangeShell.GetAnimation().GetMaxFrames() - 1 && !orangeShell.GetAnimation().GetIsInMotion()) {
                //Animate shell
                orangeShell.SetShowAnimation(false);
                orangeShell.SetShowTexture1(true);
            } else if (worldMap.GetOrangeShell().GetIsCollected()) {
                //Display shell (second state)
                orangeShell.SetShowTexture0(false);
                orangeShell.SetShowAnimation(true);
            }

            //Blue shell
            if (!worldMap.GetBlueShell().GetIsCollected()) {
                //Shell is not collected
                blueShell.SetShowTexture0(true);
            } else if (blueShell.GetAnimation().GetCurrentFrame() == blueShell.GetAnimation().GetMaxFrames() - 1 && !blueShell.GetAnimation().GetIsInMotion()) {
                //Animate shell
                blueShell.SetShowAnimation(false);
                blueShell.SetShowTexture1(true);
            } else if (worldMap.GetBlueShell().GetIsCollected()) {
                //Display shell (second state)
                blueShell.SetShowTexture0(false);
                blueShell.SetShowAnimation(true);
            }

            //Pink shell
            if (!worldMap.GetPinkShell().GetIsCollected()) {
                //Shell is not collected
                pinkShell.SetShowTexture0(true);
            } else if (pinkShell.GetAnimation().GetCurrentFrame() == pinkShell.GetAnimation().GetMaxFrames() - 1 && !pinkShell.GetAnimation().GetIsInMotion()) {
                //Animate shell
                pinkShell.SetShowAnimation(false);
                pinkShell.SetShowTexture1(true);
            } else if (worldMap.GetPinkShell().GetIsCollected()) {
                //Display shell (second state)
                pinkShell.SetShowTexture0(false);
                pinkShell.SetShowAnimation(true);
            }

            //Purple shell
            if (!worldMap.GetPurpleShell().GetIsCollected()) {
                //Shell is not collected
                purpleShell.SetShowTexture0(true);
            } else if (purpleShell.GetAnimation().GetCurrentFrame() == purpleShell.GetAnimation().GetMaxFrames() - 1 && !purpleShell.GetAnimation().GetIsInMotion()) {
                //Animate shell
                purpleShell.SetShowAnimation(false);
                purpleShell.SetShowTexture1(true);
            } else if (worldMap.GetPurpleShell().GetIsCollected()) {
                //Display shell (second state)
                purpleShell.SetShowTexture0(false);
                purpleShell.SetShowAnimation(true);
            }
        }

        /// <summary>
        /// Handle buttons for shell GUI 
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        public void HandleButtons(CollisionEngine collisionEngine, GameMouse mouse) {
            //Update the buttons over the shells, which detect mouse activity
            if (orangeShell.GetShowTexture1() && orangeButton.IsClicked(collisionEngine, mouse)) {
                clickedUI = true;
                currentShell = Shell.ShellTypes.Orange; 
            } else if (blueShell.GetShowTexture1() && blueButton.IsClicked(collisionEngine, mouse)) {
                clickedUI = true;
                currentShell = Shell.ShellTypes.Blue;
            } else if (pinkShell.GetShowTexture1() && pinkButton.IsClicked(collisionEngine, mouse)) {
                clickedUI = true;
                currentShell = Shell.ShellTypes.Pink;
            } else if (purpleShell.GetShowTexture1() && purpleButton.IsClicked(collisionEngine, mouse)) {
                clickedUI = true;
                currentShell = Shell.ShellTypes.Purple;
            }
        }

        /// <summary>
        /// Handle switching with buttons to different overlays
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="isTimerZero"></param>
        /// <param name="wantToExit"></param>
        /// <param name="player"></param>
        /// <returns> Return the new overlay </returns>
        public OverlayHandler.Overlays HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse, bool isTimerZero, bool wantToExit, Player player) {
            if (pauseButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.pause;
            }
            if(wantToExit) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.gameOverWin;
            }
            if(isTimerZero || player.GetHealth() <= 0) { //or if player dies
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.gameOverLose;
            }
            return OverlayHandler.Overlays.none;
        }

        /// <summary>
        /// Handle switching between the various map compass images
        /// </summary>
        /// <param name="worldMap"></param>
        /// <param name="player"></param>
        /// <param name="keyboard"></param>
        public void HandleMapOverlay(WorldMap worldMap, Player player, GameKeyboard keyboard) { 
            //Get the direction of the map location tile
            MapLocationTile.Directions direction = worldMap.GetMapLocationTile().GetDirection(player.GetCoordinates());

            //Set the current map image based on the direction to the map location
            if (direction == MapLocationTile.Directions.North) {
                currentMap = northMap; 
            } else if(direction == MapLocationTile.Directions.South) {
                currentMap = southMap;
            } else if (direction == MapLocationTile.Directions.East) {
                currentMap = eastMap;
            } else if (direction == MapLocationTile.Directions.West) {
                currentMap = westMap;
            } else if (direction == MapLocationTile.Directions.NorthEast) {
                currentMap = northEastMap;
            } else if (direction == MapLocationTile.Directions.NorthWest) {
                currentMap = northWestMap;
            } else if (direction == MapLocationTile.Directions.SouthEast) {
                currentMap = southEastMap;
            } else if (direction == MapLocationTile.Directions.SouthWest) {
                currentMap = southWestMap; 
            } else {
                //Player has the map and they are on the proper space
                currentMap = foundMap; 
                if(keyboard.IsEPressed()) {
                    worldMap.GetTreasureMap().SetIsUsed(true);
                    //Reward player with coins
                    worldMap.SetNumSilver(worldMap.GetNumSilver() + 4);
                    //Play sound
                    AudioHandler.PlaySound(worldMap.GetTreasureMap().GetCompletedAudio()); 
                }
            }
        }

        /// <summary>
        /// Get the number of coins and update the value for the overlay
        /// </summary>
        /// <param name="worldMap"></param>
        public void HandleCoinOverlay(WorldMap worldMap) {
            numSilver = worldMap.GetNumSilver();
            numGold = worldMap.GetNumGold(); 
        }

        /// <summary>
        /// Get the number of food and update the value for the overlay
        /// </summary>
        /// <param name="worldMap"></param>
        public void HandleFoodOverlay(WorldMap worldMap) {
            foodPoints = worldMap.GetFoodPoints();
        }

        /// <summary>
        /// Draw the overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            pauseButton.Draw(spriteBatch);

            health2.Draw(spriteBatch, gameTime); 
            health1.Draw(spriteBatch, gameTime); 
            health0.Draw(spriteBatch, gameTime);

            //Draw the map compass image
            if(currentMap != null) {
                spriteBatch.Draw(currentMap, new Vector2(10, 10), new Rectangle(0, 0, currentMap.Width, currentMap.Height), Color.White);
            }

            orangeShell.Draw(spriteBatch, gameTime);
            blueShell.Draw(spriteBatch, gameTime);
            pinkShell.Draw(spriteBatch, gameTime);
            purpleShell.Draw(spriteBatch, gameTime);

            spriteBatch.DrawString(font, "Silver: " + numSilver, changeLocation, Color.Black);
            spriteBatch.DrawString(font, "Gold: " + numGold, new Vector2(changeLocation.X + font.MeasureString("Silver:    " + numSilver).X, changeLocation.Y), Color.Black);

            spriteBatch.DrawString(font, "Food Points: " + foodPoints, foodLocation, Color.Black);

            spriteBatch.DrawString(font, "" + time, timeLocation, Color.Black);
        }

        public bool GetClickedUI() {
            return clickedUI; 
        }

        public Shell.ShellTypes GetCurrentShell() {
            return currentShell; 
        }

        public void SetCurrentShell(Shell.ShellTypes currentShell) {
            this.currentShell = currentShell; 
        }

        public BooleanTexture GetOrangeShell() {
            return orangeShell; 
        }

        public BooleanTexture GetBlueShell() {
            return blueShell;
        }

        public BooleanTexture GetPinkShell() {
            return pinkShell;
        }

        public BooleanTexture GetPurpleShell() {
            return purpleShell;
        }
    }
}
