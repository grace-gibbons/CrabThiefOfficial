using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Components that create the game world and scene
namespace CrabThief.GameComponents.Map {
    class Tile : MapComponent {

        //Types of tiles
        public enum TileTypes {
            BACKGROUND,
            WALL,
            FOOD,
            CHANGE,
            ENEMY
        }

        //Texture of the tile
        private Texture2D texture;

        //Path to the tile texture
        private string texturePath; 

        //Collision body 
        private CollisionBody collisionBody;
        
        //Position on the screen
        private Vector2 position;

        //Typical size of tiles that make up the world
        private static readonly Vector2 worldTileSize = new Vector2(48, 48); 

        //Size will need to be different for food and others, include in json file
        private Vector2 size;

        //X and Y grid positions, in relation to other tiles
        private Vector2 coordinates;

        //True if the player can collide with the tile and false otherwise
        private bool isCollidable;

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public virtual void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>(texturePath);
        }

        /// <summary>
        /// Return true if the tile is visible in the viewport and false otherwise
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public bool IsInViewport(GameCamera camera) {
            //Convert the tile world coordinates to camera coordinates
            Vector2 screen = camera.WorldToScreen(new Vector2(position.X, position.Y));

            if (screen.X > 0 - (size.X * 5) && screen.Y > 0 - (size.Y * 5) && screen.X < camera.GetViewport().Width && screen.Y < camera.GetViewport().Height) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Draw the tile
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }

        /// <summary>
        /// Return true if two tile are equal (have same coordinates)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj != null || this.GetType().Equals(obj.GetType())) {
                Tile b = (Tile)obj;
                if (this.GetCoordinates().X == b.GetCoordinates().X && this.GetCoordinates().Y == b.GetCoordinates().Y) {
                    return true;
                }
            }
            return false;
        }

        public static Vector2 GetWorldTileSize() {
            return worldTileSize; 
        }

        public Vector2 GetPosition() {
            return position; 
        }

        public void SetPosition(Vector2 position) {
            this.position = position;
        }

        public Vector2 GetCoordinates() {
            return coordinates;
        }

        public Vector2 GetSize() { 
            return size; 
        }

        public void SetSize(Vector2 size) {
            this.size = size;
        }

        public CollisionBody GetCollisionBody() {
            return collisionBody; 
        }

        public void SetCollisionBody(CollisionBody cb) {
            collisionBody = cb; 
        }

        public Texture2D GetTexture() {
            return texture; 
        }

        public void SetTexture(Texture2D texture) {
            this.texture = texture; 
        }

        public void SetTexturePath(string texturePath) {
            this.texturePath = texturePath; 
        }

        public string GetTexturePath() {
            return texturePath; 
        }

        public void SetCoordinates(Vector2 coordinates) {
            this.coordinates = coordinates; 
        }

        public void SetIsCollidable(bool isCollidable) {
            this.isCollidable = isCollidable; 
        }
    }
}
