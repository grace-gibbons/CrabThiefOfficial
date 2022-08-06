using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Shell
namespace CrabThief.GameComponents.Map.Tiles.Collectibles {
    class Shell : Collectible {

        //Types of shells
        public enum ShellTypes {
            Orange,
            Blue,
            Pink,
            Purple,
            None
        }

        //Path to data file
        private static readonly string ORANGE_SHELL_PATH = "Content/Assets/Components/Tiles/Collectibles/orangeShellTile.json";
        private static readonly string BLUE_SHELL_PATH = "Content/Assets/Components/Tiles/Collectibles/blueShellTile.json";
        private static readonly string PINK_SHELL_PATH = "Content/Assets/Components/Tiles/Collectibles/pinkShellTile.json";
        private static readonly string PURPLE_SHELL_PATH = "Content/Assets/Components/Tiles/Collectibles/purpleShellTile.json";

        //shell type
        private ShellTypes type;

        //True if the shell has been placed on the board
        private bool isPlaced; 

        /// <summary>
        /// Create shell
        /// </summary>
        /// <param name="coordinates"> Coordinates of the shell</param>
        /// <param name="type"> Type of shell to make </param>
        public Shell(Vector2 coordinates, ShellTypes type) : base() {
            this.type = type; 

            //Load path based on type
            string path = "";
            if(type == ShellTypes.Orange) {
                path = ORANGE_SHELL_PATH; 
            } else if (type == ShellTypes.Blue) {
                path = BLUE_SHELL_PATH;
            } else if (type == ShellTypes.Pink) {
                path = PINK_SHELL_PATH;
            } else if (type == ShellTypes.Purple) {
                path = PURPLE_SHELL_PATH;
            }

            //Shells are not initially placed on the board
            isPlaced = false; 

            //Read shell data from json file
            ReadCollectibleData(path);

            //Set up position, coords, and collision body
            Init(coordinates);
        }

        /// <summary>
        /// Draw shell if it is not collected or it has been placed on the board
        /// </summary>
        /// <param name="spriteBatch"></param>
        public new void Draw(SpriteBatch spriteBatch) {
            if (!GetIsCollected() || isPlaced) {
                spriteBatch.Draw(GetTexture(), GetPosition(), new Rectangle(0, 0, (int)GetSize().X, (int)GetSize().Y), Color.White);
            }
        }

        public ShellTypes GetShellType() {
            return type; 
        }

        public bool GetIsPlaced() {
            return isPlaced; 
        }

        public void SetIsPlaced(bool isPlaced) {
            this.isPlaced = isPlaced; 
        }
    }
}
