// Player arrow projectile class
// Author : Michael Frank

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
    class Arrow : IPlayerProjectile
    {
        private readonly Direction direction;
        private Point position;
        private readonly ISprite sprite;
        private readonly IAdventurePlayer owner;
        private readonly float velocity, layer;
        private int maxDistance, width, height;
        private readonly int damage;
        private readonly PlayerProjectileDataManager data;

        public Arrow(Direction direction, IAdventurePlayer owner, PlayerProjectileDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.direction = direction;
            this.position = owner.GetLocation();
            this.layer = data.GetLayer(name);
            this.owner = owner;
            sprite = SpriteFactory.Instance.CreateSprite(name, direction);

            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            maxDistance = data.GetAttackDistance(name);
            GetDimensions(data, name);
            MoveStraight(data.GetSummonDistance(name));

            SoundFactory.Instance.GetSound(Sound.ShootingArrowAndBoomerang).Play();
        }

        private void GetDimensions(PlayerProjectileDataManager data, string name)
        {
            if (direction == Direction.North || direction == Direction.South)
            {
                data.GetVertHitbox(name, out width, out height);
            }
            else
            {
                data.GetHorHitbox(name, out width, out height);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void Update(GameTime gameTime)
        {
            int movementValue = (int)(velocity * gameTime.ElapsedGameTime.TotalMilliseconds);
            maxDistance -= movementValue;
            if (maxDistance > 0)
            {
                MoveStraight(movementValue);
            }
            else
            {
                PlayerProjectileManager.RemovePlayerProjectile(this);
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

        public bool IsOwner(IAdventurePlayer player)
        {
            return this.owner == player;
        }

        public void CollideRigid()
        {
            if (PlayerProjectileManager.Contains(this))
            {
                PlayerProjectileManager.RemovePlayerProjectile(this);
                GameObjectManager.AddGameObject(new Projectiles.Particles.Arrow(position, direction, new ParticleDataManager(data.GetXPath())));
            }
        }

        public int GetDamage()
        {
            return damage;
        }
    }
}
