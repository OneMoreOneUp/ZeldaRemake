/*
 * Creates Wall Master
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Sprites;

namespace ZeldaGame.Enemies
{
    class WallMaster : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection;
        private int currentFrame, health;
        private Direction spriteDirection;
        private readonly Room Room;
        public WallMaster(Point position, EnemyDataManager data, Room room)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);
            Room = room;
            SetRandomFacing();
            this.position = position;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(name, spriteDirection);

            health = data.GetHealth(name);
            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
        }

        private void SetRandomFacing()
        {
            Random random = new Random();
            int rndNumber = random.Next(1, 4);
            if (rndNumber == 1)
            {
                spriteDirection = Direction.East;
            }
            else if (rndNumber == 2)
            {
                spriteDirection = Direction.West;
            }
            else
            {
                spriteDirection = Direction.South;
            }
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
        public void GrabPlayer(IAdventurePlayer player)
        {
            GameObjectManager.AddGameObject(new Projectiles.Particles.WallMaster(this.position, player, this.spriteDirection, data, Room, this));
        }
    }
}
