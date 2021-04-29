//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.HUD.Menu.InventoryMenuItems;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Menu.InventoryMenuItems;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.Menu
{
    class InventoryMenu : IMenuItem
    {
        private readonly AdventurePlayerInventory InnerInventory;

        private readonly IMenuSprite InventoryLayout;

        private readonly List<IMenuItem> DisplayableItems;
        private readonly Selector FlashingSelector;

        private Point InventoryLocation;

        private readonly int frameMovement = 1;


        public InventoryMenu(AdventurePlayerInventory inventory, bool playerTwo = false)
        {
            InnerInventory = inventory;

            InventoryLayout = MenuFactory.Instance.CreateSprite("InventoryFrame");

            if (!playerTwo)
            {
                InventoryLocation = MenuFactory.Instance.GetPoint("InventoryFrameLocation");
            }
            else
            {
                InventoryLocation = MenuFactory.Instance.GetPoint("P2InvFrameLoc");
            }
            DisplayableItems = new List<IMenuItem>
            {
                new Boomerang(InnerInventory, playerTwo),
                new Bomb(InnerInventory, playerTwo),
                new Bow(InnerInventory, playerTwo),
                new Candle(InnerInventory, playerTwo),
                new Flute(InnerInventory, playerTwo),
                new Meat(InnerInventory, playerTwo),
                new Potion(InnerInventory, playerTwo),
                new MagicRod(InnerInventory, playerTwo),
                new Arrow(InnerInventory, playerTwo)
            };

            if (playerTwo)
            {
                frameMovement *= -1;
            }

            FlashingSelector = new Selector(InnerInventory, playerTwo);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            InventoryLayout.Draw(spriteBatch, InventoryLocation, color);

            foreach (IMenuItem Item in DisplayableItems)
            {
                Item.Draw(spriteBatch, color);
            }

            FlashingSelector.Draw(spriteBatch, color);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, bool transition)
        {
            InventoryLayout.Draw(spriteBatch, InventoryLocation, color);
            if (!transition)
            {
                foreach (IMenuItem Item in DisplayableItems)
                {
                    Item.Draw(spriteBatch, color);
                }
                FlashingSelector.Draw(spriteBatch, color);
            }
        }

        //this method updates the menu location
        public void TransitionDisplay(bool open)
        {
            if (open)
            {
                InventoryLocation.Y += frameMovement;
            }
            else
            {
                InventoryLocation.Y -= frameMovement;
            }
        }

        public void Update()
        {
            foreach (IMenuItem Item in DisplayableItems)
            {
                Item.Update();
            }
            FlashingSelector.Update();
        }

        public void ReceiveCommand(Direction direction, bool select)
        {
            switch (direction)
            {
                case Direction.North:
                    FlashingSelector.Up();
                    break;
                case Direction.South:
                    FlashingSelector.Down();
                    break;
                case Direction.East:
                    FlashingSelector.Right();
                    break;
                case Direction.West:
                    FlashingSelector.Left();
                    break;
                case Direction.Null:
                    if (select)
                    {
                        FlashingSelector.Select();
                    }
                    break;
            }
        }

        public void TransitionFrame(bool menuPlayer)
        {
            if (menuPlayer)
            {
                InventoryLocation = MenuFactory.Instance.GetPoint("InventoryTransitionLocation");
            }
            else
            {
                InventoryLocation = MenuFactory.Instance.GetPoint("P2InvTransLoc");
            }
        }

        public void SetPoint(bool menuPlayer)
        {
            if (menuPlayer)
            {
                InventoryLocation = MenuFactory.Instance.GetPoint("InventoryFrameLocation");
            }
            else
            {
                InventoryLocation = MenuFactory.Instance.GetPoint("P2InvFrameLoc");
            }
        }

        public void SetPlayerTwoInv()
        {
            InventoryLocation = MenuFactory.Instance.GetPoint("P2InvTransLoc");
        }

        public void ResetPlayerTwo()
        {
            InventoryLocation = MenuFactory.Instance.GetPoint("P2InvFrameLoc");
        }

    }
}
