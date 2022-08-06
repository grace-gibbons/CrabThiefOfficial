using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//Game Mouse
namespace CrabThief.GameComponents.Input {
    class GameMouse {

        //Current mouse state
        private MouseState mouseState;
        //Previous mouse state
        private MouseState oldMouseState;

        //Mouse position
        private Vector2 position;

        //The screen size may change depending on the monitor, make sure the position matches it
        private Matrix screenScale;

        public GameMouse(Matrix matrix) {
            mouseState = Mouse.GetState();

            //Set screen scale
            screenScale = matrix; 
        }

        /// <summary>
        /// Update the mouse state and position
        /// </summary>
        public void Update() {
            mouseState = Mouse.GetState();
            position = new Vector2(mouseState.X, mouseState.Y);
        }

        /// <returns> Return true if the left mouse button is clicked </returns>
        public bool IsLeftButton() {
            if (oldMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed) {
                oldMouseState = mouseState;
                return true;
            } else {
                oldMouseState = mouseState;
                return false;
            }
        }

        /// <returns> Return true if the left mouse button is held </returns>
        public bool IsLeftButtonHeld() {
            if (mouseState.LeftButton == ButtonState.Pressed) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the right mouse button is clicked </returns>
        public bool IsRightButton() {
            if (oldMouseState.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed) {
                oldMouseState = mouseState;
                return true;
            } else {
                oldMouseState = mouseState;
                return false;
            }
        }

        /// <returns> Return true if the right mouse button is held </returns>
        public bool IsRightButtonHeld() {
            if (mouseState.RightButton == ButtonState.Pressed) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return the mouse position on the true screen </returns>
        public Vector2 GetPosition() {
            //Transform the position to match the screen scale 
            return Vector2.Transform(position, Matrix.Invert(screenScale));
        }

        /// <returns> Return the mouse position on the virtual screen, not changed by screen expansion matrix </returns>
        public Vector2 GetPositionUnchanged() {
            return position;
        }
    }
}
