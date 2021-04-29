/*
 * Creates Aquamentus
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
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
    public class Aquamentus : BaseEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection;
        private readonly float velocity;
        private int currentFrame, currentDistance, health;
        private Direction moving;
        private readonly EnemyDataManager data;

        public Aquamentus(Point position, EnemyDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            sprite = SpriteFactory.Instance.CreateSprite(name);
            moving = Direction.East;
            this.position = position;
            this.layer = data.GetLayer(name);

            health = data.GetHealth(name);
            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
        }

        public override void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            //Update sprite animation
            currentFrame++;
            currentFrame %= animFramerate;
            if (currentFrame == 0)
            {
                sprite.UpdateFrame();
            }

            //Change direction and attack
            if (currentDistance >= distanceToChangeDirection)
            {
                if (moving == Direction.West) SpawnFireballs();
                ChangeDirection();
            }

            //Movement
            if (currentFrame % 2 == 0 && stunDuration <= 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                Move(movementVal);
            }

            //Update stun duration
            if (stunDuration > 0) stunDuration--;
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            health -= amount;
            if (health > 0)
            {
                EnemyManager.AddEnemy(new DamagedEnemy(this, damagedFrom, false));
                EnemyManager.RemoveEnemy(this);
            }
            else
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
            }
        }

        private void Move(int distance)
        {
            switch (moving)
            {
                case Direction.East:
                    position.X += distance;
                    break;
                case Direction.West:
                    position.X -= distance;
                    break;
            }
            currentDistance += distance;
        }

        public override void ChangeDirection()
        {
            switch (moving)
            {
                case Direction.East:
                    moving = Direction.West;
                    break;
                case Direction.West:
                    moving = Direction.East;
                    break;
            }
            currentDistance = 0;        //Resets time since last turn
        }

        private void SpawnFireballs()
        {
            float angle = GetPlayerAngle();
            EnemyProjectileDataManager projectileData = new EnemyProjectileDataManager(data.GetXPath());
            EnemyProjectileManager.AddEnemyProjectile(new Fireball(position, -1, 1, angle, this, projectileData));
            EnemyProjectileManager.AddEnemyProjectile(new Fireball(position, -1, 0, angle, this, projectileData));
            EnemyProjectileManager.AddEnemyProjectile(new Fireball(position, -1, -1, angle, this, projectileData));
            SoundFactory.Instance.GetSound(Sound.BossScream1).Play();
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

        public override void ChangeDirection(Direction notDirection) { }
    }
}
