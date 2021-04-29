/*
 * Handles collisions between a player projectile and an enemy projectile.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class PlayerProjectileEnemyProjectileCollision
    {
        public static void HandleCollision(IPlayerProjectile playerProjectile, IEnemyProjectile enemyProjectile)
        {
            if (playerProjectile == null) throw new ArgumentNullException(nameof(playerProjectile));
            if (enemyProjectile == null) throw new ArgumentNullException(nameof(enemyProjectile));

            //Nothing happens
        }
    }
}
