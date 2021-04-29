// Key Item Class
// Author : Benjamin Nagel
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;
using ZeldaGame.Sprites;
namespace ZeldaGame.Items
{
    class StepLadder : IItem
    {
        private Point position;
        private readonly int cost, width, height;
        private readonly ISprite sprite;
        public StepLadder(Point position, ItemDataManager data, int cost = 0)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.cost = cost;
            this.sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetItemHitbox(name, out width, out height);
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, 0.45f);
        }

        public void Update(GameTime gametime)
        {
            //Ladder has no animation or movement
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

                //All players should get this item because it is a key item.
                List<IAdventurePlayer> players = new List<IAdventurePlayer>(PlayerManager.GetPlayers());
                foreach (IAdventurePlayer adventurePlayer in players)
                {
                    adventurePlayer.HoldItem(this);
                    adventurePlayer.GetInventory().AddItem(Item.StepLadder);
                }


                ItemManager.RemoveItem(this);
            }
        }
    }
}