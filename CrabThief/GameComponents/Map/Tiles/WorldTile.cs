using System.IO;

using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework;

//Tiles that make up the walls and background of the world
namespace CrabThief.GameComponents.Map {
    class WorldTile : Tile {

        //String attributes found in the world tile's json file
        public enum WorldTileAttributes {
            name,
            width,
            height,
            texturePath
        }

        /// <summary>
        /// Read wall tile data from the json file and build the tile accordingly
        /// </summary>
        /// <param name="jsonPath"></param>
        public void ReadWallData(string jsonPath) {
            //Convert the json file into a string
            string json = File.ReadAllText(jsonPath);

            //The json file object
            JObject jsonObject = JObject.Parse(json);

            //Get each of the wall tile's attrributes
            JToken name = jsonObject.SelectToken(WorldTile.WorldTileAttributes.name.ToString());
            JToken width = jsonObject.SelectToken(WorldTile.WorldTileAttributes.width.ToString());
            JToken height = jsonObject.SelectToken(WorldTile.WorldTileAttributes.height.ToString());
            JToken texturePath = jsonObject.SelectToken(WorldTile.WorldTileAttributes.texturePath.ToString());

            //Set the wall tile's properties based on the json file data
            SetSize(new Vector2((int)width, (int)height));
            SetTexturePath((string)texturePath);
        }
    }
}
