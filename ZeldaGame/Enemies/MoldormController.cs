/*
 * Creates MoldormController
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Enemies
{
    public class MoldormController : OrdinalEnemy
    {
        private readonly float velocity;
        private int currentFrame, currentTotalDistance, length;
        private readonly int distanceToChangeDirection, numberOfPointsBetween, maxPositionQueueLength;
        private readonly string name;
        private readonly List<MoldormBodyParts> bodys;
        private readonly EnemyDataManager Data;
        private readonly List<IEnemy> enemies;
        private readonly List<Point> positionQueue;
        private Point lastConstruct;

        public MoldormController(Point position, EnemyDataManager data, List<IEnemy> enemies)
        {
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            this.enemies = enemies ?? throw new ArgumentNullException(nameof(data));
            name = NameLookupTable.GetName(this);
            velocity = data.GetVelocity(name);
            this.length = 4;
            distanceToChangeDirection = data.GetDirectionChange(name);
            bodys = new List<MoldormBodyParts>
            {
                new MoldormBodyParts(position, 0, data, this),
            };
            enemies.AddRange(bodys);
            facing = Direction.West;
            lastConstruct = position;
            numberOfPointsBetween = 9;
            maxPositionQueueLength = length * numberOfPointsBetween;
            positionQueue = new List<Point>(maxPositionQueueLength);
            //fill position queue with points
            for (int i = 0; i < maxPositionQueueLength; i++)
            {
                positionQueue.Add(new Point(position.X, position.Y));
            }
            ChangeDirection();
        }

        public override void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            //Die if no heads
            if (bodys.Count <= 0)
            {
                EnemyManager.RemoveEnemy(this);
            }

            //Change direction
            if (currentDistance >= distanceToChangeDirection) ChangeDirection();

            //Movement
            currentFrame++;
            if (currentFrame % 2 == 0 && stunDuration <= 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                Move(movementVal);
            }

            //Add body
            if (currentTotalDistance >= 9 && length > 0)
            {
                AddBody();
                currentTotalDistance = 0;
            }

            //Update stun duration
            if (stunDuration > 0) stunDuration--;
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            if (bodys.Count > 0) bodys[^1].ApplyDamage(amount);
        }

        internal override void Move(int distance)
        {
            switch (facing)
            {
                case Direction.East:
                    MoveBodies(distance, 0);
                    break;
                case Direction.West:
                    MoveBodies(-1 * distance, 0);
                    break;
                case Direction.South:
                    MoveBodies(0, distance);
                    break;
                case Direction.North:
                    MoveBodies(0, -1 * distance);
                    break;
                case Direction.SouthEast:
                    MoveBodies(distance, distance);
                    break;
                case Direction.NorthEast:
                    MoveBodies(distance, -1 * distance);
                    break;
                case Direction.SouthWest:
                    MoveBodies(-1 * distance, distance);
                    break;
                case Direction.NorthWest:
                    MoveBodies(-1 * distance, -1 * distance);
                    break;
            }
            positionQueue.RemoveAt(positionQueue.Count - 1);
            currentDistance += distance;
            currentTotalDistance += distance;
        }

        public void MoveBodies(int X, int Y)
        {
            Point newPosition = new Point(positionQueue[0].X + X, positionQueue[0].Y + Y);
            positionQueue.Insert(0, newPosition);
        }

        public Point GetBodyPosition(int index)
        {
            return positionQueue[index];
        }

        public void RemoveBody()
        {
            this.bodys.RemoveAt(this.bodys.Count - 1);
        }

        public override void UpdateLocation(int X, int Y) { }

        public void AddBody()
        {
            length--;
            MoldormBodyParts newBody = new MoldormBodyParts(lastConstruct, bodys.Count * numberOfPointsBetween - 1, Data, this);
            bodys.Add(newBody);
            enemies.Add(newBody);
            EnemyManager.AddEnemy(newBody);
        }

        public bool CanTakeDamage()
        {
            bool canTakeDamage = true;
            foreach (MoldormBodyParts body in bodys)
            {
                if (!EnemyManager.Contains(body)) canTakeDamage = false;
            }
            return canTakeDamage;
        }
    }
}
