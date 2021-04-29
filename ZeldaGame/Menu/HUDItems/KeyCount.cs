//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;
using ZeldaGame.Menu.Enums;
using ZeldaGame.Player;

namespace ZeldaGame.HUD.HUDItems
{
    class KeyCount : IMenuItem
    {
        private int Count;
        private readonly int MaxDisplayableValue = 99;

        private readonly IMenuSprite FirstBlock;
        private readonly IMenuSprite SecondBlock;
        private readonly IMenuSprite ThirdBlock;

        private Point FirstBlockPoint;
        private Point SecondBlockPoint;
        private Point ThirdBlockPoint;

        private readonly PlayerInventory Inventory;
        public KeyCount(PlayerInventory inventory, bool inHUD = true, bool playerTwo = false)
        {
            FirstBlock = MenuFactory.Instance.CreateSprite("CountSymbols");
            FirstBlock.Update((int)CountSymbol.X);

            SecondBlock = MenuFactory.Instance.CreateSprite("CountSymbols");
            SecondBlock.Update((int)CountSymbol.Zero);

            ThirdBlock = MenuFactory.Instance.CreateSprite("CountSymbols");
            ThirdBlock.Update((int)CountSymbol.Zero);

            List<Point> collection = MenuFactory.Instance.GetPointCollection("KeyCount");
            FirstBlockPoint = collection[0];
            SecondBlockPoint = collection[1];
            ThirdBlockPoint = collection[2];

            if (playerTwo)
            {
                FirstBlockPoint.Y += MenuFactory.Instance.GetPoint("PlayerTwoOffset").Y;
                SecondBlockPoint.Y += MenuFactory.Instance.GetPoint("PlayerTwoOffset").Y;
                ThirdBlockPoint.Y += MenuFactory.Instance.GetPoint("PlayerTwoOffset").Y;

                if (!inHUD)
                {
                    FirstBlockPoint.Y -= MenuFactory.Instance.GetPoint("PlayerTwoMenuOffset").Y;
                    SecondBlockPoint.Y -= MenuFactory.Instance.GetPoint("PlayerTwoMenuOffset").Y;
                    ThirdBlockPoint.Y -= MenuFactory.Instance.GetPoint("PlayerTwoMenuOffset").Y;
                }
            }
            else
            {
                if (!inHUD)
                {
                    FirstBlockPoint.Y += MenuFactory.Instance.GetPoint("MenuOffset").Y;
                    SecondBlockPoint.Y += MenuFactory.Instance.GetPoint("MenuOffset").Y;
                    ThirdBlockPoint.Y += MenuFactory.Instance.GetPoint("MenuOffset").Y;
                }
            }

            Inventory = inventory;
            Count = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            FirstBlock.Draw(spriteBatch, FirstBlockPoint, color);
            SecondBlock.Draw(spriteBatch, SecondBlockPoint, color);
            ThirdBlock.Draw(spriteBatch, ThirdBlockPoint, color);
        }

        public void Update()
        {
            if (Count != Inventory.ItemAmount(Item.Key))
            {
                Count = Inventory.ItemAmount(Item.Key);
                SecondBlock.Update(Count / 10);
                ThirdBlock.Update(Count % 10);
            }
            if (Count > MaxDisplayableValue)
            {
                SecondBlock.Update((int)CountSymbol.Nine);
                ThirdBlock.Update((int)CountSymbol.Nine);
            }
        }
    }
}
