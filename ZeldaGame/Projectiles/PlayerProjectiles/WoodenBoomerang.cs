// Player boomerang projectile
// Author : Michael Frank, Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Projectiles.Particles;
using ZeldaGame.Sprites;

namespace ZeldaGame.PlayerProjectiles
{
    class WoodenBoomerang : IPlayerProjectile
    {
        private readonly Direction direction;
        private Point position;
        private readonly ISprite sprite;
        private readonly IAdventurePlayer owner;
        private int maxDistance, updateCounter;
        private readonly float velocity, layer;
        private readonly int damage, animFramerate, width, height;
        private readonly PlayerProjectileDataManager data;
        private readonly SoundEffectInstance sound;

        public WoodenBoomerang(Direction direction, IAdventurePlayer owner, PlayerProjectileDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = owner.GetLocation();
            this.direction = direction;
            this.owner = owner;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name);

            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            maxDistance = data.GetAttackDistance(name);
            data.GetPlayerProjectileHitbox(name, out width, out height);
            MoveStraight(data.GetSummonDistance(name));

            this.sound = SoundFactory.Instance.GetSound(Sound.ShootingArrowAndBoomerang).CreateInstance();
            this.sound.IsLooped = true;
            this.sound.Play();
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
                MoveTowardsOwner(movementValue);
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

        private void MoveTowardsOwner(int movementValue)
        {
            //Code based on https://stackoverflow.com/questions/20518218/how-to-make-an-object-move-towards-another-object-in-c-sharp-xna
            Vector2 difference = owner.GetLocation().ToVector2() - this.position.ToVector2();
            difference.Normalize();
            float speed = (float)(difference.Length() / 1.0 * movementValue);
            this.position += (difference * speed).ToPoint();
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
            if (maxDistance > 0) GameObjectManager.AddGameObject(new Spark(this.position, new ParticleDataManager(data.GetXPath())));
            maxDistance = 0;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void StopSound()
        {
            this.sound.Stop();
        }
    }
}
