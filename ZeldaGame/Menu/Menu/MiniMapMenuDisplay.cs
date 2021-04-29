//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.HUD.Menu.InventoryMenuItems;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Menu.Menu.MiniMapMenuDisplayItems;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.Menu
{
    class MiniMapMenuDisplay : IMenuItem
    {

        private readonly Level CurrentLevel;
        private readonly PlayerInventory InnerInventory;
        private readonly IMenuSprite MapLayout;
        private readonly List<IMenuItem> DisplayableItems;
        private Point MapLocation;
        private readonly int frameMovement = 1;

        public MiniMapMenuDisplay(Level level, PlayerInventory heldInventory, bool playerTwo = false)
        {
            CurrentLevel = level;
            InnerInventory = heldInventory;

            MapLayout = MenuFactory.Instance.CreateSprite("DungeonMapFrame");

            if (!playerTwo)
            {
                MapLocation = MenuFactory.Instance.GetPoint("MenuMiniMapFrameLocation");
            }
            else
            {
                MapLocation = MenuFactory.Instance.GetPoint("P2MapFrameLoc");
            }

            DisplayableItems = new List<IMenuItem>
            {
                new MenuMiniMap(CurrentLevel, InnerInventory, playerTwo),
                new Map(InnerInventory, playerTwo),
                new Compass(InnerInventory, playerTwo)
            };

            if (playerTwo)
            {
                frameMovement *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            MapLayout.Draw(spriteBatch, MapLocation, color);
            foreach (IMenuItem Item in DisplayableItems)
            {
                Item.Draw(spriteBatch, color);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color, bool transition)
        {
            MapLayout.Draw(spriteBatch, MapLocation, color);
            if (!transition)
            {
                foreach (IMenuItem Item in DisplayableItems)
                {
                    Item.Draw(spriteBatch, color);
                }
            }
        }

        public void Update()
        {

            foreach (IMenuItem Item in DisplayableItems)
            {
                Item.Update();
            }
        }

        internal void ChangeLevel(Level level, bool playerTwo)
        {
            DisplayableItems[0] = new MenuMiniMap(level, InnerInventory, playerTwo);
        }

        //this method updates the menu location
        public void TransitionDisplay(bool open)
        {
            if (open)
            {
                MapLocation.Y += frameMovement;
            }
            else
            {
                MapLocation.Y -= frameMovement;
            }
        }

        public void TransitionFrame(bool menuPlayer)
        {
            if (menuPlayer)
            {
                MapLocation = MenuFactory.Instance.GetPoint("MenuMapFrameTransition");
            }
            else
            {
                MapLocation = MenuFactory.Instance.GetPoint("P2MapTransLoc");
            }
        }

        public void SetPoint(bool menuPlayer)
        {
            if (menuPlayer)
            {
                MapLocation = MenuFactory.Instance.GetPoint("MenuMiniMapFrameLocation");
            }
            else
            {
                MapLocation = MenuFactory.Instance.GetPoint("P2MapFrameLoc");
            }
        }

        public void SetPlayerTwoMap()
        {
            MapLocation = MenuFactory.Instance.GetPoint("P2MapTransLoc");
        }

        public void ResetPlayerTwo()
        {
            MapLocation = MenuFactory.Instance.GetPoint("P2MapFrameLoc");
        }
    }
}
