/*
 * Creates Rope
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Sprites;
using ZeldaGame.Triggers;

namespace ZeldaGame.Enemies
{
    public class Rope : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection;
        private int currentFrame, health;
        private readonly TriggerBoxManager triggers;
        private bool attacking;

        public Rope(Point position, EnemyDataManager data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            name = NameLookupTable.GetName(this);

            lastNotDirection = Direction.North;
            ChangeDirection(Direction.South);

            this.position = position;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name, facing);

            velocity = data.GetVelocity(name);
            health = data.GetHealth(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);

            int attackDist = data.GetHorizontalAttackDistance(name);
            triggers = new TriggerBoxManager();
            triggers.AddTriggerBox(new RopeTrigger(this, Direction.East, attackDist));
            triggers.AddTriggerBox(new RopeTrigger(this, Direction.West, attackDist));
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
            }

            //Movement (move if not stunned)
            if (stunDuration <= 0)
            {
                triggers.DetectPlayerCollisions();
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                if (!attacking && currentFrame % 2 == 0)
                {
                    Move(movementVal);
                    currentDistance += movementVal;
                }
                else if (attacking)
                {
                    Move(movementVal);
                    attacking = false;
                }
            }
            else
            {
                stunDuration--;
            }
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

        public override void ChangeDirection()
        {
            Random random = new Random();
            int rndNumber = random.Next(1, 5);
            switch (rndNumber)
            {
                case 1:
                    facing = Direction.East;
                    sprite = SpriteFactory.Instance.CreateSprite(name, Direction.East);
                    break;
                case 2:
                    facing = Direction.West;
                    sprite = SpriteFactory.Instance.CreateSprite(name, Direction.West);
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

        public void Attack(Direction attackDirection)
        {
            if (facing != attackDirection)
            {
                facing = attackDirection;
                sprite = SpriteFactory.Instance.CreateSprite(name, attackDirection);
            }
            attacking = true;
        }
    }
}
