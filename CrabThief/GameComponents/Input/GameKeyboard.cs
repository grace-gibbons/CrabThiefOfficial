using Microsoft.Xna.Framework.Input;

//Game keyboard
namespace CrabThief.GameComponents.Input {
    class GameKeyboard {

        //Current keyboard state
        private KeyboardState state;
        //Old keyboard state
        private KeyboardState oldState;

        /// <summary>
        /// Create GameKeyboard
        /// </summary>
        public GameKeyboard() {
            state = Keyboard.GetState();
        }

        /// <summary>
        /// Update keyboard state
        /// </summary>
        public void Update() {
            state = Keyboard.GetState();
        }

        /// <returns> Return true if the 'A' key is pressed and false otherwise </returns>
        public bool IsAPressed() {
            if (oldState.IsKeyUp(Keys.A) && state.IsKeyDown(Keys.A)) {
                oldState = state;
                return true;
            } else {
                oldState = state;
                return false;
            }
        }

        /// <returns> Return true if the 'A' key is held and false otherwise </returns>
        public bool IsAHeld() {
            if (state.IsKeyDown(Keys.A)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'D' key is pressed and false otherwise </returns>
        public bool IsDPressed() {
            if (oldState.IsKeyUp(Keys.D) && state.IsKeyDown(Keys.D)) {
                oldState = state;
                return true;
            } else {
                oldState = state;
                return false;
            }
        }

        /// <returns> Return true if the 'D' key is held and false otherwise </returns>
        public bool IsDHeld() {
            if (state.IsKeyDown(Keys.D)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'W' key WAS pressed and false otherwise </returns>
        public bool WasAHeld() {
            if (oldState.IsKeyDown(Keys.A)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'W' key is held and false otherwise </returns>
        public bool IsWHeld() {
            if (state.IsKeyDown(Keys.W)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'S' key is held and false otherwise </returns>
        public bool IsSHeld() {
            if (state.IsKeyDown(Keys.S)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'UP' key is held and false otherwise </returns>
        public bool IsUpHeld() {
            if (state.IsKeyDown(Keys.Up)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'DOWN' key is held and false otherwise </returns>
        public bool IsDownHeld() {
            if (state.IsKeyDown(Keys.Down)) {
                return true;
            } else {
                return false;
            }
        }

        /// <returns> Return true if the 'E' key is pressed and false otherwise </returns>
        public bool IsEPressed() {
            if (oldState.IsKeyUp(Keys.E) && state.IsKeyDown(Keys.E)) {
                oldState = state;
                return true;
            } else {
                oldState = state;
                return false;
            }
        }
    }
}
