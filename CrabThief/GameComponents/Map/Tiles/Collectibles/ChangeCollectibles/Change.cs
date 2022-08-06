using Microsoft.Xna.Framework;

//Change Tile
namespace CrabThief.GameComponents.Map {
    class Change : Collectible {

        //Types of change
        public enum ChangeTypes {
            Silver,
            Gold
        }

        //Path to silver data file
        private static readonly string SILVER_JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/silverTile.json";
        //Path to data gold file
        private static readonly string GOLD_JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/goldTile.json";

        //The number of seconds that silver coins add to the timer
        public static readonly int silverSecondsAdded = 5;
        //The number of seconds that gold coins add to the timer
        public static readonly int goldSecondsAdded = 10;

        //Type of change
        private ChangeTypes changeType; 

        /// <summary>
        /// Create change
        /// </summary>
        /// <param name="coordinates"> Coordinates of the change </param>
        /// <param name="changeType"> Type of change to create </param>
        public Change(Vector2 coordinates, ChangeTypes changeType) : base() {
            this.changeType = changeType; 

            //Read change data from json file
            if(changeType == ChangeTypes.Silver) {
                ReadCollectibleData(SILVER_JSON_PATH);
            } else {
                ReadCollectibleData(GOLD_JSON_PATH);
            }

            //Set up position, coords, and collision body
            Init(coordinates);
        }

        /// <returns> Return the change type </returns>
        public ChangeTypes GetChangeType() {
            return changeType; 
        }

        /// <returns> Return the number of seconds to add to the timer </returns>
        public int GetSecondsAdded() {
            if(changeType == ChangeTypes.Silver) {
                return silverSecondsAdded;
            } 
            return goldSecondsAdded;
        }
    }
}
