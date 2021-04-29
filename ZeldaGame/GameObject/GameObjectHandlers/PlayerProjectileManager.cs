using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class PlayerProjectileManager
    {
        private static List<IPlayerProjectile> playerProjectiles = new List<IPlayerProjectile>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddPlayerProjectile(IPlayerProjectile playerProjectile)
        {
            if (!playerProjectiles.Contains(playerProjectile))
            {
                playerProjectiles.Add(playerProjectile);
            }
            else
            {
                Trace.WriteLine("[WARNING] PlayerProjectile could not be added to game objects. It already exists.");
            }
        }

        public static void AddPlayerProjectile(List<IPlayerProjectile> playerProjectiles)
        {
            if (playerProjectiles == null) throw new ArgumentNullException(nameof(playerProjectiles));

            foreach (IPlayerProjectile playerProjectile in playerProjectiles)
            {
                AddPlayerProjectile(playerProjectile);
            }
        }

        public static void RemovePlayerProjectile(IPlayerProjectile playerProjectile)
        {
            if (playerProjectiles.Contains(playerProjectile))
            {
                playerProjectiles.Remove(playerProjectile);
            }
            else
            {
                Trace.WriteLine("[WARNING] PlayerProjectile could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemovePlayerProjectile(List<IPlayerProjectile> playerProjectiles)
        {
            if (playerProjectiles == null) throw new ArgumentNullException(nameof(playerProjectiles));

            foreach (IPlayerProjectile playerProjectile in playerProjectiles)
            {
                RemovePlayerProjectile(playerProjectile);
            }
        }

        public static List<IPlayerProjectile> GetPlayerProjectiles()
        {
            return playerProjectiles;
        }

        public static void RemoveAllPlayerProjectiles()
        {
            playerProjectiles = new List<IPlayerProjectile>();
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IPlayerProjectile> copyProjectiles = new List<IPlayerProjectile>(playerProjectiles);
                foreach (IPlayerProjectile playerProjectile in copyProjectiles)
                {
                    playerProjectile.Update(gametime);
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
            foreach (IPlayerProjectile playerProjectile in playerProjectiles)
            {
                playerProjectile.Draw(spriteBatch, Color.White);
            }
        }

        public static bool Contains(IPlayerProjectile playerProjectile)
        {
            return playerProjectiles.Contains(playerProjectile);
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
