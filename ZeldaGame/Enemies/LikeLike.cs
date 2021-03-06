/*
 * Creates Gel Small
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
    public class LikeLike : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection;
        private int currentFrame, health;

        public LikeLike(Point position, EnemyDataManager data)
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
            if (currentFrame == 0) sprite.UpdateFrame();

            //Change direction and attack
            if (currentDistance >= distanceToChangeDirection)
            {
                ChangeDirection();
            }

            if (stunDuration <= 0)
            {
                if (currentDistance < distanceToChangeDirection && currentDistance > distanceToChangeDirection / 3)
                {
                    if (currentFrame % 2 == 0)
                    {
                        int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                        Move(movementVal);
                    }
                }
                else
                {
                    currentDistance++;
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
    }
}


