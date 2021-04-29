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
    public class Arrow : IGameObject
    {
        private Point position;
        private readonly ISprite sprite;
        private readonly int duration;
        private Vector2 velocity;
        private int updateCounter;
        private readonly float layer;

        public Arrow(Point position, Direction direction, ParticleDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name, direction);

            duration = data.GetDuration(name);
            SetVelocity(data.GetVelocity(name), direction);
        }

        private void SetVelocity(float speed, Direction direction)
        {
            velocity = new Vector2();
            switch (direction)
            {
                case Direction.North:
                    velocity.Y = speed;
                    break;
                case Direction.South:
                    velocity.Y = -1 * speed;
                    break;
                case Direction.East:
                    velocity.X = -1 * speed;
                    break;
                default:
                    velocity.X = speed;
                    break;
            }
            RotateRandom();
        }

        private void RotateRandom()
        {
            Random random = new Random();
            float angle = (float)(((Math.PI / 2) * random.NextDouble()) - (Math.PI / 4));
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(angle));
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
