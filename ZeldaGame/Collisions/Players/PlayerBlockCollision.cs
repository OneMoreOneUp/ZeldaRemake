/*
 * Handles collisions between a player and a block.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Blocks;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;
using ZeldaGame.Sprites;

namespace ZeldaGame.Collisions
{
    public static class PlayerBlockCollision
    {
        public static void HandleCollision(IAdventurePlayer player, IBlock block)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (block == null) throw new ArgumentNullException(nameof(block));

            Rectangle playerRect = player.GetPlayerHitbox();
            Rectangle blockRect = block.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(playerRect, blockRect);
            if (intersection.IsEmpty) return; //No collision with the player itself
            Direction fromDirection = CollisionDetector.GetCollisionDirection(playerRect, blockRect, intersection);

            string blockClass = block.GetType().Name;

            if (!(player is DamagedLink))
            {
                switch (blockClass)
                {
                    case nameof(Door):
                        HandleDoor(player, (Door)block, intersection, fromDirection);
                        break;
                    case nameof(Stair):
                        HandleStairs(player, (Stair)block);
                        break;
                    case nameof(Ladder):
                        ((Ladder)block).Transition();
                        break;
                    case nameof(Water):
                        HandleWater(player, (Water)block, intersection, fromDirection);
                        break;

                    case nameof(Divider):
                        if (block.IsRigid())
                        {

                            if (!((Divider)block).Moveable())
                            {
                                HandleStepLadder(player, (Divider)block, intersection, fromDirection);
                            }
                            else
                            {
                                ((Divider)block).LinkCollision(fromDirection);
                                UnintersectPlayer(player, intersection, fromDirection);
                            }
                        }
                        break;
                    default:
                        if (block.IsRigid())
                        {
                            UnintersectPlayer(player, intersection, fromDirection);
                        }
                        break;
                }
            }
            else
            {
                if (block.IsRigid())
                {
                    UnintersectPlayer(player, intersection, fromDirection);
                }
            }
        }

        private static void UnintersectPlayer(IAdventurePlayer player, Rectangle intersection, Direction fromDirection)
        {
            switch (fromDirection)
            {
                case Direction.North:
                    player.UpdateLocation(0, -1 * intersection.Height);
                    break;
                case Direction.South:
                    player.UpdateLocation(0, intersection.Height);
                    break;
                case Direction.West:
                    player.UpdateLocation(-1 * intersection.Width, 0);
                    break;
                case Direction.East:
                    player.UpdateLocation(intersection.Width, 0);
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unimplemented direction," + fromDirection.ToString() + ", in PlayerBlockCollision");
                    break;
            }
        }

        private static void HandleDoor(IAdventurePlayer player, Door door, Rectangle intersection, Direction fromDirection)
        {
            switch (door.GetWallType())
            {
                case WallType.LockedDoor:
                    if (door.LinkCollision(player.GetInventory().ItemAmount(Item.Key) > 0, player.GetInventory().ItemAmount(Item.BossKey) > 0))
                    {
                        door.ChangeType(WallType.OpenDoor);
                        player.GetInventory().RemoveItem(Item.Key);
                    }
                    UnintersectPlayer(player, intersection, fromDirection);
                    break;
                case WallType.OpenDoor:
                case WallType.BombedDoor:
                    door.Transition();
                    break;
                default:
                    UnintersectPlayer(player, intersection, fromDirection);
                    break;
            }
        }

        private static void HandleStairs(IAdventurePlayer player, Stair stair)
        {
            stair.Transition(player.GetLocation());
            SoundFactory.Instance.GetSound(Sound.Stairs).Play();
        }

        private static void HandleWater(IAdventurePlayer player, Water water, Rectangle intersection, Direction fromDirection)
        {
            /*
             * If there is a raft at the same position as water, then do nothing
             * Else if the player has a raft in their inventory, then place a raft
             * Else, unintersect player
             */
            bool found = false;
            List<IGameObject> objects = new List<IGameObject>(GameObjectManager.GetGameObjects());
            foreach (IGameObject Object in objects)
            {
                if (Object is Projectiles.Particles.Raft raft && raft.GetLocation() == water.GetLocation())
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {

                if (player.GetInventory().ItemAmount(Item.Raft) > 0)
                {
                    GameObjectManager.AddGameObject(new Projectiles.Particles.Raft(water.GetLocation(), player, new ParticleDataManager(water.GetData().GetXPath())));
                    player.GetInventory().RemoveItem(Item.Raft);
                }
                else
                {
                    UnintersectPlayer(player, intersection, fromDirection);
                }
            }

        }
        private static void HandleStepLadder(IAdventurePlayer player, Divider block, Rectangle intersection, Direction fromDirection)
        {

            bool found = false;
            List<IGameObject> objects = new List<IGameObject>(GameObjectManager.GetGameObjects());
            foreach (IGameObject Object in objects)
            {
                if (Object is Projectiles.Particles.Stepladder ladder && ladder.GetLocation() == block.GetLocation())
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {

                if (player.GetInventory().ItemAmount(Item.StepLadder) > 0)
                {
                    GameObjectManager.AddGameObject(new Projectiles.Particles.Stepladder(block.GetLocation(), player, new ParticleDataManager(block.GetData().GetXPath())));
                    player.GetInventory().RemoveItem(Item.StepLadder);
                }
                else
                {
                    UnintersectPlayer(player, intersection, fromDirection);
                }
            }

        }
    }
}
