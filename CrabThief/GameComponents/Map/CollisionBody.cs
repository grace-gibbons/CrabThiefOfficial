using Microsoft.Xna.Framework;

//Body that is used when detecting collisions
namespace CrabThief.GameComponents.Map {
    class CollisionBody {

        //Position of the collision body
        private Vector2 position;

        //Size of the collision body
        private Vector2 size;

        //True if the body is enabled, or able to detect collisions, false otherwise
        private bool isEnabled;

        /// <summary>
        /// Create collision body
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public CollisionBody(Vector2 position, Vector2 size) {
            this.position = position;
            this.size = size;
            isEnabled = true; 
        }

        public Vector2 GetPosition() {
            return position;
        }

        public Vector2 GetSize() {
            return size;
        }

        public void SetSize(Vector2 size) {
            this.size = size;
        }

        public void SetPosition(Vector2 position) {
            this.position = position; 
        }

        public bool GetIsEnabled() {
            return isEnabled; 
        }

        public void SetIsEnabled(bool isEnabled) {
            this.isEnabled = isEnabled; 
        }
    }
}
