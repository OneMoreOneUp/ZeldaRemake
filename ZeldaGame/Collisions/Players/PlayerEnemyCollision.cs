/*
 * Handles collisions between a player and an enemy.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using ZeldaGame.Enemies;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class PlayerEnemyCollision
    {
        public static void HandleCollision(IAdventurePlayer player, IEnemy enemy)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            HandlePlayerEnemyCollision(player, enemy);
            HandleMeleeEnemyCollisions(player, enemy);
        }

        private static void HandlePlayerEnemyCollision(IAdventurePlayer player, IEnemy enemy)
        {
            Rectangle playerRect = player.GetPlayerHitbox();
            Rectangle enemyRect = enemy.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(playerRect, enemyRect);
            if (intersection.IsEmpty) return; //No collision with the player itself
            Direction fromDirection = CollisionDetector.GetCollisionDirection(enemyRect, playerRect, intersection);

            if (enemy is OldMan || enemy is Merchant)
            {
                UnintersectPlayer(player, intersection, fromDirection);
            }
            else if (enemy is WallMaster master)
            {
                master.GrabPlayer(player);
            }
            else
            {
                player.TakeDamage(enemy.GetDamage(), fromDirection);
            }
        }

        private static void HandleMeleeEnemyCollisions(IAdventurePlayer player, IEnemy enemy)
        {
            Rectangle meleeRect = player.GetMeleeHitbox();
            Rectangle enemyRect = enemy.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(meleeRect, enemyRect);
            if (intersection.IsEmpty) return; //No collision with the melee weapon
            Direction fromDirection = GetMeleeDirection(player.GetPlayerHitbox(), enemyRect);
            if (!(enemy is Dodongo || enemy is Gohma))
            {
                enemy.TakeDamage(fromDirection, player.GetDamage());
            }

        }

        private static Direction GetMeleeDirection(Rectangle playerRect, Rectangle enemyRect)
        {
            int horizontalDist = Math.Abs(playerRect.Center.X - enemyRect.Center.X);
            int verticalDist = Math.Abs(playerRect.Center.Y - enemyRect.Center.Y);

            if (playerRect.Center.Y <= enemyRect.Center.Y && verticalDist >= horizontalDist) return Direction.North;
            else if (playerRect.Center.Y >= enemyRect.Center.Y && verticalDist >= horizontalDist) return Direction.South;
            else if (playerRect.Center.X <= enemyRect.Center.X && verticalDist <= horizontalDist) return Direction.West;
            else return Direction.East;
        }

        private static void UnintersectPlayer(IAdventurePlayer player, Rectangle intersection, Direction fromDirection)
        {
            switch (fromDirection)
            {
                case Direction.South:
                    player.UpdateLocation(0, -1 * intersection.Height);
                    break;
                case Direction.North:
                    player.UpdateLocation(0, intersection.Height);
                    break;
                case Direction.East:
                    player.UpdateLocation(-1 * intersection.Width, 0);
                    break;
                case Direction.West:
                    player.UpdateLocation(intersection.Width, 0);
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unimplemented direction," + fromDirection.ToString() + ", in PlayerBlockCollision");
                    break;
            }
        }
    }
}