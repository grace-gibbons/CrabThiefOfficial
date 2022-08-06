using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Interface for various non-tile components that appear in the game world
namespace CrabThief.GameComponents.Map {
    interface MapComponent {

        public void LoadContent(ContentManager content);

        public void Draw(SpriteBatch spriteBatch);

        public bool IsInViewport(GameCamera camera) {
            return false; 
        }
    }
}
