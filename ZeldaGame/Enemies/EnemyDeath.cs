/*
 * Creates the enemy death animation
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Items;
using ZeldaGame.Sprites;

namespace ZeldaGame.Enemies
{
    public class EnemyDeath : IEnemy
    {
        private readonly ISprite sprite;
        private Point position;
        private int lastFrame;
        private readonly int animFramerate, damage, width, height;
        private readonly ItemDataManager itemData;
        private readonly float layer;

        public EnemyDeath(Point position, EnemyDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);
            itemData = new ItemDataManager(data.GetXPath());
            this.position = position;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name);

            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
            SoundFactory.Instance.GetSound(Sound.Death).Play();
        }

        public void Update(GameTime gametime)
        {
            lastFrame++;
            lastFrame %= animFramerate;
            if (lastFrame == 0 && sprite.UpdateFrame())
            {
                EnemyManager.RemoveEnemy(this);
                LootTable loot = new LootTable();
                IItem item = loot.GetDropItem(position, itemData);
                if (item != null)
                {
                    ItemManager.AddItem(item);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void TakeDamage(Direction directionFrom, int amount)
        {
            // Left blank Intentionally 
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public void UpdateLocation(int X, int Y)
        {
            position.X += X;
            position.Y += Y;
        }

        public Point GetLocation()
        {
            return this.position;
        }

        public void ChangeDirection(Direction fromDirection)
        {

        }

        public void ChangeDirection()
        {

        }

        public int GetDamage()
        {
            return this.damage;
        }

        public void Stun(int duration)
        {
            //Enemy death ignores stuns
        }
    }
}
