using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Projectiles.Particles
{
    public class SwordBlast : IGameObject
    {
        private Point position;
        private readonly ISprite sprite;
        private readonly int duration, animFramerate;
        private Vector2 velocity;
        private int updateCounter, currentAnimFrame;
        private readonly float layer;

        public SwordBlast(Point position, Direction direction, ParticleDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name, direction);

            duration = data.GetDuration(name);
            animFramerate = data.GetAnimationFramerate(name);
            SetVelocity(data.GetVelocity(name), direction);
        }

        private void SetVelocity(float speed, Direction direction)
        {
            velocity = direction switch
            {
                Direction.NorthEast => new Vector2(speed, -1 * speed),
                Direction.NorthWest => new Vector2(-1 * speed, -1 * speed),
                Direction.SouthEast => new Vector2(speed, speed),
                _ => new Vector2(-1 * speed, speed),
            };
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            //Check if it's time to remove this
            updateCounter++;
            if (updateCounter >= duration && GameObjectManager.Contains(this))
            {
                GameObjectManager.RemoveGameObject(this);
            }

            //Update animation
            currentAnimFrame = (currentAnimFrame + 1) % animFramerate;
            if (currentAnimFrame == 0) sprite.UpdateFrame();

            //Movement
            UpdateLocation(velocity * (float)gametime.ElapsedGameTime.TotalMilliseconds);
        }

        private void UpdateLocation(Vector2 distance)
        {
            position += distance.ToPoint();
        }

        public Rectangle GetHitbox()
        {
            return Rectangle.Empty;
        }
    }
}
