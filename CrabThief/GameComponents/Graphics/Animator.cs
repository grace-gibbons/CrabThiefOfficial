using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Animate animations
namespace CrabThief.GameComponents.Graphics {
    class Animator {

        /// <summary>
        /// Draws a single frame of an animation
        /// </summary>
        /// <param name="animation"> The animation to draw </param>
        /// <param name="spriteBatch"></param>
        public void DrawFrame(Animation animation, SpriteBatch spriteBatch) {
            //The texture for the entir spritesheet
            Texture2D spriteSheetTexture = animation.GetSpriteSheet().GetTexture();

            //Get the Rectangle location of the current frame on the spriteSheet
            Rectangle textureLocation = animation.GetSpriteSheet().GetSpriteLocations().ElementAt(animation.GetCurrentFrame());

            //Draw the frame
            spriteBatch.Draw(spriteSheetTexture, animation.GetPosition(), textureLocation, Color.White);
        }

        /// <summary>
        /// Draw a continuous animation
        /// </summary>
        /// <param name="animation"> The animation to draw </param>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Animate(Animation animation, GameTime gameTime, SpriteBatch spriteBatch) {
            //Draw frame
            DrawFrame(animation, spriteBatch); 

            //Change frame
            if (animation.GetFrameRate() < animation.GetTimer()) {
                //Set frame to next frame
                if (animation.GetCurrentFrame() < animation.GetMaxFrames() - 1) {
                    animation.IncrementCurrentFrame();
                    animation.SetIsInMotion(true);
                } else {
                    //Reset the frame to the original, to restart the animation
                    animation.SetCurrentFrame(0);
                    animation.SetIsInMotion(false);
                }
                //Reset timer at frame change
                animation.SetTimer(0);
            } else {
                //Frame does not change, increment timer
                animation.IncrementTimer((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Loop an animation once
        /// </summary>
        /// <param name="animation"> The animation to draw </param>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void AnimateOnce(Animation animation, GameTime gameTime, SpriteBatch spriteBatch) {
            //Draw frame
            DrawFrame(animation, spriteBatch);

            //Change frame
            if (animation.GetFrameRate() < animation.GetTimer()) {
                //Set frame to next frame
                if (animation.GetCurrentFrame() < animation.GetMaxFrames() - 1) {
                    animation.IncrementCurrentFrame();
                    animation.SetIsInMotion(true);
                } else {
                    //Stop animation
                    animation.SetIsInMotion(false);
                }
                //Reset timer at frame change
                animation.SetTimer(0);
            } else {
                //Frame does not change, increment timer
                animation.IncrementTimer((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }
    }
}
