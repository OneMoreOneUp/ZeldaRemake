using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enemies;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Triggers
{
    public class RopeTrigger : ITriggerBox
    {
        private readonly Rope rope;
        private readonly Direction attackDirection;
        private readonly int distance;

        public RopeTrigger(Rope rope, Direction attackDirection, int distance)
        {
            this.rope = rope ?? throw new ArgumentNullException(nameof(rope));
            this.attackDirection = attackDirection;
            this.distance = distance;
        }

        private static Rectangle CreateBoundingRectangle(Direction direction, int x, int y, int width, int height, int distance)
        {
            return direction switch
            {
                Direction.North => new Rectangle(x, y - distance, width, distance),
                Direction.South => new Rectangle(x, y + height, width, distance),
                Direction.East => new Rectangle(x + width, y, distance, height),
                _ => new Rectangle(x - distance, y, distance, height),
            };
        }

        public Rectangle GetRectangle()
        {
            Rectangle hitbox = rope.GetHitbox();
            return CreateBoundingRectangle(attackDirection, hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height, distance);
        }

        public void Trigger()
        {
            rope.Attack(attackDirection);
        }
    }
}
