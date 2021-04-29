using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Projectiles.Particles
{
    public class Spark : IGameObject
    {
        private Point position;
        private readonly ISprite sprite;
        private readonly int duration;
        private int updateCounter;
        private readonly float layer;

        public Spark(Point position, ParticleDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);
            this.position = position;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name);
            duration = data.GetDuration(name);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void Update(GameTime gametime)
        {
            //Check if it's time to remove this
            updateCounter++;
            if (updateCounter >= duration && GameObjectManager.Contains(this))
            {
                GameObjectManager.RemoveGameObject(this);
            }
        }

        public Rectangle GetHitbox()
        {
            return Rectangle.Empty;
        }
    }
}
