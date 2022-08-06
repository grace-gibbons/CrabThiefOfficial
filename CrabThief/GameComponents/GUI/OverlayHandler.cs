using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.GUI.Overlays;
using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Map.Tiles.Collectibles;

//Stores and instance of each overlay and handles swapping between them 
namespace CrabThief.GameComponents.GUI {
    class OverlayHandler {

        //Overlays
        MainMenu mainMenu;
        Gameplay gameplay;
        Settings settings;
        Pause pause;
        Credits credits;
        HowToPlay howToPlay;
        GameOverWin gameOverWin;
        GameOverLose gameOverLose;

        //Virtual size of the screen
        private Vector2 virtualScreenSize; 

        //Each overlay type
        public enum Overlays {
            mainMenu,
            gameplay,
            settings,
            pause,
            credits,
            howToPlay,
            gameOverWin,
            gameOverLose,
            none
        }

        //The previous state is used to determine where the back button goes from settings (main or pause)
        //Previous will only be set to main or pause
        Overlays previousOverlay;

        //The current overlay being displayed
        Overlays currentOverlay;

        //True when the game needs to reload
        private bool doReload; 

        /// <summary>
        /// Create overlay handler
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public OverlayHandler(Vector2 virtualScreenSize) {
            this.virtualScreenSize = virtualScreenSize; 

            mainMenu = new MainMenu(virtualScreenSize);
            gameplay = new Gameplay(virtualScreenSize);
            settings = new Settings(virtualScreenSize);
            pause = new Pause(virtualScreenSize);
            credits = new Credits(virtualScreenSize);
            howToPlay = new HowToPlay(virtualScreenSize);
            gameOverWin = new GameOverWin(virtualScreenSize);
            gameOverLose = new GameOverLose(virtualScreenSize); 

            doReload = false; 
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            mainMenu.LoadContent(content);
            gameplay.LoadContent(content);
            settings.LoadContent(content);
            pause.LoadContent(content);
            credits.LoadContent(content);
            howToPlay.LoadContent(content);
            gameOverWin.LoadContent(content);
            gameOverLose.LoadContent(content); 
        }

        /// <summary>
        /// Update the current overlay and handle switching 
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="player"></param>
        /// <param name="worldMap"></param>
        /// <param name="keyboard"></param>
        /// <param name="isTimerZero"></param>
        /// <param name="wantToExit"></param>
        public void Update(CollisionEngine collisionEngine, GameMouse mouse, Player player, WorldMap worldMap, GameKeyboard keyboard, bool isTimerZero, bool wantToExit) {
            HandleSwitching(collisionEngine, mouse, isTimerZero, wantToExit, player);

            gameplay.Update(player, worldMap, collisionEngine, mouse, keyboard);

            settings.Update(collisionEngine, mouse);

            gameOverWin.Update(worldMap);
        }

        /// <summary>
        /// Reset the gameplay gui so the health textures display properly
        /// </summary>
        public void ResetHealth() {
            gameplay = new Gameplay(virtualScreenSize); 
        }

