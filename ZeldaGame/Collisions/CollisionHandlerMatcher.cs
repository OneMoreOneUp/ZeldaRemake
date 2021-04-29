/*
 * Detects and handles collisions between any game objects.
 * 
 * Author: Matthew Crabtree
 */

using System;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class CollisionHandlerMatcher
    {
        public static void MatchCollisionHandler(IGameObject object1, IGameObject object2)
        {
            if (object1 == null) throw new ArgumentNullException(nameof(object1));
            if (object2 == null) throw new ArgumentNullException(nameof(object2));

            if (object1 is IAdventurePlayer player)
            {
                PlayerCollision(player, object2);
            }
            else if (object1 is IEnemy enemy)
            {
                EnemyCollision(enemy, object2);
            }
            else if (object1 is IItem item)
            {
                ItemCollision(item, object2);
            }
            else if (object1 is IPlayerProjectile playerProjectile)
            {
                PlayerProjectileCollision(playerProjectile, object2);
            }
            else if (object1 is IEnemyProjectile enemyProjectile)
            {
                EnemyProjectileCollision(enemyProjectile, object2);
            }
            else if (object1 is IBlock block)
            {
                BlockCollision(block, object2);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object1.GetType().Name);
            }
        }

        private static void PlayerCollision(IAdventurePlayer player, IGameObject object2)
        {
            if (object2 is IAdventurePlayer player2)
            {
                PlayerPlayerCollision.HandleCollision(player, player2);
            }
            else if (object2 is IEnemy enemy)
            {
                PlayerEnemyCollision.HandleCollision(player, enemy);
            }
            else if (object2 is IItem item)
            {
                PlayerItemCollision.HandleCollision(player, item);
            }
            else if (object2 is IPlayerProjectile playerProjectile)
            {
                PlayerPlayerProjectileCollision.HandleCollision(player, playerProjectile);
            }
            else if (object2 is IEnemyProjectile enemyProjectile)
            {
                PlayerEnemyProjectileCollision.HandleCollision(player, enemyProjectile);
            }
            else if (object2 is IBlock block)
            {
                PlayerBlockCollision.HandleCollision(player, block);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object2.GetType().Name);
            }
        }

        private static void EnemyCollision(IEnemy enemy, IGameObject object2)
        {
            if (object2 is IAdventurePlayer player)
            {
                PlayerEnemyCollision.HandleCollision(player, enemy);
            }
            else if (object2 is IEnemy enemy2)
            {
                EnemyEnemyCollision.HandleCollision(enemy, enemy2);
            }
            else if (object2 is IItem item)
            {
                EnemyItemCollision.HandleCollision(enemy, item);
            }
            else if (object2 is IPlayerProjectile playerProjectile)
            {
                EnemyPlayerProjectileCollision.HandleCollision(enemy, playerProjectile);
            }
            else if (object2 is IEnemyProjectile enemyProjectile)
            {
                EnemyEnemyProjectileCollision.HandleCollision(enemy, enemyProjectile);
            }
            else if (object2 is IBlock block)
            {
                EnemyBlockCollision.HandleCollision(enemy, block);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object2.GetType().Name);
            }
        }

        private static void ItemCollision(IItem item, IGameObject object2)
        {
            if (object2 is IAdventurePlayer player)
            {
                PlayerItemCollision.HandleCollision(player, item);
            }
            else if (object2 is IEnemy enemy)
            {
                EnemyItemCollision.HandleCollision(enemy, item);
            }
            else if (object2 is IItem item2)
            {
                ItemItemCollision.HandleCollision(item, item2);
            }
            else if (object2 is IPlayerProjectile playerProjectile)
            {
                ItemPlayerProjectileCollision.HandleCollision(item, playerProjectile);
            }
            else if (object2 is IEnemyProjectile enemyProjectile)
            {
                ItemEnemyProjectileCollision.HandleCollision(item, enemyProjectile);
            }
            else if (object2 is IBlock block)
            {
                ItemBlockCollision.HandleCollision(item, block);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object2.GetType().Name);
            }
        }

        private static void PlayerProjectileCollision(IPlayerProjectile playerProjectile, IGameObject object2)
        {
            if (object2 is IAdventurePlayer player)
            {
                PlayerPlayerProjectileCollision.HandleCollision(player, playerProjectile);
            }
            else if (object2 is IEnemy enemy)
            {
                EnemyPlayerProjectileCollision.HandleCollision(enemy, playerProjectile);
            }
            else if (object2 is IItem item)
            {
                ItemPlayerProjectileCollision.HandleCollision(item, playerProjectile);
            }
            else if (object2 is IPlayerProjectile playerProjectile2)
            {
                PlayerProjectilePlayerProjectileCollision.HandleCollision(playerProjectile, playerProjectile2);
            }
            else if (object2 is IEnemyProjectile enemyProjectile)
            {
                PlayerProjectileEnemyProjectileCollision.HandleCollision(playerProjectile, enemyProjectile);
            }
            else if (object2 is IBlock block)
            {
                PlayerProjectileBlockCollision.HandleCollision(playerProjectile, block);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object2.GetType().Name);
            }
        }

        private static void EnemyProjectileCollision(IEnemyProjectile enemyProjectile, IGameObject object2)
        {
            if (object2 is IAdventurePlayer player)
            {
                PlayerEnemyProjectileCollision.HandleCollision(player, enemyProjectile);
            }
            else if (object2 is IEnemy enemy)
            {
                EnemyEnemyProjectileCollision.HandleCollision(enemy, enemyProjectile);
            }
            else if (object2 is IItem item)
            {
                ItemEnemyProjectileCollision.HandleCollision(item, enemyProjectile);
            }
            else if (object2 is IPlayerProjectile playerProjectile)
            {
                PlayerProjectileEnemyProjectileCollision.HandleCollision(playerProjectile, enemyProjectile);
            }
            else if (object2 is IEnemyProjectile enemyProjectile2)
            {
                EnemyProjectileEnemyProjectileCollision.HandleCollision(enemyProjectile, enemyProjectile2);
            }
            else if (object2 is IBlock block)
            {
                EnemyProjectileBlockCollision.HandleCollision(enemyProjectile, block);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object2.GetType().Name);
            }
        }

        private static void BlockCollision(IBlock block, IGameObject object2)
        {
            if (object2 is IAdventurePlayer player)
            {
                PlayerBlockCollision.HandleCollision(player, block);
            }
            else if (object2 is IEnemy enemy)
            {
                EnemyBlockCollision.HandleCollision(enemy, block);
            }
            else if (object2 is IItem item)
            {
                ItemBlockCollision.HandleCollision(item, block);
            }
            else if (object2 is IPlayerProjectile playerProjectile)
            {
                PlayerProjectileBlockCollision.HandleCollision(playerProjectile, block);
            }
            else if (object2 is IEnemyProjectile enemyProjectile)
            {
                EnemyProjectileBlockCollision.HandleCollision(enemyProjectile, block);
            }
            else if (object2 is IBlock block2)
            {
                BlockBlockCollision.HandleCollision(block, block2);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented collision interface from: " + object2.GetType().Name);
            }
        }
    }
}
