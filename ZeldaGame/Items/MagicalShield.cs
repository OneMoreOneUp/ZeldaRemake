// Magical shield item Class
// Author : Benjamin J Nagel
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
    public class MagicalShield : IItem
    {
        private Point position;
        private ISprite font;
        private readonly int cost, width, height;
        private readonly ISprite sprite;
        private readonly float layer;

        public MagicalShield(Point position, ItemDataManager data, int cost = 0)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);
            this.position = position;
            this.cost = cost;
            this.layer = data.GetLayer(name);
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

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public void Update(GameTime gametime)
        {
            //Arrow has no animation or movement
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
                inventory.AddItem(Item.MagicalShield);
                inventory.SetShield(AdventurePlayerInventory.Shield.MagicalShield);
                player.HoldItem(this);
                ItemManager.RemoveItem(this);
            }
        }
    }
}
