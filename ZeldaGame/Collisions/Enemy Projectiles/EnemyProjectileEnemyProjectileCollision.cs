/*
 * Handles collisions between an enemyProjectile and a enemyProjectile.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class EnemyProjectileEnemyProjectileCollision
    {
        public static void HandleCollision(IEnemyProjectile enemyProjectile1, IEnemyProjectile enemyProjectile2)
        {
            if (enemyProjectile1 == null) throw new ArgumentNullException(nameof(enemyProjectile1));
            if (enemyProjectile2 == null) throw new ArgumentNullException(nameof(enemyProjectile2));

            //Nothing happens
        }
    }
}
