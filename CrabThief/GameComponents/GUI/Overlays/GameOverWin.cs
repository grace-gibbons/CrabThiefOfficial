using System;

using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Game over win overlay
namespace CrabThief.GameComponents.GUI.Overlays {
    class GameOverWin : Overlay {

        //Background image
        private Texture2D background;

        //Main menu button
        private Button mainMenuButton;
        private static readonly string mainMenuButtonPath = "Assets/Textures/GUI/Buttons/mainMenuButton";

        //Font used for gui display
        private SpriteFont font;

        //Displays the total points from the food gathered 
        private int foodPoints;
        //Displays time spent in the game
        private double totalTime;

        //Position for the words
        private Vector2 foodPosition;
        private Vector2 timePosition;

        //Win message position
        private Vector2 textPosition;

        /// <summary>
        /// Create overlay
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public GameOverWin(Vector2 virtualScreenSize) : base() {
            mainMenuButton = new Button(new Vector2((virtualScreenSize.X / 2) - 72, virtualScreenSize.Y - (virtualScreenSize.Y / 3) - 16), new Vector2(144, 32));

            //Set initial values
            foodPoints = 0;
            totalTime = 0;

            foodPosition = new Vector2((virtualScreenSize.X / 2), (virtualScreenSize.Y / 2)); 
            timePosition = new Vector2((virtualScreenSize.X / 2), (virtualScreenSize.Y / 2) + 25);

            textPosition = new Vector2((virtualScreenSize.X / 2), (virtualScreenSize.Y / 2) - 30);
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        public void LoadContent(ContentManager content) {
            mainMenuButton.LoadContent(content, mainMenuButtonPath);

            font = content.Load<SpriteFont>("Assets/Fonts/gameOverlayFont");

            background = content.Load<Texture2D>("Assets/Textures/Backgrounds/menuBackground");
        }

        /// <summary>
        /// Update the food points and total time
        /// </summary>
        /// <param name="worldMap"></param>
        public void Update(WorldMap worldMap) {
            foodPoints = worldMap.GetFoodPoints();

            totalTime = worldMap.GetTimerTile().GetElaspsedGameTime(); 
        }


        /// <summary>
        /// Handle switching with buttons to different overlays
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Return the new overlay </returns>
        public OverlayHandler.Overlays HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse) {
            if (mainMenuButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.mainMenu;
            }
            return OverlayHandler.Overlays.none;
        }

        /// <summary>
        /// Draw overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            spriteBatch.DrawString(font, "You win!", textPosition - (font.MeasureString("You win!") / 2), Color.Black);

            spriteBatch.DrawString(font, "Total Food Points: " + foodPoints, foodPosition - (font.MeasureString("Total Food Points: 00") / 2), Color.Black);
            spriteBatch.DrawString(font, "Total Time: " + Math.Round(totalTime, 2) + " seconds", timePosition - (font.MeasureString("Total Time: 000 seconds") / 2), Color.Black);

            mainMenuButton.Draw(spriteBatch); 
        }

        

    }
}
