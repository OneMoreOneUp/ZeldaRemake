// Interface for player
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using ZeldaGame.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.Interfaces
{
    public interface IAdventurePlayer : IGameObject
    {
        /// <summary>
        /// The following methods refer to moving in each direction
        /// </summary>
        public void GoUp();
        public void GoDown();
        public void GoLeft();
        public void GoRight();

        /// <summary>
        /// Injures the player and if they are out of health, show they are dead
        /// </summary>
        /// <param name="amount">Amount of damage to take / health to lose</param>
        /// <param name="fromDireciton">Direction damage was taken from</param>
        public void TakeDamage(int amount, Direction fromDireciton);

        /// <summary>
        /// Use the item in slot 1 (A)
        /// </summary>
        public void UseSlot1();

        /// <summary>
        /// Use the item in slot 2 (B)
        /// </summary>
        public void UseSlot2();

        /// <summary>
        /// Makes link hold the item above his head
        /// </summary>
        /// <param name="item">The item to hold above link's head</param>
        public void HoldItem(IItem item);

        /// <summary>
        /// Gets the hitbox of the player's melee hitbox
        /// </summary>
        /// <returns>Rectangle of the hitbox of the player's melee weapon, empty if unequpped</returns>
        public Rectangle GetMeleeHitbox();

        /// <summary>
        /// Gets the hitbox of the player themselves
        /// </summary>
        /// <returns>Rectangle of the player's hitbox</returns>
        public Rectangle GetPlayerHitbox();

        /// <summary>
        /// Updates the location of the player
        /// </summary>
        /// <param name="x">Adds this to the old X value</param>
        /// <param name="y">Adds this to the old Y value</param>
        public void UpdateLocation(int x, int y);

        /// <summary>
        /// Gets the location of the player
        /// </summary>
        /// <returns>Returns the location of the player</returns>
        public Point GetLocation();

        /// <summary>
        /// Sets the position of the player
        /// </summary>
        /// <param name="position">Position to set the player to</param>
        public void SetLocation(Point position);

        /// <summary>
        /// Returns the player inventory for the player. Contains health and inventroy information.
        /// </summary>
        /// <returns>AdventurePlayerInventory of the player</returns>
        public AdventurePlayerInventory GetInventory();

        /// <summary>
        /// Returns wether or not the player is able to block with their shield.
        /// </summary>
        /// <param name="fromDirection">The direction the damage is coming from</param>
        /// <returns>T/F if the player cna block with their shield</returns>
        public bool CanBlock(Direction fromDirection);

        /// <summary>
        /// Gets the damage that the player's melee does.
        /// </summary>
        /// <returns>The amount of damage the player's sword does</returns>
        public int GetDamage();

        public ISprite GetSprite();
    }
}
