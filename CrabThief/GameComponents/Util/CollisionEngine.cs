using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using CrabThief.GameComponents.Audio;
using CrabThief.GameComponents.Entities;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Map.Tiles.Collectibles;

//Detect and resolve various collisions between different components
namespace CrabThief.GameComponents.Util {
    class CollisionEngine {

        /// <summary>
        /// Call collision handling methods for an entity (player)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wallTiles"></param>
        /// <param name="foodTiles"></param>
        /// <param name="changeTiles"></param>
        /// <param name="map"></param>
        /// <param name="shells"></param>
        /// <param name="worldMap"></param>
        public void Update(Entity entity, List<WallTile> wallTiles, List<Food> foodTiles, List<Change> changeTiles, TreasureMap map, List<Shell> shells, WorldMap worldMap) {
            //Resolve tile collisions
            for (int i = 0; i < wallTiles.Count; i++) {
                ResolveEntityTileCollision(entity, wallTiles[i].GetCollisionBody());
            }

            //Resolve food collisions
            for (int i = 0; i < foodTiles.Count; i++) {
                ResolvePlayerFoodCollision(entity, foodTiles[i], worldMap);
            }

            //Resolve change collisions
            for (int i = 0; i < changeTiles.Count; i++) {
                ResolvePlayerChangeCollision(entity, changeTiles[i], worldMap);
            }

            //Resolve map collision
            ResolvePlayerTreasureMapCollision(entity, map);

            //Resolve shell collisions
            for (int i = 0; i < shells.Count; i++) {
                ResolvePlayerShellCollision(entity, shells[i]);
            }
        }

        /// <summary>
        /// Call collision handling methods for an entity (enemy)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wallTiles"></param>
        public void Update(Entity entity, List<WallTile> wallTiles) {
            //Resolve tile collisions
            for (int i = 0; i < wallTiles.Count; i++) {
                ResolveEntityTileCollision(entity, wallTiles[i].GetCollisionBody());
            }
        }

        /// <summary>
        /// Detect and resolve a collision between entity and collision tile
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cb"></param>
        public void ResolveEntityTileCollision(Entity entity, CollisionBody cb) {
            //Position and size of the player and collision body
            Vector2 playerPosition = entity.GetPosition();
            Vector2 playerSize = entity.GetSize();
            Vector2 cbPosition = cb.GetPosition();
            Vector2 cbSize = cb.GetSize();

            if (IsCollision(entity.GetCollisionBody(), cb)) {
                //If going right
                if (playerPosition.X + playerSize.X > cbPosition.X && entity.GetPreviousPosition().X + playerSize.X <= cbPosition.X) {
                    entity.SetPosition(new Vector2(cbPosition.X - playerSize.X - 0.1f, playerPosition.Y));
                } 

                //If going left
                if (playerPosition.X < cbPosition.X + cbSize.X && entity.GetPreviousPosition().X >= cbPosition.X + cbSize.X) {
                    entity.SetPosition(new Vector2(cbPosition.X + cbSize.X + 0.1f, playerPosition.Y));
                }

                //If going down
                if (entity.GetPosition().Y + entity.GetSize().Y > cb.GetPosition().Y && entity.GetPreviousPosition().Y + entity.GetSize().Y <= cb.GetPosition().Y) {
                    entity.SetPosition(new Vector2(playerPosition.X, cbPosition.Y - playerSize.Y - 0.1f));
                }

                //If going up
                if (playerPosition.Y < cbPosition.Y + cbSize.Y && entity.GetPreviousPosition().Y >= cbPosition.Y + cbSize.Y) {
                    entity.SetPosition(new Vector2(playerPosition.X, cbPosition.Y + cbSize.Y + 0.1f));
                }
            }
        }

        /// <summary>
        /// Detect and resolve a collision between the player and food
        /// </summary>
        /// <param name="player"></param>
        /// <param name="food"></param>
        /// <param name="worldMap"></param>
        public void ResolvePlayerFoodCollision(Entity player, Food food, WorldMap worldMap) {
            if(IsCollision(player.GetCollisionBody(), food.GetCollisionBody())) {
                food.Collect();
                //Add proper amount of food points
                if(food.GetFoodType() == Food.FoodTypes.Cake || food.GetFoodType() == Food.FoodTypes.Sandwich) {
                    worldMap.SetFoodPoints(worldMap.GetFoodPoints() + 2);
                } else {
                    worldMap.SetFoodPoints(worldMap.GetFoodPoints() + 1);
                }

                //Player sound here
                AudioHandler.PlaySound(food.GetCollectedAudio());
            }
        }

        /// <summary>
        /// Detect and resolve a collision between the player and change
        /// </summary>
        /// <param name="player"></param>
        /// <param name="change"></param>
        /// <param name="worldMap"></param>
        public void ResolvePlayerChangeCollision(Entity player, Change change, WorldMap worldMap) {
            if (IsCollision(player.GetCollisionBody(), change.GetCollisionBody())) {
                change.Collect();
                //Add coins
                if(change.GetChangeType() == Change.ChangeTypes.Silver) {
                    worldMap.SetNumSilver(worldMap.GetNumSilver() + 1); 
                } else {
                    worldMap.SetNumGold(worldMap.GetNumGold() + 1);
                }

                //Play sound here
                AudioHandler.PlaySound(change.GetCollectedAudio());
            }
        }

        /// <summary>
        /// Detect and resolve collision between player and map
        /// </summary>
        /// <param name="player"></param>
        /// <param name="map"></param>
        public void ResolvePlayerTreasureMapCollision(Entity player, TreasureMap map) {
            if(IsCollision(player.GetCollisionBody(), map.GetCollisionBody())) {
                map.Collect();

                //Play sound here
                AudioHandler.PlaySound(map.GetCollectedAudio());
            }
        }

