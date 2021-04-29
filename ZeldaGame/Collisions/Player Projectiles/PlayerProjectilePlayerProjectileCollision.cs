/*
 * Handles collisions between an playerProjectile and a playerProjectile.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class PlayerProjectilePlayerProjectileCollision
    {
        public static void HandleCollision(IPlayerProjectile playerProjectile1, IPlayerProjectile playerProjectile2)
        {
            if (playerProjectile1 == null) throw new ArgumentNullException(nameof(playerProjectile1));
            if (playerProjectile2 == null) throw new ArgumentNullException(nameof(playerProjectile2));

            //Nothing happens
        }
    }
}
