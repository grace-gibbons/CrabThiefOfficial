using System.Collections.Generic;

using Microsoft.Xna.Framework;

//Background tile 
namespace CrabThief.GameComponents.Map {
    class BackgroundTile : WorldTile {

        //Path to background tile data
        private static readonly string JSON_PATH = "Content/Assets/Components/Tiles/WorldTiles/backgroundTile.json";

        //The background tiles "next to" the background tile (separated by a wall)
        private readonly List<BackgroundTile> neighbours;

        //The neighbouring background tiles with no separating walls, filled after maze is generated
        private List<BackgroundTile> connected;

        /// <summary>
        /// Create background tile
        /// </summary>
        /// <param name="coordinates"> Coordinates of tile </param>
        public BackgroundTile(Vector2 coordinates) : base() {
            //Read json file data
            ReadWallData(JSON_PATH);

            SetCoordinates(coordinates);
            SetPosition(coordinates * GetWorldTileSize()); 
            SetIsCollidable(false);

            //Set position basd on size and grid location
            SetPosition(coordinates * GetWorldTileSize());

            neighbours = new List<BackgroundTile>();
            connected = new List<BackgroundTile>();
        }

        /// <summary>
        /// Add a neighbour to list
        /// Neighbours are any cell that connects to tile, regardless of walls
        /// </summary>
        /// <param name="cell"></param>
        public void AddNeighbour(BackgroundTile cell) {
            if (!neighbours.Contains(cell)) {
                neighbours.Add(cell);
            }
        }

        /// <summary>
        /// Add a neighbour to list
        /// Connected are cells that connected to the tile and are NOT blocked by walls
        /// </summary>
        /// <param name="cell"></param>
        public void AddConnected(BackgroundTile cell) {
            if (!connected.Contains(cell)) {
                connected.Add(cell);
            }
        }

        /// <summary>
        /// Return the location of the wall between this cell and the next cell
        /// </summary>
        /// <param name="next"> the next cell </param>
        /// <returns> the wall location </returns>
        public Vector2 RemoveWall(BackgroundTile next) {
            //If next cell is left of the current cell
            if(next.GetCoordinates().X + 2 == GetCoordinates().X && next.GetCoordinates().Y == GetCoordinates().Y) {
                return new Vector2(next.GetCoordinates().X + 1, next.GetCoordinates().Y); 
            }

            //If next cell is right of the current cell
            if (next.GetCoordinates().X - 2 == GetCoordinates().X && next.GetCoordinates().Y == GetCoordinates().Y) {
                return new Vector2(GetCoordinates().X + 1, GetCoordinates().Y);
            }

            //If next cell is on top of the current cell
            if (next.GetCoordinates().Y + 2 == GetCoordinates().Y && next.GetCoordinates().X == GetCoordinates().X) { 
                return new Vector2(next.GetCoordinates().X, next.GetCoordinates().Y + 1);
            }

            //If next cell is below of the current cell
            if (next.GetCoordinates().Y - 2 == GetCoordinates().Y && next.GetCoordinates().X == GetCoordinates().X) {
                return new Vector2(GetCoordinates().X, GetCoordinates().Y + 1);
            }
            return Vector2.Zero; 
        }

        public List<BackgroundTile> GetNeighbours() {
            return neighbours;
        }

        public List<BackgroundTile> GetConnected() {
            return connected;
        }
    }
}
