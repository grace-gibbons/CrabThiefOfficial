using System;
using System.Collections.Generic;

using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Map.Tiles;
using CrabThief.GameComponents.Map.Tiles.Collectibles;
using CrabThief.GameComponents.Map.Tiles.WorldTiles;

using Microsoft.Xna.Framework;

//Generate initial map and transfrom into binary list
namespace CrabThief.GameComponents.Util {
    class MapGenerator {

        //Numbers that correspond to tiles
        public enum MapNumbers {
            background = 0,
            wall = 1,
            food = 2,
            change = 3,
            enemy = 4
        }

        private static readonly int MIN_ROOM_NUM = 4; 
        private static readonly int MAX_ROOM_NUM = 8;
        private static readonly int MIN_ROOM_SIZE = 3;
        private static readonly int MAX_ROOM_SIZE = 6;

        private static readonly int MIN_FOOD = 10;
        private static readonly int MAX_FOOD = 16;

        private static readonly int MIN_CHANGE = 14;
        private static readonly int MAX_CHANGE = 18;

        //Rows and columns of the map - size of the map
        private static int rows = 15;
        private static int columns = 15;

        //The maze before and during generation, without proper borders and such
        private WorldTile[,] generatedMaze;

        //The maze post-generation, with proper borders
        //This maze is used to generate the world
        private Tile[,] fullMaze; 

        //Queue of cells that came before the current cell
        private LinkedList<WorldTile> previousQueue;

        //Store each cell that has been visited during maze generation
        private List<WorldTile> visitedList;

        //Current cell - used during generation
        private WorldTile currentCell;

        /// <summary>
        /// Create map generator
        /// </summary>
        public MapGenerator() {
            //fullMaze = new WorldTile[(columns * 2) + 1, (rows * 2) + 1]; 
            fullMaze = new Tile[(columns * 2) + 1, (rows * 2) + 1]; 
            generatedMaze = new WorldTile[columns * 2, rows * 2]; 
            previousQueue = new LinkedList<WorldTile>();
            visitedList = new List<WorldTile>();

            //Initialize maze's tiles - every other tile is a background
            for (int x = 0; x < columns * 2; x += 2) {
                for (int y = 0; y < rows * 2; y += 2) {
                    //Background tile
                    generatedMaze[x, y] = new BackgroundTile(new Vector2(x, y));

                    //Each background has three surrounding wall tiles
                    //Right wall
                    generatedMaze[x + 1, y] = new WallTile(new Vector2(x + 1, y));
                    //Bottom wall
                    generatedMaze[x, y + 1] = new WallTile(new Vector2(x, y + 1));
                    //Bottom right corner wall
                    generatedMaze[x + 1, y + 1] = new WallTile(new Vector2(x + 1, y + 1));
                }
            }

            //Add neighbouring background tiles 
            SetAllNeighbours();

            //Set the starting cell to the top left cell
            int randomRow = 0;
            int randomCol = 0;
            currentCell = generatedMaze[randomCol, randomRow];
            visitedList.Add(currentCell);
            previousQueue.AddLast(currentCell);
        }

        /// <summary>
        /// Add all neighbouring background tiles (separated by wall) to each background tile
        /// </summary>
        public void SetAllNeighbours() {
            for (int x = 0; x < columns * 2; x += 2) {
                for (int y = 0; y < rows * 2; y += 2) {
                    //Add top neighbour
                    if (y >= 2) {
                        BackgroundTile t = (BackgroundTile)generatedMaze[x, y]; 
                        t.AddNeighbour((BackgroundTile)generatedMaze[x, y - 2]);
                    }
                    //Add left neighbour
                    if (x >= 2) {
                        BackgroundTile t = (BackgroundTile)generatedMaze[x, y];
                        t.AddNeighbour((BackgroundTile)generatedMaze[x - 2, y]);
                    }
                    //Add right neighbour
                    if (x <= (columns * 2) - 3) {
                        BackgroundTile t = (BackgroundTile)generatedMaze[x, y];
                        t.AddNeighbour((BackgroundTile)generatedMaze[x + 2, y]);
                    }
                    //Add bottom neighbour
                    if (y <= (rows * 2) - 3) {
                        BackgroundTile t = (BackgroundTile)generatedMaze[x, y];
                        t.AddNeighbour((BackgroundTile)generatedMaze[x, y + 2]);
                    }
                }
            }
        }

