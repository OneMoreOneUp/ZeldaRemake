/*
 * Handles collisions between an block and a block.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class BlockBlockCollision
    {
        public static void HandleCollision(IBlock block1, IBlock block2)
        {
            if (block1 == null) throw new ArgumentNullException(nameof(block1));
            if (block2 == null) throw new ArgumentNullException(nameof(block2));

            //Nothing happens
        }
    }
}
