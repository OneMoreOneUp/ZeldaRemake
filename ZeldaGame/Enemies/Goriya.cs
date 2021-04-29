/*
 * Red Goriya enemy class
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using ZeldaGame.EnemyProjectiles;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;

namespace ZeldaGame.Enemies
{
    public class Goriya : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection, attackDuration;
        private int currentFrame, currentAttackFrame, health;
        private bool attacking;
        private readonly string Color;
        public Goriya(Point position, EnemyDataManager data, string color)

        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            name = NameLookupTable.GetName(this);
            Color = color;
            this.position = position;
            this.layer = data.GetLayer(name);
            health = data.GetHealth(name);
            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            attackDuration = data.GetEnemyAttackDuration(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
            name = color + name;
            ChangeDirection();
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
                SummonBoomerang();
            }

            //Movement or hold after attack
            if (!attacking)
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
            currentAttackFrame++;
            currentAttackFrame %= attackDuration;
            if (currentAttackFrame == 0) attacking = false;
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            health -= amount;
            if (health >= 0)
            {
                EnemyManager.AddEnemy(new DamagedEnemy(this, damagedFrom, damagedFrom == facing));
                EnemyManager.RemoveEnemy(this);
            }
            else
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
            }
        }

        private void SummonBoomerang()
        {
            Point boomerangPos = position;
            int distance = ((width + height) / 2) + 2;                      //Distance from the enemy to summon the boomerang
            switch (facing)
            {
                case Direction.North:
                    boomerangPos.Y -= distance;
                    break;
                case Direction.South:
                    boomerangPos.Y += distance;
                    break;
                case Direction.West:
                    boomerangPos.X -= distance;
                    break;
                case Direction.East:
                    boomerangPos.X += distance;
                    break;
            }
            if (Color == "Red")
            {
                EnemyProjectileManager.AddEnemyProjectile(new WoodenBoomerang(boomerangPos, facing, this, new EnemyProjectileDataManager(data.GetXPath())));
            }
            else
            {
                EnemyProjectileManager.AddEnemyProjectile(new MagicalBoomerang(boomerangPos, facing, this, new EnemyProjectileDataManager(data.GetXPath())));
            }
            attacking = true;
        }
    }
}