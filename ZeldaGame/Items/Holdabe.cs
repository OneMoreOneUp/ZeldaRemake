// A hitboxless item that cannot be picked up. Mocks the sprite of another item.
// Author : Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Items
{
    class Holdable : IItem
    {
        private Point position;
        private readonly ISprite sprite;
        private readonly float layer;

        public Holdable(Point position, IItem item, ItemDataManager data)
        {
            string name = NameLookupTable.GetName(this);

            this.layer = data.GetLayer(name);
            this.position = position;
            this.sprite = SpriteFactory.Instance.CreateSprite(NameLookupTable.GetName(item));
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public Rectangle GetHitbox()
        {
            return Rectangle.Empty;
        }

        public void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));
        }

        public void UpdateLocation(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
        }

        public int GetCost()
        {
            return 0;
        }

        public void PlayerPickUp(IAdventurePlayer player)
        {
            //Cannot pickup holdable
        }
    }
}