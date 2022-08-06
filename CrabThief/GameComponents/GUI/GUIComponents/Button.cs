using System;
using System.Collections.Generic;
using System.Text;
using CrabThief.GameComponents.Input;
using CrabThief.GameComponents.Map;
using CrabThief.GameComponents.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Button: can be clicked 
namespace CrabThief.GameComponents.GUI.GUIComponents {
    class Button {
        
        //Position of the button
        private Vector2 position;
        //Size of the button
        private Vector2 size; 

        private CollisionBody collisionBody;

        //Button texture
        private Texture2D texture;

        //True if the button is held, false otherwise
        private bool isHeld;

        //For buttons that have a boolean quality
        private bool isToggled;
        //Second texture for toggled state
        private Texture2D texture2;

        //Draw at smaller size
        private bool halfSize;

        /// <summary>
        /// Create a normal sized button
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Button(Vector2 position, Vector2 size) {
            this.position = position;
            this.size = size; 
            collisionBody = new CollisionBody(position, size);
            isHeld = false;
            isToggled = false;
            halfSize = false; 
        }

        /// <summary>
        /// Create a half sized button
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="halfSize"></param>
        public Button(Vector2 position, Vector2 size, bool halfSize) {
            this.position = position;
            this.size = size; 
            this.halfSize = halfSize; 
            collisionBody = new CollisionBody(position, size / 2);
            isHeld = false;
            isToggled = false;
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"> Path of the button texture </param>
        public void LoadContent(ContentManager content, string path) {
            texture = content.Load<Texture2D>(path);
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"> Path for normal button state </param>
        /// <param name="path2"> Path for toggled button state </param>
        public void LoadContent(ContentManager content, string path, string path2) {
            texture = content.Load<Texture2D>(path);
            texture2 = content.Load<Texture2D>(path2);
        }


        /// <summary>
        /// Handles button hovering 
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Returns true if the button is being hovered over and false otherwise </returns>
        public bool IsHovered(CollisionEngine collisionEngine, GameMouse mouse) {
            if(collisionEngine.IsMouseCollision(mouse, collisionBody)) {
                return true;
            }
            return false; 
        }
 
        /// <summary>
        /// Handles button clicking
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Returns true if the button has been clicked and false otherwise </returns>
        public bool IsClicked(CollisionEngine collisionEngine, GameMouse mouse) {
            if (IsHovered(collisionEngine, mouse) && mouse.IsLeftButton()) {
                return true;
            }
            return false;
        }

        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Returns true if the button is clicked and held, false otherwise </returns>
        public bool IsHeld(CollisionEngine collisionEngine, GameMouse mouse) {
            if (IsHovered(collisionEngine, mouse) && mouse.IsLeftButton()) {
                isHeld = true; 
                return true;
            } else if(isHeld && mouse.IsLeftButtonHeld()) {
                return true; 
            }
            isHeld = false; 
            return false;
        }

        /// <summary>
        /// Follow the mouse on the x-axis if the button is clicked
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        /// <returns> Return true is followinf mouse, false otherwise </returns>
        public bool FollowMouseXAxis(CollisionEngine collisionEngine, GameMouse mouse) {
            IsHeld(collisionEngine, mouse);
            if(isHeld) {
                position.X = mouse.GetPosition().X;
                collisionBody.SetPosition(new Vector2(mouse.GetPosition().X, collisionBody.GetPosition().Y));
                return true;
            }
            return false; 
        }

        /// <summary>
        /// Set the toggled state of the button, if clicked
        /// </summary>
        /// <param name="collisionEngine"></param>
        /// <param name="mouse"></param>
        public void SetIsToggled(CollisionEngine collisionEngine, GameMouse mouse) {
            if(IsClicked(collisionEngine, mouse)) {
                isToggled = !isToggled; 
            }
        }

        /// <summary>
        /// Draw the button
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch) {
            //Precaution since some buttons are "invisible" and don't have textures
            if(texture != null) {
                if(!isToggled) {
                    if(!halfSize) {
                        spriteBatch.Draw(texture, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
                    } else {
                        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X / 2, (int)size.Y / 2), new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
                    }
                } else if(isToggled) {
                    if (!halfSize) {
                        spriteBatch.Draw(texture2, position, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
                    } else {
                        spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, (int)size.X / 2, (int)size.Y / 2), new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
                    }
                }
                
            }
        }

        /// <returns> Return the button texture </returns>
        public Texture2D GetTexture() {
            return texture; 
        }

        /// <summary>
        /// Set the button size
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(Vector2 size) {
            this.size = size; 
        }

        /// <returns> Return the button size </returns>
        public Vector2 GetSize() {
            return size; 
        }

        /// <summary>
        /// Set the button's position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position) {
            this.position = position;
        }

        /// <returns> Return the position of the button </returns>
        public Vector2 GetPosition() {
            return position; 
        }

        /// <summary>
        /// Set the collision body
        /// </summary>
        /// <param name="cb"></param>
        public void SetCollisionBody(CollisionBody cb) {
            collisionBody = cb; 
        }

        /// <returns> Return the collision body </returns>
        public CollisionBody GetCollisionBody() {
            return collisionBody; 
        }

        /// <summary>
        /// Set the isHeld state
        /// </summary>
        /// <param name="isHeld"></param>
        public void SetIsHeld(bool isHeld) {
            this.isHeld = isHeld;
        }
    }
}
