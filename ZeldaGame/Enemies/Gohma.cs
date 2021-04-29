/*
 * Creates Gohma
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

    public class Gohma : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection;
        private int currentFrame, health;
        public Gohma(Point position, EnemyDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name);

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
                ChangeDirection();
                SpawnFireballs();
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
            if (health >= 0)
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

        //Overwritten because sprite doesnt change with facing
        public override void ChangeDirection()
        {
            Random random = new Random();
            int rndNumber = random.Next(1, 5);
            switch (rndNumber)
            {
                case 1:
                    facing = Direction.East;
                    break;
                case 2:
                    facing = Direction.West;
                    break;
                case 3:
                    facing = Direction.South;
                    break;
                case 4:
                    facing = Direction.North;
                    break;
            }
            currentDistance = 0;        //Resets time since last turn
        }
        private void SpawnFireballs()
        {
            float angle = GetPlayerAngle();
            EnemyProjectileDataManager projectileData = new EnemyProjectileDataManager(data.GetXPath());
            EnemyProjectileManager.AddEnemyProjectile(new Fireball(position, -1, 0, angle, this, projectileData));
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

    }
}
