//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.Menu.InventoryMenuItems
{
    class Compass : IMenuItem
    {
        private readonly PlayerInventory Inventory;
        private bool set;
        private readonly IMenuSprite Sprite;
        private readonly Point location;

        public Compass(PlayerInventory inventory, bool playerTwo)
        {
            Inventory = inventory;
            Sprite = MenuFactory.Instance.CreateSprite("InventoryItems", (int)InventoryItem.NoneWide);
            location = MenuFactory.Instance.GetPoint("Compass");

            if (playerTwo)
            {
                location.Y = MenuFactory.Instance.GetPoint("PlayerTwoCompassOffset").Y;
            }
        }


        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Sprite.Draw(spriteBatch, location, color);
        }

        public void Update()
        {
            if (Inventory.ItemAmount(Item.Compass) > 0)
            {
                if (!set)
                {
                    Sprite.Update((int)InventoryItem.Compass);
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
