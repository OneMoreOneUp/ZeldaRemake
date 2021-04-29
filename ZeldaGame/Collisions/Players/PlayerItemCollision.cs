/*
 * Handles collisions between a player and an item.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;
using ZeldaGame.Items;

namespace ZeldaGame.Collisions
{
    public static class PlayerItemCollision
    {
        public static void HandleCollision(IAdventurePlayer player, IItem item)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!player.GetPlayerHitbox().Intersects(item.GetHitbox())) return; //No collision with the player itself

            string itemClass = item.GetType().Name;

            switch (itemClass)
            {
                case nameof(TriForcePiece):

                    //player picks up triforce dun dun dun
                    item.PlayerPickUp(player);
                    break;
                default:
                    item.PlayerPickUp(player);
                    break;
            }
        }
    }
}
