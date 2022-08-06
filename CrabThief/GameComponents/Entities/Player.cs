using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrabThief.GameComponents.Entities {
    class Player : Entity {

        //Player texture
        private Texture2D texture;

        //Physics variables
        private Vector2 position;
        private Vector2 previousPosition;
        private Vector2 velocity;
        private Vector2 acceleration;

        //X and Y grid positions, in relation to other tiles
        private Vector2 coordinates;

        //Size of player image
        private static readonly Vector2 size = new Vector2(16, 16);

        //True when the player has just attacked and false after the enemy has taken that damage (should be 1 frame)
        private bool isAttacking;

        //Time player must wait to attack again, attack cooldown
        private static readonly float attackCooldown = 0.6f;
        //Tracks time player has been waiting for cooldown
        private static float attackTimer = 0f;

        //Player health
        private float health;
        private float previousHealth;
        private static readonly float maxHealth = 3;

        //Positon that player starts at
        private static readonly int startPosition = 64; 

        private CollisionBody collisionBody;
        private CollisionBody hitbox;
        private static readonly Vector2 hitboxSize = new Vector2(16, 16); //8 pixel buffer on each side of player

        private static readonly float friction = 0.3f;

        //Take damage sound
        private SoundEffect takeDamageAudio; 

        /// <summary>
        /// Create player
        /// </summary>
        /// <param name="viewport"></param>
        public Player(Viewport viewport) { 
            //Set initial position
            position = new Vector2((viewport.X / 2) - (size.X / 2) + startPosition, (viewport.Y / 2) - (size.Y / 2) + startPosition);

            //Player starts here, might change later, should prob get these from somewhere
            coordinates = new Vector2(1, 1); 

            previousPosition = position;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            isAttacking = false;
            health = maxHealth;
            collisionBody = new CollisionBody(position, size);
            hitbox = new CollisionBody(position - (hitboxSize / 2), size + hitboxSize);
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"> Content Manager </param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>("Assets/Textures/Entities/Player/playerCrab");
            takeDamageAudio = content.Load<SoundEffect>("Assets/Sounds/damaged");
        }

        /// <summary>
        /// Update the player state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="keyboard"></param>
        /// <param name="mouse"></param>
        public void Update(GameTime gameTime, GameKeyboard keyboard, GameMouse mouse) {
            previousPosition = position;
            previousHealth = health; 

            Move(keyboard);

            UpdateAttackState(mouse);
            HandleAttackCooldown(gameTime); 

            //Update coordinates
            coordinates = PositionToCoords();

            //Move collision body and hitbox to new position
            collisionBody.SetPosition(position);
            hitbox.SetPosition(position - (hitboxSize / 2));
        }

        /// <summary>
        /// Get the direction the player is moving in based on keyboard input
        /// </summary>
        /// <param name="keyboard"></param>
        /// <returns> The direction of motion </returns>
        public Vector2 GetMovementDirection(GameKeyboard keyboard) {
            var movementDirection = Vector2.Zero;

            if(keyboard.IsAHeld()) {
                movementDirection -= Vector2.UnitX;
            }

            if(keyboard.IsDHeld()) { 
                movementDirection += Vector2.UnitX;
            }

            if(keyboard.IsWHeld()) {
                movementDirection -= Vector2.UnitY;
            }

            if(keyboard.IsSHeld()) {
                movementDirection += Vector2.UnitY;
            }

            return movementDirection; 
        }

        /// <summary>
        /// Move the player in the proper direction
        /// </summary>
        /// <param name="keyboard"></param>
        public void Move(GameKeyboard keyboard) {
            //Get the direction of motion
            Vector2 movementDirection = GetMovementDirection(keyboard);

            acceleration = movementDirection;

            velocity += acceleration - (friction * velocity);
            
            position += velocity; 
        }

        /// <summary>
        /// Update player's attack state
        /// </summary>
        /// <param name="mouse"></param>
        public void UpdateAttackState(GameMouse mouse) {
            //Can only attack when button is pressed and cooldown period is over
            if (mouse.IsLeftButtonHeld() && attackTimer == 0) {
                isAttacking = true;
            } else {
                isAttacking = false;
            }
        }

        /// <summary>
        /// Handle cooldown time between attacks
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
        /// Draw the player
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White); 
        }

        /// <summary>
        /// Transform position to tile coords
        /// </summary>
        /// <returns> The coordinates </returns>
        public Vector2 PositionToCoords() {
            return new Vector2((int)((position.X + (size.X / 2)) / Tile.GetWorldTileSize().X), (int)((position.Y + (size.Y / 2)) / Tile.GetWorldTileSize().Y));
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

        public Vector2 GetCoordinates() {
            return coordinates;
        }

        public void SetPosition(Vector2 position) {
            this.position = position; 
        }

        public void SetCoordinates(Vector2 coords) {
            coordinates = coords;
        }

        public CollisionBody GetHitbox() {
            return hitbox; 
        }

        public bool GetIsAttacking() {
            return isAttacking; 
        }

        public void SetIsAttacking(bool isAttacking) {
            this.isAttacking = isAttacking; 
        }

        public float GetHealth() {
            return health; 
        }

        public void DecrementHealth() {
            if(health > 0) {
                health -= 1;
            }
        }

        public float GetPreviousHealth() {
            return previousHealth;
        }

        public float GetAttackTimer() {
            return attackTimer; 
        }

        public SoundEffect GetTakeDamageAudio() {
            return takeDamageAudio; 
        }
    }
}
