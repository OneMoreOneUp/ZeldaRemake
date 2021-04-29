//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.HUD.HUDItems;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Player;

namespace ZeldaGame.HUD
{
    class HUDDisplay : IMenuItem
    {

        private readonly Level CurrentLevel;
        private readonly AdventurePlayerInventory InnerInventory;
        public static int Height = 56;

        private readonly IMenuSprite HUDLayout;

        private readonly List<IMenuItem> DisplayableItems;

        private Point HUDLocation;

        private readonly bool InMenu;

        private readonly int frameMovement = 1;


        public HUDDisplay(Level level, AdventurePlayerInventory inventory, bool inMenu = false, bool playerTwo = false)
        {
            CurrentLevel = level;
            InnerInventory = inventory;
            InMenu = inMenu;

            HUDLayout = MenuFactory.Instance.CreateSprite("HUDFrame");
            HUDLocation = MenuFactory.Instance.GetPoint("HUDTransitionLocation");

            DisplayableItems = new List<IMenuItem>
            {
                new MiniMap(CurrentLevel, InnerInventory, !inMenu, playerTwo),
                new KeyCount(InnerInventory, !inMenu, playerTwo),
                new BombCount(InnerInventory, !inMenu, playerTwo),
                new LifeHearts(InnerInventory, !inMenu, playerTwo),
                new RupeeCount(InnerInventory, !inMenu, playerTwo),
                new InventoryItemA(InnerInventory, !inMenu, playerTwo),
                new InventoryItemB(InnerInventory, !inMenu, playerTwo)
            };

            if (playerTwo)
            {
                frameMovement = -1;
                SetPlayerTwoHud();
            }

        }

        public void SetPlayerTwo()
        {
            HUDLocation = MenuFactory.Instance.GetPoint("P2HUDTransLoc");
        }

        public void ChangeLevel(Level level)
        {
            DisplayableItems[0] = new MiniMap(level, InnerInventory, !InMenu);
            if (InnerInventory.ItemAmount(Item.Map) > 0) InnerInventory.RemoveItem(Item.Map, InnerInventory.ItemAmount(Item.Map));
            if (InnerInventory.ItemAmount(Item.Compass) > 0) InnerInventory.RemoveItem(Item.Compass, InnerInventory.ItemAmount(Item.Compass));
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            HUDLayout.Draw(spriteBatch, HUDLocation, color);
            foreach (IMenuItem Item in DisplayableItems)
            {
                Item.Draw(spriteBatch, color);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color, bool transition)
        {
            HUDLayout.Draw(spriteBatch, HUDLocation, color);
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

        //this method updates the menu location
        public void TransitionDisplay(bool open)
        {
            if (open)
            {
                HUDLocation.Y += frameMovement;
            }
            else
            {
                HUDLocation.Y -= frameMovement;
            }
        }

        public void TransitionFrame(bool menuPlayer)
        {
            if (menuPlayer)
            {
                HUDLocation = MenuFactory.Instance.GetPoint("HUDTransitionLocation");
            }
            else
            {
                HUDLocation = MenuFactory.Instance.GetPoint("P2HUDTransLoc");
            }
        }

        public void ResetPoint()
        {
            HUDLocation = MenuFactory.Instance.GetPoint("HUDFrameLocation");
        }

        public void SetPlayerTwoHud()
        {
            HUDLocation = MenuFactory.Instance.GetPoint("P2HUDTransLoc");
        }

        public void ResetPlayerTwo()
        {
            HUDLocation = MenuFactory.Instance.GetPoint("P2HUDTransLoc");
        }
    }
}
