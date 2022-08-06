using System.Collections.Generic;

//Cells used for initial generation
namespace CrabThief.GameComponents.Util {
    class MapCell {

        //X and y positions
        private readonly int x;
        private readonly int y;

        //right and bottom wall
        private bool hasRightWall = true;
        private bool hasBottomWall = true;

        //Cell modifiers
        private bool inRoom = false;
        private bool hasFood = false; 
        private bool hasChange = false;
        private bool hasEnemy = false;

        //List of all cells adjacent to the cell
        private List<MapCell> neighbours;
        //List of neighbouring cells not blocked by a wall
        private List<MapCell> connected; 

        /// <summary>
        /// Create map cell
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MapCell(int x, int y) {
            neighbours = new List<MapCell>();
            connected = new List<MapCell>();
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Add neighbour, all adjacent cells
        /// </summary>
        /// <param name="cell"></param>
        public void AddNeighbour(MapCell cell) {
            if (!neighbours.Contains(cell)) {
                neighbours.Add(cell);
            }
        }

        /// <summary>
        /// Add connected, adjacent cells not blocked by a wall
        /// </summary>
        /// <param name="cell"></param>
        public void AddConnected(MapCell cell) {
            if (!connected.Contains(cell)) {
                connected.Add(cell);
            }
        }

        /// <summary>
        /// Remove the wall between two cells
        /// </summary>
        /// <param name="next"></param>
        public void RemoveWall(MapCell next) {
            //If next cell is left of the current cell
            if (next.x + 1 == x && next.y == y) {
                next.SetHasRightWall(false);
            }

            //If next cell is right of the current cell
            if (next.x - 1 == x && next.y == y) {
                SetHasRightWall(false);
            }

            //If next cell is on top of the current cell
            if (next.y + 1 == y && next.x == x) {
                next.SetHasBottomWall(false);
            }

            //If next cell is below of the current cell
            if (next.y - 1 == y && next.x == x) {
                SetHasBottomWall(false);
            }
        }

        public bool GetHasRightWall() {
            return hasRightWall;
        }

        public void SetHasRightWall(bool hasRightWall) {
            this.hasRightWall = hasRightWall;
        }

        public bool GetHasBottomWall() {
            return hasBottomWall;
        }

        public void SetHasBottomWall(bool hasBottomWall) {
            this.hasBottomWall = hasBottomWall;
        }

        public List<MapCell> GetNeighbours() {
            return neighbours;
        }

        public int GetX() {
            return x;
        }

        public int GetY() {
            return y;
        }

        public bool GetInRoom() {
            return inRoom;
        }

        public void SetInRoom(bool inRoom) {
            this.inRoom = inRoom; 
        }

        public bool GetHasFood() {
            return hasFood;
        }

        public void SetHasFood(bool hasFood) {
            this.hasFood = hasFood;
        }

        public bool GetHasChange() {
            return hasChange;
        }

        public void SetHasChange(bool hasChange) {
            this.hasChange = hasChange;
        }

        public bool GetHasEnemy() {
            return hasEnemy;
        }

        public void SetHasEnemy(bool hasEnemy) {
            this.hasEnemy = hasEnemy;
        }
    }
}
