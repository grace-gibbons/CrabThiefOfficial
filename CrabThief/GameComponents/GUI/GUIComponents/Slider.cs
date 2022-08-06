using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


//Slider: has image and button that you can slide
//Positions carefully calculated, probably won't work for images of varying sizes without some editing
namespace CrabThief.GameComponents.GUI.GUIComponents {
    class Slider {

        //Actual slider bar image
        private Texture2D sliderTexture;
        //The little slider button thingy
        private Button sliderControl; 

        //position and size of the slider bar 
        private Vector2 position;
        private Vector2 size;

        //Position bounds for the slider button, so it cannot be dragged off the slider
        private float minPosition;
        private float maxPosition;

        //A slider has 10 states: 0 - 9
        private int sliderState;
        private readonly int numSliderStates = 10; 

        /// <summary>
        /// Create slider
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Slider(Vector2 position, Vector2 size) {
            this.position = position;
            this.size = size;
            minPosition = position.X + 5;
            maxPosition = position.X + size.X - 6; 
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sliderPath"></param>
        /// <param name="buttonPath"></param>
        public void LoadContent(ContentManager content, string sliderPath, string buttonPath) {
            //Load the texture for the slider bar
            sliderTexture = content.Load<Texture2D>(sliderPath);

            //Create the button for the slider control
            sliderControl = new Button(position, Vector2.Zero);

            //Set the button size to its image size and set the proper y position, depending on size
            sliderControl.LoadContent(content, buttonPath);
            sliderControl.SetSize(new Vector2(sliderControl.GetTexture().Width, sliderControl.GetTexture().Height));
            sliderControl.SetPosition(new Vector2(position.X, position.Y + 1));
            sliderControl.SetCollisionBody(new CollisionBody(sliderControl.GetPosition(), sliderControl.GetSize()));

            //Set the volume slider to start at 40% volume
            sliderState = 4; 
        }

        /// <summary>
        /// Update the slider state
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        public void Update(CollisionEngine collisionEngine, GameMouse mouse) {
            //If the mouse tries to drag the slider out of bounds
            if(sliderControl.GetPosition().X + (sliderControl.GetSize().X / 2) < minPosition || sliderControl.GetPosition().X + (sliderControl.GetSize().X / 2) > maxPosition + 2) {
                //Release the slider button
                sliderControl.SetIsHeld(false);
                //Update the position
                UpdateSliderPosition();
            } else if(sliderControl.FollowMouseXAxis(collisionEngine, mouse)) {
                UpdateSliderState();
            } else {
                UpdateSliderPosition();
            }
        }

        /// <summary>
        /// Update the slider state based on the position of the button
        /// </summary>
        public void UpdateSliderState() {
            float range = maxPosition - minPosition;
            float pixelsBetweenMarker = range / (numSliderStates - 1);

            //Proper center position of the slider
            float sliderPosition = sliderControl.GetPosition().X + (sliderControl.GetSize().X / 2);

            if (sliderPosition >= minPosition && sliderPosition < minPosition + (pixelsBetweenMarker / 2)) {
                sliderState = 0; 
            } else if(sliderPosition >= minPosition + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + pixelsBetweenMarker + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 1; 
            } else if(sliderPosition > minPosition + pixelsBetweenMarker + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 2) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 2; 
            } else if(sliderPosition > minPosition + (pixelsBetweenMarker * 2) + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 3) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 3; 
            } else if (sliderPosition > minPosition + (pixelsBetweenMarker * 3) + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 4) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 4;
            } else if (sliderPosition > minPosition + (pixelsBetweenMarker * 4) + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 5) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 5;
            } else if (sliderPosition > minPosition + (pixelsBetweenMarker * 5) + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 6) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 6;
            } else if (sliderPosition > minPosition + (pixelsBetweenMarker * 6) + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 7) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 7;
            } else if (sliderPosition > minPosition + (pixelsBetweenMarker * 7) + (pixelsBetweenMarker / 2) && sliderPosition < minPosition + (pixelsBetweenMarker * 8) + (pixelsBetweenMarker / 2) - 1) {
                sliderState = 8;
            } else if (sliderPosition > minPosition + (pixelsBetweenMarker * 8) + (pixelsBetweenMarker / 2) && sliderPosition <= maxPosition) {
                sliderState = 9;
            }
        }

        
        /// <summary>
        /// Set the slider to one of the preset positions, depending on state
        /// </summary>
        public void UpdateSliderPosition() {
            float y = sliderControl.GetPosition().Y;
            float size = sliderControl.GetSize().X / 2; 

            float range = maxPosition - minPosition;
            float pixelsBetweenMarker = range / (numSliderStates - 1);

            if (sliderState == 0) {
                sliderControl.SetPosition(new Vector2(minPosition - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition - size, y)); 
            } else if(sliderState == 1) {
                sliderControl.SetPosition(new Vector2(minPosition + pixelsBetweenMarker - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + pixelsBetweenMarker - size, y));
            } else if (sliderState == 2) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 2) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 2) - size, y));
            } else if (sliderState == 3) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 3) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 3) - size, y));
            } else if (sliderState == 4) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 4) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 4) - size, y));
            } else if (sliderState == 5) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 5) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 5) - size, y));
            } else if (sliderState == 6) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 6) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 6) - size, y));
            } else if (sliderState == 7) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 7) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 7) - size, y));
            } else if (sliderState == 8) {
                sliderControl.SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 8) - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(minPosition + (pixelsBetweenMarker * 8) - size, y));
            } else if(sliderState == 9) {
                sliderControl.SetPosition(new Vector2(maxPosition - size, y));
                sliderControl.GetCollisionBody().SetPosition(new Vector2(maxPosition - size, y));
            }
        }

        /// <summary>
        /// Draw the slider
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sliderTexture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
            sliderControl.Draw(spriteBatch); 
            
        }

        /// <returns> Return the slider button </returns>
        public Button GetSliderControl() {
            return sliderControl; 
        }

        /// <returns> Return the slider state </returns>
        public int GetSliderState() {
            return sliderState; 
        }
    }
}
