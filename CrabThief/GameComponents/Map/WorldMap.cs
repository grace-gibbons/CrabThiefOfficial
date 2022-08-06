using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.Util;
using CrabThief.GameComponents.Map.Tiles.Collectibles;
using CrabThief.GameComponents.Map.Tiles.WorldTiles;
using CrabThief.GameComponents.Map.Tiles;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.GUI;
using CrabThief.GameComponents.Audio;

//Store locations and tiles across the game world, display game world
namespace CrabThief.GameComponents.Map {
    class WorldMap {

        //Lists of tiles
        private List<BackgroundTile> backgroundTiles;
        private List<WallTile> wallTiles;
        private List<Food> foodTiles;
        private List<Change> changeTiles;
        private GoldenCrab goldenCrab;
        private TreasureMap map;
        private MapLocationTile mapLocationTile;
        private List<Shell> shells;
        private List<ShellBoard> shellBoards; 
        private List<Enemy> enemies;
        private TimerTile timerTile; 


        //Width and height in tiles
        private static int mapWidth;
        private static int mapHeight;

        //The number of gold coins collected
        private int numGold;
        //The number of silver collected
        private int numSilver; 

        //The number of total food points
        private int foodPoints; 

        //tracks the exact loop where the shell board is complete so the player is only awarded once
        private bool completeShellBoardFlag = false;

        //Static initializers
        public void SetMapWidth(int width) {
            mapWidth = width;
        }

        public void SetMapHeight(int height) {
            mapHeight = height;
        }

        /// <summary>
        /// Create world map
        /// </summary>
        public WorldMap() {
            backgroundTiles = new List<BackgroundTile>();
            wallTiles = new List<WallTile>();
            foodTiles = new List<Food>(); 
            changeTiles = new List<Change>();
            enemies = new List<Enemy>();
            shells = new List<Shell>();
            shellBoards = new List<ShellBoard>();

            foodPoints = 0;
        }

        /// <summary>
        /// Load the maze and put all components into the proper lists
        /// </summary>
        /// <param name="maze"></param>
        public void ParseMaze(Tile[,] maze) {
            //Loop through whole maze
            for (int x = 0; x < (mapWidth * 2) + 1; x++) {
                for (int y = 0; y < (mapHeight * 2) + 1; y++) {

                    //Create a new tile at maze coords
                    Tile tile = maze[x, y];

                    //Add tiles to proper lists
                    if (tile is WallTile) {
                        wallTiles.Add((WallTile)tile);
                    } else if (tile is BackgroundTile) {
                        backgroundTiles.Add((BackgroundTile)tile);
                    } else if (tile is Food) {
                        backgroundTiles.Add(new BackgroundTile(new Vector2(x, y)));
                        foodTiles.Add((Food)tile);
                    } else if (tile is Change) {
                        backgroundTiles.Add(new BackgroundTile(new Vector2(x, y)));
                        changeTiles.Add((Change)tile);
                    } else if (tile is GoldenCrab) {
                        backgroundTiles.Add(new BackgroundTile(new Vector2(x, y)));
                        goldenCrab = (GoldenCrab)tile;
                    } else if (tile is TreasureMap) {
                        backgroundTiles.Add(new BackgroundTile(new Vector2(x, y)));
                        map = (TreasureMap)tile;
                    } else if (tile is MapLocationTile) {
                        backgroundTiles.Add(new BackgroundTile(new Vector2(x, y)));
                        mapLocationTile = (MapLocationTile)tile;
                    } else if (tile is Shell) {
                        backgroundTiles.Add(new BackgroundTile(new Vector2(x, y)));
                        shells.Add((Shell)tile);
                    } else if (tile is ShellBoard) {
                        wallTiles.Add(new WallTile(new Vector2(x, y)));
                        shellBoards.Add((ShellBoard)tile); 
                    } else if (tile is TimerTile) {
                        wallTiles.Add(new WallTile(new Vector2(x, y)));
                        timerTile = (TimerTile)tile;
                    }
                }
            }
            //Connect background tiles
            ConnectTiles();
        }

