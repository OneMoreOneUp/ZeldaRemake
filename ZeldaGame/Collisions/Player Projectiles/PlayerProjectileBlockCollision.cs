/*
 * Handles collisions between a player projectile and a block.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Blocks;
using ZeldaGame.Interfaces;
using ZeldaGame.PlayerProjectiles;

namespace ZeldaGame.Collisions
{
    public static class PlayerProjectileBlockCollision
    {
        public static void HandleCollision(IPlayerProjectile playerProjectile, IBlock block)
        {
            if (playerProjectile == null) throw new ArgumentNullException(nameof(playerProjectile));
            if (block == null) throw new ArgumentNullException(nameof(block));

            if (block is Door door && playerProjectile is Bomb bomb && !bomb.IsIntact())
            {
                door.BombCollision();
            }
            else if (block.IsRigid() && !(block is Divider) && !(block is Water))
            {
                playerProjectile.CollideRigid();
            }
        }
    }
}
