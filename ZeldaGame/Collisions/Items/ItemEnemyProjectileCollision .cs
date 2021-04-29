/*
 * Handles collisions between an item and an enemy projectile.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class ItemEnemyProjectileCollision
    {
        public static void HandleCollision(IItem item, IEnemyProjectile enemyProjectile)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (enemyProjectile == null) throw new ArgumentNullException(nameof(enemyProjectile));

            //Nothing happens
        }
    }
}
