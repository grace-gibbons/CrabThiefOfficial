
//General overlay template
namespace CrabThief.GameComponents.GUI {
    class Overlay {

        //True if the overlay is the current one displayed, false otherwise
        private bool isCurrentOverlay;

        /// <summary>
        /// Create overlay
        /// </summary>
        public Overlay() {
            isCurrentOverlay = false;
        }

        /// <summary>
        /// Set the overlay
        /// </summary>
        /// <param name="isCurrentOverlay"></param>
        public void SetIsCurrentOverlay(bool isCurrentOverlay) {
            this.isCurrentOverlay = isCurrentOverlay;
        }

        /// <returns> Return isCurrentOverlay </returns>
        public bool GetIsCurrentOverlay() {
            return isCurrentOverlay; 
        }
    }
}
