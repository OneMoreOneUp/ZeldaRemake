//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Blocks;
using ZeldaGame.Enums;
using ZeldaGame.HUD;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.Menu.Menu.MiniMapMenuDisplayItems
{
    class MenuMiniMap : IMenuItem
    {
        private readonly Dictionary<IMenuSprite, Point> SpriteMap;
        private readonly Dictionary<Point, Room> LocationMap;
        private readonly PlayerInventory Inventory;
        private readonly Point Location;
        private readonly int Height = 8, Width = 8;
        private readonly int TileHeight = 8, TileWidth = 8;

        private bool Built;

        public MenuMiniMap(Level level, PlayerInventory inventory, bool playerTwo)
        {
            LocationMap = level.LocationMap;
            SpriteMap = new Dictionary<IMenuSprite, Point>();
            Inventory = inventory;
            Location = MenuFactory.Instance.GetPoint("MenuMiniMap");

            if (playerTwo)
            {
                Location.Y = MenuFactory.Instance.GetPoint("PlayerTwoMenuMapOffset").Y;
            }
        }

        public void BuildMap()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Point location = new Point(Location.X + col * TileWidth, Location.Y - row * TileHeight);
                    IMenuSprite MapPiece = MenuFactory.Instance.CreateSprite("OrangeMapPieces", (int)OrangeMapPiece.Orange);
                    Point LevelLocation = new Point(row, col);
                    if (LocationMap.ContainsKey(LevelLocation))
                    {
                        LocationMap.TryGetValue(LevelLocation, out Room room);
                        MapPiece.Update((int)GetEnum(room));
                    }
                    SpriteMap.Add(MapPiece, location);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            foreach (IMenuSprite sprite in SpriteMap.Keys)
            {
                SpriteMap.TryGetValue(sprite, out Point location);
                sprite.Draw(spriteBatch, location, color);
            }
        }

        public void Update()
        {
            if (!Built && Inventory.ItemAmount(Item.Map) > 0)
            {
                this.BuildMap();
                Built = true;
            }
        }

        private static OrangeMapPiece GetEnum(Room room)
        {
            if (IsDoor(room.GetDoor(Direction.North)))
            {
                if (IsDoor(room.GetDoor(Direction.East)))
                {
                    if (IsDoor(room.GetDoor(Direction.South)))
                    {
                        if (IsDoor(room.GetDoor(Direction.West)))
                        {
                            return OrangeMapPiece.All;
                        }
                        return OrangeMapPiece.NoWest;
                    }
                    if (IsDoor(room.GetDoor(Direction.West)))
                    {
                        return OrangeMapPiece.NoSouth;
                    }
                    return OrangeMapPiece.NorthEast;
                }
                if (IsDoor(room.GetDoor(Direction.South)))
                {
                    if (IsDoor(room.GetDoor(Direction.West)))
                    {
                        return OrangeMapPiece.NoEast;
                    }
                    return OrangeMapPiece.NorthSouth;
                }
                if (IsDoor(room.GetDoor(Direction.West)))
                {
                    return OrangeMapPiece.NorthWest;
                }
                return OrangeMapPiece.North;
            }
            if (IsDoor(room.GetDoor(Direction.East)))
            {
                if (IsDoor(room.GetDoor(Direction.South)))
                {
                    if (IsDoor(room.GetDoor(Direction.West)))
                    {
                        return OrangeMapPiece.NoNorth;
                    }
                    return OrangeMapPiece.SouthEast;
                }
                if (IsDoor(room.GetDoor(Direction.West)))
                {
                    return OrangeMapPiece.EastWest;
                }
                return OrangeMapPiece.East;
            }
            if (IsDoor(room.GetDoor(Direction.South)))
            {
                if (IsDoor(room.GetDoor(Direction.West)))
                {
                    return OrangeMapPiece.SouthWest;
                }
                return OrangeMapPiece.South;
            }
            if (IsDoor(room.GetDoor(Direction.West)))
            {
                return OrangeMapPiece.West;
            }
            return OrangeMapPiece.None;
        }

        private static bool IsDoor(Door door)
        {
            switch (door.GetWallType())
            {
                case WallType.BombedDoor:
                case WallType.ClosedDoor:
                case WallType.LockedDoor:
                case WallType.OpenDoor:
                    return true;
                default:
                    return false;
            }
        }
    }
}