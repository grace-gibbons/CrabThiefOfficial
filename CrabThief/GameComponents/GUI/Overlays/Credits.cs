using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

//Credits menu
namespace CrabThief.GameComponents.GUI.Overlays {
    class Credits : Overlay {

        //Background image
        private Texture2D background;

        //Back button
        private Button backButton;
        private static readonly string backButtonPath = "Assets/Textures/GUI/Buttons/backButton";

        /// <summary>
        /// Create credits menu
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public Credits(Vector2 virtualScreenSize) : base() {
            backButton = new Button(new Vector2(virtualScreenSize.X - 80 - 5, virtualScreenSize.Y - 32 - 5), new Vector2(80, 32));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            backButton.LoadContent(content, backButtonPath);

            background = content.Load<Texture2D>("Assets/Textures/Backgrounds/creditsBackground");
        }

        /// <summary>
        /// Handle switching with buttons to different overlays
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Return the new overlay </returns>
        public OverlayHandler.Overlays HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse) {
            if (backButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return OverlayHandler.Overlays.mainMenu;
            }
            return OverlayHandler.Overlays.none;
        }

        /// <summary>
        /// Draw the credits menu
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            backButton.Draw(spriteBatch);
        }
    }
}
