using Microsoft.Xna.Framework;

//Food
namespace CrabThief.GameComponents.Map {
    class Food : Collectible {

        //Types of food
        public enum FoodTypes {
            Apple,
            Cherries,
            Sandwich,
            Cake
        }

        //Path to data files
        private static readonly string APPLE_JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/appleTile.json";
        private static readonly string SANDWICH_JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/sandwichTile.json";
        private static readonly string CHERRIES_JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/cherriesTile.json";
        private static readonly string CAKE_JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/cakeTile.json";

        //The number of points the food is worth
        private static readonly int applePointAmount = 1;
        private static readonly int cherriesPointAmount = 1;
        private static readonly int sandwichPointAmount = 2;
        private static readonly int cakePointAmount = 2;

        //Type of food
        private FoodTypes foodType; 

        /// <summary>
        /// Create food
        /// </summary>
        /// <param name="coordinates"> Coordinates of the food </param>
        /// <param name="foodType"> Type of food </param>
        public Food(Vector2 coordinates, FoodTypes foodType) : base() {
            this.foodType = foodType;

            //Read food data from json file
            if(foodType == FoodTypes.Apple) {
                ReadCollectibleData(APPLE_JSON_PATH);
            } else if(foodType == FoodTypes.Cherries) {
                ReadCollectibleData(CHERRIES_JSON_PATH);
            } else if (foodType == FoodTypes.Cake) {
                ReadCollectibleData(CAKE_JSON_PATH);
            } else {
                ReadCollectibleData(SANDWICH_JSON_PATH);
            }

            //Set up position, coords, and collision body
            Init(coordinates);
        }

        /// <returns> Return the type of food </returns>
        public FoodTypes GetFoodType() {
            return foodType; 
        }

        /// <returns>Return the amount of points rewarded for the food </returns>
        public int GetPointAmount() {
            if (foodType == FoodTypes.Apple) {
                return applePointAmount; 
            } else if (foodType == FoodTypes.Cherries) {
                return cherriesPointAmount; 
            } else if (foodType == FoodTypes.Cake) {
                return cakePointAmount; 
            }
            return sandwichPointAmount; 
        }
    }
}
