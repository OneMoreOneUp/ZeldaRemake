/*
 * Handles collisions between a player and an enemy.
 * 
 * Author: Matthew Crabtree
 */

using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public static class PlayerPlayerCollision
    {
        public static void HandleCollision(IAdventurePlayer player1, IAdventurePlayer player2)
        {
            if (player1 == null) throw new ArgumentNullException(nameof(player1));
            if (player2 == null) throw new ArgumentNullException(nameof(player2));

            //Do nothing
        }
    }
}
