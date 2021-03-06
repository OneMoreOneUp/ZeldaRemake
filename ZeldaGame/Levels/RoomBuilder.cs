using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using ZeldaGame.Blocks;
using ZeldaGame.Enemies;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Items;

namespace ZeldaGame.Levels
{
    class RoomBuilder
    {
        public const int HUDoffset = 56;
        public const int TileGridWidth = 12, TileGridHeight = 7, TotalTiles = 84;
        public const int TileEdgeLength = 16;
        public const int CornerEdgeLength = 32;
        public const int HorizontalWallLength = 112, VerticalWallLength = 40, WallWidth = 32;
        public static int TransitionXOffset, TransitionYOffset;
        private static string[] lines;
        private static int levelNum;

        private static readonly GameObjectDataManager gameObjectData = new GameObjectDataManager("../../../GameObject/GameObjectData/GameObjectData.xml");
        private static readonly EnemyDataManager enemyData = new EnemyDataManager(gameObjectData.GetXPath());
        private static readonly ItemDataManager itemData = new ItemDataManager(gameObjectData.GetXPath());
        private static readonly BlockDataManager blockData = new BlockDataManager(gameObjectData.GetXPath());

        public static void Level(int num)
        {
            levelNum = num;
        }

        private enum RoomType
        {
            Standard, Underground, OldMan
        }
        private static RoomType roomType;

        public static string[] ReadCSV(string path)
        {
            lines = System.IO.File.ReadAllLines(@"../../../Levels/CSV/Level" + levelNum + "/" + path + ".csv");
            return lines;
        }

        public static void SetLines(string[] L)
        {
            lines = L;
        }

        public static Room BuildRoom(Room room)
        {
            SetRoomType(lines[0].Split(',')[0]);

            switch (roomType)
            {
                case RoomType.Standard:
                    SetStandardTiling(room);
                    SetWalls(room);
                    for (int row = 0; row < TileGridHeight; row++)
                    {
                        HandleItemsAndEnemies(room, lines[row + 2].Split(','), row);
                    }
                    break;
                case RoomType.OldMan:
                    SetWalls(room);
                    SetOldManTiling(room);
                    for (int row = 0; row < TileGridHeight; row++)
                    {
                        HandleItemsAndEnemies(room, lines[row + 2].Split(','), row);
                    }
                    SetRoomText(room, lines[TileGridHeight + 2]);
                    break;
                case RoomType.Underground:
                    SetUndergroundTiling(room);
                    for (int row = 0; row < TileGridHeight + 4; row++)
                    {
                        HandleItemsAndEnemies(room, lines[row + 2].Split(','), row);
                    }
                    break;
            }
            return room;
        }

        public static void SetTransitionOffset(int transitionXOffset, int transitionYOffset)
        {
            TransitionXOffset = transitionXOffset;
            TransitionYOffset = transitionYOffset;
        }

        private static void SetRoomText(Room room, string text)
        {
            string[] splitStr = text.Split(',');
            IFormatProvider format = CultureInfo.CurrentCulture;

            room.roomText = splitStr[0];
            room.textPosition = new Point(int.Parse(splitStr[1], format), int.Parse(splitStr[2], format));
            room.textMaxCharactersPerLine = int.Parse(splitStr[3], format);
        }

        public static void SetLevel(int level)
        {
            levelNum = level;
        }
        private static void SetRoomType(string label)
        {
            levelNum = int.Parse(label.Substring(2, 1), CultureInfo.InvariantCulture);
            switch (label.Substring(4, 1))
            {
                case "S":
                    roomType = RoomType.Standard;
                    break;
                case "U":
                    roomType = RoomType.Underground;
                    break;
                case "T":
                    roomType = RoomType.OldMan;
                    break;
            }

        }

        private static void SetStandardTiling(Room room)
        {
            int constantTileOffset = CornerEdgeLength + TileEdgeLength / 2;
            for (int i = 0; i < TotalTiles; i++)
            {
                int tileXOffset = TileEdgeLength * (i % TileGridWidth) + TransitionXOffset;
                int tileYOffset = TileEdgeLength * (i / TileGridWidth) + HUDoffset + TransitionYOffset;
                room.Tiling.Add(new Floor(new Point((constantTileOffset + tileXOffset), (constantTileOffset + tileYOffset)), levelNum, blockData, false));
            }
        }

