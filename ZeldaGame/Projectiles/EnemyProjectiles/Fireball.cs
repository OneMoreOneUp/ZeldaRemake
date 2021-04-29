/*
 * Enemy fireball projectile class
 * 
 * Author: Michael Frank, Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enemies;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.EnemyProjectiles
{
    public class Fireball : IEnemyProjectile
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly int animFramerate, damage, width, height;
        private readonly Vector2 velocity;
        private int maxDistance, lastFrame;
        private readonly IEnemy owner;
        private readonly float layer;

        public Fireball(Point position, float xScalar, float yScalar, float angle, IEnemy owner, EnemyProjectileDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            sprite = SpriteFactory.Instance.CreateSprite(name);
            this.position = position;
            this.owner = owner;
            this.layer = data.GetLayer(name);

            damage = data.GetDamage(name);
            maxDistance = data.GetAttackDistance(name);
            animFramerate = data.GetAnimationFramerate(name);
            float baseVelocity = data.GetVelocity(name);
            velocity = new Vector2(xScalar * baseVelocity, yScalar * baseVelocity);
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(angle));
            data.GetEnemyProjectileHitbox(name, out width, out height);
        }
        public void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            lastFrame++;
            if (lastFrame >= animFramerate)
            {
                lastFrame = 0;
                sprite.UpdateFrame();
            }

            Vector2 travelledDistance = new Vector2
            {
                X = (float)Math.Round((velocity.X * gametime.ElapsedGameTime.TotalMilliseconds)),
                Y = (float)Math.Round((velocity.Y * gametime.ElapsedGameTime.TotalMilliseconds))
            };
            maxDistance -= (int)travelledDistance.Length();
            if (maxDistance > 0)
            {
                MoveFireballs(travelledDistance);
            }
            else if (EnemyProjectileManager.Contains(this))
            {
                EnemyProjectileManager.RemoveEnemyProjectile(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        private void MoveFireballs(Vector2 travelledDistance)
        {
            position.X += (int)travelledDistance.X;
            position.Y += (int)travelledDistance.Y;
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public bool IsOwner(IEnemy enemy)
        {
            return this.owner == enemy;
        }

        public void CollideRigid()
        {
            if (EnemyProjectileManager.Contains(this)) EnemyProjectileManager.RemoveEnemyProjectile(this);
        }

        public bool CanDefelct(Shield shield)
        {
            bool canDeflect = false;

            if (!(this.owner is Aquamentus) && shield == Shield.MagicalShield)
            {
                canDeflect = true;
            }

            return canDeflect;
        }

        public int GetDamage()
        {
            return damage;
        }
    }
}
