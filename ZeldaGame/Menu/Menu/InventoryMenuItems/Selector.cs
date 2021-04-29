using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.HUD;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.Menu.Menu.InventoryMenuItems
{
    class Selector : IMenuItem
    {
        private readonly AdventurePlayerInventory InnerInventory;
        private int Position;

        private readonly List<IMenuSprite> RedSprites;
        private readonly List<IMenuSprite> BlueSprites;
        private List<IMenuSprite> CurrentSprites;
        private readonly IMenuSprite SelectedObjectSprite;
        private readonly Point SelectedObjectLocation;

        private readonly List<Point> SelectorPoints;
        private readonly Point Seperation;

        private bool blue = true;
        private int changeCounter;

        private enum PotentialItems
        {
            Boomerang, Bomb, BowAndArrow, Candle, Flute,
        }
        public Selector(AdventurePlayerInventory adventurePlayerInventory, bool playerTwo)
        {
            InnerInventory = adventurePlayerInventory;

            RedSprites = new List<IMenuSprite>
            {
                MenuFactory.Instance.CreateSprite("RedSelector", 0),
                MenuFactory.Instance.CreateSprite("RedSelector", 1),
                MenuFactory.Instance.CreateSprite("RedSelector", 2),
                MenuFactory.Instance.CreateSprite("RedSelector", 3)
            };

            BlueSprites = new List<IMenuSprite>
            {
                MenuFactory.Instance.CreateSprite("BlueSelector", 0),
                MenuFactory.Instance.CreateSprite("BlueSelector", 1),
                MenuFactory.Instance.CreateSprite("BlueSelector", 2),
                MenuFactory.Instance.CreateSprite("BlueSelector", 3)
            };

            SelectedObjectSprite = MenuFactory.Instance.CreateSprite("InventoryItems", (int)InventoryItem.None);

            if (!playerTwo)
            {
                SelectedObjectLocation = MenuFactory.Instance.GetPoint("SelectedItem");
                SelectorPoints = MenuFactory.Instance.GetPointCollection("SelectorCorners");
            }
            else
            {
                SelectedObjectLocation = MenuFactory.Instance.GetPoint("PlayerTwoSelectedItem");
                SelectorPoints = MenuFactory.Instance.GetPointCollection("PlayerTwoSelectorCorners");
            }
            Seperation = MenuFactory.Instance.GetPoint("SelectorShiftDimensions");

            CurrentSprites = BlueSprites;
            Position = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            for (int i = 0; i < CurrentSprites.Count; i++)
            {
                Point OffsetLocation = new Point(SelectorPoints[i].X + Seperation.X * (Position % 4), SelectorPoints[i].Y + Seperation.Y * (Position / 4));
                CurrentSprites[i].Draw(spriteBatch, OffsetLocation, color);
            }
            SelectedObjectSprite.Draw(spriteBatch, SelectedObjectLocation, color);

        }

        public void Update()
        {
            UpdateSelectedItem();
            if (changeCounter > 5)
            {
                if (blue)
                {
                    blue = false;
                    CurrentSprites = RedSprites;
                }
                else
                {
                    blue = true;
                    CurrentSprites = BlueSprites;
                }
                changeCounter = 0;
            }
            changeCounter++;
        }

        public int GetItem()
        {
            return Position;
        }

        public void Up()
        {
            if (Position > 3)
            {
                Position -= 4;
            }

        }

        public void Down()
        {
            if (Position < 4)
            {
                Position += 4;
            }
        }

        public void Right()
        {
            if (Position % 4 < 3)
            {
                Position += 1;
            }
        }

        public void Left()
        {
            if (Position % 4 > 0)
            {
                Position -= 1;
            }
        }

        public void Select()
        {
            switch (Position)
            {
                case 0:
                    if (InnerInventory.ItemAmount(Item.WoodenBoomerang) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.WoodenBoomerang);
                    }
                    break;
                case 1:
                    if (InnerInventory.ItemAmount(Item.Bomb) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.Bomb);
                    }
                    break;
                case 2:
                    if (InnerInventory.ItemAmount(Item.Arrow) > 0 && InnerInventory.ItemAmount(Item.Bow) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.BowArrow);
                    }
                    break;
                case 3:
                    if (InnerInventory.ItemAmount(Item.RedCandle) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.RedCandle);
                    }
                    break;
                case 4:
                    if (InnerInventory.ItemAmount(Item.Flute) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.Flute);
                    }
                    break;
                case 5:
                    if (InnerInventory.ItemAmount(Item.Food) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.Food);
                    }
                    break;
                case 6:
                    if (InnerInventory.ItemAmount(Item.LifePotion) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.LifePotion);
                    }
                    break;
                case 7:
                    if (InnerInventory.ItemAmount(Item.MagicalRod) > 0)
                    {
                        InnerInventory.SetSlot2(AdventurePlayerInventory.Slot2Item.MagicalRod);
                    }
                    break;
            }

        }

        private void UpdateSelectedItem()
        {
            switch (Position)
            {
                case 0:
                    if (InnerInventory.ItemAmount(Item.WoodenBoomerang) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.WoodenBoomerang);
                    }
                    break;
                case 1:
                    if (InnerInventory.ItemAmount(Item.Bomb) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.Bomb);
                    }
                    break;
                case 2:
                    if (InnerInventory.ItemAmount(Item.Bow) > 0)
                    {
                        if (InnerInventory.ItemAmount(Item.Arrow) > 0)
                        {
                            SelectedObjectSprite.Update((int)InventoryItem.Bow);
                        }
                        else
                        {
                            SelectedObjectSprite.Update((int)InventoryItem.None);
                        }

                    }
                    break;
                case 3:
                    if (InnerInventory.ItemAmount(Item.RedCandle) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.RedCandle);
                    }
                    break;
                case 4:
                    if (InnerInventory.ItemAmount(Item.Flute) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.Flute);
                    }
                    break;
                case 5:
                    if (InnerInventory.ItemAmount(Item.Food) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.Meat);
                    }
                    break;
                case 6:
                    if (InnerInventory.ItemAmount(Item.LifePotion) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.BluePotion);
                    }
                    break;
                case 7:
                    if (InnerInventory.ItemAmount(Item.MagicalRod) > 0)
                    {
                        SelectedObjectSprite.Update((int)InventoryItem.MagicRod);
                    }
                    break;
            }
        }
    }
}
