// Heart Item Class
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
    class Heart : IItem
    {
        private Point position;
        private readonly ISprite sprite;
        private ISprite font;
        private readonly int cost, width, height, animFramerate;
        private int updateCounter;
        private readonly float layer;

        public Heart(Point position, ItemDataManager data, int cost = 0)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.cost = cost;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetItemHitbox(name, out width, out height);
            animFramerate = data.GetAnimationFramerate(name);
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
            updateCounter++;
            updateCounter %= animFramerate;
            if (updateCounter == 0)
            {
                sprite.UpdateFrame();
            }
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
                IncreasePlayerHealth(inventory, 2);
                SoundFactory.Instance.GetSound(Sound.GetHeartOrKey).Play();
                ItemManager.RemoveItem(this);
            }
        }

        private static void IncreasePlayerHealth(PlayerInventory inventory, int amount)
        {
            int currentHealth = inventory.GetHealth() + amount;
            int maxHealth = inventory.GetMaxHealth();

            if (currentHealth > maxHealth) currentHealth = maxHealth; //Prevents player from healing over max health

            inventory.SetHealth(currentHealth);
        }
    }
}
