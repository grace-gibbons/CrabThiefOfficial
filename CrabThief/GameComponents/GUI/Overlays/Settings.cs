using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using CrabThief.GameComponents.GUI.GUIComponents;
using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.Input;

//Settings overlay
namespace CrabThief.GameComponents.GUI.Overlays {
    class Settings : Overlay {

        //Background image
        private Texture2D background;

        //Pause button
        private Button backButton;
        private static readonly string backButtonPath = "Assets/Textures/GUI/Buttons/backButton";

        //Volume Slider
        private Slider volumeSlider;
        private Vector2 sliderPosition; 
        private static readonly string volumeSliderPath = "Assets/Textures/GUI/Sliders/sliderBar";
        private static readonly string volumeButtonPath = "Assets/Textures/GUI/Sliders/sliderButton";

        //Volume text
        private SpriteFont font; 

        public Settings(Vector2 virtualScreenSize) : base() {
            backButton = new Button(new Vector2(virtualScreenSize.X - 80 - 5, virtualScreenSize.Y - 32 - 5), new Vector2(80, 32));

            sliderPosition = new Vector2((virtualScreenSize.X / 2) - 95, (virtualScreenSize.Y / 2)); 
            volumeSlider = new Slider(sliderPosition, new Vector2(191, 11));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            backButton.LoadContent(content, backButtonPath);

            volumeSlider.LoadContent(content, volumeSliderPath, volumeButtonPath);

            font = content.Load<SpriteFont>("Assets/Fonts/gameOverlayFont");

            background = content.Load<Texture2D>("Assets/Textures/Backgrounds/menuBackground");
        }

        /// <summary>
        /// Update settings state
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        public void Update(CollisionEngine collisionEngine, GameMouse mouse) {
            volumeSlider.Update(collisionEngine, mouse);

            //change volume based on volume slider
            SoundEffect.MasterVolume = volumeSlider.GetSliderState() * 0.1f;
        }

        /// <summary>
        /// Handle switching with buttons to different overlays
        /// Specific buttons are linked to switching and will return the new overlay
        /// Settings will either return to main menu or pause menu depending on where it was clicked from
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="previousOverlay"></param>
        /// <returns> Return the new overlay </returns>
        public OverlayHandler.Overlays HandleSwitching(CollisionEngine collisionEngine, GameMouse mouse, OverlayHandler.Overlays previousOverlay) {
            if (backButton.IsClicked(collisionEngine, mouse)) {
                SetIsCurrentOverlay(false);
                return previousOverlay;
            }
            return OverlayHandler.Overlays.none;
        }

        /// <summary>
        /// Draw overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            backButton.Draw(spriteBatch);

            volumeSlider.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Volume:", new Vector2(sliderPosition.X, sliderPosition.Y - 14), Color.Black);
        }
    }
}
