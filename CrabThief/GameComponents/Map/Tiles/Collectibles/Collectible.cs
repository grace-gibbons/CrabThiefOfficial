using System.IO;

using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

//General collectible tile
namespace CrabThief.GameComponents.Map {
    class Collectible : Tile {

        //String attributes found in the collectible's json file
        public enum CollectibleAttributes {
            name,
            width,
            height,
            texturePath,
            collectedAudioPath
        }

        //True if the item is collected and false otherwise
        private bool isCollected;

        //Make collision body a little bigger then the tile
        //Adds a buffer of half this size to each edge
        private static readonly Vector2 collisionBodyBuffer = new Vector2(4, 4);

        //Audio that plays when an item is collected
        private string collectedAudioPath;
        private SoundEffect collectedAudio;

        /// <summary>
        /// Read collectible data from the json file and build accordingly
        /// </summary>
        /// <param name="jsonPath"></param>
        public void ReadCollectibleData(string jsonPath) {
            //Convert the json file into a string
            string json = File.ReadAllText(jsonPath);

            //The json file object
            JObject jsonObject = JObject.Parse(json);

            //Get each of the wall tile's attrributes
            JToken name = jsonObject.SelectToken(CollectibleAttributes.name.ToString());
            JToken width = jsonObject.SelectToken(CollectibleAttributes.width.ToString());
            JToken height = jsonObject.SelectToken(CollectibleAttributes.height.ToString());
            JToken texturePath = jsonObject.SelectToken(CollectibleAttributes.texturePath.ToString());
            JToken collectedAudioPath = jsonObject.SelectToken(CollectibleAttributes.collectedAudioPath.ToString());

            //Set the wall tile's properties based on the json file data
            SetSize(new Vector2((int)width, (int)height));
            SetTexturePath((string)texturePath);
            this.collectedAudioPath = (string)collectedAudioPath; 
        }

        /// <summary>
        /// Create collectible
        /// </summary>
        public Collectible(): base() {
            isCollected = false;

            //All collectibles are collidable
            SetIsCollidable(true);
        }
        
        /// <summary>
        /// Set up the coordinates, position, and collision body
        /// </summary>
        /// <param name="coordinates"> Coordinates of the collectible </param>
        public void Init(Vector2 coordinates) {
            SetCoordinates(coordinates);

            //Set position based on coords and "normal" grid size
            SetPosition((coordinates * GetWorldTileSize()) + GetSize());

            //Set collision body (Must be done after position is set)
            SetCollisionBody(new CollisionBody(GetPosition() - (GetCollisionBodyBuffer() / 2), GetSize() + GetCollisionBodyBuffer()));
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            //Load data from json file
            SetTexture(content.Load<Texture2D>(GetTexturePath()));
            collectedAudio = content.Load<SoundEffect>(collectedAudioPath); 
        }

        /// <summary>
        /// Handle collection
        /// </summary>
        public void Collect() {
            isCollected = true;
            GetCollisionBody().SetIsEnabled(false); 
        }

        /// <summary>
        /// Draw collectible if not collected
        /// </summary>
        /// <param name="spriteBatch"></param>
        public new void Draw(SpriteBatch spriteBatch) {
            if(!isCollected) {
                spriteBatch.Draw(GetTexture(), GetPosition(), new Rectangle(0, 0, (int)GetSize().X, (int)GetSize().Y), Color.White);
            }
        }

        /// <summary>
        /// Get the large collision body, with buffer
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCollisionBodyBuffer() {
            return collisionBodyBuffer; 
        }

        public void SetIsCollected(bool isCollected) {
            this.isCollected = isCollected; 
        }

        public bool GetIsCollected() {
            return isCollected; 
        }

        public string GetCollectedAudioPath() {
            return collectedAudioPath; 
        }

        public SoundEffect GetCollectedAudio() {
            return collectedAudio; 
        }
    }
}
