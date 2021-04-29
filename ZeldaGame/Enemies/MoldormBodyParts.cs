/*
 * Creates MoldormBodyParts
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
using ZeldaGame.Sprites;
namespace ZeldaGame.Enemies
{
    public class MoldormBodyParts : IEnemy
    {
        private readonly ISprite sprite;
        private readonly MoldormController Controller;
        private Point position;
        private readonly int damage, width, height, queueIndex;
        private int health, stunDuration;
        private readonly EnemyDataManager data;
        private readonly string name;
        private readonly float layer;


        public MoldormBodyParts(Point position, int queueIndex, EnemyDataManager data, MoldormController controller)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));

            Controller = controller;
            name = NameLookupTable.GetName(this);
            this.position = position;
            this.layer = data.GetLayer(name);
            health = data.GetHealth(name);
            damage = data.GetDamage(name);
            sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetEnemyHitbox(name, out width, out height);
            this.queueIndex = queueIndex;
        }

        public void Update(GameTime gametime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            position = Controller.GetBodyPosition(queueIndex);
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void TakeDamage(Direction damagedFrom, int amount)
        {
            if (Controller.CanTakeDamage())
            {
                Controller.TakeDamage(damagedFrom, amount);

                if (health > 0)
                {
                    EnemyManager.AddEnemy(new DamagedEnemy(this, damagedFrom, false));
                    EnemyManager.RemoveEnemy(this);
                }
            }
        }

        public void ApplyDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
                Controller.RemoveBody();
            }
        }

        public void ChangeDirection()
        {
            Controller.ChangeDirection();
        }

        public void ChangeDirection(Direction notDirection)
        {
            Controller.ChangeDirection(notDirection);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public void UpdateLocation(int X, int Y)
        {
            if (queueIndex == 0)
            {
                position.X += X;
                position.Y += Y;
            }
        }

        public Point GetLocation()
        {
            return this.position;
        }

        public int GetDamage()
        {
            if (stunDuration > 0)
            {
                return 0;
            }
            else
            {
                return this.damage;
            }
        }

        public void Stun(int duration)
        {
            this.stunDuration = duration;
        }
    }
}
