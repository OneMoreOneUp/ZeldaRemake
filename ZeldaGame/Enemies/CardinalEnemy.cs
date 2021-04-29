// Enemy that moves only in the 4 cardinal directions
// Author: Matthew Crabtree

using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Sprites;

namespace ZeldaGame.Enemies
{
    public abstract class CardinalEnemy : BaseEnemy
    {
        internal int currentDistance;
        internal float velocity;
        internal Direction lastNotDirection;
        internal string name;
        internal EnemyDataManager data;

        internal void Move(int distance)
        {
            switch (facing)
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
        }

        public override void ChangeDirection(Direction notDirection)
        {
            do
            {
                ChangeDirection();
            } while (facing == notDirection || facing == lastNotDirection);
            lastNotDirection = notDirection;
        }

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

            sprite = SpriteFactory.Instance.CreateSprite(name, facing);
            currentDistance = 0;        //Resets time since last turn
        }
    }
}
