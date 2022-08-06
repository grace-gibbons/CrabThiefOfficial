using CrabThief.GameComponents.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Texture that has two states, usually swapped with an animation
//Ex: health, no health, and an in-between animation
//Assumes that textures and animations will have the same size and position
namespace CrabThief.GameComponents.GUI.GUIComponents {
    class BooleanTexture {
        //The first texture
        private Texture2D texture0;
        //The second texture
        private Texture2D texture1;

        //Paths to each texture
        private string texture0Path;
        private string texture1Path;

        //the animtion that links the textures
        private Animation animation;
        
        //Position and size of the boolean texture (constant for both textures and animation)
        private Vector2 position; 
        private Vector2 size;
        
        //Visibility states for each texture and animtion
        private bool showTexture0;
        private bool showTexture1;
        private bool showAnimation;

        private Animator animator; 

        /// <summary>
        /// Create a booleanTexture
        /// </summary>
        /// <param name="position"> Position of the texture </param>
        /// <param name="size"> Size of the texture </param>
        /// <param name="texture0Path"> Path to texture 0 </param>
        /// <param name="texture1Path"> Path to texture 1 </param>
        /// <param name="animation"> Animtion </param>
        public BooleanTexture(Vector2 position, Vector2 size, string texture0Path, string texture1Path, Animation animation) {
            this.position = position; 
            this.size = size;
            this.texture0Path = texture0Path;
            this.texture1Path = texture1Path;
            this.animation = animation;

            //Initial states 
            showTexture0 = true;
            showTexture1 = false;
            showAnimation = false;

            animator = new Animator();
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            texture0 = content.Load<Texture2D>(texture0Path);
            texture1 = content.Load<Texture2D>(texture1Path);
            animation.LoadContent(content); 
        }

        /// <summary>
        /// Update the animation 
        /// </summary>
        public void Update() {
            if (animation.GetCurrentFrame() == animation.GetMaxFrames()) {
                showAnimation = false;
                animation.SetCurrentFrame(0); 
            }
        }
        
        /// <summary>
        /// Draw the boolean texture 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            if(showTexture0) {
                spriteBatch.Draw(texture0, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
            } else if(showTexture1) {
                spriteBatch.Draw(texture1, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
            } else if(showAnimation) {
                animator.AnimateOnce(animation, gameTime, spriteBatch); 
            }
        }

        /// <returns> Return the visibility state of the first texture </returns>
        public bool GetShowTexture0() {
            return showTexture0; 
        }

        /// <summary>
        /// Set the state of the first texture
        /// </summary>
        /// <param name="showTexture0"></param>
        public void SetShowTexture0(bool showTexture0) {
            this.showTexture0 = showTexture0;
        }

        /// <returns> Return the visibility state of the second texture </returns>
        public bool GetShowTexture1() {
            return showTexture1;
        }

        /// <summary>
        /// Set the state of the second texture
        /// </summary>
        /// <param name="showTexture1"></param>
        public void SetShowTexture1(bool showTexture1) {
            this.showTexture1 = showTexture1;
        }

        /// <returns> Return the visibility state of the animation </returns>
        public bool GetShowAnimation() {
            return showAnimation;
        }

        /// <summary>
        /// Set the state of the animtion
        /// </summary>
        /// <param name="showAnimation"></param>
        public void SetShowAnimation(bool showAnimation) {
            this.showAnimation = showAnimation;
        }

        /// <returns> Return the animtion </returns>
        public Animation GetAnimation() {
            return animation; 
        }
    }
}
