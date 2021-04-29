using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Interfaces
{
    public interface IEnemyProjectile : IGameObject
    {
        /// <summary>
        /// Wether or not $enemy is the one who summoned (or owns) the projectile
        /// </summary>
        /// <param name="enemy">The enemy to test ownership of</param>
        /// <returns>T/F iff $enemy is the owner of the projectile</returns>
        public bool IsOwner(IEnemy enemy);

        /// <summary>
        /// Handle what would happen if the projectile collided with a rigid object
        /// </summary>
        public void CollideRigid();

        /// <summary>
        /// Wether or not $shield can deflect $this projectile
        /// </summary>
        /// <returns>T/F if $shield can deflect $this</returns>
        public bool CanDefelct(Shield shield);

        /// <summary>
        /// Get the damage this projectile does
        /// </summary>
        /// <returns>The damage of the projectile</returns>
        public int GetDamage();
    }
}