        /// <summary>
        /// Add background tiles to each other's connected list
        /// </summary>
        /// <param name="maze"></param>
        public void ConnectTiles() {
            //For each background tile
            foreach(BackgroundTile currentTile in backgroundTiles) {
                //For each possibly connected background tile
                foreach (BackgroundTile possibleConnected in backgroundTiles) {
                    
                    //Connect to right neighbour
                    if (currentTile.GetCoordinates().X + 1 == possibleConnected.GetCoordinates().X && currentTile.GetCoordinates().Y == possibleConnected.GetCoordinates().Y) {
                        currentTile.AddConnected(possibleConnected);
                    }
                    //Connect to left neighbour
                    if (currentTile.GetCoordinates().X - 1 == possibleConnected.GetCoordinates().X && currentTile.GetCoordinates().Y == possibleConnected.GetCoordinates().Y) {
                        currentTile.AddConnected(possibleConnected);
                    }
                    //Connect to bottom neighbour
                    if (currentTile.GetCoordinates().Y + 1 == possibleConnected.GetCoordinates().Y && currentTile.GetCoordinates().X == possibleConnected.GetCoordinates().X) {
                        currentTile.AddConnected(possibleConnected);
                    }
                    //Connect to top neighbour
                    if (currentTile.GetCoordinates().Y - 1 == possibleConnected.GetCoordinates().Y && currentTile.GetCoordinates().X == possibleConnected.GetCoordinates().X) {
                        currentTile.AddConnected(possibleConnected);
                    }
                }
            }
        }

        /// <summary>
        /// Generate and place enemy
        /// </summary>
        public void GenerateEnemies() {
            //Amount of enemys to generate
            int amount = 1; 
            int x = 0;
            int y = 0;
            //Generate enemy until it has been added
            while(enemies.Count != amount) {
                //Generate random location
                x = new Random().Next(1, (mapWidth * 2) - 1);
                y = new Random().Next(1, (mapHeight * 2) - 1);

                //Create background tile at that location
                BackgroundTile attempt = new BackgroundTile(new Vector2(x, y)); 

                //Loop through background tiles and make sure new attempted tile is there, if not, while loop continues
                foreach(BackgroundTile tile in backgroundTiles) {
                    if(tile.Equals(attempt)) {
                        Enemy enemy = new Enemy(new Vector2(x, y));
                        enemies.Add(enemy);
                    }
                }
            }
        }

        /// <summary>
        /// Update the maze state and its components
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="camera"></param>
        /// <param name="overlayHandler"></param>
        public void Update(GameTime gameTime, Player player, CollisionEngine collisionEngine, GameMouse mouse, GameCamera camera, OverlayHandler overlayHandler) {
            //Update enemy
            foreach(Enemy e in enemies) {

                //Update enemy state(motion, position)
                e.Update(gameTime, player, this);

                //Enemy-wall collision
                collisionEngine.Update(e, GetWallTiles());

                //Player-enemy collision
                collisionEngine.ResolvePlayerEnemyCollision(player, e);
            }

            //Update shell boards
            foreach(ShellBoard s in shellBoards) {
                s.Update(collisionEngine, mouse, camera, player, overlayHandler, this); 
            }

            //Update shell board when completed
            if(shellBoards[0].Equals(shellBoards[1]) && !completeShellBoardFlag) {
                completeShellBoardFlag = true;
                //Reward player
                numSilver += 5;
                numGold += 1;
                foodPoints += 3; 
                //Play board complete audio
                AudioHandler.PlaySound(shellBoards[0].GetCompletedBoardAudio()); 
            }

            //Update timer tile
            timerTile.Update(collisionEngine, player, mouse, camera, overlayHandler, this, gameTime); 
        }

        /// <summary>
        /// Load tile images and other world component images
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content) {
            foreach(WorldTile tile in backgroundTiles) {
                tile.LoadContent(content);
            }
            foreach (WorldTile tile in wallTiles) {
                tile.LoadContent(content);
            }
            foreach (Tile tile in foodTiles) {
                tile.LoadContent(content);
            }
            foreach (Tile tile in changeTiles) {
                tile.LoadContent(content);
            }
            foreach (Enemy enemy in enemies) {
                enemy.LoadContent(content);
            }
            if(goldenCrab != null) {
                goldenCrab.LoadContent(content);
            }
            if (map != null) {
                map.LoadContent(content);
            }
            if(mapLocationTile != null) {
                mapLocationTile.LoadContent(content); 
            }
            foreach(Shell shell in shells) {
                shell.LoadContent(content); 
            }
            foreach(ShellBoard board in shellBoards) {
                board.LoadContent(content);
            }
            timerTile.LoadContent(content); 
        }

