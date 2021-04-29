// ISprite Interface
//
// @author Brian Sharp

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaGame.Interfaces
{
    public interface ISprite
    {
        /// <summary>
        /// Updates the sprite to show the next frame of animation.
        /// </summary>
        /// <returns>Returns a true when a loop of the sprites animation has completed, else false</returns>
        public bool UpdateFrame();

        /// <summary>
        /// Draws the sprite centered on the screen coordinates given by position with a tint given by color.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw the sprite with</param>
        /// <param name="position"> the position to draw the sprite</param>
        /// <param name="color">The color to overlay the sprite with</param>
        /// <param name="layer">The layer to draw the sprite on (between 0 [front] and 1 [back])</param>
        public void Draw(SpriteBatch spriteBatch, Point position, Color color, float layer);
    }
}