        /// <summary>
        /// Generate a maze, which will act as the game world
        /// </summary>
        public void Generate() {
            //Get the next cell from the neighbours of the current
            BackgroundTile next = GetRandUnvisitedNeighbour((BackgroundTile)currentCell);

            if (next != null) {
                BackgroundTile current = (BackgroundTile)currentCell; 
                //Remove the wall between the current and next cell
                Vector2 coords = current.RemoveWall(next);
                //Replace the removed wall with a background tile
                generatedMaze[(int)coords.X, (int)coords.Y] = new BackgroundTile(coords);

                currentCell = next;
                previousQueue.AddLast(currentCell);
                visitedList.Add(currentCell);
            } else {
                //Backtrack
                if (previousQueue.Count != 0) {
                    currentCell = previousQueue.First.Value;
                    previousQueue.RemoveFirst();
                }
            }
        }

        /// <summary>
        /// Get a random unvisited neighbour
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public BackgroundTile GetRandUnvisitedNeighbour(BackgroundTile cell) {
            //Get all the cell's neighbours and put them in the unchecked list
            List<BackgroundTile> uncheckedNeighbours = cell.GetNeighbours();

            while (uncheckedNeighbours.Count > 0) {
                BackgroundTile random = uncheckedNeighbours[new Random().Next(uncheckedNeighbours.Count)];
                uncheckedNeighbours.Remove(random);
                if (!visitedList.Contains(random)) {
                    return random;
                }
            }
            return null;
        }

        /// <summary>
        /// Due to generation, we need to expand the 2d array to add a top and left row and column 
        /// All the tiles in the array must be moved
        /// </summary>
        /// <param name="before"></param>
        private void AddTopAndLeftBorder(WorldTile[,] before) {
            for (var x = 0; x < before.GetLength(0); x++) {
                for (var y = 0; y < before.GetLength(1); y++) {
                    //shift the tile coordinates and position
                    before[x, y].SetCoordinates(new Vector2(x + 1, y + 1)); 
                    before[x, y].SetPosition(before[x, y].GetCoordinates() * Tile.GetWorldTileSize());
                    //Shift collision bodies for wall tiles only
                    if(before[x, y] is WallTile) {
                        before[x, y].GetCollisionBody().SetPosition(before[x, y].GetPosition());
                    }
                    //Set the full maze
                    fullMaze[x + 1, y + 1] = before[x, y]; 
                }
            }
        }

        /// <summary>
        /// Fill the new spots in the maze with wall tiles
        /// </summary>
        public void CreateWalls() {
            //Exapnd the maze
            AddTopAndLeftBorder(generatedMaze);

            //Fill with walls
            for (int x = 0; x < (columns * 2) + 1; x++) {
                for (int y = 0; y < (rows * 2) + 1; y++) {
                    if (x == 0 || y == 0) {
                        fullMaze[x, y] = new WallTile(new Vector2(x, y));
                    }
                }
            } 
        }

        /// <summary>
        /// Generates a random number of rooms with no overlap
        /// </summary>
        public void GenerateRooms() {
            //Generate a random number of rooms
            int roomNum = new Random().Next(MIN_ROOM_NUM, MAX_ROOM_NUM + 1);
            //Console.WriteLine(roomNum); 

            //Initialize isInRoom
            bool[,] isInRoom = new bool[(columns * 2) + 1, (rows * 2) + 1]; 
            for (int x = 0; x < (columns * 2) + 1; x++) {
                for (int y = 0; y < (rows * 2) + 1; y++) {
                    isInRoom[x, y] = false; 
                }
            }

            for (int i = 0; i < roomNum; i++) {
                //Generate a random width and height between 2 and 3 (inclusive)
                int roomW = new Random().Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
                int roomH = new Random().Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE);

                int roomX = new Random().Next(1, (columns * 2) - roomW);
                int roomY = new Random().Next(1, (rows * 2) - roomH);

                //Set isInRoom to true
                for (int x = roomX; x < roomX + roomW; x++) {
                    for (int y = roomY; y < roomY + roomH; y++) {
                        isInRoom[x, y] = true;
                    }
                }

                //Set room tiles to background tiles
                for (int x = roomX; x < roomX + roomW; x++) {
                    for (int y = roomY; y < roomY + roomH; y++) {
                        fullMaze[x, y] = new BackgroundTile(new Vector2(x, y)); 
                    }
                }
            }
        }