        /// <summary>
        /// Detect and resolve collision between player and shell
        /// </summary>
        /// <param name="player"></param>
        /// <param name="shell"></param>
        public void ResolvePlayerShellCollision(Entity player, Shell shell) {
            if (IsCollision(player.GetCollisionBody(), shell.GetCollisionBody())) {
                shell.Collect();

                //Play some sound here
                AudioHandler.PlaySound(shell.GetCollectedAudio());
            }
        }

        /// <summary>
        /// Detect and resolve collision between player and enemy
        /// Uses hitboxes, which give a larger buffer than collision bodies
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        public void ResolvePlayerEnemyCollision(Player player, Enemy enemy) {
            //If there is a collision
            if(IsCollision(player.GetHitbox(), enemy.GetHitbox())) {
                //Resolve player attack is they have (timer and states not handled here, enemy damage is tho) - this attack is a one loop subtraction thing
                if (player.GetIsAttacking()) {
                    //enemy.setIsStunned - will stop them moving, set that state which will trigger the animation
                    enemy.SetIsStunned(true);
                    //TODO Player some eney taking damage sound 
                    AudioHandler.PlaySound(player.GetTakeDamageAudio()); 
                }

                //Enemy attack
                if (enemy.GetIsAttacking()) { 
                    player.DecrementHealth();
                    enemy.SetIsAttacking(false);
                    //TODO Play some player taking damge sound
                    AudioHandler.PlaySound(enemy.GetTakeDamageAudio());
                }
            }
        }

        /// <summary>
        /// Return true if there is a collision and false otherwise
        /// </summary>
        /// <param name="cb1"></param>
        /// <param name="cb2"></param>
        /// <returns></returns>
        public bool IsCollision(CollisionBody cb1, CollisionBody cb2) {
            //No collision if one or both bodies are disbaled
            if (!cb1.GetIsEnabled() || !cb2.GetIsEnabled()) {
                return false; 
            }
            if (cb1.GetPosition().X + cb1.GetSize().X < cb2.GetPosition().X) {
                return false;
            }
            if (cb1.GetPosition().X > cb2.GetPosition().X + cb2.GetSize().X) {
                return false;
            }
            if (cb1.GetPosition().Y + cb1.GetSize().Y < cb2.GetPosition().Y) {
                return false;
            }
            if (cb1.GetPosition().Y > cb2.GetPosition().Y + cb2.GetSize().Y) {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Given two coords (tile locations) find the number of tiles between them
        /// </summary>
        /// <param name="coords1"></param>
        /// <param name="coords2"></param>
        /// <returns></returns>
        public double GetDistance(Vector2 coords1, Vector2 coords2) {
            //Get the x and y distance between the coords, forming two sides of the triangle
            float xDist = Math.Abs(coords1.X - coords2.X);
            float yDist = Math.Abs(coords1.Y - coords2.Y);

            //Return the distance between the two points
            return Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
        }


        /*
         * Mouse collisions
         */
        /// <summary>
        /// Return true if mouse collides with cb
        /// Useful for screen coordinates (coords that do not change based on player position, Ex: menus and menu buttons)
        /// </summary>
        /// <param name="mouse"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        public bool IsMouseCollision(GameMouse mouse, CollisionBody cb) {
            if (mouse.GetPosition().X < cb.GetPosition().X) {
                return false;
            }
            if (mouse.GetPosition().X > cb.GetPosition().X + cb.GetSize().X) {
                return false;
            }
            if (mouse.GetPosition().Y < cb.GetPosition().Y) {
                return false;
            }
            if (mouse.GetPosition().Y > cb.GetPosition().Y + cb.GetSize().Y) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return true if mouse collides with cb
        /// Converts to world coordinates, for coords that change based on player position
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="mouse"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        public bool IsMouseCollision(GameCamera camera, GameMouse mouse, CollisionBody cb) {
            if (mouse.GetPositionUnchanged().X < camera.WorldToScreen(cb.GetPosition()).X) {
                return false;
            }
            if (mouse.GetPositionUnchanged().X > camera.WorldToScreen(cb.GetPosition() + cb.GetSize()).X ) {
                return false;
            }
            if (mouse.GetPositionUnchanged().Y < camera.WorldToScreen(cb.GetPosition()).Y) {
                return false;
            }
            if (mouse.GetPositionUnchanged().Y > camera.WorldToScreen(cb.GetPosition() + cb.GetSize()).Y) {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Pre: There is a collision occurring with the given collision body
        /// Given that a collision with a collision body and mouse is occurring, return whether that collision is in the top left, bottom left, top right, or bottom right
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="mouse"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        public int GetAreaCollision(GameCamera camera, GameMouse mouse, CollisionBody cb) {
            if (mouse.GetPositionUnchanged().X <= camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).X && mouse.GetPositionUnchanged().Y < camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).Y) {
                return 0;
            }
            if (mouse.GetPositionUnchanged().X > camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).X && mouse.GetPositionUnchanged().Y <= camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).Y) {
                return 1;
            }
            if (mouse.GetPositionUnchanged().X <= camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).X && mouse.GetPositionUnchanged().Y > camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).Y) {
                return 2;
            }
            if (mouse.GetPositionUnchanged().X > camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).X && mouse.GetPositionUnchanged().Y >= camera.WorldToScreen(cb.GetPosition() + (cb.GetSize() / 2)).Y) {
                return 3;
            }
            return -1;
        }
    }
}
