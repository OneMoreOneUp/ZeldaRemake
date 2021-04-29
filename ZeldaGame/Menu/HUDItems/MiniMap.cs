//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.HUDItems
{
    class MiniMap : IMenuItem
    {

        private bool Built;
        private bool DrawnBossPoint;

        private readonly IMenuSprite BossRoomSprite;
        private readonly Dictionary<Point, IMenuSprite> SpriteMap;

        private readonly Dictionary<Point, Room> LocationMap;
        private readonly PlayerInventory Inventory;

        private readonly Point BossPoint;
        private readonly int Height = 12, Width = 8;
        private Point Location;
        private readonly int TileHeight = 4, TileWidth = 8;

        private readonly Level level;
        private Point CurrentPoint;
        public MiniMap(Level level, PlayerInventory inventory, bool inHUD = true, bool playerTwo = false)
        {
            this.level = level;
            LocationMap = level.LocationMap;
            SpriteMap = new Dictionary<Point, IMenuSprite>();
            Inventory = inventory;
            BossPoint = level.BossPoint;
            Location = MenuFactory.Instance.GetPoint("HUDMiniMap");


            if (playerTwo)
            {
                Location.Y += MenuFactory.Instance.GetPoint("PlayerTwoOffset").Y;
                if (!inHUD)
                {
                    Location.Y -= MenuFactory.Instance.GetPoint("PlayerTwoMenuOffset").Y;
                }
            }
            else
            {
                if (!inHUD)
                {
                    Location.Y += MenuFactory.Instance.GetPoint("MenuOffset").Y;
                }
            }

            BossRoomSprite = MenuFactory.Instance.CreateSprite("BlueMapPieces", (int)BlueMapPeice.Empty);
        }

        private void BuildMap()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Point LevelLocation = new Point(row, col);
                    Point location = MapCoordinatesFromLevelCoordinates(LevelLocation);
                    IMenuSprite MapPiece = MenuFactory.Instance.CreateSprite("BlueMapPieces", (int)BlueMapPeice.Empty);
                    if (LocationMap.ContainsKey(LevelLocation))
                    {
                        MapPiece.Update((int)BlueMapPeice.Present);
                    }
                    else
                    {
                        MapPiece.Update((int)BlueMapPeice.Empty);
                    }
                    if (LevelLocation == level.CurrentPoint)
                    {
                        MapPiece.Update((int)BlueMapPeice.Green);
                    }
                    MapPiece = HandleBossPoint(LevelLocation, MapPiece);
                    SpriteMap.Add(location, MapPiece);
                }
            }
            CurrentPoint = level.CurrentPoint;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            foreach (var pair in SpriteMap)
            {
                pair.Value.Draw(spriteBatch, pair.Key, color);
            }
        }

        private IMenuSprite HandleBossPoint(Point LevelLocation, IMenuSprite MapPiece)
        {
            if (LevelLocation.Equals(BossPoint))
            {
                MapPiece = BossRoomSprite;
                SpriteMap.Remove(BossPoint);
                if (DrawnBossPoint)
                {
                    BossRoomSprite.Update((int)BlueMapPeice.BossPresent);
                }
                else
                {
                    BossRoomSprite.Update((int)BlueMapPeice.Present);
                }
            }
            return MapPiece;
        }


        public void Update()
        {
            if (!Built && Inventory.ItemAmount(Item.Map) > 0)
            {
                Built = true;
                BuildMap();
            }
            if (!DrawnBossPoint && Inventory.ItemAmount(Item.Compass) > 0)
            {
                DrawnBossPoint = true;
                BossRoomSprite.Update((int)BlueMapPeice.BossPresent);
            }
            if (Built)
            {
                if (CurrentPoint != level.CurrentPoint)
                {
                    if (LocationMap.ContainsKey(CurrentPoint))
                    {
                        SpriteMap.TryGetValue(MapCoordinatesFromLevelCoordinates(CurrentPoint), out IMenuSprite LinkLocation);
                        if (LinkLocation != null) LinkLocation.Update((int)BlueMapPeice.Present);
                    }
                    CurrentPoint = level.CurrentPoint;
                    if (LocationMap.ContainsKey(CurrentPoint) && CurrentPoint.X >= 0 && CurrentPoint.Y >= 0)
                    {
                        SpriteMap.TryGetValue(MapCoordinatesFromLevelCoordinates(CurrentPoint), out IMenuSprite LinkLocation);
                        if (LinkLocation != null) LinkLocation.Update((int)BlueMapPeice.Green);
                    }
                }
            }
        }


        private Point MapCoordinatesFromLevelCoordinates(Point point)
        {
            return new Point(Location.X + point.Y * TileWidth, Location.Y - point.X * TileHeight);
        }
    }
}
