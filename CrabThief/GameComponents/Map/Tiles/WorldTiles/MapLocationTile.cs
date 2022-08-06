using Microsoft.Xna.Framework;

//Map location tile - location of treasure
namespace CrabThief.GameComponents.Map.Tiles.WorldTiles {
    class MapLocationTile : WorldTile {

        //All possible map directions
        public enum Directions {
            North,
            East,
            South,
            West,
            NorthEast,
            SouthEast,
            NorthWest,
            SouthWest,
            Location
        }

        //Path to data file
        private static readonly string JSON_PATH = "Content/Assets/Components/Tiles/WorldTiles/mapLocationTile.json";

        /// <summary>
        /// Create MapLocationTile
        /// </summary>
        /// <param name="coordinates"></param>
        public MapLocationTile(Vector2 coordinates) : base() {
            //Read data from json file
            ReadWallData(JSON_PATH);

            SetCoordinates(coordinates);
            SetIsCollidable(true);

            //Set position basd on size and grid location
            SetPosition(coordinates * GetWorldTileSize());

            //Set collision body
            SetCollisionBody(new CollisionBody(GetPosition(), GetSize()));
        }

        /// <summary>
        /// Return the direction of the treasure tile in reference to the player
        /// </summary>
        /// <param name="playerCoordinates"></param>
        /// <returns> The direction </returns>
        public Directions GetDirection(Vector2 playerCoordinates) {
            //Player is east of map, return west
            if(playerCoordinates.X > GetCoordinates().X && playerCoordinates.Y == GetCoordinates().Y) {
                return Directions.West;
            }
            //Player is south of map, return north
            if (playerCoordinates.X == GetCoordinates().X && playerCoordinates.Y > GetCoordinates().Y) {
                return Directions.North;
            }
            //Player is west of map, return east
            if (playerCoordinates.X < GetCoordinates().X && playerCoordinates.Y == GetCoordinates().Y) {
                return Directions.East;
            }
            //Player is north of map, return south
            if (playerCoordinates.X == GetCoordinates().X && playerCoordinates.Y < GetCoordinates().Y) {
                return Directions.South;
            }
            //Player is north east of map, return south west
            if (playerCoordinates.X > GetCoordinates().X && playerCoordinates.Y < GetCoordinates().Y) {
                return Directions.SouthWest;
            }
            //Player is south east of map, return north west
            if (playerCoordinates.X > GetCoordinates().X && playerCoordinates.Y > GetCoordinates().Y) {
                return Directions.NorthWest;
            }
            //Player is south west of map, return north east
            if (playerCoordinates.X < GetCoordinates().X && playerCoordinates.Y > GetCoordinates().Y) {
                return Directions.NorthEast;
            }
            //Player is north west of map, return south east
            if (playerCoordinates.X < GetCoordinates().X && playerCoordinates.Y < GetCoordinates().Y) {
                return Directions.SouthEast;
            }

            //Return location because the player is on the tilemap location
            return Directions.Location;
        }
    }
}
