using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.Input;

//Main menu overlay - starts as the opening menu for the game
namespace CrabThief.GameComponents.GUI.Overlays {
    class MainMenu : Overlay {

        //Background image
        private Texture2D background; 

        //Play button
        private Button playButton;
        private static readonly string playButtonPath = "Assets/Textures/GUI/Buttons/playButton";

        //Credits button
        private Button creditsButton;
        private static readonly string creditsButtonPath = "Assets/Textures/GUI/Buttons/creditsButton";

        //How to play button
        private Button howToPlayButton;
        private static readonly string howToPlayButtonPath = "Assets/Textures/GUI/Buttons/howToPlayButton";

        //Settings button
        private Button settingsButton;
        private static readonly string settingsButtonPath = "Assets/Textures/GUI/Buttons/settingsButton";

        /// <summary>
        /// Create overlay
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public MainMenu(Vector2 virtualScreenSize) : base() {
            SetIsCurrentOverlay(true); 
            //Create buttons
            playButton = new Button(new Vector2((virtualScreenSize.X / 2) - 40, (virtualScreenSize.Y / 4) - 16 + 50), new Vector2(80, 32));
            howToPlayButton = new Button(new Vector2((virtualScreenSize.X / 2) - 88, (virtualScreenSize.Y / 2) - 16), new Vector2(176, 32));
            creditsButton = new Button(new Vector2((virtualScreenSize.X / 3) - 64, virtualScreenSize.Y - (virtualScreenSize.Y / 4) - 16 - 50), new Vector2(128, 32));
            settingsButton = new Button(new Vector2(virtualScreenSize.X - (virtualScreenSize.X / 3) - 72, virtualScreenSize.Y - (virtualScreenSize.Y / 4) - 16 - 50), new Vector2(144, 32));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            playButton.LoadContent(content, playButtonPath);
            creditsButton.LoadContent(content, creditsButtonPath);
            howToPlayButton.LoadContent(content, howToPlayButtonPath);
            settingsButton.LoadContent(content, settingsButtonPath);

            background = content.Load<Texture2D>("Assets/Textures/Backgrounds/titleBackground");
        }


        /// <summary>
        /// Handle switching with buttons to different overlays
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Return the new overlay </returns>
        public OverlayHandler.Overlays HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse) {
            if(playButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.gameplay; 
            } else if (creditsButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.credits;
            } else if (settingsButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.settings;
            } else if (howToPlayButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.howToPlay;
            }
            return OverlayHandler.Overlays.none; 
        }

        /// <summary>
        /// Draw overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            playButton.Draw(spriteBatch);
            creditsButton.Draw(spriteBatch);
            howToPlayButton.Draw(spriteBatch);
            settingsButton.Draw(spriteBatch);
        }
    }
}
