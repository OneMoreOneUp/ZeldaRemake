/*
 * Handles collisions between an enemy and an enemy projectile.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Enemies;
using ZeldaGame.EnemyProjectiles;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class EnemyEnemyProjectileCollision
    {
        public static void HandleCollision(IEnemy enemy, IEnemyProjectile enemyProjectile)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            if (enemyProjectile == null) throw new ArgumentNullException(nameof(enemyProjectile));

            IEnemy rootEnemy = GetRootEnemy(enemy);

            if (enemyProjectile is WoodenBoomerang)
            {
                if (enemyProjectile.IsOwner(rootEnemy) && EnemyProjectileManager.Contains(enemyProjectile))
                {
                    EnemyProjectileManager.RemoveEnemyProjectile(enemyProjectile);
                }
            }
        }

        /// <summary>
        /// Gets the non-decorated/root enemy of enemy. If the enemy is not decorated it will just return $enemy
        /// </summary>
        /// <param name="enemy">The enemy to get the root enemy of</param>
        /// <returns>Non-decorated version of enemy</returns>
        public static IEnemy GetRootEnemy(IEnemy enemy)
        {
            IEnemy rootEnemy = enemy;
            if (enemy is DamagedEnemy de)
            {
                rootEnemy = de;
            }
            return rootEnemy;
        }
    }
}
