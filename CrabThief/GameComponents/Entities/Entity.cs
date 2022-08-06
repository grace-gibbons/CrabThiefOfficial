using CrabThief.GameComponents.Map;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Entity interface
namespace CrabThief.GameComponents.Entities {
    interface Entity {

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"> Content Manager </param>
        public void LoadContent(ContentManager content);

        /// <summary>
        /// Draw entity
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Time cooldown between attacks
        /// </summary>
        /// <param name="gameTime"></param>
        public void HandleAttackCooldown(GameTime gameTime);

        /// <summary>
        /// Transform entity pixel position to tile coordinates
        /// </summary>
        /// <returns></returns>
        public Vector2 PositionToCoords(); 

        /// <returns> Return entity collision body </returns>
        public CollisionBody GetCollisionBody();

        /// <returns> Return entity position </returns>
        public Vector2 GetPosition();

        /// <returns> Return previous position </returns>
        public Vector2 GetPreviousPosition();

        /// <returns> Return size </returns>
        public Vector2 GetSize();

        /// <returns> Return current tile coordinates </returns>
        public Vector2 GetCoordinates();

        /// <returns> Return hitbox </returns>
        public CollisionBody GetHitbox();

        /// <returns> Return true if entity is attacking and false otherwise </returns>
        public bool GetIsAttacking();

        /// <returns> Return the attack timer </returns>
        public float GetAttackTimer();

        /// <returns> Return damaged sound effect </returns>
        public SoundEffect GetTakeDamageAudio();

        /// <param name="coords"> Value to set coordinates to </param>
        public void SetCoordinates(Vector2 coords);

        /// <param name="position"> Value to set position to </param>
        public void SetPosition(Vector2 position);

        /// <param name="isAttacking"> Value to set isAttacking to </param>
        public void SetIsAttacking(bool isAttacking);
    }
}
