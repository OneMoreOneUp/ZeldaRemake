using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class ItemManager
    {
        private static List<IItem> items = new List<IItem>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddItem(IItem item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
            else
            {
                Trace.WriteLine("[WARNING] Item could not be added to game objects. It already exists.");
            }
        }

        public static void AddItem(List<IItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (IItem item in items)
            {
                AddItem(item);
            }
        }

        public static void RemoveItem(IItem item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
            else
            {
                Trace.WriteLine("[WARNING] Item could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemoveItem(List<IItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (IItem item in items)
            {
                RemoveItem(item);
            }
        }

        public static List<IItem> GetItems()
        {
            return items;
        }

        public static void RemoveAllItems()
        {
            items = new List<IItem>();
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IItem> itemsCopy = new List<IItem>(items);
                foreach (IItem item in itemsCopy)
                {
                    item.Update(gametime);
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
            foreach (IItem item in items)
            {
                item.Draw(spriteBatch, Color.White);
            }
        }

        public static bool Contains(IItem item)
        {
            return items.Contains(item);
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
