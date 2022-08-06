using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Sheet of sprites, with w width and height of 1
namespace CrabThief.GameComponents.Graphics {
    class SpriteSheet {

        //Path to the texture image
        private string texturePath; 

        //The whole texture
        private Texture2D texture;

        //Width of the whole sheet, in number of sprites
        private int widthInSprites;

        //Size of each individual sprite on the sheet, in pixels
        private Vector2 spriteSize;

        //Rectangle bounds of each sprite on the sheet (location of sprite on the sheet)
        private List<Rectangle> spriteLocations;

        /// <summary>
        /// Create a spritesheet
        /// </summary>
        /// <param name="texturePath"> The path to the texture </param>
        /// <param name="widthInSprites"> The number of sprites on the sheet </param>
        public SpriteSheet(string texturePath, int widthInSprites) {
            this.texturePath = texturePath;
            this.widthInSprites = widthInSprites;
            spriteLocations = new List<Rectangle>(); 
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"> Content Manager </param>
        public void LoadContent(ContentManager content) {
            //Load texture
            texture = content.Load<Texture2D>(texturePath); 

            //Set size of each sprite
            spriteSize.X = texture.Width / widthInSprites;
            spriteSize.Y = texture.Height; 

            //Create a rectangle location for each sprite
            for(int i = 0; i < widthInSprites; i++) {
                spriteLocations.Add(new Rectangle((int)spriteSize.X * i , 0, (int)spriteSize.X, (int)spriteSize.Y));
            }
        }

        /// <returns> The width of the spritesheet, in sprites </returns>
        public int GetWidthInSprites() {
            return widthInSprites; 
        }
 
        /// <returns> Return the size of each sprite, in pixels </returns>
        public Vector2 GetSpriteSize() {
            return spriteSize; 
        }


        /// <returns> Return the texture </returns>
        public Texture2D GetTexture() {
            return texture; 
        }

        /// <returns> The location of each sprite on the sheet </returns>
        public List<Rectangle> GetSpriteLocations() {
            return spriteLocations; 
        }

    }
}
