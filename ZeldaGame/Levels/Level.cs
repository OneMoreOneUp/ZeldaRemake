using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.GameState;
using ZeldaGame.HUD;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Levels
{
    class Level
    {
        private string[] index;

        public int LevelNum = 1;
        public Dictionary<Point, Room> LocationMap { get; set; }
        public Dictionary<Point, Room> UndergroundMap { get; set; }

        public Dictionary<Point, string> RoomPath { get; set; }
        public List<Point> PointList;
        public int ListIndex;

        public Point StartPoint { get; set; }
        public Point BossPoint { get; set; }
        public List<Point> UndergroundPoints = new List<Point>();

        public Room CurrentRoom;
        public Room OldRoom;
        public Point CurrentPoint { get; set; }
        public Point OldPoint { get; set; }

        public List<IAdventurePlayer> PlayerList { get; set; }
        private HUDDisplay HUD;
        private HUDDisplay HUD2;
        private MenuDisplay Menu;
        private bool isMultiplayer;

        public bool transitionRoom;
        public Direction moveNext;

        public Level(IAdventurePlayer player, int num)
        {
            LevelNum = num;
            index = System.IO.File.ReadAllLines(@"../../../Levels/CSV/Level" + LevelNum + "/Level" + LevelNum + "Index.txt");

            RoomBuilder.Level(LevelNum);

            LocationMap = new Dictionary<Point, Room>();
            UndergroundMap = new Dictionary<Point, Room>();
            RoomPath = new Dictionary<Point, string>();

            PlayerList = new List<IAdventurePlayer>
            {
                player
            };
        }

        public void AddPlayer(IAdventurePlayer player)
        {
            PlayerList.Add(player);
        }
        public void AddMenu(MenuDisplay menu)
        {
            Menu = menu;
        }

        public void AddHUD(HUDDisplay hud)
        {
            HUD = hud;
        }

        public void AddHUD2(HUDDisplay hud)
        {
            HUD2 = hud;
            isMultiplayer = true;
        }

        public void BuildMap()
        {
            LevelHandler.BuildMap(index, this);
            LevelHandler.MakePointList(this);
        }

        public void StartLevel(bool isReturning = false)
        {
            if (CurrentRoom != null)
            {
                CurrentRoom.Clear();
            }
            LocationMap.TryGetValue(StartPoint, out CurrentRoom);

            //This sets links start for any dungeon to these specific coordinates
            List<IAdventurePlayer> players = new List<IAdventurePlayer>(PlayerManager.GetPlayers());
            foreach (IAdventurePlayer player in players)
            {
                player.SetLocation(LevelHandler.LinkDungeonStart());
            }

            if (!isReturning) CurrentRoom.Build();
            else
            {
                CurrentPoint = StartPoint;
            }
            CurrentRoom.Enter();
        }

        public void Transition(Direction direction)
        {

            transitionRoom = true;
            //clear current room and change room
            //before building room and entering new room unlock new rooms locked door
            //need the opposite direction for new room door flip
            //get the door change to open and add it back
            OldRoom = CurrentRoom;
            OldPoint = CurrentPoint;
            CurrentRoom.Clear();
            LevelHandler.ChangeRoom(direction, this);
            moveNext = LevelHandler.OppositeDirection(direction);

            PlayerList = PlayerManager.GetPlayers();
            PlayerManager.RemoveAllPlayers();

            Point location = LevelHandler.NextPlayerLocation(direction, CurrentRoom);
            foreach (IAdventurePlayer player in PlayerList)
            {
                player.SetLocation(location);
            }
        }

        public void TransitionThoughList(bool forward)
        {
            //clear current room and change room
            CurrentRoom.Clear();
            ListIndex += forward ? 1 : -1;
            if (forward && ListIndex == PointList.Count) ListIndex = 0;
            if (!forward && ListIndex == -1) ListIndex = PointList.Count - 1;
            CurrentPoint = PointList[ListIndex];
            LocationMap.TryGetValue(CurrentPoint, out CurrentRoom);
            //before building room and entering new room unlock new rooms locked door
            //need the opposite direction for new room door flip
            //get the door change to open and add it back

            if (CurrentRoom.FirstBuild)
            {
                CurrentRoom.Build();
            }
            CurrentRoom.Enter();
            Point location = LevelHandler.NextPlayerLocation(Direction.North, CurrentRoom);
            foreach (IAdventurePlayer player in PlayerList)
            {
                player.SetLocation(location);
            }
        }

        public Dictionary<Point, Room> GetLocationMap()
        {
            return LocationMap;
        }

        //This function is called when a player is done colliding with a triforce piece
        //Transitions them to next level
        public void TransitionLevel()
        {
            GameStateManager.NextLevel(EndLevelTransition);
        }

        private void EndLevelTransition()
        {
            //clear the current room
            CurrentRoom.Clear();

            //increment level
            if (LevelNum != 7)
            {
                LevelNum++;
            }
            else
            {
                GameStateManager.GameWin();
                return;
            }

            RoomPath = new Dictionary<Point, string>();

            //build next level
            index = System.IO.File.ReadAllLines(@"../../../Levels/CSV/Level" + LevelNum + "//Level" + LevelNum + "Index.txt");

            RoomBuilder.Level(LevelNum);

            LocationMap = new Dictionary<Point, Room>();
            UndergroundMap = new Dictionary<Point, Room>();
            LevelHandler.BuildMap(index, this);
            LevelHandler.MakePointList(this);
            Menu.ChangeLevel(this, false);
            HUD.ChangeLevel(this);

            if (isMultiplayer)
            {
                HUD2.ChangeLevel(this);
                Menu.ChangeLevel(this, true);
            }

           
            LocationMap.TryGetValue(CurrentPoint, out CurrentRoom);

            //This sets links start for any dungeon to these specific coordinates
            foreach (IAdventurePlayer player in PlayerList)
            {
                player.SetLocation(LevelHandler.LinkDungeonStart());
            }

            CurrentRoom.Build();
            CurrentRoom.Enter();
        }

        public void RunDefeatedEnemiesActions()
        {
            CurrentRoom.DropItems();
            //CurrentRoom.OpenDoor();
        }

        public void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (EnemyManager.GetEnemies().Count == 0)
            {
                CurrentRoom.DropItems();
                if (CurrentRoom.movedBlock) CurrentRoom.EnemiesDead();
            }
        }
    }
}
