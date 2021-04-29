/*
 * Creates Merchant
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Enemies
{
    public class Merchant : IEnemy
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly int damage, width, height;
        private readonly float layer;

        public Merchant(Point position, EnemyDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            sprite = SpriteFactory.Instance.CreateSprite(name);
            this.position = position;
            this.layer = data.GetLayer(name);

            damage = data.GetDamage(name);
            data.GetEnemyHitbox(name, out width, out height);
        }
        public void Update(GameTime gametime)
        {
            //Doesn't move or update sprite
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }
        public void TakeDamage(Direction directionFrom, int amount)
        {
            //Doesn't take damage
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
            //Merchant cannot be stunned
        }
    }
}
