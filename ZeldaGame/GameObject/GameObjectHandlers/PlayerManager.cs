using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class PlayerManager
    {
        private static List<IAdventurePlayer> players = new List<IAdventurePlayer>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddPlayer(IAdventurePlayer player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }
            else
            {
                Trace.WriteLine("[WARNING] Player could not be added to game objects. It already exists.");
            }
        }

        public static void AddPlayer(List<IAdventurePlayer> players)
        {
            if (players == null) throw new ArgumentNullException(nameof(players));

            foreach (IAdventurePlayer player in players)
            {
                AddPlayer(player);
            }
        }

        public static void RemovePlayer(IAdventurePlayer player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
            }
            else
            {
                Trace.WriteLine("[WARNING] Player could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemovePlayer(List<IAdventurePlayer> players)
        {
            if (players == null) throw new ArgumentNullException(nameof(players));

            foreach (IAdventurePlayer player in players)
            {
                RemovePlayer(player);
            }
        }

        public static List<IAdventurePlayer> GetPlayers()
        {
            return players;
        }

        public static void RemoveAllPlayers()
        {
            players = new List<IAdventurePlayer>();
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IAdventurePlayer> playersClone = new List<IAdventurePlayer>(players);
                foreach (IAdventurePlayer player in playersClone)
                {
                    player.Update(gametime);
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
            foreach (IAdventurePlayer player in players)
            {
                player.Draw(spriteBatch, color);
            }
        }

        public static bool Contains(IAdventurePlayer player)
        {
            return players.Contains(player);
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
