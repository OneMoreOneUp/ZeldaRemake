/*
 * Handles collisions between a player and an enemy projectile.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Collisions
{
    public static class PlayerEnemyProjectileCollision
    {
        public static void HandleCollision(IAdventurePlayer player, IEnemyProjectile enemyProjectile)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (enemyProjectile == null) throw new ArgumentNullException(nameof(enemyProjectile));
            if (!player.GetPlayerHitbox().Intersects(enemyProjectile.GetHitbox())) return; //No collision with the player itself

            Rectangle playerRect = player.GetHitbox();
            Rectangle enemyProjectileRect = enemyProjectile.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(playerRect, enemyProjectileRect);
            Direction fromDirection = CollisionDetector.GetCollisionDirection(enemyProjectileRect, playerRect, intersection);

            if (!player.CanBlock(fromDirection) || !enemyProjectile.CanDefelct(player.GetInventory().GetShield()))
            {
                player.TakeDamage(enemyProjectile.GetDamage(), fromDirection);
            }
            else
            {
                SoundFactory.Instance.GetSound(Sound.Deflect).Play();
            }
            enemyProjectile.CollideRigid();
        }
    }
}
