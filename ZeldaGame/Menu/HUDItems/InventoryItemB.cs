//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.HUDItems
{
    class InventoryItemB : IMenuItem
    {
        private readonly IMenuSprite Sprite;

        private readonly AdventurePlayerInventory OuterInventory;
        private AdventurePlayerInventory.Slot2Item HeldItem;

        private Point Location;

        public InventoryItemB(AdventurePlayerInventory outerInventory, bool inHUD = true, bool playerTwo = false)
        {
            Sprite = MenuFactory.Instance.CreateSprite("InventoryItems", (int)InventoryItem.None);
            HeldItem = AdventurePlayerInventory.Slot2Item.Nothing;
            OuterInventory = outerInventory;
            Location = MenuFactory.Instance.GetPoint("SlotBDisplay");

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
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Sprite.Draw(spriteBatch, Location, color);
        }

        public void Update()
        {
            if (HeldItem != OuterInventory.GetSlot2())
            {
                HeldItem = OuterInventory.GetSlot2();
                Sprite.Update((int)GetItemEnum(HeldItem));
            }
        }

        private static InventoryItem GetItemEnum(AdventurePlayerInventory.Slot2Item Item)
        {
            return Item switch
            {
                AdventurePlayerInventory.Slot2Item.BlueCandle => InventoryItem.BlueCandle,
                AdventurePlayerInventory.Slot2Item.RedCandle => InventoryItem.RedCandle,
                AdventurePlayerInventory.Slot2Item.Bomb => InventoryItem.Bomb,
                AdventurePlayerInventory.Slot2Item.Bow => InventoryItem.Bow,
                AdventurePlayerInventory.Slot2Item.Flute => InventoryItem.Flute,
                AdventurePlayerInventory.Slot2Item.Food => InventoryItem.Meat,
                AdventurePlayerInventory.Slot2Item.LifePotion => InventoryItem.BluePotion,
                AdventurePlayerInventory.Slot2Item.SecondLifePotion => InventoryItem.RedPotion,
                AdventurePlayerInventory.Slot2Item.MagicalBoomerang => InventoryItem.MagicBoomerang,
                AdventurePlayerInventory.Slot2Item.WoodenBoomerang => InventoryItem.WoodenBoomerang,
                AdventurePlayerInventory.Slot2Item.MagicalRod => InventoryItem.MagicRod,
                _ => InventoryItem.None,
            };
        }
    }
}
