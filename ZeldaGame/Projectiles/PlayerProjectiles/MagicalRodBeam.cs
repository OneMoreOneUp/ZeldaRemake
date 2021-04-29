// Player magical rod beam projectile class
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
    class MagicalRodBeam : IPlayerProjectile
    {
        private readonly Direction direction;
        private Point position;
        private readonly ISprite sprite;
        private readonly IAdventurePlayer owner;
        private readonly float velocity, layer;
        private int updateCounter, maxDistance;
        private readonly int damage, animFramerate, width, height;

        public MagicalRodBeam(Direction direction, IAdventurePlayer owner, PlayerProjectileDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.direction = direction;
            this.position = owner.GetLocation();
            this.owner = owner;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name, direction);

            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            maxDistance = data.GetAttackDistance(name);
            data.GetPlayerProjectileHitbox(name, out width, out height);
            MoveStraight(data.GetSummonDistance(name));

            SoundFactory.Instance.GetSound(Sound.MagicRod).Play();
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void Update(GameTime gameTime)
        {
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
            if (PlayerProjectileManager.Contains(this)) PlayerProjectileManager.RemovePlayerProjectile(this);
        }

        public int GetDamage()
        {
            return damage;
        }
    }
}
