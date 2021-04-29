// Enemy that moves only in the 4 cardinal directions
// Author: Matthew Crabtree

using System;
using ZeldaGame.Enums;

namespace ZeldaGame.Enemies
{
    public abstract class OrdinalEnemy : BaseEnemy
    {
        internal Direction lastNotDirection;
        internal int currentDistance;

        public override void ChangeDirection()
        {
            Random random = new Random();
            int rndNumber = random.Next(1, 9);
            switch (rndNumber)
            {
                case 1:
                    facing = Direction.East;
                    break;
                case 2:
                    facing = Direction.West;
                    break;
                case 3:
                    facing = Direction.North;
                    break;
                case 4:
                    facing = Direction.South;
                    break;
                case 5:
                    facing = Direction.SouthEast;
                    break;
                case 6:
                    facing = Direction.NorthEast;
                    break;
                case 7:
                    facing = Direction.SouthWest;
                    break;
                case 8:
                    facing = Direction.NorthWest;
                    break;
            }

            currentDistance = 0;
        }

        public override void ChangeDirection(Direction notDirection)
        {
            do
            {
                ChangeDirection();
            } while (facing == notDirection || facing == lastNotDirection || facing.ToString().Contains(notDirection.ToString(), StringComparison.InvariantCulture));
            lastNotDirection = notDirection;
        }

        internal virtual void Move(int distance)
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
                case Direction.SouthEast:
                    UpdateLocation(distance, distance);
                    break;
                case Direction.NorthEast:
                    UpdateLocation(distance, -1 * distance);
                    break;
                case Direction.SouthWest:
                    UpdateLocation(-1 * distance, distance);
                    break;
                case Direction.NorthWest:
                    UpdateLocation(-1 * distance, -1 * distance);
                    break;
            }
            currentDistance += distance;
        }
    }
}