        /// <summary>
        /// Get the new overlay if there is one and set it to the current overlay
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="isTimerZero"></param>
        /// <param name="wantToExit"></param>
        /// <param name="player"></param>
        public void HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse, bool isTimerZero, bool wantToExit, Player player) {
            //Get the overlay to switch to, if there is one
            Overlays newOverlay = Overlays.none; 
            if(mainMenu.GetIsCurrentOverlay()) {
                newOverlay = mainMenu.HandleSwitching(collisionEngine, mouse);
                previousOverlay = Overlays.mainMenu; 
            } else if(gameplay.GetIsCurrentOverlay()) {
                newOverlay = gameplay.HandleSwitching(collisionEngine, mouse, isTimerZero, wantToExit, player);
            } else if (settings.GetIsCurrentOverlay()) {
                newOverlay = settings.HandleSwitching(collisionEngine, mouse, previousOverlay);
            } else if (pause.GetIsCurrentOverlay()) {
                newOverlay = pause.HandleSwitching(collisionEngine, mouse);
                previousOverlay = Overlays.pause;
            } else if (credits.GetIsCurrentOverlay()) {
                newOverlay = credits.HandleSwitching(collisionEngine, mouse);
            } else if (howToPlay.GetIsCurrentOverlay()) {
                newOverlay = howToPlay.HandleSwitching(collisionEngine, mouse);
            } else if(gameOverWin.GetIsCurrentOverlay()) {
                newOverlay = gameOverWin.HandleSwitching(collisionEngine, mouse); 
            } else if (gameOverLose.GetIsCurrentOverlay()) {
                newOverlay = gameOverLose.HandleSwitching(collisionEngine, mouse);
            }

            //Set the current overlay to the new overlay
            if (newOverlay != Overlays.none) {
                currentOverlay = newOverlay;
            }
             
            //Switch to the new overlay
            if (newOverlay == Overlays.gameplay) {
                gameplay.SetIsCurrentOverlay(true);
            } else if (newOverlay == Overlays.mainMenu) {
                mainMenu.SetIsCurrentOverlay(true);
                //Reload the game when main menu is active
                doReload = true; 
            } else if (newOverlay == Overlays.pause) {
                pause.SetIsCurrentOverlay(true);
            } else if (newOverlay == Overlays.credits) {
                credits.SetIsCurrentOverlay(true);
            } else if (newOverlay == Overlays.howToPlay) {
                howToPlay.SetIsCurrentOverlay(true);
            } else if (newOverlay == Overlays.settings) {
                settings.SetIsCurrentOverlay(true);
            } else if(newOverlay == Overlays.gameOverWin) {
                gameOverWin.SetIsCurrentOverlay(true);
            } else if (newOverlay == Overlays.gameOverLose) {
                gameOverLose.SetIsCurrentOverlay(true);
            }
        }

        /// <summary>
        /// Draw the current overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            if(mainMenu.GetIsCurrentOverlay()) {
                mainMenu.Draw(spriteBatch);
            } else if(gameplay.GetIsCurrentOverlay()) {
                gameplay.Draw(spriteBatch, gameTime);
            } else if (pause.GetIsCurrentOverlay()) {
                pause.Draw(spriteBatch);
            } else if (credits.GetIsCurrentOverlay()) {
                credits.Draw(spriteBatch);
            } else if (howToPlay.GetIsCurrentOverlay()) {
                howToPlay.Draw(spriteBatch);
            } else if (settings.GetIsCurrentOverlay()) {
                settings.Draw(spriteBatch);
            } else if(gameOverWin.GetIsCurrentOverlay()) {
                gameOverWin.Draw(spriteBatch); 
            } else if (gameOverLose.GetIsCurrentOverlay()) {
                gameOverLose.Draw(spriteBatch);
            }

        }

        /// <returns> Return the current overlay </returns>
        public Overlays GetCurrentOverlay() {
            return currentOverlay; 
        }

        /// <returns> Return the gameplay overlay </returns>
        public Gameplay GetGameplay() {
            return gameplay; 
        }

        /// <returns> Return the current shell </returns>
        public Shell.ShellTypes GetCurrentShell() {
            return gameplay.GetCurrentShell(); 
        }

        /// <summary>
        /// Set the current shell
        /// </summary>
        /// <param name="currentShell"></param>
        public void SetCurrentShell(Shell.ShellTypes currentShell) {
            gameplay.SetCurrentShell(currentShell);
        }

        /// <returns> Return doReload </returns>
        public bool GetDoReload() {
            return doReload; 
        }

        /// <summary>
        /// Set doReload
        /// </summary>
        /// <param name="doReload"></param>
        public void SetDoReload(bool doReload) {
            this.doReload = doReload; 
        }
    }
}
