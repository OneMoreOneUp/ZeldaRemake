﻿// Boss key Item Class
// Author : Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;
using ZeldaGame.Sprites;

namespace ZeldaGame.Items
{
    class BossKey : IItem
    {

        private Point position;
        private ISprite font;
        private readonly ISprite sprite;
        private readonly int cost, width, height;
        private readonly float layer;

        public BossKey(Point position, ItemDataManager data, int cost = 0)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.layer = data.GetLayer(name);
            this.cost = cost;
            this.sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetItemHitbox(name, out width, out height);
            CreateFont();
        }

        private void CreateFont()
        {
            if (cost != 0)
            {
                string costString = cost.ToString(CultureInfo.InvariantCulture);
                this.font = SpriteFactory.Instance.CreateFontSprite(costString, costString.Length);
                for (int i = 0; i < costString.Length; i++)
                {
                    this.font.UpdateFrame();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
            if (font != null)
            {
                Point fontPosition = new Point(position.X - ((int)(cost.ToString(CultureInfo.InvariantCulture).Length / 2.0) * SpriteFactory.FontWidth), position.Y - (height / 2) - SpriteFactory.FontHeight);
                this.font.Draw(spriteBatch, fontPosition, Color.White, layer);
            }
        }

        public void Update(GameTime gametime)
        {
            //BossKey has no animation or movement
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public void UpdateLocation(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
        }

        public int GetCost()
        {
            return this.cost;
        }

        public void PlayerPickUp(IAdventurePlayer player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            AdventurePlayerInventory inventory = player.GetInventory();

            if (this.cost <= inventory.GetBalance())
            {
                inventory.RemoveBalance(cost);
                inventory.AddItem(Item.Key);
                SoundFactory.Instance.GetSound(Sound.GetHeartOrKey).Play();
                ItemManager.RemoveItem(this);
            }
        }
    }
}
