using Microsoft.Xna.Framework.Audio;

/**
 * Incredibly useful and necessary class
 */
namespace CrabThief.GameComponents.Audio {
    static class AudioHandler {

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="effect"> Sound to play </param>
        public static void PlaySound(SoundEffect effect) {
            effect.CreateInstance().Play(); 
        }
    }
}
