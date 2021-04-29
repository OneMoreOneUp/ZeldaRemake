//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.Menu.InventoryMenuItems
{
    class Bow : IMenuItem
    {
        private readonly PlayerInventory Inventory;
        private bool set;
        private readonly IMenuSprite Sprite;
        private readonly Point location;

        public Bow(PlayerInventory inventory, bool playerTwo)
        {
            Inventory = inventory;
            Sprite = MenuFactory.Instance.CreateSprite("InventoryItems", (int)InventoryItem.None);
            location = MenuFactory.Instance.GetPoint("Bow");

            if (playerTwo)
            {
                location.Y += MenuFactory.Instance.GetPoint("PlayerTwoInvOffset").Y;
            }
        }


        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Sprite.Draw(spriteBatch, location, color);

        }

        public void Update()
        {
            if (Inventory.ItemAmount(Item.Bow) > 0)
            {
                if (!set)
                {
                    Sprite.Update((int)InventoryItem.Bow);
                    set = true;
                }
            }
            else
            {
                if (set)
                {
                    Sprite.Update((int)InventoryItem.None);
                    set = false;
                }
            }
        }
    }
}
