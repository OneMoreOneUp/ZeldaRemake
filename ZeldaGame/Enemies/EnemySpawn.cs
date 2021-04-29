/*
 * Creates the enemy spawn animation
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
    public class EnemySpawn : IEnemy
    {
        private readonly ISprite enemySpawn;
        private Point position;
        private int lastFrame;
        private readonly int animFramerate, damage, width, height;
        private readonly IEnemy owner;
        private readonly float layer;

        public EnemySpawn(Point position, IEnemy owner, EnemyDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.owner = owner;
            this.position = position;
            this.layer = data.GetLayer(name);
            enemySpawn = SpriteFactory.Instance.CreateSprite(name);

            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
        }

        public void Update(GameTime gametime)
        {
            lastFrame++;
            lastFrame %= animFramerate;
            if (lastFrame == 0 && enemySpawn.UpdateFrame())
            {
                SpawnEnemy();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            enemySpawn.Draw(spriteBatch, position, color, layer);
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

        public void SpawnEnemy()
        {
            if (EnemyManager.Contains(this))
            {
                EnemyManager.RemoveEnemy(this);
                EnemyManager.AddEnemy(owner);
            }
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
            this.owner.Stun(duration);
        }
    }
}