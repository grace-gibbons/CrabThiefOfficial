using Microsoft.Xna.Framework;

//Wall tile
namespace CrabThief.GameComponents.Map {
    class WallTile : WorldTile {

        //Path to data file
        private static readonly string JSON_PATH = "Content/Assets/Components/Tiles/WorldTiles/wallTile.json";

        /// <summary>
        /// Create wall tile
        /// </summary>
        /// <param name="coordinates"></param>
        public WallTile(Vector2 coordinates) : base() {
            //Read data from json file
            ReadWallData(JSON_PATH); 

            SetCoordinates(coordinates);
            SetIsCollidable(true);

            //Set position basd on size and grid location
            SetPosition(coordinates * GetWorldTileSize());

            //Set collision body
            SetCollisionBody(new CollisionBody(GetPosition(), GetSize()));
        }
    }
}
