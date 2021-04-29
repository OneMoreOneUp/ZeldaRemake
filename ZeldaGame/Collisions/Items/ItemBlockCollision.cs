/*
 * Handles collisions between an item and a block.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;
using ZeldaGame.Items;

namespace ZeldaGame.Collisions
{
    public static class ItemBlockCollision
    {
        public static void HandleCollision(IItem item, IBlock block)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (block == null) throw new ArgumentNullException(nameof(block));

            Rectangle itemRect = item.GetHitbox();
            Rectangle blockRect = block.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(itemRect, blockRect);
            Direction fromDirection = CollisionDetector.GetCollisionDirection(itemRect, blockRect, intersection);

            if (block.IsRigid())
            {
                UnintersectItem(item, intersection, fromDirection);
                if (item is Fairy fairy)
                {
                    fairy.ChangeDirection();
                }
            }
        }

        private static void UnintersectItem(IItem item, Rectangle intersection, Direction fromDirection)
        {
            switch (fromDirection)
            {
                case Direction.North:
                    item.UpdateLocation(0, -1 * intersection.Height);
                    break;
                case Direction.South:
                    item.UpdateLocation(0, intersection.Height);
                    break;
                case Direction.West:
                    item.UpdateLocation(-1 * intersection.Width, 0);
                    break;
                case Direction.East:
                    item.UpdateLocation(intersection.Width, 0);
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unimplemented direction," + fromDirection.ToString() + ",in PlayerBlockCollision");
                    break;
            }
        }
    }
}
