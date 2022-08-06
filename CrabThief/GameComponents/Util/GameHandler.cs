using Microsoft.Xna.Framework;

using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.GUI;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;

//Handles relation between generating world and creating world map
//Handles updating the current world (by updating world map)
namespace CrabThief.GameComponents.Util {
    class GameHandler {

        //Map generator for initial map genearation
        private MapGenerator mapGenerator;

        //World map - actual game play map
        private WorldMap worldMap; 

        /// <summary>
        /// Create game handler
        /// </summary>
        /// <param name="mapGenerator"></param>
        /// <param name="worldMap"></param>
        public GameHandler(MapGenerator mapGenerator, WorldMap worldMap) {
            this.mapGenerator = mapGenerator;
            this.worldMap = worldMap;
            worldMap.SetMapWidth(mapGenerator.GetColumns()); 
            worldMap.SetMapHeight(mapGenerator.GetRows()); 
        }

        /// <summary>
        /// Create a world by generating map and converting it to a full world
        /// </summary>
        public void CreateWorld() {
            
            //Generate the maze map
            while(mapGenerator.GetVisitedList().Count != ((mapGenerator.GetRows() * 2) * (mapGenerator.GetColumns() * 2)) / 4) {
                mapGenerator.Generate();
            }
            //Fill in new border with walls
            mapGenerator.CreateWalls(); 

            //Generate features
            mapGenerator.GenerateRooms();
            mapGenerator.PlaceMapComponents(); 


            //Parse the world data 
            worldMap.ParseMaze(mapGenerator.GetFullMaze());
            worldMap.GenerateEnemies();
        }

        /// <summary>
        /// Update world state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <param name="camera"></param>
        /// <param name="overlayHandler"></param>
        public void Update(GameTime gameTime, Player player, CollisionEngine collisionEngine, GameMouse mouse, GameCamera camera, OverlayHandler overlayHandler) {
            //Update the world state
            worldMap.Update(gameTime, player, collisionEngine, mouse, camera, overlayHandler);
        }
    }
}
