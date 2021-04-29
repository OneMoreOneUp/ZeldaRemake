using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enemies;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Triggers
{
    public class SpikeTrapTrigger : ITriggerBox
    {
        private readonly SpikeTrap spikeTrap;
        private readonly Direction attackDirection;
        private Rectangle box;

        public SpikeTrapTrigger(SpikeTrap spikeTrap, Direction attackDirection, int distance)
        {
            this.spikeTrap = spikeTrap ?? throw new ArgumentNullException(nameof(spikeTrap));
            this.attackDirection = attackDirection;

            Rectangle hitbox = spikeTrap.GetHitbox();
            CreateBoundingRectangle(attackDirection, hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height, distance);
        }

        private void CreateBoundingRectangle(Direction direction, int x, int y, int width, int height, int distance)
        {
            box = direction switch
            {
                Direction.North => new Rectangle(x, y - distance, width, distance),
                Direction.South => new Rectangle(x, y + height, width, distance),
                Direction.East => new Rectangle(x + width, y, distance, height),
                _ => new Rectangle(x - distance, y, distance, height),
            };
        }

        public Rectangle GetRectangle()
        {
            return this.box;
        }

        public void Trigger()
        {
            spikeTrap.Attack(attackDirection);
        }
    }
}
