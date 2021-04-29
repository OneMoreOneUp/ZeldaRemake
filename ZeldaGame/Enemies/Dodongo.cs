/*
 * Creates Dodongo
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Sprites;

namespace ZeldaGame.Enemies
{

    public class Dodongo : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection, damageDuration;
        private int currentFrame, currentDamageFrame, health;
        private bool damaged;

        public Dodongo(Point position, EnemyDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            name = NameLookupTable.GetName(this);

            this.position = position;
            this.layer = data.GetLayer(name);
            ChangeDirection();

            health = data.GetHealth(name);
            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            animFramerate = data.GetAnimationFramerate(name);
            damageDuration = data.GetEnemyAttackDuration(name);
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

            //Change direction 
            if (currentDistance >= distanceToChangeDirection)
            {
                ChangeDirection();
                SoundFactory.Instance.GetSound(Sound.BossScream2).Play();
            }

            //Movement or hold after attack
            if (!damaged)
            {
                if (currentFrame % 2 == 0 && stunDuration <= 0)
                {
                    int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                    Move(movementVal);
                }
            }
            else
            {
                Hold();
            }

            //Update stun duration
            if (stunDuration > 0) stunDuration--;
        }

        private void Hold()
        {
            currentDamageFrame++;
            currentDamageFrame %= damageDuration;
            if (currentDamageFrame == 0)
            {
                damaged = false;
                ChangeDirection();
            }
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            health -= amount;
            if (health > 0)
            {
                sprite = SpriteFactory.Instance.CreateSprite("DodongoDamaged", facing);
            }
            else
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, this.data));
                EnemyManager.RemoveEnemy(this);
            }
            damaged = true;
        }
    }
}