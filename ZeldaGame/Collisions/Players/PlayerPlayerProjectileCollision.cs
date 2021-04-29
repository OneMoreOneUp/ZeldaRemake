/*
 * Handles collisions between a player and a player projectile.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;
using ZeldaGame.PlayerProjectiles;

namespace ZeldaGame.Collisions
{
    public static class PlayerPlayerProjectileCollision
    {
        public static void HandleCollision(IAdventurePlayer player, IPlayerProjectile playerProjectile)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (playerProjectile == null) throw new ArgumentNullException(nameof(playerProjectile));

            Rectangle playerRect = player.GetPlayerHitbox();
            Rectangle playerProjectileRect = playerProjectile.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(playerRect, playerProjectileRect);
            if (intersection.IsEmpty) return; //There is no collision with the player itself
            Direction fromDirection = CollisionDetector.GetCollisionDirection(playerProjectileRect, playerRect, intersection);

            IAdventurePlayer rootPlayer = GetRootPlayer(player);

            string playerProjectileClass = playerProjectile.GetType().Name;
            switch (playerProjectileClass)
            {
                case nameof(Food):
                case nameof(SwordBeam):
                case nameof(Bomb):
                    break;
                case nameof(MagicalBoomerang):
                    HandleBoomerang(player, rootPlayer, playerProjectile, fromDirection, Item.MagicalBoomerang);
                    ((MagicalBoomerang)playerProjectile).StopSound();
                    break;
                case nameof(WoodenBoomerang):
                    HandleBoomerang(player, rootPlayer, playerProjectile, fromDirection, Item.WoodenBoomerang);
                    ((WoodenBoomerang)playerProjectile).StopSound();
                    break;
                default:
                    if (!playerProjectile.IsOwner(rootPlayer))
                    {
                        playerProjectile.CollideRigid();
                        player.TakeDamage(playerProjectile.GetDamage(), fromDirection);
                    }
                    break;
            }
        }

        private static void HandleBoomerang(IAdventurePlayer player, IAdventurePlayer rootPlayer, IPlayerProjectile projectile, Direction fromDirection, Item item)
        {
            if (projectile.IsOwner(rootPlayer) && PlayerProjectileManager.Contains(projectile))
            {
                player.GetInventory().AddItem(item);
                PlayerProjectileManager.RemovePlayerProjectile(projectile);
            }
            else if (!projectile.IsOwner(rootPlayer))
            {
                projectile.CollideRigid();
                player.TakeDamage(projectile.GetDamage(), fromDirection);
            }
        }

        /// <summary>
        /// Gets the decorated/root player if decorated, or just returns the player
        /// </summary>
        /// <returns></returns>
        private static IAdventurePlayer GetRootPlayer(IAdventurePlayer player)
        {
            IAdventurePlayer rootPlayer = player;
            if (player is LinkDead ld)
            {
                rootPlayer = ld.GetDecoratedPlayer();
            }
            else if (player is DamagedLink dl)
            {
                rootPlayer = dl.GetDecoratedPlayer();
            }
            return rootPlayer;
        }
    }
}
