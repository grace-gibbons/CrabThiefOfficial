using CrabThief.GameComponents.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Controllable camera
namespace CrabThief.GameComponents.Map {
    class GameCamera {

        //Position of the camera
        private Vector2 position;
        //Camera origin
        private Vector2 origin;
        //Camera zoom amount
        private float zoom;
        //Camera rotation, 0 for no rotation
        private static readonly float rotation = 0;

        //Zoom in limit and factors - testing only
        private static readonly int zoomInLimit = 3; 
        private static readonly double zoomOutLimit = 0.4; 
        private static readonly float zoomFactor = 0.2f; 

        //viewport
        private Viewport cameraViewport;

        //Screen scale 
        private Vector2 scale; 

        /// <summary>
        /// Create camera
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="scale"></param>
        public GameCamera(Viewport viewport, Vector2 scale) {
            cameraViewport = viewport;
            position = Vector2.Zero;
            origin = new Vector2(viewport.Width / 2, viewport.Height / 2);
            zoom = 1.4f;

            //Screen size scale
            this.scale = scale; 
        }

        /// <summary>
        /// Return view matrix
        /// </summary>
        /// <param name="parallax"></param>
        /// <returns></returns>
        public Matrix GetViewMatrix(Vector2 parallax) {
            return Matrix.CreateTranslation(new Vector3(-position * parallax, 0f)) *
                Matrix.CreateTranslation(new Vector3(-origin, 0f)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(zoom * scale.X, zoom * scale.Y, 1) *
                Matrix.CreateTranslation(new Vector3(origin, 0f));
        }

        /// <summary>
        /// At look at to a specific point
        /// </summary>
        /// <param name="lookAt"></param>
        /// <param name="bodySize"></param>
        /// <param name="viewport"></param>
        public void LookAt(Vector2 lookAt, Vector2 bodySize, Viewport viewport) {
            position = lookAt - new Vector2((viewport.Width / 2.0f) - (bodySize.X / 2), (viewport.Height / 2.0f) - (bodySize.Y / 2));
        }

        /// <summary>
        /// Handle zoom feature - testing only
        /// </summary>
        /// <param name="keyboard"></param>
        public void Zoom(GameKeyboard keyboard) {
            if(keyboard.IsUpHeld()) {
                if (zoom < zoomInLimit) {
                    zoom += zoomFactor;
                }
            }
            if (keyboard.IsDownHeld()) {
                if (zoom > zoomOutLimit) {
                    zoom -= zoomFactor;
                }
            }
        }

        /// <summary>
        /// Convert world coordinates (based on position in space) to screen coordinates (based on position on screen)
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns> Vector2 </returns>
        public Vector2 WorldToScreen(Vector2 worldPosition) {
            return Vector2.Transform(worldPosition, GetViewMatrix(new Vector2(1f, 1f)));
        }

        /// <summary>
        /// Convert sreen coordinates (based on position on screen) to world coordinates (based on position in space) 
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns> Vector2 </returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition) {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix(new Vector2(1f, 1f))));
        }

        public void SetPosition(Vector2 position) {
            this.position = position;
        }

        public Vector2 GetPosition() {
            return position;
        }

        public Viewport GetViewport() {
            return cameraViewport;
        }
    }
}
