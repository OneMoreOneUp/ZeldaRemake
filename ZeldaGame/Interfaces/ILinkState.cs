// Interface for link state
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using ZeldaGame.Enums;
using ZeldaGame.Player;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Interfaces
{
    public interface ILinkState
    {
        /// <summary>
        /// Updates the sprite
        /// </summary>
        public void Update(GameTime gameTime);

        /// <summary>
        /// The following methods refer to moving in each direction
        /// </summary>
        public void GoUp();
        public void GoDown();
        public void GoLeft();
        public void GoRight();

        /// <summary>
        /// Use the item in slot 1 (A)
        /// </summary>
        /// <paramref name="slot1"/> The item in slot1
        public void UseSlot1(ProjectileSummoner itemHandler, Slot1Item slot1);

        /// <summary>
        /// Use the item in slot 2 (B)
        /// <paramref name="slot2"/> The item in slot2
        /// </summary>
        public void UseSlot2(ProjectileSummoner itemHandler, Slot2Item slot2);

        /// <summary>
        /// Returns wether or not the player is able to block with their shield.
        /// </summary>
        /// <param name="fromDirection">The direction the damage is coming from</param>
        /// <returns>T/F if the player block with their shield</returns>
        public bool CanBlock(Direction fromDirection);

        /// <summary>
        /// The hitbox of the player's melee weapon (empty if not attacking)
        /// </summary>
        /// <param name="meleeLength">The length of the melee weapon</param>
        /// <returns>The hitbox of the melee weapon</returns>
        public Rectangle GetMeleeHitbox(int meleeLength);
    }
}