        private static void SetOldManTiling(Room room)
        {
            int constantTileOffset = CornerEdgeLength + TileEdgeLength / 2;
            for (int i = 0; i < TotalTiles; i++)
            {
                int tileXOffset = TileEdgeLength * (i % TileGridWidth) + TransitionXOffset;
                int tileYOffset = TileEdgeLength * (i / TileGridWidth) + HUDoffset + TransitionYOffset;
                room.Tiling.Add(new Empty(new Point((constantTileOffset + tileXOffset), (constantTileOffset + tileYOffset)), blockData, false));
            }
        }

        private static void SetUndergroundTiling(Room room)
        {
            int constantTileOffset = TileEdgeLength / 2;
            for (int i = 0; i < TotalTiles; i++)
            {
                int tileXOffset = TileEdgeLength * (i % (TileGridWidth + 2)) + TransitionXOffset;
                int tileYOffset = TileEdgeLength * (i / (TileGridWidth + 2)) + HUDoffset + TransitionYOffset;
                room.Tiling.Add(new Empty(new Point((constantTileOffset + tileXOffset), (constantTileOffset + tileYOffset)), blockData, false));
            }
        }



        public static void SetWalls(Room room)
        {
            room.Walls.AddRange(GetWall(Direction.North));
            room.Walls.AddRange(GetWall(Direction.East));
            room.Walls.AddRange(GetWall(Direction.South));
            room.Walls.AddRange(GetWall(Direction.West));
        }

        private static List<IBlock> GetWall(Direction direction)
        {

            Point[] locationArray = GetWallPoints(direction);
            Point leftWallLocation = locationArray[0];
            leftWallLocation.Y += HUDoffset + TransitionYOffset;
            leftWallLocation.X += TransitionXOffset;
            Point rightWallLocation = locationArray[2];
            rightWallLocation.Y += HUDoffset + TransitionYOffset;
            rightWallLocation.X += TransitionXOffset;
            List<IBlock> WallList = new List<IBlock>();

            switch (direction)
            {
                case Direction.North:
                    WallList.Add(new Wall(leftWallLocation, WallType.TopLeftWall, levelNum, blockData));
                    WallList.Add(new Wall(rightWallLocation, WallType.TopRightWall, levelNum, blockData));
                    break;
                case Direction.South:
                    WallList.Add(new Wall(leftWallLocation, WallType.BottomLeftWall, levelNum, blockData));
                    WallList.Add(new Wall(rightWallLocation, WallType.BottomRightWall, levelNum, blockData));
                    break;
                case Direction.East:
                    WallList.Add(new Wall(leftWallLocation, WallType.RightTopWall, levelNum, blockData));
                    WallList.Add(new Wall(rightWallLocation, WallType.RightBottomWall, levelNum, blockData));
                    break;
                case Direction.West:
                    WallList.Add(new Wall(leftWallLocation, WallType.LeftTopWall, levelNum, blockData));
                    WallList.Add(new Wall(rightWallLocation, WallType.LeftBottomWall, levelNum, blockData));
                    break;
                default:
                    break;
            }

            return WallList;
        }

        private static WallType GetDoorType(string line)
        {
            var type = line switch
            {
                "lk_" => WallType.LockedDoor,
                "op_" => WallType.OpenDoor,
                "no_" => WallType.NoDoor,
                "bb_" => WallType.BombableDoor,
                "cl_" => WallType.ClosedDoor,
                "bd_" => WallType.BossDoor,
                _ => WallType.NoDoor,
            };
            return type;
        }

        public static void SetDoors(string[] entries, Room room)
        {
            room.SetDoor(Direction.North, GetDoor(entries[0], Direction.North, room));
            room.SetDoor(Direction.East, GetDoor(entries[1], Direction.East, room));
            room.SetDoor(Direction.South, GetDoor(entries[2], Direction.South, room));
            room.SetDoor(Direction.West, GetDoor(entries[3], Direction.West, room));
        }

        private static Door GetDoor(string line, Direction direction, Room room)
        {
            WallType type = GetDoorType(line);
            Point doorLocation = GetWallPoints(direction)[1];
            doorLocation.Y += HUDoffset + TransitionYOffset;
            doorLocation.X += TransitionXOffset;
            Door answer = new Door(doorLocation, direction, type, levelNum, room, blockData);
            room.DoorWays.Add(answer);
            return answer;
        }

