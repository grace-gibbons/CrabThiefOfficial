using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

//Animations consist of spritesheets, which are split into images 
//The texture location of the current frame is stored as a rectangle
namespace CrabThief.GameComponents.Graphics {
    class Animation {

        //The sprite sheet to animate
        private SpriteSheet spriteSheet;

        //The position of the animation
        private Vector2 position;

        //The maximum number of frames in the animation
        private int maxFrames;
        //The current frame being displayed
        private int currentFrame;

        //Size of each sprite, in pixels
        private Vector2 spriteSize;

        //Time between frames
        private int frameRate;
        //Counting time between frames
        private float timer = 0;

        //True if the animation is currently in use
        private bool isInMotion = false;

        /// <summary>
        /// Create an animation
        /// </summary>
        /// <param name="position"> Position of the animation </param>
        /// <param name="spriteSheet"> Sprite sheet to animate </param>
        /// <param name="frameRate"> Framerate, speed of display </param>
        public Animation(Vector2 position, SpriteSheet spriteSheet, int frameRate) {
            this.position = position; 
            this.spriteSheet = spriteSheet;
            //Set the max frames to the width of the sprite sheet
            maxFrames = spriteSheet.GetWidthInSprites();
            //Start animation at frame 0 
            currentFrame = 0;
            this.frameRate = frameRate;
            spriteSize = spriteSheet.GetSpriteSize();
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"> Content Manager </param>
        public void LoadContent(ContentManager content) {
            spriteSheet.LoadContent(content); 
        }


        /// <returns> Return the sprite sheet to animate </returns>
        public SpriteSheet GetSpriteSheet() {
            return spriteSheet;
        }

        /// <returns> Return the current frame of the animation </returns>
        public int GetCurrentFrame() {
            return currentFrame;
        }

        /// <summary>
        /// Set the current frame to display
        /// </summary>
        /// <param name="currentFrame"> The frame to display </param>
        public void SetCurrentFrame(int currentFrame) {
            this.currentFrame = currentFrame; 
        }

        /// <returns> Return the framerate </returns>
        public int GetFrameRate() {
            return frameRate; 
        }

        /// <returns> Return the max number of frames </returns>
        public int GetMaxFrames() {
            return maxFrames;
        }

        /// <summary>
        /// Increment the current frame by 1
        /// </summary>
        public void IncrementCurrentFrame() {
            currentFrame++;
        }


        /// <returns> Return the timer, or time between frames </returns>
        public float GetTimer() {
            return timer;
        }

        /// <summary>
        /// Set the timer
        /// </summary>
        /// <param name="timer"> Value to set the timer to </param>
        public void SetTimer(float timer) {
            this.timer = timer;
        }

        /// <summary>
        /// Increment the timer by a given amount
        /// </summary>
        /// <param name="increment"> Amount to increment the timer by </param>
        public void IncrementTimer(float increment) {
            timer += increment;
        }


        /// <returns> Return if the animtion is active or not </returns>
        public bool GetIsInMotion() {
            return isInMotion;
        }

        /// <summary>
        /// Set isInMotion
        /// </summary>
        /// <param name="isInMotion"> Value to set isInMotion to </param>
        public void SetIsInMotion(bool isInMotion) {
            this.isInMotion = isInMotion;
        }

        /// <returns> Return the size of the animation sprite </returns>
        public Vector2 GetSpriteSize() {
            return spriteSize;
        }

        /// <returns> Return the position of the animation </returns>
        public Vector2 GetPosition() {
            return position; 
        }
    }
}
