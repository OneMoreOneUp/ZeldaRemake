using Microsoft.Xna.Framework;
using ZeldaGame.Enums;

namespace ZeldaGame.Interfaces
{
    public interface IEnemy : IGameObject
    {
        /// <summary>
        /// Damages the enemy. Removes $amount of health from enemy.
        /// </summary>
        /// <param name="directionFrom">The direction the damage was taken from</param>
        /// <param name="amount">The amount of damage to take</param>
        public void TakeDamage(Direction directionFrom, int amount);

        /// <summary>
        /// Updates the location of the enemy by adding x = #x + $x, y = #y + $y and sets the facing direction to $direction
        /// </summary>
        /// <param name="x">Adds this to x coordinate</param>
        /// <param name="y">Adds this to y coordinate</param>
        /// <param name="direction">Direction to face</param>
        public void UpdateLocation(int x, int y);

        /// <summary>
        /// Gets the location of the enemy
        /// </summary>
        /// <returns>Location of the enemy</returns>
        public Point GetLocation();

        /// <summary>
        /// Enemy turns a random direction.
        /// </summary>        
        public void ChangeDirection();

        /// <summary>
        /// Enemy turns a random direction that is not $notDirection
        /// </summary>
        /// <param name="notDirection">The direction the enemy cannot turn</param>
        public void ChangeDirection(Direction notDirection);

        /// <summary>
        /// Gets the damage the enemy does on contact
        /// </summary>
        /// <returns>The damage the enemy does on contact</returns>
        public int GetDamage();


        /// <summary>
        /// Stuns the enemy, preventing them from movement and damage for the duration of the stun.
        /// </summary>
        /// <param name="duration">How many frames to stun the enemy</param>
        public void Stun(int duration);
    }
}
