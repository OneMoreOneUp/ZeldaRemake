namespace ZeldaGame.Interfaces
{
    public interface IPlayerProjectile : IGameObject
    {
        /// <summary>
        /// Wether or not $player is the one who summoned (or owns) the projectile
        /// </summary>
        /// <param name="player">The player to test ownership of</param>
        /// <returns>T/F iff $player is the owner of the projectile</returns>
        public bool IsOwner(IAdventurePlayer player);

        /// <summary>
        /// Handle what would happen if the projectile collided with a rigid object
        /// </summary>
        public void CollideRigid();

        /// <summary>
        /// Gets the damage that this projectile does
        /// </summary>
        /// <returns>The damage of the projectile</returns>
        public int GetDamage();
    }
}
