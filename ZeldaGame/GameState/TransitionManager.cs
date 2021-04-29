using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.HUD;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;

namespace ZeldaGame.GameState
{
    class TransitionManager
    {
        private static List<IBlock> blocks = new List<IBlock>();
        private static Room roomStart;
        private static Room roomNext;
        private static Direction directionTrans;
        public static bool transitionDone;
        public const int leftRightOffset = 256, topOffSet = 232, botOffSet = 176;
        private static int offsetXAmt, offsetYAmt, offsetChange;

        public static void DrawRoomTransition(SpriteBatch spriteBatch, Level currentLevel)
        {
            directionTrans = currentLevel.moveNext;
            transitionDone = false;

            SetOffsetAmt();

            if (currentLevel.CurrentRoom.underground)
            {
                //if its underground just enter no camera slide
                CreateFinalRoom(currentLevel);

            }
            else
            {
                //if its not underground need to make emptyRoom and emptyNext room for transition
                if (!UpdateFrame())
                {
                    roomStart = new Room(currentLevel.RoomPath[currentLevel.OldPoint], currentLevel);
                    roomNext = new Room(currentLevel.RoomPath[currentLevel.CurrentPoint], currentLevel);

                    CreateNextRoomTransition(spriteBatch);
                }
                else
                {
                    //once the transition is done create the final new room
                    CreateFinalRoom(currentLevel);
                }
            }

        }

        public static void DrawMenuTransition(SpriteBatch spriteBatch, MenuDisplay menu, bool menuPlayer, HUDDisplay hud)
        {
            //MenuDisplay menu is necessary to handle changing the transition Boolean

            directionTrans = Direction.South;
            transitionDone = false;

            SetOffsetAmt();

            if (menu.Down && offsetChange == 0)
            {
                //menuPlayer true == player1
                //menuPlayer false == player2
                menu.TransitionFrames(menuPlayer);
                hud.TransitionFrame(menuPlayer);
            }

            menu.TransitionDisplay();

            menu.Draw(spriteBatch, Color.White);
        }

        public static bool CheckTransitionMenu()
        {
            return (Math.Abs(offsetYAmt) - offsetChange) > 0;
        }

        public static void ResetTransition()
        {
            offsetChange = 0;
            offsetXAmt = 0;
            offsetYAmt = 0;
        }

        private static void CreateNextRoomTransition(SpriteBatch spriteBatch)
        {
            RoomOffsetStart();

            roomStart.Build();
            roomStart.InitializeDoors();

            RoomOffsetNext();

            roomNext.Build();
            roomNext.InitializeDoors();

            roomStart.EnterTransition();
            roomNext.EnterTransition();

            blocks = new List<IBlock>(BlockManager.GetBlocks());

            foreach (IBlock block in blocks)
            {
                block.Draw(spriteBatch, Color.White);
            }

            BlockManager.RemoveAllBlocks();
        }

        private static void CreateFinalRoom(Level currentLevel)
        {
            ResetTransition();
            currentLevel.transitionRoom = false;
            SetOffsetAmt();

            ResetOffset();
            directionTrans = LevelHandler.OppositeDirection(directionTrans);
            //transition has been completed build the whole room and move player
            PlayerManager.AddPlayer(currentLevel.PlayerList);

            if (currentLevel.CurrentRoom.FirstBuild)
            {
                currentLevel.CurrentRoom.Build();
            }
            currentLevel.CurrentRoom.Enter();

            Point location = LevelHandler.NextPlayerLocation(directionTrans, currentLevel.CurrentRoom);
            foreach (IAdventurePlayer player in currentLevel.PlayerList)
            {
                player.SetLocation(location);
            }
        }

        //This function determines how much offset is necessary to move room
        private static void SetOffsetAmt()
        {
            switch (directionTrans)
            {
                case Direction.East:
                    RoomOffsetEast();
                    break;
                case Direction.West:
                    RoomOffsetWest();
                    break;
                case Direction.North:
                    RoomOffsetNorth();
                    break;
                case Direction.South:
                    RoomOffsetSouth();
                    break;
                case Direction.Down:
                case Direction.Up:
                    RoomOffsetUnder();
                    break;
                default:
                    break;
            }
        }

        //This function lets us know the transition is done when checkoffset is true
        public static bool UpdateFrame()
        {
            if (!transitionDone)
            {
                if (CheckOffset())
                {
                    transitionDone = true;
                }
                else
                {
                    offsetChange += 1;
                }
            }

            return transitionDone;
        }

        //This function checks if the offsetAmt - change is less than or equal to 0
        public static bool CheckOffset()
        {
            //offsetXAmt and offsetYAmt subtracting change
            return (Math.Abs(offsetYAmt + offsetXAmt) - offsetChange) < 0;
        }

        private static void RoomOffsetNorth()
        {
            offsetYAmt = 176;
            offsetXAmt = 0;
        }

        private static void RoomOffsetSouth()
        {
            offsetYAmt = -176;
            offsetXAmt = 0;
        }

        private static void RoomOffsetEast()
        {
            offsetXAmt = -256;
            offsetYAmt = 0;
        }

        private static void RoomOffsetWest()
        {
            offsetXAmt = 256;
            offsetYAmt = 0;
        }

        private static void RoomOffsetUnder()
        {
            offsetXAmt = 0;
            offsetYAmt = 0;
        }


        public static void RoomOffsetStart()
        {
            //this is the amount of pixels to set the offset to
            //startRoom is moving away from frame (negative)
            //nextRoom is moving toward frame(positive)
            switch (directionTrans)
            {
                case Direction.East:
                    RoomBuilder.SetTransitionOffset(0 + offsetChange, 0);
                    break;
                case Direction.West:
                    RoomBuilder.SetTransitionOffset(0 - offsetChange, 0);
                    break;
                case Direction.North:
                    RoomBuilder.SetTransitionOffset(0, 0 - offsetChange);
                    break;
                case Direction.South:
                    RoomBuilder.SetTransitionOffset(0, 0 + offsetChange);
                    break;
                case Direction.Down:
                    RoomBuilder.SetTransitionOffset(0, 0 + offsetChange);
                    break;
                case Direction.Up:
                    RoomBuilder.SetTransitionOffset(0, 0 - offsetChange);
                    break;
                default:
                    break;
            }
        }

        public static void RoomOffsetNext()
        {
            //this is the amount of pixels to set the offset to
            //startRoom is moving away from frame (negative)
            //nextRoom is moving toward frame(positive)
            switch (directionTrans)
            {
                case Direction.East:
                    RoomBuilder.SetTransitionOffset(-256 + offsetChange, 0);
                    break;
                case Direction.West:
                    RoomBuilder.SetTransitionOffset(256 - offsetChange, 0);
                    break;
                case Direction.North:
                    RoomBuilder.SetTransitionOffset(0, 176 - offsetChange);
                    break;
                case Direction.South:
                    RoomBuilder.SetTransitionOffset(0, -176 + offsetChange);
                    break;
                case Direction.Down:
                    RoomBuilder.SetTransitionOffset(0, -176 + offsetChange);
                    break;
                case Direction.Up:
                    RoomBuilder.SetTransitionOffset(0, 176 - offsetChange);
                    break;
                default:
                    break;
            }
        }

        public static void ResetOffset()
        {
            RoomBuilder.SetTransitionOffset(0, 0);
        }
    }
}
