using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using ZeldaGame.Enums;

namespace ZeldaGame.Levels
{
    class LevelHandler
    {
        //these are the starting X,Y for each dungeon room
        protected internal static int CenterSouthXDirection = 126, CenterSouthYDirection = 190;    //Southern Direction means northern coordinates
        protected internal static int CenterNorthXDirection = 130, CenterNorthYDirection = 96;     //Northern Direction means southern coordinates
        protected internal static int CenterEastXDirection = 213, CenterEastYDirection = 143;      //Eastern Direction means western coordinates
        protected internal static int CenterWestXDirection = 43, CenterWestYDirection = 143;       //Western Direction means eastern coordinates
        protected internal static int DefaultUndergroundXCoord = 58, DefaultUndergroundYCoord = 88;

        //these are the old offset
        //private static int RightSideEntranceOffset = 11 * RoomBuilder.TileEdgeLength / 2;
        //private static int BottomSideEntranceOffset = 16 * RoomBuilder.TileEdgeLength / 2;
        public static Point LinkDungeonStart()
        {
            return new Point(CenterSouthXDirection, CenterSouthYDirection);
        }

        public static Point NextPoint(Point p, Direction d)
        {
            Point nextPoint = p;
            switch (d)
            {
                case Direction.North:
                    nextPoint = new Point(p.X + 1, p.Y);
                    break;
                case Direction.East:
                    nextPoint = new Point(p.X, p.Y + 1);
                    break;
                case Direction.South:
                    nextPoint = new Point(p.X - 1, p.Y);
                    break;
                case Direction.West:
                    nextPoint = new Point(p.X, p.Y - 1);
                    break;
                case Direction.Up:
                    nextPoint = new Point(p.X + 1, p.Y);
                    break;
                case Direction.Down:
                    nextPoint = new Point(p.X - 1, p.Y);
                    break;
            }

            return nextPoint;
        }

        public static Point NextPlayerLocation(Direction d, Room room)
        {
            Point nextPoint = new Point();
            switch (d)
            {
                case Direction.North:
                    nextPoint = new Point(CenterSouthXDirection, CenterSouthYDirection);
                    break;
                case Direction.East:
                    nextPoint = new Point(CenterWestXDirection, CenterWestYDirection);
                    break;
                case Direction.South:
                    nextPoint = new Point(CenterNorthXDirection, CenterNorthYDirection);
                    break;
                case Direction.West:
                    nextPoint = new Point(CenterEastXDirection, CenterEastYDirection);
                    break;
                case Direction.Down:
                    nextPoint = new Point(DefaultUndergroundXCoord, DefaultUndergroundYCoord);
                    break;
                case Direction.Up:
                    nextPoint = room.GetReturnLocation();
                    break;
            }

            return nextPoint;
        }

        public static void ChangeRoom(Direction direction, Level level)
        {
            level.CurrentPoint = LevelHandler.NextPoint(level.CurrentPoint, direction);
            switch (direction)
            {
                case Direction.North:
                case Direction.East:
                case Direction.South:
                case Direction.West:
                    level.LocationMap.TryGetValue(level.CurrentPoint, out level.CurrentRoom);
                    break;
                case Direction.Down:
                    level.UndergroundMap.TryGetValue(level.CurrentPoint, out level.CurrentRoom);
                    break;
                case Direction.Up:
                    level.LocationMap.TryGetValue(level.CurrentPoint, out level.CurrentRoom);
                    break;
            }
        }


        internal static void OpenOppositeDoor(Direction direction, Level level)
        {
            Point point = NextPoint(level.CurrentPoint, direction);
            level.LocationMap.TryGetValue(point, out Room OverRoom);
            OverRoom.GetDoor(OppositeDirection(direction)).ChangeType(WallType.OpenDoor);
        }

        internal static void BombOppositeDoor(Direction direction, Level level)
        {
            Point point = NextPoint(level.CurrentPoint, direction);
            level.LocationMap.TryGetValue(point, out Room OverRoom);
            OverRoom.GetDoor(OppositeDirection(direction)).ChangeType(WallType.BombedDoor);
        }

        public static Direction OppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                _ => Direction.Null,
            };
        }

        public static void BuildMap(string[] index, Level level)
        {
            int startRoomIndex = 0;
            int bossRoomIndex = 0;
            List<int> undergroundRoomIndices = new List<int>();

            for (int i = 0; i < index.Length; i++)
            {

                string item = index[i];

                if (item.Substring(0, 4) == "Room")
                {
                    int coordSplitIndex = item.IndexOf('_', StringComparison.InvariantCulture);
                    int xCoord = int.Parse(item[4..coordSplitIndex], CultureInfo.InvariantCulture);
                    int yCoord = int.Parse(item.Substring(coordSplitIndex + 1), CultureInfo.InvariantCulture);
                    Room room = new Room(item, level);
                    Point location = new Point(xCoord, yCoord);

                    level.RoomPath.Add(location, item);

                    room.InitializeDoors();
                    if (i == (startRoomIndex - 1))
                    {
                        level.CurrentPoint = location;
                        level.StartPoint = location;
                    }
                    else if (i == (bossRoomIndex - 1))
                    {
                        level.BossPoint = location;
                    }
                    foreach (int j in undergroundRoomIndices)
                    {
                        if (i == (j - 1))
                        {
                            level.UndergroundPoints.Add(location);
                            level.UndergroundMap.Add(location, room);
                            room.underground = true;
                        }
                    }
                    if (!undergroundRoomIndices.Contains(i + 1))
                    {
                        level.LocationMap.Add(location, room);
                    }
                }
                else
                {

                    if (item.Substring(0, 1) == "O")
                    {
                        startRoomIndex = int.Parse(item.Substring(2, 2), CultureInfo.InvariantCulture);
                    }
                    else if (item.Substring(0, 1) == "B")
                    {
                        bossRoomIndex = int.Parse(item.Substring(2, 2), CultureInfo.InvariantCulture);
                    }
                    else if (item.Substring(0, 1) == "U")
                    {
                        undergroundRoomIndices.Add(int.Parse(item.Substring(2, 2), CultureInfo.InvariantCulture));
                    }
                }
            }
        }


        public static void MakePointList(Level level)
        {
            level.PointList = new List<Point>();
            foreach (var Pair in level.LocationMap)
            {
                level.PointList.Add(Pair.Key);
            }
        }

    }
}
