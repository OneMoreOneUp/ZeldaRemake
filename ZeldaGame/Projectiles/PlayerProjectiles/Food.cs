// Player food class
// Author : Michael Frank, Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.PlayerProjectiles
{
    class Food : IPlayerProjectile
    {
        private Point position;
        private readonly IAdventurePlayer owner;
        private readonly ISprite sprite;
        private int expireTime;
        private readonly int damage, width, height;
        private readonly float layer;

        public Food(Direction direction, IAdventurePlayer owner, PlayerProjectileDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = owner.GetLocation();
            this.owner = owner;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name);

            damage = data.GetDamage(name);
            data.GetFoodAttributes(out expireTime);
            data.GetPlayerProjectileHitbox(name, out width, out height);
            CalculateLocation(data.GetSummonDistance(name), direction);
        }

        private void CalculateLocation(int distance, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    position.Y -= distance;
                    break;
                case Direction.South:
                    position.Y += distance;
                    break;
                case Direction.West:
                    position.X -= distance;
                    break;
                default:
                    position.X += distance;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void Update(GameTime gameTime)
        {
            if (expireTime > 0)
            {
                expireTime--;
            }
            else
            {
                PlayerProjectileManager.RemovePlayerProjectile(this);
            }
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public bool IsOwner(IAdventurePlayer player)
        {
            return this.owner == player;
        }

        public void CollideRigid()
        {
            //Food ignores collision with rigid objects
        }

        public int GetDamage()
        {
            return damage;
        }
    }
}
