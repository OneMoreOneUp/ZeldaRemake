using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Projectiles.Particles;
using ZeldaGame.Sprites;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.EnemyProjectiles
{
    public class MagicalRodBeam : IEnemyProjectile
    {
        private readonly Direction direction;
        private Point position;
        private readonly ISprite sprite;
        private readonly IEnemy owner;
        private int maxDistance, updateCounter;
        private readonly float velocity, layer;
        private readonly int damage, animFramerate, width, height;
        private readonly EnemyProjectileDataManager data;
        public MagicalRodBeam(Point position, Direction direction, IEnemy owner, EnemyProjectileDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.direction = direction;
            this.owner = owner;
            sprite = SpriteFactory.Instance.CreateSprite(name, direction);

            this.layer = data.GetLayer(name);
            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            maxDistance = data.GetAttackDistance(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyProjectileHitbox(name, out width, out height);
            SoundFactory.Instance.GetSound(Sound.MagicRod).Play();
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime == null) throw new ArgumentNullException(nameof(gameTime));
            updateCounter++;
            if (updateCounter >= animFramerate)
            {
                sprite.UpdateFrame();
                updateCounter = 0;
            }

            int movementValue = (int)(velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            maxDistance -= movementValue;
            if (maxDistance > 0)
            {
                MoveStraight(movementValue);
            }
            else
            {
                EnemyProjectileManager.RemoveEnemyProjectile(this);
            }
        }

        private void MoveStraight(int movementValue)
        {
            switch (direction)
            {
                case Direction.North:
                    position.Y -= movementValue;
                    break;
                case Direction.South:
                    position.Y += movementValue;
                    break;
                case Direction.East:
                    position.X += movementValue;
                    break;
                case Direction.West:
                    position.X -= movementValue;
                    break;
            }
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
            if (maxDistance > 0) GameObjectManager.AddGameObject(new Spark(this.position, new ParticleDataManager(data.GetXPath())));
            maxDistance = 0;
        }

        public bool CanDefelct(Shield shield)
        {
            return true;
        }

        public int GetDamage()
        {
            return this.damage;
        }
    }
}