        /// <summary>
        /// Return true if the room overlaps with a previous room
        /// </summary>
        /// <param name="isInRoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public bool HasOverlap(bool[,] isInRoom, int x, int y, int w, int h) {
            if (isInRoom[x, y] || isInRoom[x + w, y] || isInRoom[x, y + h] || isInRoom[x + w, y + h]) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Place other components on the map on background tiles
        /// THIS METHOD IS A MESS BUT I DONT FEEL LIKE FIXING IT AND IT WORKS SO HAHA
        /// </summary>
        public void PlaceMapComponents() {
            //Make sure components do not overlap each other
            List<Vector2> componentLocations = new List<Vector2>(); 

            //Generate a random number of food
            int amount = new Random().Next(MIN_FOOD, MAX_FOOD + 1);
            int x = 0;
            int y = 0;
            int count = 0;
            //Place food in random spots
            while (count != amount) {
                //Make sure the components will not generate on filled spaces
                while(componentLocations.Contains(new Vector2(x, y))) {
                    x = new Random().Next(1, (columns * 2) - 1);
                    y = new Random().Next(1, (rows * 2) - 1);
                }
                componentLocations.Add(new Vector2(x, y)); 

                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a background tile
                    if (tile.Equals(attempt) && tile is BackgroundTile) {
                        //Generate random num - determines which type of food will be generated
                        int rand = new Random().Next(1, 5);
                        if(rand == 1) {
                            //Add food to that location
                            fullMaze[x, y] = new Food(new Vector2(x, y), Food.FoodTypes.Sandwich);
                            count++;
                        } else if (rand == 2) {
                            //Add food to that location
                            fullMaze[x, y] = new Food(new Vector2(x, y), Food.FoodTypes.Cake);
                            count++;
                        } else if (rand == 3) {
                            //Add food to that location
                            fullMaze[x, y] = new Food(new Vector2(x, y), Food.FoodTypes.Apple);
                            count++;
                        } else if (rand == 4) {
                            //Add food to that location
                            fullMaze[x, y] = new Food(new Vector2(x, y), Food.FoodTypes.Cherries);
                            count++;
                        }

                    }
                }
            }


