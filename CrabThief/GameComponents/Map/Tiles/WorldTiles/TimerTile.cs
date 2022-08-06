using CrabThief.GameComponents.Audio;
using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.GUI;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

//Timer tile
namespace CrabThief.GameComponents.Map.Tiles.WorldTiles {
    class TimerTile : WorldTile {

        //Path to json data file
        private static readonly string JSON_PATH = "Content/Assets/Components/Tiles/WorldTiles/timerTile.json";

        //Maximum time that the timer can hold at any time
        public static readonly int MAX_TIME = 60; 

        //The time on the timer, in seconds
        private int time;

        //To keep game time
        private int previousTime;
        private int currentTime;
        
        //Time spent in the game
        private double elapsedGameTime;

        //If the player clicks the timer, then they want to exit the game
        private bool wantToExit;

        //Sound that plays when coin is placed in the timer
        private SoundEffect coinPlacedAudio;

        /// <summary>
        /// Create timer tile 
        /// </summary>
        /// <param name="coordinates"></param>
        public TimerTile(Vector2 coordinates) : base() {
            //Read data from json file
            ReadWallData(JSON_PATH);

            SetCoordinates(coordinates);
            SetIsCollidable(true);

            //Set position basd on size and grid location
            SetPosition(coordinates * GetWorldTileSize());

            //Set collision body
            SetCollisionBody(new CollisionBody(GetPosition(), GetSize()));

            //Time starts at max time
            time = MAX_TIME;

            wantToExit = false; 
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            base.LoadContent(content);
            coinPlacedAudio = content.Load<SoundEffect>("Assets/Sounds/ShellPlaced");
        }

        /// <summary>
        /// Update the timer 
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="player"></param>
        /// <param name="mouse"></param>
        /// <param name="camera"></param>
        /// <param name="overlayHandler"></param>
        /// <param name="worldMap"></param>
        /// <param name="gameTime"></param>
        public void Update(CollisionEngine collisionEngine, Player player, GameMouse mouse, GameCamera camera, OverlayHandler overlayHandler, WorldMap worldMap, GameTime gameTime) {
            elapsedGameTime = gameTime.TotalGameTime.TotalSeconds;
            currentTime = (int)elapsedGameTime;
            
            //Decrement the time every second
            if(currentTime != previousTime && time > 0) {
                time--; 
            }

            //If the distance between the player and the timer is 3 or less, the mouse is collising with the timer, and the mouse (LEFT BUTTON) has been clicked
            if (collisionEngine.GetDistance(player.GetCoordinates(), GetCoordinates()) <= 3 && collisionEngine.IsMouseCollision(camera, mouse, GetCollisionBody()) && mouse.IsLeftButton()) {
                //Get gold and silver
                int numGold = worldMap.GetNumGold();
                int numSilver = worldMap.GetNumSilver();

                //Attempt to add one gold first, then try silver
                if (numGold >= 1 && MAX_TIME - time >= Change.goldSecondsAdded) {
                    time += Change.goldSecondsAdded;
                    worldMap.SetNumGold(numGold - 1);

                    //Play sound
                    AudioHandler.PlaySound(coinPlacedAudio);

                } else if (numSilver >= 1 && MAX_TIME - time >= Change.silverSecondsAdded) {
                    time += Change.silverSecondsAdded;
                    worldMap.SetNumSilver(numSilver - 1);

                    //Play sound
                    AudioHandler.PlaySound(coinPlacedAudio);
                }
            }

            //Leave the game when timer is right clicked
            //If the distance between the player and the timer is 3 or less, the mouse is collising with the timer, and the mouse (RIGHT BUTTON) has been clicked
            if (collisionEngine.GetDistance(player.GetCoordinates(), GetCoordinates()) <= 3 && collisionEngine.IsMouseCollision(camera, mouse, GetCollisionBody()) && mouse.IsRightButtonHeld()) {
                //Console.WriteLine("leave the game!"); 
                wantToExit = true; 
            }

            previousTime = currentTime; 
        }

        public int GetTime() {
            return time; 
        }

        public bool IsTimerZero() {
            if(time == 0) {
                return true;
            }
            return false; 
        }

        public bool GetWantToExit() {
            return wantToExit; 
        }

        public double GetElaspsedGameTime() {
            return elapsedGameTime; 
        }
    }
}
