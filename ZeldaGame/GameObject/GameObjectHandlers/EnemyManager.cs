using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class EnemyManager
    {
        private static List<IEnemy> enemies = new List<IEnemy>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddEnemy(IEnemy enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
            else
            {
                Trace.WriteLine("[WARNING] Enemy could not be added to game objects. It already exists.");
            }
        }

        public static void AddEnemy(List<IEnemy> enemies)
        {
            if (enemies == null) throw new ArgumentNullException(nameof(enemies));

            foreach (IEnemy enemy in enemies)
            {
                AddEnemy(enemy);
            }
        }

        public static void RemoveEnemy(IEnemy enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
            else
            {
                Trace.WriteLine("[WARNING] Enemy could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemoveEnemy(List<IEnemy> enemies)
        {
            if (enemies == null) throw new ArgumentNullException(nameof(enemies));

            foreach (IEnemy enemy in enemies)
            {
                RemoveEnemy(enemy);
            }
        }

        public static List<IEnemy> GetEnemies()
        {
            return enemies;
        }

        public static void RemoveAllEnemies()
        {
            enemies = new List<IEnemy>();
        }

        public static void Update(IEnemy enemy, GameTime gametime)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            enemy.Update(gametime);
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IEnemy> enemiesCopy = new List<IEnemy>(enemies);
                foreach (IEnemy enemy in enemiesCopy)
                {
                    enemy.Update(gametime);
                }
            }
            else if (pauseDuration > 0)
            {
                pauseDuration -= gametime.ElapsedGameTime.TotalSeconds;
                if (pauseDuration <= 0) UnPause();
            }
        }

        public static void DrawAll(SpriteBatch spriteBatch, Color color)
        {
            foreach (IEnemy enemy in enemies)
            {
                enemy.Draw(spriteBatch, color);
            }
        }

        public static bool Contains(IEnemy enemy)
        {
            return enemies.Contains(enemy);
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