            //Generate a random number of silver 
            amount = new Random().Next(MIN_CHANGE, MAX_CHANGE + 1);
            x = 0;
            y = 0;
            count = 0; 
            //Place change in random spots
            while (count != amount) {
                //Make sure the components will not generate on filled spaces
                while (componentLocations.Contains(new Vector2(x, y))) {
                    x = new Random().Next(1, (columns * 2) - 1);
                    y = new Random().Next(1, (rows * 2) - 1);
                }
                componentLocations.Add(new Vector2(x, y));

                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a background tile
                    if (tile.Equals(attempt) && tile is BackgroundTile) {
                        //Add change to that location
                        fullMaze[x, y] = new Change(new Vector2(x, y), Change.ChangeTypes.Silver);
                        count++;
                    }
                }
            }
            //Generate a random number of gold
            amount = new Random().Next(MIN_CHANGE/2, MAX_CHANGE/2);
            x = 0;
            y = 0;
            count = 0;
            //Place change in random spots
            while (count != amount) {
                //Make sure the components will not generate on filled spaces
                while (componentLocations.Contains(new Vector2(x, y))) {
                    x = new Random().Next(1, (columns * 2) - 1);
                    y = new Random().Next(1, (rows * 2) - 1);
                }
                componentLocations.Add(new Vector2(x, y));

                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a background tile
                    if (tile.Equals(attempt) && tile is BackgroundTile) {
                        //Add change to that location
                        fullMaze[x, y] = new Change(new Vector2(x, y), Change.ChangeTypes.Gold);
                        count++;
                    }
                }
            }



            //Generate 4 colored shells
            amount = 4;
            x = 0;
            y = 0;
            count = 0;
            //Place change in random spots
            while (count != amount) {
                //Make sure the components will not generate on filled spaces
                while (componentLocations.Contains(new Vector2(x, y))) {
                    x = new Random().Next(1, (columns * 2) - 1);
                    y = new Random().Next(1, (rows * 2) - 1);
                }
                componentLocations.Add(new Vector2(x, y));

                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a background tile
                    if (tile.Equals(attempt) && tile is BackgroundTile) {
                        //Add shell
                        if(count == 0) {
                            fullMaze[x, y] = new Shell(new Vector2(x, y), Shell.ShellTypes.Orange);
                        } else if (count == 1) {
                            fullMaze[x, y] = new Shell(new Vector2(x, y), Shell.ShellTypes.Blue);
                        } else if (count == 2) {
                            fullMaze[x, y] = new Shell(new Vector2(x, y), Shell.ShellTypes.Pink);
                        } else if (count == 3) {
                            fullMaze[x, y] = new Shell(new Vector2(x, y), Shell.ShellTypes.Purple);
                        }

                        count++;
                    }
                }
            }


            //Generate map
            amount = 1;
            x = 0;
            y = 0;
            count = 0;
            //Place change in random spots
            while (count != amount) {
                //Make sure the components will not generate on filled spaces
                while (componentLocations.Contains(new Vector2(x, y))) {
                    x = new Random().Next(1, (columns * 2) - 1);
                    y = new Random().Next(1, (rows * 2) - 1);
                }
                componentLocations.Add(new Vector2(x, y));

                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a background tile
                    if (tile.Equals(attempt) && tile is BackgroundTile) {
                        //Add change to that location
                        fullMaze[x, y] = new TreasureMap(new Vector2(x, y));
                        count++;
                    }
                }
            }

            //Generate the tile that the map leads to
            amount = 1;
            x = 0;
            y = 0;
            count = 0;
            //Place change in random spots
            while (count != amount) {
                //Make sure the components will not generate on filled spaces
                //while (componentLocations.Contains(new Vector2(x, y))) {
                x = new Random().Next(1, (columns * 2) - 1);
                y = new Random().Next(1, (rows * 2) - 1);
                //}
                //componentLocations.Add(new Vector2(x, y));

                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a background tile
                    if (tile.Equals(attempt) && tile is BackgroundTile) {
                        //Add change to that location
                        fullMaze[x, y] = new MapLocationTile(new Vector2(x, y));
                        count++;
                    }
                }
            }

            //1 in 1000 chance of getting the golden crab
            int rand1 = new Random().Next(0, 1000);
            if(rand1 == 0) {
                Console.WriteLine("GOLDEN CRAB!");
                //Small chance of generating the golden crab
                amount = 1;
                x = 0;
                y = 0;
                count = 0;
                //Place change in random spots
                while (count != amount) {
                    //Make sure the components will not generate on filled spaces
                    while (componentLocations.Contains(new Vector2(x, y))) {
                        x = new Random().Next(1, (columns * 2) - 1);
                        y = new Random().Next(1, (rows * 2) - 1);
                    }
                    componentLocations.Add(new Vector2(x, y));

                    BackgroundTile attempt = new BackgroundTile(new Vector2(x, y));
                    //Loop through maze
                    foreach (Tile tile in fullMaze) {
                        //Check if coords match and tile is a background tile
                        if (tile.Equals(attempt) && tile is BackgroundTile) {

                            //Add change to that location
                            fullMaze[x, y] = new GoldenCrab(new Vector2(x, y));
                            count++;
                        }
                    }
                }
            }

            //Generate the timer tile
            fullMaze[0, 0] = new TimerTile(new Vector2(0, 0));

            //Generate the empty empty and full shell boards, placed on a wall tile!!!
            amount = 2;
            x = 0;
            y = 0;
            count = 0;
            //TODO: Need to make sure boards wont overlap or be placed near each other!!!!!!!!!
            Vector2 firstSet = new Vector2(-1, -1); 
            //Place change in random spots
            while (count != amount) {
                //Generate a spot for the tiles
                /*if(firstSet.X != -1) {
                    while (Math.Abs(firstSet.X - x) > 5 && Math.Abs(firstSet.Y - y) > 5) {
                        x = new Random().Next(1, (columns * 2) - 1);
                        y = new Random().Next(1, (rows * 2) - 1);
                    }
                } else {*/
                    x = new Random().Next(1, (columns * 2) - 1);
                    y = new Random().Next(1, (rows * 2) - 1);
                //}
                
                //Store the first set of coords generated
                if(firstSet.X == -1) {
                    firstSet = new Vector2(x, y); 
                }

                WallTile attempt = new WallTile(new Vector2(x, y));
                //Loop through maze
                foreach (Tile tile in fullMaze) {
                    //Check if coords match and tile is a wall tile
                    if (tile.Equals(attempt) && tile is WallTile) {
                        //Add change to that location
                        if(count == 0) {
                            fullMaze[x, y] = new ShellBoard(new Vector2(x, y), ShellBoard.BoardTypes.empty);
                        } else {
                            fullMaze[x, y] = new ShellBoard(new Vector2(x, y), ShellBoard.BoardTypes.reference);
                        }
                        count++;
                    }
                }
            }
        }

        public Tile[,] GetFullMaze() {
            return fullMaze; 
        }

        public int GetColumns() {
            return columns;
        }

        public int GetRows() {
            return rows;
        }

        public List<WorldTile> GetVisitedList() {
            return visitedList;
        }
    }
}
