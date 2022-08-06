using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

//Treasure map
namespace CrabThief.GameComponents.Map.Tiles.Collectibles {
    class TreasureMap : Collectible {

        //Path to data file
        private static readonly string JSON_PATH = "Content/Assets/Components/Tiles/Collectibles/mapTile.json";

        //True if the map has been collected and brought to it's spot
        private bool isUsed;

        //Audio that plays when the treasure has been found
        private SoundEffect completedAudio;

        /// <summary>
        /// Create treasure map
        /// </summary>
        /// <param name="coordinates"></param>
        public TreasureMap(Vector2 coordinates) : base() {
            //Read data from json file
            ReadCollectibleData(JSON_PATH);

            //Set up position, coords, and collision body
            Init(coordinates);

            isUsed = false; 
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content) {
            base.LoadContent(content);
            completedAudio = content.Load<SoundEffect>("Assets/Sounds/completedTask");
        }

        public void SetIsUsed(bool isUsed) {
            this.isUsed = isUsed; 
        }

        public bool GetIsUsed() {
            return isUsed; 
        }

        public SoundEffect GetCompletedAudio() {
            return completedAudio;
        }
    }
}
