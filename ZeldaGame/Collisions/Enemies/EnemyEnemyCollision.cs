/*
 * Handles collisions between an enemy and a enemy.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class EnemyEnemyCollision
    {
        public static void HandleCollision(IEnemy enemy1, IEnemy enemy2)
        {
            if (enemy1 == null) throw new ArgumentNullException(nameof(enemy1));
            if (enemy2 == null) throw new ArgumentNullException(nameof(enemy2));

            //Nothing happens
        }
    }
}
