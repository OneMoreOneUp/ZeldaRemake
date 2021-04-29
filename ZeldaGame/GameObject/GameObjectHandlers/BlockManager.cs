using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class BlockManager
    {
        private static List<IBlock> blocks = new List<IBlock>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddBlock(IBlock block)
        {
            if (!blocks.Contains(block))
            {
                blocks.Add(block);
            }
            else
            {
                Trace.WriteLine("[WARNING] Block could not be added to game objects. It already exists.");
            }
        }

        public static void AddBlock(List<IBlock> blocks)
        {
            if (blocks == null) throw new ArgumentNullException(nameof(blocks));

            foreach (IBlock block in blocks)
            {
                AddBlock(block);
            }
        }

        public static void RemoveBlock(IBlock block)
        {
            if (blocks.Contains(block))
            {
                blocks.Remove(block);
            }
            else
            {
                Trace.WriteLine("[WARNING] Block could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemoveBlock(List<IBlock> blocks)
        {
            if (blocks == null) throw new ArgumentNullException(nameof(blocks));

            foreach (IBlock block in blocks)
            {
                RemoveBlock(block);
            }
        }

        public static List<IBlock> GetBlocks()
        {
            return blocks;
        }

        public static void RemoveAllBlocks()
        {
            blocks = new List<IBlock>();
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IBlock> blocksCopy = new List<IBlock>(blocks);
                foreach (IBlock block in blocksCopy)
                {
                    block.Update(gametime);
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
            foreach (IBlock block in blocks)
            {
                block.Draw(spriteBatch, Color.White);
            }
        }

        public static bool Contains(IBlock block)
        {
            return blocks.Contains(block);
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