        /// <summary>
        /// Draw the game world
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera"></param>
        public void Draw(SpriteBatch spriteBatch, GameCamera camera) {
            //Background
            for (int i = 0; i < backgroundTiles.Count; i++) {
                if (backgroundTiles[i].IsInViewport(camera)) {
                    backgroundTiles[i].Draw(spriteBatch);
                }
            }

            //Walls
            for (int i = 0; i < wallTiles.Count; i++) {
                if(wallTiles[i].IsInViewport(camera)) {
                    wallTiles[i].Draw(spriteBatch);
                }
            }

            //Tile that map leads to
            if (mapLocationTile != null) {
                mapLocationTile.Draw(spriteBatch);
            }

            //Food
            for (int i = 0; i < foodTiles.Count; i++) {
                if (foodTiles[i].IsInViewport(camera)) {
                    foodTiles[i].Draw(spriteBatch);
                }
            }

            //Change
            for (int i = 0; i < changeTiles.Count; i++) {
                if (changeTiles[i].IsInViewport(camera)) {
                    changeTiles[i].Draw(spriteBatch);
                }
            }

            //Enemy
            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].Draw(spriteBatch);
            }

            //Golden Crab
            if(goldenCrab != null) {
                goldenCrab.Draw(spriteBatch);
            }

            //Treasure map
            if (map != null) {
                map.Draw(spriteBatch);
            }

            //Shell Boards
            for (int i = 0; i < shellBoards.Count; i++) {
                if (shellBoards[i].IsInViewport(camera)) {
                    shellBoards[i].Draw(spriteBatch);
                }
            }

            //Shells
            for (int i = 0; i < shells.Count; i++) {
                if (shells[i].IsInViewport(camera)) {
                    shells[i].Draw(spriteBatch);
                }
            }

            //timer tile
            timerTile.Draw(spriteBatch); 
        }

        /// <summary>
        /// Get a specific background tile from its coordinates
        /// </summary>
        /// <param name="coords"></param>
        /// <returns> Return background tile from its coords </returns>
        public BackgroundTile GetBackgroundByCoords(Vector2 coords) {
            foreach (BackgroundTile tile in backgroundTiles) {
                if(tile.GetCoordinates() == coords) {
                    return tile; 
                }
            }
            return null;
        }

        public List<BackgroundTile> GetBackgroundTiles() {
            return backgroundTiles; 
        }

        public List<WallTile> GetWallTiles() {
            return wallTiles; 
        }

        public List<Food> GetFoodTiles() {
            return foodTiles;
        }

        public List<Change> GetChangeTiles() {
            return changeTiles;
        }

        public List<Enemy> GetEnemyTiles() {
            return enemies;
        }

        public TreasureMap GetTreasureMap() {
            return map; 
        }

        public MapLocationTile GetMapLocationTile() {
            return mapLocationTile; 
        }

        public List<Shell> GetShells() {
            return shells; 
        }

        //Return the instance of the orange shell
        public Shell GetOrangeShell() {
            foreach (Shell shell in shells) {
                if (shell.GetShellType() == Shell.ShellTypes.Orange) {
                    return shell;
                }
            }
            return null;
        }

        //Return the instance of the blue shell
        public Shell GetBlueShell() {
            foreach (Shell shell in shells) {
                if (shell.GetShellType() == Shell.ShellTypes.Blue) {
                    return shell;
                }
            }
            return null;
        }

        //Return the instance of the pink shell
        public Shell GetPinkShell() {
            foreach (Shell shell in shells) {
                if (shell.GetShellType() == Shell.ShellTypes.Pink) {
                    return shell;
                }
            }
            return null;
        }

        //Return the instance of the purple shell
        public Shell GetPurpleShell() {
            foreach (Shell shell in shells) {
                if (shell.GetShellType() == Shell.ShellTypes.Purple) {
                    return shell;
                }
            }
            return null;
        }

        public int GetNumSilver() {
            return numSilver; 
        }

        public void SetNumSilver(int numSilver) {
            this.numSilver = numSilver; 
        }

        public int GetNumGold() {
            return numGold;
        }

        public void SetNumGold(int numGold) {
            this.numGold = numGold;
        }

        public int GetFoodPoints() {
            return foodPoints;
        }

        public void SetFoodPoints(int foodPoints) {
            this.foodPoints = foodPoints;
        }

        public TimerTile GetTimerTile() {
            return timerTile; 
        }
    }
}
