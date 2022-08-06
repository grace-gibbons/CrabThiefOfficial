using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.Input;

//Pause overlay
namespace CrabThief.GameComponents.GUI.Overlays {
    class Pause : Overlay {

        //Background image
        private Texture2D background;

        //Resume button
        private Button resumeButton;
        private static readonly string resumeButtonPath = "Assets/Textures/GUI/Buttons/resumeButton";

        //Settings button
        private Button settingsButton;
        private static readonly string settingsButtonPath = "Assets/Textures/GUI/Buttons/settingsButton";

        //Main menu button
        private Button mainMenuButton;
        private static readonly string mainMenuButtonPath = "Assets/Textures/GUI/Buttons/mainMenuButton";

        /// <summary>
        /// Create overlay
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public Pause(Vector2 virtualScreenSize) : base() {
            resumeButton = new Button(new Vector2((virtualScreenSize.X / 2) - 56, (virtualScreenSize.Y / 4) - 16), new Vector2(112, 32));
            settingsButton = new Button(new Vector2((virtualScreenSize.X / 2) - 72, (virtualScreenSize.Y / 2) - 16), new Vector2(144, 32));
            mainMenuButton = new Button(new Vector2((virtualScreenSize.X / 2) - 72, virtualScreenSize.Y - (virtualScreenSize.Y / 4) - 16), new Vector2(144, 32));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            resumeButton.LoadContent(content, resumeButtonPath);
            mainMenuButton.LoadContent(content, mainMenuButtonPath);
            settingsButton.LoadContent(content, settingsButtonPath);

            background = content.Load<Texture2D>("Assets/Textures/Backgrounds/menuBackground");
        }

        /// <summary>
        /// Handle switching with buttons to different overlays
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Return the new overlay </returns>
        public OverlayHandler.Overlays HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse) {
            if (resumeButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.gameplay;
            } else if (mainMenuButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.mainMenu;
            } else if (settingsButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.settings;
            }
            return OverlayHandler.Overlays.none;
        }

        /// <summary>
        /// Draw overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            resumeButton.Draw(spriteBatch);
            mainMenuButton.Draw(spriteBatch);
            settingsButton.Draw(spriteBatch);
        }
    }
}
