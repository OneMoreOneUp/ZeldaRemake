/*
 * Creates Trap
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Sprites;
using ZeldaGame.Triggers;

namespace ZeldaGame.Enemies
{
    public class SpikeTrap : BaseEnemy
    {
        private readonly int vertAttackDist, horAttackDist;
        private int currentDistance;
        private readonly float velocity;
        private bool attacking, returning;
        private Direction attackDirection;
        private readonly TriggerBoxManager triggers;

        public SpikeTrap(Point position, EnemyDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            sprite = SpriteFactory.Instance.CreateSprite(name);
            this.position = position;
            this.layer = data.GetLayer(name);

            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            data.GetEnemyHitbox(name, out width, out height);
            vertAttackDist = data.GetVerticalAttackDistance(name);
            horAttackDist = data.GetHorizontalAttackDistance(name);

            triggers = new TriggerBoxManager();
            triggers.AddTriggerBox(new SpikeTrapTrigger(this, Direction.North, vertAttackDist * 2));
            triggers.AddTriggerBox(new SpikeTrapTrigger(this, Direction.South, vertAttackDist * 2));
            triggers.AddTriggerBox(new SpikeTrapTrigger(this, Direction.East, horAttackDist * 2));
            triggers.AddTriggerBox(new SpikeTrapTrigger(this, Direction.West, horAttackDist * 2));
        }

        public override void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            int movementVal;
            //Don't move if stunned
            if (stunDuration <= 0)
            {
                if (returning)
                {
                    movementVal = (int)((velocity / 2) * gametime.ElapsedGameTime.TotalMilliseconds);
                    ReturnMove(movementVal);
                }
                else if (attacking)
                {
                    movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                    AttackMove(movementVal);
                }
                else
                {
                    triggers.DetectPlayerCollisions();
                }
            }
            else
            {
                stunDuration--;
            }
        }

        private void AttackMove(int distance)
        {
            switch (attackDirection)
            {
                case Direction.East:
                    UpdateLocation(distance, 0);
                    break;
                case Direction.West:
                    UpdateLocation(-1 * distance, 0);
                    break;
                case Direction.South:
                    UpdateLocation(0, distance);
                    break;
                case Direction.North:
                    UpdateLocation(0, -1 * distance);
                    break;
            }
            currentDistance += distance;
            CheckAttackDistance();
        }

        private void CheckAttackDistance()
        {
            if (attackDirection == Direction.East || attackDirection == Direction.West)
            {
                if (currentDistance >= horAttackDist)
                {
                    attacking = false;
                    returning = true;
                }
            }
            else if (currentDistance >= vertAttackDist)
            {
                attacking = false;
                returning = true;
            }
        }

        private void ReturnMove(int distance)
        {
            switch (attackDirection)
            {
                case Direction.East:
                    UpdateLocation(-1 * distance, 0);
                    break;
                case Direction.West:
                    UpdateLocation(distance, 0);
                    break;
                case Direction.South:
                    UpdateLocation(0, -1 * distance);
                    break;
                case Direction.North:
                    UpdateLocation(0, distance);
                    break;
            }

            currentDistance -= distance;
            if (currentDistance <= 0) returning = false;
        }

        public override void TakeDamage(Direction damagedFrom, int amount) { }
        public override void ChangeDirection(Direction fromDirection) { }
        public override void ChangeDirection() { }

        public void Attack(Direction direction)
        {
            if (!returning)
            {
                attacking = true;
                attackDirection = direction;
            }
        }
    }
}
