using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

//Golden Crab, spawns 1/1000 times
namespace CrabThief.GameComponents.Map.Tiles.Collectibles {
    class GoldenCrab : Collectible {

        //Path to data file
        private static readonly string JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/goldenCrabTile.json";

        /// <summary>
        /// Create golden crab
        /// </summary>
        /// <param name="coordinates"></param>
        public GoldenCrab(Vector2 coordinates) : base() {
            //Read data from json file
            ReadCollectibleData(JSON_PATH);

            //Set up position, coords, and collision body
            Init(coordinates);
        }
    }
}
