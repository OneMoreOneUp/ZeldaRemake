//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.HUDItems
{
    class InventoryItemA : IMenuItem
    {
        private readonly IMenuSprite Sprite;

        private readonly AdventurePlayerInventory OuterInventory;
        private AdventurePlayerInventory.Slot1Item HeldItem;

        private Point Location;

        public InventoryItemA(AdventurePlayerInventory outerInventory, bool inHUD = true, bool playerTwo = false)
        {
            Sprite = MenuFactory.Instance.CreateSprite("InventoryItems", (int)InventoryItem.None);
            HeldItem = AdventurePlayerInventory.Slot1Item.Nothing;
            OuterInventory = outerInventory;
            Location = MenuFactory.Instance.GetPoint("SlotADisplay");

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
            if (HeldItem != OuterInventory.GetSlot1())
            {
                HeldItem = OuterInventory.GetSlot1();
                Sprite.Update((int)GetInventoryEnum(HeldItem));
            }
        }

        private static InventoryItem GetInventoryEnum(AdventurePlayerInventory.Slot1Item Item)
        {
            switch (Item)
            {
                case AdventurePlayerInventory.Slot1Item.WoodSword:
                    return InventoryItem.WoodenSword;
                case AdventurePlayerInventory.Slot1Item.MagicalSword:
                case AdventurePlayerInventory.Slot1Item.WhiteSword:
                    return InventoryItem.WhiteSword;
                default:
                    return InventoryItem.None;
            }
        }
    }
}
