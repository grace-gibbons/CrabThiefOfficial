using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Game over lose menu
namespace CrabThief.GameComponents.GUI.Overlays {
    class GameOverLose : Overlay {

        //Background image
        private Texture2D background;

        //Main menu button
        private Button mainMenuButton;
        private static readonly string mainMenuButtonPath = "Assets/Textures/GUI/Buttons/mainMenuButton";

        //Font used for gui display
        private SpriteFont font;

        //Position for "you lose" text
        private Vector2 textPosition; 

        /// <summary>
        /// Create overlay
        /// </summary>
        /// <param name="virtualScreenSize"></param>
        public GameOverLose(Vector2 virtualScreenSize) : base() {
            mainMenuButton = new Button(new Vector2((virtualScreenSize.X / 2) - 72, virtualScreenSize.Y - (virtualScreenSize.Y / 3) - 16), new Vector2(144, 32));

            textPosition = new Vector2((virtualScreenSize.X / 2), (virtualScreenSize.Y / 2));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            mainMenuButton.LoadContent(content, mainMenuButtonPath);

            font = content.Load<SpriteFont>("Assets/Fonts/gameOverlayFont");

            background = content.Load<Texture2D>("Assets/Textures/Backgrounds/menuBackground");
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

            spriteBatch.DrawString(font, "You lose!", textPosition - (font.MeasureString("You lose!")/2), Color.Black);

            mainMenuButton.Draw(spriteBatch);
        }
    }
}
