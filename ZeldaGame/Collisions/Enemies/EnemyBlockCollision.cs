/*
 * Handles collisions between an enemy and a block.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using ZeldaGame.Blocks;
using ZeldaGame.Enemies;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;


namespace ZeldaGame.Collisions
{
    public static class EnemyBlockCollision
    {
        public static void HandleCollision(IEnemy enemy, IBlock block)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            if (block == null) throw new ArgumentNullException(nameof(block));

            Rectangle enemyRect = enemy.GetHitbox();
            Rectangle blockRect = block.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(enemyRect, blockRect);
            Direction fromDirection = CollisionDetector.GetCollisionDirection(blockRect, enemyRect, intersection);

            string enemyClass = enemy.GetType().Name;
            switch (enemyClass)
            {
                case nameof(DamagedEnemy):
                    HandleCollision(((DamagedEnemy)enemy).GetDecoratedEnemy(), block);
                    break;
                case nameof(Aquamentus):
                case nameof(WallMaster):
                    //Ignore collisions with all blocks
                    break;
                case nameof(Keese):
                case nameof(Vire):
                case nameof(Wizzrobe):
                    if (DoesKeeseCollide(block) && block.IsRigid())
                    {
                        UnintersectEnemy(enemy, intersection, fromDirection);
                        enemy.ChangeDirection(fromDirection);
                    }
                    break;
                default:
                    if (block.IsRigid())
                    {
                        UnintersectEnemy(enemy, intersection, fromDirection);
                        enemy.ChangeDirection(fromDirection);
                    }
                    break;
            }
        }

        private static void UnintersectEnemy(IEnemy enemy, Rectangle intersection, Direction fromDirection)
        {

            switch (fromDirection)
            {
                case Direction.North:

                    enemy.UpdateLocation(0, intersection.Height);
                    break;
                case Direction.South:

                    enemy.UpdateLocation(0, -1 * intersection.Height);
                    break;
                case Direction.West:

                    enemy.UpdateLocation(intersection.Width, 0);
                    break;
                case Direction.East:

                    enemy.UpdateLocation(-1 * intersection.Width, 0);
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unimplemented direction," + fromDirection.ToString() + ",in PlayerBlockCollision");
                    break;
            }
        }

        private static bool DoesKeeseCollide(IBlock block)
        {
            switch (block.GetType().Name)
            {
                case nameof(Door):
                case nameof(Wall):
                    return true;
                default:
                    return false;
            }
        }
    }
}