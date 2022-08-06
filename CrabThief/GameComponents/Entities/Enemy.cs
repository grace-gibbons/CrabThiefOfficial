using System.Collections.Generic;
using System.Linq;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrabThief.GameComponents.Entities {
    class Enemy : Entity {

        //Enemy texture
        private Texture2D texture;

        //Enemy position and previous position
        private Vector2 position;
        private Vector2 previousPosition;

        //X and Y grid positions, in relation to other tiles
        private Vector2 coordinates;

        //Size of enemy image
        private static readonly Vector2 size = new Vector2(16, 16);

        //When enemy is stunned they cannot move or attack and a special animation will play
        private bool isStunned;
        private bool isAttacking; 

        //Time enemy is stunned for, stun cooldown (perhaps this should be a bit more variable, maybe more random??)
        private static readonly float stunCooldown = 1.6f;
        //Tracks time enemy has been waiting for cooldown
        private static float stunTimer = 0f;

        //Time player must wait to attack again, attack cooldown
        private static readonly float attackCooldown = 0.6f;
        //Tracks time player has been waiting for cooldown
        private static float attackTimer = 0f;

        private CollisionBody collisionBody;
        private CollisionBody hitbox;
        private static readonly Vector2 hitboxSize = new Vector2(16, 16); //8 pixel buffer on each side of enemy

        //Path finder so enemy can navigate maze
        private PathFinder pathFinder;

        //Take damage sound
        private SoundEffect takeDamageAudio;

        /// <summary>
        /// Create enemy
        /// </summary>
        /// <param name="coordinates"> Starting coordinates </param>
        public Enemy(Vector2 coordinates) {
            this.coordinates = coordinates; 
            position = coordinates * Tile.GetWorldTileSize();
            //Center components if they are smaller then the typical tile
            if (size.X < Tile.GetWorldTileSize().X || size.Y < Tile.GetWorldTileSize().Y) {
                position += (Tile.GetWorldTileSize() / 2) - (size / 2);
            }

            previousPosition = position;
            isStunned = false;
            isAttacking = false; 
            collisionBody = new CollisionBody(position, size);
            hitbox = new CollisionBody(position - (hitboxSize / 2), size + hitboxSize);

            pathFinder = new PathFinder(); 
        }


        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"> Content Manager </param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>("Assets/Textures/Entities/Enemy/enemyCrab");
            takeDamageAudio = content.Load<SoundEffect>("Assets/Sounds/damaged");
        }

        /// <summary>
        /// Update the enemy state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"> Player </param>
        /// <param name="map"> Current world map </param>
        public void Update(GameTime gameTime, Player player, WorldMap map) {
            //Set previous position
            previousPosition = position;

            //Follow player
            FollowPlayer(player, map);

            //Stun cooldown
            HandleStunCooldown(gameTime);

            //Attack
            UpdateAttackState(); 

            //Attack colldown
            HandleAttackCooldown(gameTime); 
            
            //Move collision body and hitbox to new position
            collisionBody.SetPosition(position);
            hitbox.SetPosition(position - (hitboxSize / 2));
        }

        /// <summary>
        /// USe the paht finder to generate the quickest path to the player
        /// </summary>
        /// <param name="player"> The player to find </param>
        /// <param name="map"> The world to navigate </param>
        public void FollowPlayer(Player player, WorldMap map) {
            //Get tile that corresponds to enemy and player coords
            BackgroundTile enemyTile = map.GetBackgroundByCoords(coordinates);
            BackgroundTile playerTile = map.GetBackgroundByCoords(player.GetCoordinates());
           
            //Calculate shortest path to player (pathfinder)
            LinkedList<BackgroundTile> path = pathFinder.shortestPath(enemyTile, playerTile);

            //Move in proper direction, if there is a path to follow and enemy is not stunned
            if (path.Count > 1 && !isStunned) {
                //If the first path tile is to the right of the enemy
                if (path.ElementAt(1).GetPosition().X + (path.ElementAt(1).GetSize().X / 2) > position.X && (path.ElementAt(1).GetPosition().Y >= position.Y || path.ElementAt(1).GetPosition().Y <= position.Y + (size.Y/2))) {
                    position.X += 1;
                    //Console.WriteLine("go right");
                } else if (path.ElementAt(1).GetPosition().X + (path.ElementAt(1).GetSize().X / 2) < position.X && (path.ElementAt(1).GetPosition().Y >= position.Y || path.ElementAt(1).GetPosition().Y <= position.Y + (size.Y / 2))) {
                    position.X -= 1;
                    //Console.WriteLine("go left");
                }

                if (path.ElementAt(1).GetPosition().Y + (path.ElementAt(1).GetSize().Y / 2) > position.Y && (path.ElementAt(1).GetPosition().X >= position.X || path.ElementAt(1).GetPosition().X <= position.X + (size.X / 2))) {
                    position.Y += 1;
                    //Console.WriteLine("go down");
                } else if (path.ElementAt(1).GetPosition().Y + (path.ElementAt(1).GetSize().Y / 2) < position.Y && (path.ElementAt(1).GetPosition().X >= position.X || path.ElementAt(1).GetPosition().X <= position.X + (size.X / 2))) {
                    position.Y -= 1;
                    //Console.WriteLine("go up");
                }
            }

            //Transform my position to coords, update my coords
            coordinates = PositionToCoords(); 
        }

        /// <summary>
        /// Handles time enemy is stunned
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleStunCooldown(GameTime gameTime) {
            //Increment timer if attacking or timer has already started
            if (isStunned || stunTimer > 0) {
                stunTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Reset the timer if max time is met
                if (stunTimer > stunCooldown) {
                    stunTimer = 0;
                    isStunned = false;
                }
            } else {
                stunTimer = 0;
                isStunned = false;
            }
        }

        /// <summary>
        /// Set isAttacking state
        /// </summary>
        public void UpdateAttackState() {
            //Can only attack when cooldown period is over and is not stunned
            if (attackTimer == 0 && !isStunned) {
                isAttacking = true;
            } else {
                isAttacking = false;
            }
        }

        /// <summary>
        /// Handles time between enemy attacks
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleAttackCooldown(GameTime gameTime) {
            //Increment timer if attacking or timer has already started
            if (isAttacking || attackTimer > 0) {
                attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Reset the timer if max time is met
                if (attackTimer > attackCooldown) {
                    attackTimer = 0;
                    isAttacking = false;
                }
            } else {
                attackTimer = 0;
                isAttacking = false;
            }
        }

        /// <summary>
        /// Draw enemy
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }

        /// <summary>
        /// Transform tile coords to world position
        /// </summary>
        /// <param name="coords"> Coordinates to transform </param>
        /// <returns> The position </returns>
        public Vector2 CoordsToPosition(Vector2 coords) {
            return coords * Tile.GetWorldTileSize(); 
        }

        /// <summary>
        /// Transform position to tile coords
        /// </summary>
        /// <returns> The coordinates </returns>
        public Vector2 PositionToCoords() {
            return new Vector2((int)((position.X + (size.X/2)) / Tile.GetWorldTileSize().X), (int)((position.Y + (size.Y/2)) / Tile.GetWorldTileSize().Y));
        }

        public Vector2 GetPosition() {
            return position;
        }

        public Vector2 GetPreviousPosition() {
            return previousPosition;
        }

        public Vector2 GetSize() {
            return size;
        }

        public CollisionBody GetCollisionBody() {
            return collisionBody; 
        }

        public void SetPosition(Vector2 position) {
            this.position = position;
        }

        public Vector2 GetCoordinates() {
            return coordinates;
        }

        public void SetCoordinates(Vector2 coords) {
            coordinates = coords;
        }

        public CollisionBody GetHitbox() {
            return hitbox;
        }

        public void SetIsStunned(bool isStunned) {
            this.isStunned = isStunned; 
        }

        public float GetStunTimer() {
            return stunTimer; 
        }

        public bool GetIsStunned() {
            return isStunned; 
        }

        public bool GetIsAttacking() {
            return isAttacking;
        }

        public void SetIsAttacking(bool isAttacking) {
            this.isAttacking = isAttacking;
        }

        public float GetAttackTimer() {
            return attackTimer; 
        }

        public SoundEffect GetTakeDamageAudio() {
            return takeDamageAudio;
        }
    }
}
