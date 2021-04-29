/*
 * Handles collisions between an item and an item.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class ItemItemCollision
    {
        public static void HandleCollision(IItem item1, IItem item2)
        {
            if (item1 == null) throw new ArgumentNullException(nameof(item1));
            if (item2 == null) throw new ArgumentNullException(nameof(item2));

            //Nothing happens
        }
    }
}
