using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class EnemyProjectileManager
    {
        private static List<IEnemyProjectile> enemyProjectiles = new List<IEnemyProjectile>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddEnemyProjectile(IEnemyProjectile enemyProjectile)
        {
            if (!enemyProjectiles.Contains(enemyProjectile))
            {
                enemyProjectiles.Add(enemyProjectile);
            }
            else
            {
                Trace.WriteLine("[WARNING] EnemyProjectile could not be added to game objects. It already exists.");
            }
        }

        public static void AddEnemyProjectile(List<IEnemyProjectile> enemyProjectiles)
        {
            if (enemyProjectiles == null) throw new ArgumentNullException(nameof(enemyProjectiles));

            foreach (IEnemyProjectile enemyProjectile in enemyProjectiles)
            {
                AddEnemyProjectile(enemyProjectile);
            }
        }

        public static void RemoveEnemyProjectile(IEnemyProjectile enemyProjectile)
        {
            if (enemyProjectiles.Contains(enemyProjectile))
            {
                enemyProjectiles.Remove(enemyProjectile);
            }
            else
            {
                Trace.WriteLine("[WARNING] EnemyProjectile could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemoveEnemyProjectile(List<IEnemyProjectile> enemyProjectiles)
        {
            if (enemyProjectiles == null) throw new ArgumentNullException(nameof(enemyProjectiles));

            foreach (IEnemyProjectile enemyProjectile in enemyProjectiles)
            {
                RemoveEnemyProjectile(enemyProjectile);
            }
        }

        public static List<IEnemyProjectile> GetEnemyProjectiles()
        {
            return enemyProjectiles;
        }

        public static void RemoveAllEnemyProjectiles()
        {
            enemyProjectiles = new List<IEnemyProjectile>();
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IEnemyProjectile> copyProjectiles = new List<IEnemyProjectile>(enemyProjectiles);
                foreach (IEnemyProjectile enemyProjectile in copyProjectiles)
                {
                    enemyProjectile.Update(gametime);
                }
            }
            else if (pauseDuration > 0)
            {
                pauseDuration -= gametime.ElapsedGameTime.TotalSeconds;
                if (pauseDuration <= 0) UnPause();
            }
        }

        public static void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (IEnemyProjectile enemyProjectile in enemyProjectiles)
            {
                enemyProjectile.Draw(spriteBatch, Color.White);
            }
        }

        public static bool Contains(IEnemyProjectile enemyProjectile)
        {
            return enemyProjectiles.Contains(enemyProjectile);
        }

        public static void Pause(double duration = 0)
        {
            paused = true;
            pauseDuration = duration;
        }

        public static void UnPause()
        {
            paused = false;
        }
    }
}
