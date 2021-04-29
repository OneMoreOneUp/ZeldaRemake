/*
 * Handles collisions between an item and a player projectile.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class ItemPlayerProjectileCollision
    {
        public static void HandleCollision(IItem item, IPlayerProjectile playerProjectile)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (playerProjectile == null) throw new ArgumentNullException(nameof(playerProjectile));

            //Nothing happens
        }
    }
}
