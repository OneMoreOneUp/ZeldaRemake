/*
 * Handles collisions between an enemy and an item.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class EnemyItemCollision
    {
        public static void HandleCollision(IEnemy enemy, IItem item)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            if (item == null) throw new ArgumentNullException(nameof(item));

            //Nothing happens
        }
    }
}
