using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Enums;
using ZeldaGame.HUD;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Enemies
{
    public abstract class BaseEnemy : IEnemy
    {
        internal int stunDuration, damage, width, height;
        internal Point position;
        internal ISprite sprite;
        internal float layer;
        internal Direction facing;

        public void Stun(int duration)
        {
            this.stunDuration = duration;
        }

        public virtual void UpdateLocation(int X, int Y)
        {
            if (position.X + X < 0 || position.X + X > Game1.DefaultWindowWidth || position.Y + Y < HUDDisplay.Height || position.Y + Y > Game1.DefaultWindowHeight)
            {
                //Prevents running outside bounds of game
                ChangeDirection(this.facing);
            }
            else
            {
                position.X += X;
                position.Y += Y;
            }
        }

        public Point GetLocation()
        {
            return this.position;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (sprite != null) sprite.Draw(spriteBatch, position, color, layer);
        }

        public int GetDamage()
        {
            if (stunDuration > 0)
            {
                return 0;
            }
            else
            {
                return this.damage;
            }
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public abstract void ChangeDirection();
        public abstract void ChangeDirection(Direction notDirection);
        public abstract void TakeDamage(Direction directionFrom, int amount);
        public abstract void Update(GameTime gametime);
    }
}
