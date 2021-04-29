/*
 * Handles collisions between an enemy and a player projectile.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enemies;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.PlayerProjectiles;

namespace ZeldaGame.Collisions
{
    public static class EnemyPlayerProjectileCollision
    {
        public static void HandleCollision(IEnemy enemy, IPlayerProjectile playerProjectile)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            if (playerProjectile == null) throw new ArgumentNullException(nameof(playerProjectile));

            Rectangle enemyRect = enemy.GetHitbox();
            Rectangle playerProjectileRect = playerProjectile.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(enemyRect, playerProjectileRect);
            Direction fromDirection = CollisionDetector.GetCollisionDirection(playerProjectileRect, enemyRect, intersection);


            if (playerProjectile is Bomb bomb)
            {
                if (enemy is Dodongo && bomb.IsIntact())
                {
                    enemy.TakeDamage(fromDirection, bomb.GetDamage());
                    PlayerProjectileManager.RemovePlayerProjectile(bomb);
                }
                else if (!(enemy is Dodongo) && !bomb.IsIntact())
                {
                    enemy.TakeDamage(fromDirection, bomb.GetDamage());
                }
            }
            else if (enemy is Gohma && playerProjectile is Arrow arrow)
            {
                enemy.TakeDamage(fromDirection, arrow.GetDamage());
                PlayerProjectileManager.RemovePlayerProjectile(arrow);
            }
            else if (playerProjectile is WoodenBoomerang || playerProjectile is MagicalBoomerang)
            {
                HandleBoomerang(enemy, playerProjectile, fromDirection);
            }
            else if (!(playerProjectile is Food) && !(enemy is EnemyDeath) && !(enemy is Dodongo) && !(enemy is Gohma))
            {
                enemy.TakeDamage(fromDirection, playerProjectile.GetDamage());
                playerProjectile.CollideRigid();
            }
        }

        private static void HandleBoomerang(IEnemy enemy, IPlayerProjectile playerProjectile, Direction fromDirection)
        {
            if (enemy is Gel)
            {
                enemy.TakeDamage(fromDirection, playerProjectile.GetDamage());
            }
            else
            {
                enemy.Stun(playerProjectile.GetDamage());
            }

            playerProjectile.CollideRigid();
        }
    }
}
