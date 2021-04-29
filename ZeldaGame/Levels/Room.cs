using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using ZeldaGame.Blocks;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Levels
{
    class Room
    {
        public List<IBlock> Tiling { get; set; }
        public List<IBlock> Walls { get; set; }
        public List<IItem> Items { get; set; }
        public List<IItem> StaticItems { get; set; }
        public List<IEnemy> Enemies { get; set; }
        public List<IBlock> SecondaryTiling { get; set; }

        public List<IBlock> DoorWays { get; set; }

        //Items like Keys, Compasses and Maps that are dropped once all the enemies are killed within a room
        public List<IItem> DroppedItems { get; set; }

        //Items like Keys, Compasses and Maps that are dropped once all the enemies are killed within a room
        public List<IItem> SingleSpawnItems { get; set; }

        //Enemies like Bosses that are spawned only once
        public List<IEnemy> SingleSpawnEnemies { get; set; }

        public bool HasDroppedItems;
        public Dictionary<Direction, Door> DoorMap { get; set; }
        public Stair StairCase;
        public Ladder UpLadder;
        public Divider MoveableBlock;

        private readonly string[] Lines;
        private readonly Level AssignedLevel;

        public bool underground;

        public bool FirstBuild;

        public bool movedBlock = true;

        // String of the text in the room and the position of the first letter
        public string roomText;
        public Point textPosition;
        public int textMaxCharactersPerLine;


        public Room(string path, Level level)
        {
            AssignedLevel = level;

            //Added to Manager Whenever Link Enters
            Tiling = new List<IBlock>();
            Walls = new List<IBlock>();
            Items = new List<IItem>();
            Enemies = new List<IEnemy>();
            SecondaryTiling = new List<IBlock>();
            DoorWays = new List<IBlock>();

            //Added only once
            DroppedItems = new List<IItem>();
            SingleSpawnItems = new List<IItem>();
            SingleSpawnEnemies = new List<IEnemy>();


            DoorMap = new Dictionary<Direction, Door>();

            Lines = RoomBuilder.ReadCSV(path);
            FirstBuild = true;
        }


        public void Build()
        {
            RoomBuilder.SetLines(Lines);
            RoomBuilder.BuildRoom(this);
            FirstBuild = false;
        }


        public void Clear()
        {
            Items = ItemManager.GetItems();
            Enemies = EnemyManager.GetEnemies();

            BlockManager.RemoveAllBlocks();
            EnemyManager.RemoveAllEnemies();
            ItemManager.RemoveAllItems();
            PlayerProjectileManager.RemoveAllPlayerProjectiles();
            EnemyProjectileManager.RemoveAllEnemyProjectiles();
        }

        public void EnterTransition()
        {
            BlockManager.AddBlock(Walls);
            BlockManager.AddBlock(Tiling);
            BlockManager.AddBlock(SecondaryTiling);
            BlockManager.AddBlock(DoorWays);

        }

        public void Enter()
        {
            BlockManager.AddBlock(Walls);
            BlockManager.AddBlock(Tiling);
            BlockManager.AddBlock(SecondaryTiling);
            if (!underground)
            {
                BlockManager.AddBlock(DoorMap.Values.Cast<IBlock>().ToList());
            }
            EnemyManager.AddEnemy(Enemies);
            ItemManager.AddItem(Items);

            //Add text to the room if included, this has to be added seperately
            //becuase we want the text to reset each time the room is entered
            if (roomText != null)
            {
                BlockManager.AddBlock(new Text(textPosition, roomText, textMaxCharactersPerLine));
            }

        }

        internal Point GetReturnLocation()
        {
            return StairCase.GetSpawnPoint();
        }

        internal void UnlockOppositeDoor(Direction direction)
        {
            LevelHandler.OpenOppositeDoor(direction, AssignedLevel);
        }

        internal void BombedOppositeDoor(Direction direction)
        {
            LevelHandler.BombOppositeDoor(direction, AssignedLevel);
        }

        public void EnemiesDead()
        {
            //for each direction check if door is locked, if locked, change to open
            foreach (var item in DoorMap.Values)
            {
                if (item.GetWallType() == WallType.ClosedDoor)
                {
                    item.ChangeType(WallType.OpenDoor);
                }
            }
        }

        public static void Exit()
        {
            BlockManager.RemoveAllBlocks();
            EnemyManager.RemoveAllEnemies();
            ItemManager.RemoveAllItems();
            PlayerProjectileManager.RemoveAllPlayerProjectiles();
            EnemyProjectileManager.RemoveAllEnemyProjectiles();
        }

        public void DropItems()
        {
            if (!HasDroppedItems)
            {
                ItemManager.AddItem(DroppedItems);
                HasDroppedItems = true;
            }
        }

        public void OpenDoor(Direction direction)
        {
            //get the door for the current entrance direction
            Door door = GetDoor(direction);
            //change the door type to open
            door.ChangeType(WallType.OpenDoor);
            SetDoor(direction, door);
        }

        public Door GetDoor(Direction direction)
        {
            DoorMap.TryGetValue(direction, out Door door);
            return door;
        }

        public void SetDoor(Direction direction, Door door)
        {
            //changed setDoor to actually set the door if passed a direction not contained currently
            if (!DoorMap.ContainsKey(direction))
            {
                DoorMap.Add(direction, door);
            }
        }

        public void InitializeDoors()
        {
            RoomBuilder.SetDoors(Lines[1].Split(','), this);
        }

        public void Transition(Direction d)
        {
            AssignedLevel.Transition(d);
        }

        public void TransitionLevel()
        {
            AssignedLevel.TransitionLevel();
        }
        public void ReturnToStart()
        {
            AssignedLevel.StartLevel(true);
        }
    }
}