/*
 * Handles collisions between an enemy projectile and a block.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Blocks;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class EnemyProjectileBlockCollision
    {
        public static void HandleCollision(IEnemyProjectile enemyProjectile, IBlock block)
        {
            if (enemyProjectile == null) throw new ArgumentNullException(nameof(enemyProjectile));
            if (block == null) throw new ArgumentNullException(nameof(block));

            if (block.IsRigid() && !(block is Water) && !(block is Divider))
            {
                enemyProjectile.CollideRigid();
            }
        }
    }
}
