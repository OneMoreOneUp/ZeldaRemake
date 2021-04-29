/*
 * Creates StoneStatueTwo
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ZeldaGame.EnemyProjectiles;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
namespace ZeldaGame.Enemies
{
    public class RightStatueOne : IEnemy
    {
        private readonly ISprite sprite;
        private Point position;
        private int timeToAttack, stunDuration;
        private readonly int width, height, damage, attackTime;
        private readonly EnemyProjectileDataManager projectileData;
        private readonly float layer;

        public RightStatueOne(Point position, int level, EnemyDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name, Direction.Null, level);
            projectileData = new EnemyProjectileDataManager(data.GetXPath());

            attackTime = data.GetEnemyAttackDuration(name);
            damage = data.GetDamage(name);
            data.GetEnemyHitbox(name, out width, out height);
        }
        public void Update(GameTime gametime)
        {
            if (stunDuration <= 0)
            {
                if (timeToAttack >= attackTime)
                {
                    SpawnFireballs();
                    timeToAttack = 0;
                }
                timeToAttack++;
            }
            else
            {
                stunDuration--;
            }
        }
        public void SpawnFireballs()
        {
            EnemyProjectileManager.AddEnemyProjectile(new Fireball(position, -1, 0, GetPlayerAngle(), this, projectileData));
        }

        private float GetPlayerAngle()
        {
            List<IAdventurePlayer> players = PlayerManager.GetPlayers();
            if (players.Count == 0) return 0;

            Random random = new Random();
            IAdventurePlayer player = players[random.Next(0, players.Count)];
            Point location = player.GetLocation();

            double x = position.X - location.X;
            double y = position.Y - location.Y;

            return (float)Math.Atan2(y, x);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }
        public void TakeDamage(Direction directionFrom, int amount)
        {
            //Cannot take damage
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
