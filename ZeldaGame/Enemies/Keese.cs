/*
 * Creates Keese
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
    public class Keese : OrdinalEnemy
    {
        private readonly int distanceToChangeDirection, maxAnimFramerate;
        private int currentFrame, animFramerate, health, Timetillslowdown, SlowDownTimer;
        private float velocity;
        private readonly float speedup;
        private bool SlowDown;
        private readonly EnemyDataManager data;

        public Keese(Point position, EnemyDataManager data, String color)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);
            this.position = position;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(color + name);
            health = data.GetHealth(name);
            velocity = data.GetVelocity(name);
            speedup = data.GetSpeedup(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            maxAnimFramerate = data.GetAnimationFramerate(name);
            animFramerate = maxAnimFramerate;
            data.GetEnemyHitbox(name, out width, out height);
            SlowDown = false;
        }
        public override void Update(GameTime gametime)
        {
            //Update sprite animation
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            currentFrame++;
            currentFrame %= animFramerate;
            if (currentFrame == 0)
            {
                if (velocity > 0.060)
                {
                    sprite.UpdateFrame();
                }
                if (animFramerate != maxAnimFramerate / 2 && !SlowDown)
                {
                    velocity += speedup;
                    animFramerate -= 1;
                }
                else if (SlowDown && animFramerate != maxAnimFramerate + 2)
                {
                    velocity -= speedup;
                    animFramerate += 1;
                }
            }
            //Change direction and attack
            if (currentDistance >= distanceToChangeDirection)
            {
                ChangeDirection();
            }
            if (Timetillslowdown > 500)
            {
                SlowDownKeese();
            }
            if (currentFrame % 2 == 0 && stunDuration <= 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                Move(movementVal);
            }
            Timetillslowdown++;

            //Update stun duration
            if (stunDuration > 0) stunDuration--;
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
            }
        }

        private void SlowDownKeese()
        {
            SlowDown = true;
            SlowDownTimer++;
            if (SlowDownTimer > 300)
            {
                ResetSlowDown();
            }
        }

        private void ResetSlowDown()
        {
            SlowDown = false;
            Timetillslowdown = 0;
            SlowDownTimer = 0;
        }
    }
}