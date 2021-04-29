/*
 * Creates BlueDarknut
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
using ZeldaGame.Sprites;
namespace ZeldaGame.Enemies
{
    public class ManHandlaController : OrdinalEnemy
    {
        private readonly List<IEnemy> heads;
        private readonly int distanceToChangeDirection;
        private float velocity;
        private readonly float speedup;
        private int currentFrame;
        private readonly string name;
        private readonly EnemyDataManager data;

        public ManHandlaController(Point position, EnemyDataManager data, List<IEnemy> enemies)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            if (enemies == null) throw new ArgumentNullException(nameof(data));
            this.position = position;
            name = NameLookupTable.GetName(this);
            this.layer = data.GetLayer(name);
            velocity = data.GetVelocity(name);
            speedup = data.GetSpeedup(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetEnemyHitbox(name, out width, out height);
            ChangeDirection();

            heads = new List<IEnemy>
            {
                new ManhandlaHeads(new Point(position.X + 16, position.Y), data, Direction.East, this),
                new ManhandlaHeads(new Point(position.X - 16, position.Y), data, Direction.West, this),
                new ManhandlaHeads(new Point(position.X, position.Y - 16), data, Direction.North, this),
                new ManhandlaHeads(new Point(position.X, position.Y + 16), data, Direction.South, this)
            };
            enemies.AddRange(heads);
        }
        public override void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            //Die if no heads
            if (heads.Count == 0)
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
            }

            //Change direction and attack
            if (currentDistance >= distanceToChangeDirection)
            {
                ChangeDirection();
            }

            //Movement
            currentFrame++;
            if (currentFrame % (2 + (2 * heads.Count)) == 0 && stunDuration <= 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                Move(movementVal);
            }

            //Update stun duration
            if (stunDuration > 0) stunDuration--;
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            //Controller itself does not take damage
        }

        public void RemoveHead(ManhandlaHeads head)
        {
            this.heads.Remove(head);
            this.velocity += (speedup * (4 - heads.Count));
        }

        public override void UpdateLocation(int X, int Y)
        {
            position.X += X;
            position.Y += Y;

            foreach (ManhandlaHeads head in heads)
            {
                head.UpdateHeadLocation(X, Y);
            }
        }
    }
}
