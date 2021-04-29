// Interface for triggers boxes
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;

namespace ZeldaGame.Interfaces
{
    public interface ITriggerBox
    {
        /// <summary>
        /// Activates the trigger box event
        /// </summary>
        public void Trigger();

        /// <summary>
        /// Gets the bounding rectangle of the trigger box
        /// </summary>
        /// <returns>Rectangle of the trigger box</returns>
        public Rectangle GetRectangle();
    }
}
