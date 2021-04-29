//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.HUDItems
{
    class LifeHearts : IMenuItem
    {
        private readonly PlayerInventory Inventory;
        private int Health, MaxHealth;
        private const int MaxSpaces = 16;
        private const int edgelength = 8;
        private readonly List<IMenuSprite> HeartSprites;
        private Point Location;
        public LifeHearts(PlayerInventory inventory, bool inHUD = true, bool playerTwo = false)
        {
            Inventory = inventory;
            MaxHealth = inventory.GetMaxHealth();
            Health = 0;
            HeartSprites = new List<IMenuSprite>();
            for (int i = 0; i < MaxSpaces; i++)
            {
                HeartSprites.Add(MenuFactory.Instance.CreateSprite("HeartSymbols", (int)HeartState.Black));
            }
            Location = MenuFactory.Instance.GetPoint("LifeHearts");

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
            for (int i = 0; i < MaxSpaces; i++)
            {
                HeartSprites[i].Draw(spriteBatch, new Point(Location.X + (edgelength * (i % 8)), Location.Y + (edgelength * (i / 8))), color);
            }
        }

        public void Update()
        {
            if (Health != Inventory.GetHealth())
            {
                MaxHealth = Inventory.GetMaxHealth();
                Health = Inventory.GetHealth();
                for (int i = 0; i < MaxHealth / 2; i++)
                {
                    if (i < Health / 2)
                    {
                        HeartSprites[i].Update((int)HeartState.Full);
                    }
                    else
                    {
                        HeartSprites[i].Update((int)HeartState.Empty);
                    }
                }
                if (Health % 2 == 1)
                {
                    HeartSprites[Health / 2].Update((int)HeartState.Half);
                }
            }
        }
    }
}