        private static Point[] GetWallPoints(Direction d)
        {
            Point[] pointArray = new Point[3];
            Point leftWallLocation = new Point();
            Point doorLocation = new Point();
            Point rightWallLocation = new Point();

            int xOffset, yOffset;
            switch (d)
            {
                case Direction.North:
                    yOffset = (WallWidth / 2);
                    leftWallLocation = new Point((HorizontalWallLength / 2), yOffset);
                    doorLocation = new Point((HorizontalWallLength + (WallWidth / 2)), yOffset);
                    rightWallLocation = new Point((HorizontalWallLength + WallWidth + (HorizontalWallLength / 2)), yOffset);
                    break;
                case Direction.East:
                    xOffset = (WallWidth + (TileEdgeLength * TileGridWidth) + (WallWidth / 2));
                    leftWallLocation = new Point(xOffset, (WallWidth + (VerticalWallLength / 2)));
                    doorLocation = new Point(xOffset, (WallWidth + VerticalWallLength + (WallWidth / 2)));
                    rightWallLocation = new Point(xOffset, (2 * WallWidth + VerticalWallLength + (VerticalWallLength / 2)));
                    break;
                case Direction.South:
                    yOffset = (2 * WallWidth + 2 * VerticalWallLength + (WallWidth / 2));
                    leftWallLocation = new Point((HorizontalWallLength / 2), yOffset);
                    doorLocation = new Point((HorizontalWallLength + (WallWidth / 2)), yOffset);
                    rightWallLocation = new Point((HorizontalWallLength + WallWidth + (HorizontalWallLength / 2)), yOffset);
                    break;
                case Direction.West:
                    xOffset = (WallWidth / 2);
                    leftWallLocation = new Point(xOffset, (WallWidth + (VerticalWallLength / 2)));
                    doorLocation = new Point(xOffset, (WallWidth + VerticalWallLength + (WallWidth / 2)));
                    rightWallLocation = new Point(xOffset, (2 * WallWidth + VerticalWallLength + (VerticalWallLength / 2)));
                    break;
            }

            pointArray[0] = leftWallLocation;
            pointArray[1] = doorLocation;
            pointArray[2] = rightWallLocation;
            return pointArray;
        }

        private static Point GetLocation(int row, int col)
        {
            var location = roomType switch
            {
                RoomType.Standard => new Point((CornerEdgeLength + TileEdgeLength * col + TileEdgeLength / 2), (CornerEdgeLength + TileEdgeLength * row + TileEdgeLength / 2)),
                RoomType.Underground => new Point((TileEdgeLength * col + TileEdgeLength / 2), (TileEdgeLength * row + TileEdgeLength / 2)),
                RoomType.OldMan => new Point((CornerEdgeLength + TileEdgeLength * col + TileEdgeLength / 2), (CornerEdgeLength + TileEdgeLength * row + TileEdgeLength / 2)),
                _ => new Point(),
            };
            return location;
        }

        private static void HandleItemsAndEnemies(Room room, string[] entries, int row)
        {
            for (int col = 0; col < entries.Length; col++)
            {
                string entry = entries[col];
                while (entry.Substring(0, 1) != "_")
                {
                    Point location = GetLocation(row, col);
                    location.Y += HUDoffset + TransitionYOffset;
                    location.X += TransitionXOffset;
                    string sprite = entry.Substring(0, 2);
                    switch (sprite)
                    {
                        case "ls":
                            room.SecondaryTiling.Add(new LeftStatue(location, levelNum, blockData));
                            break;
                        case "l2":
                            room.SecondaryTiling.Add(new LeftStatue(location, levelNum, blockData, 2));
                            break;
                        case "rs":
                            room.SecondaryTiling.Add(new RightStatue(location, levelNum, blockData));
                            break;
                        case "r2":
                            room.SecondaryTiling.Add(new RightStatue(location, levelNum, blockData, 2));
                            break;
                        case "bl":
                            room.SecondaryTiling.Add(new Divider(location, levelNum, room, blockData));
                            break;
                        case "bj":
                            room.MoveableBlock = new Divider(location, levelNum, room, blockData, true, true);
                            room.SecondaryTiling.Add(room.MoveableBlock);
                            room.movedBlock = false;
                            break;
                        case "pt":
                            room.SecondaryTiling.Add(new Empty(location, blockData, false));
                            break;
                        case "pr":
                            room.SecondaryTiling.Add(new Empty(location, blockData));
                            break;
                        case "b2":
                            room.SecondaryTiling.Add(new HalfDivider(location, levelNum, room, blockData));
                            break;
                        case "sr":
                            room.StairCase = new Stair(location, levelNum, room, blockData, false);
                            room.SecondaryTiling.Add(room.StairCase);
                            break;
                        case "ld":
                            room.SecondaryTiling.Add(new Ladder(location, room, blockData, false));
                            break;
                        case "lT":
                            room.UpLadder = new Ladder(location, room, blockData, false, true);
                            room.SecondaryTiling.Add(room.UpLadder);
                            break;
                        case "wa":
                            room.SecondaryTiling.Add(new Water(location, levelNum, blockData));
                            break;
                        case "br":
                            room.SecondaryTiling.Add(new Brick(location, blockData));
                            break;
                        case "dt":
                            room.SecondaryTiling.Add(new Dirt(location, levelNum, blockData, false));
                            break;
                        case "d2":
                            room.SecondaryTiling.Add(new Dirt(location, levelNum, blockData, false, 2));
                            break;
                        case "d3":
                            room.SecondaryTiling.Add(new Dirt(location, levelNum, blockData, false, 3));
                            break;
                        case "fl":
                            room.SecondaryTiling.Add(new Flame(location, blockData));
                            break;
                        case "aq":
                            room.Enemies.Add(new EnemySpawn(location, new Aquamentus(location, enemyData), enemyData));
                            break;
                        case "dd":
                            room.Enemies.Add(new EnemySpawn(location, new Dodongo(location, enemyData), enemyData));
                            break;
                        case "go":
                            room.Enemies.Add(new EnemySpawn(location, new Goriya(location, enemyData, "Red"), enemyData));
                            break;
                        case "ro":
                            room.Enemies.Add(new EnemySpawn(location, new Rope(location, enemyData), enemyData));
                            break;
                        case "ks":
                            room.Enemies.Add(new EnemySpawn(location, new Keese(location, enemyData, "Blue"), enemyData));
                            break;
                        case "RS":
                            room.Enemies.Add(new EnemySpawn(location, new Keese(location, enemyData, "Red"), enemyData));
                            break;
                        case "sg":
                            room.Enemies.Add(new EnemySpawn(location, new Gel(location, enemyData, ""), enemyData));
                            break;
                        case "BG":
                            room.Enemies.Add(new EnemySpawn(location, new Gel(location, enemyData, "Black"), enemyData));
                            break;
                        case "sf":
                            room.Enemies.Add(new EnemySpawn(location, new Stalfos(location, enemyData), enemyData));
                            break;
                        case "wm":
                            room.Enemies.Add(new EnemySpawn(location, new WallMaster(location, enemyData, room), enemyData));
                            break;
                        case "gb":
                            room.Enemies.Add(new EnemySpawn(location, new Goriya(location, enemyData, "Blue"), enemyData));
                            break;
                        case "zb":
                            room.Enemies.Add(new EnemySpawn(location, new Zol(location, enemyData, ""), enemyData));
                            break;
                        case "so":
                            room.Enemies.Add(new LeftStatueOne(location, levelNum, enemyData));
                            break;
                        case "st":
                            room.Enemies.Add(new RightStatueOne(location, levelNum, enemyData));
                            break;
                        case "bu":
                            room.Enemies.Add(new EnemySpawn(location, new Bubble(location, enemyData), enemyData));
                            break;
                        case "rn":
                            room.Enemies.Add(new EnemySpawn(location, new Darknut(location, enemyData, ""), enemyData));
                            break;
                        case "bn":
                            room.Enemies.Add(new EnemySpawn(location, new Darknut(location, enemyData, "Blue"), enemyData));
                            break;
                        case "ll":
                            room.Enemies.Add(new EnemySpawn(location, new LikeLike(location, enemyData), enemyData));
                            break;
                        case "Gi":
                            room.Enemies.Add(new EnemySpawn(location, new Gibdo(location, enemyData), enemyData));
                            break;
                        case "Vi":
                            room.Enemies.Add(new EnemySpawn(location, new Vire(location, enemyData), enemyData));
                            break;
                        case "Wi":
                            room.Enemies.Add(new EnemySpawn(location, new Wizzrobe(location, enemyData, ""), enemyData));
                            break;
                        case "OW":
                            room.Enemies.Add(new EnemySpawn(location, new Wizzrobe(location, enemyData, "Orange"), enemyData));
                            break;
                        case "bz":
                            room.Enemies.Add(new EnemySpawn(location, new Zol(location, enemyData, "Black"), enemyData));
                            break;
                        case "gz":
                            room.Enemies.Add(new EnemySpawn(location, new Zol(location, enemyData, "Green"), enemyData));
                            break;
                        case "lz":
                            room.Enemies.Add(new EnemySpawn(location, new Zol(location, enemyData, "Gold"), enemyData));
                            break;
                        case "GO":
                            room.Enemies.Add(new EnemySpawn(location, new Gohma(location, enemyData), enemyData));
                            break;
                        case "me":
                            location.X += TileEdgeLength / 2;
                            room.Enemies.Add(new Merchant(location, enemyData));
                            break;
                        case "om":
                            location.X += TileEdgeLength / 2;
                            room.Enemies.Add(new OldMan(location, enemyData));
                            break;
                        case "sp":
                            room.Enemies.Add(new SpikeTrap(location, enemyData));
                            break;
                        case "mc":
                            room.Enemies.Add(new EnemySpawn(location, new ManHandlaController(location, enemyData, room.Enemies), enemyData));
                            break;
                        case "mo":
                            room.Enemies.Add(new MoldormController(location, enemyData, room.Enemies));
                            break;
                        case "bm":
                            room.Items.Add(new WoodenBoomerang(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "bw":
                            room.Items.Add(new Bow(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "cs":
                            room.Items.Add(new Compass(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "cD":
                            room.DroppedItems.Add(new Compass(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "mp":
                            room.Items.Add(new Map(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "mD":
                            room.DroppedItems.Add(new Map(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "ky":
                            room.Items.Add(new Key(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "kD":
                            room.DroppedItems.Add(new Key(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "bk":
                            room.Items.Add(new BossKey(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "bD":
                            room.DroppedItems.Add(new BossKey(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "bb":
                            room.Items.Add(new Bomb(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "hc":
                            room.Items.Add(new HeartContainer(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "hD":
                            room.DroppedItems.Add(new HeartContainer(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "tr":
                            location.X += TileEdgeLength / 2;
                            room.Items.Add(new TriForcePiece(location, room, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "ar":
                            room.Items.Add(new Items.Arrow(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "mb":
                            room.Items.Add(new MagicalBoomerang(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "ra":
                            room.Items.Add(new Raft(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "ma":
                            room.Items.Add(new MagicalShield(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "Le":
                            room.Items.Add(new Letter(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "lp":
                            room.Items.Add(new LifePotion(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "fo":
                            room.Items.Add(new Food(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "bg":
                            room.Items.Add(new BlueRing(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "bc":
                            room.Items.Add(new BlueCandle(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "rr":
                            room.Items.Add(new RedRing(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "sl":
                            room.Items.Add(new SecondLifePotion(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "rc":
                            room.Items.Add(new RedCandle(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "fa":
                            room.Items.Add(new Fairy(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "ru":
                            room.Items.Add(new BlueRupee(location, itemData, GetItemCost(entry, out entry)));
                            break;
                        case "fr":
                            room.Items.Add(new BlueRupee(location, itemData));
                            break;
                        case "LD":
                            room.Items.Add(new StepLadder(location, itemData, GetItemCost(entry, out entry)));
                            break;
                    }
                    entry = entry.Substring(2);
                    //Trace.WriteLine("[ERROR] Tile at (" + row + "," + col + ") " + (removed ? "was" : "was not") + " removed");
                }
            }
        }

        private static int GetItemCost(string entry, out string entryNoCost)
        {
            string entryNoId = entry.Substring(2);
            int cost = 0, i = 0;

            //Calculate cost
            while (int.TryParse(entryNoId[i].ToString(), out int digit))
            {
                cost = (10 * cost) + digit;
                i++;
            }

            //Remove cost from string
            entryNoCost = entry.Substring(0, 2) + entryNoId.Substring(i);

            return cost;
        }
    }
}