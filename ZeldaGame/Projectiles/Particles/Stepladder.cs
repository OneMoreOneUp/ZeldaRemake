
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
namespace ZeldaGame.Projectiles.Particles
{
    public class Stepladder : IGameObject
    {
        private Point Position;
        private readonly ISprite sprite;
        private readonly float layer;
        private readonly IAdventurePlayer Player;
        private readonly int width, height;

        public Stepladder(Point position, IAdventurePlayer player, ParticleDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            Position = position;
            string name = NameLookupTable.GetName(this);
            sprite = SpriteFactory.Instance.CreateSprite(name);
            layer = data.GetLayer(name);
            Player = player ?? throw new ArgumentNullException(nameof(data));
            data.GetParticleHitbox(name, out width, out height);
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, Position, color, layer);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(Position.X - (width / 2), Position.Y - (height / 2), width, height);
        }

        public void Update(GameTime gametime)
        {

            if (!Player.GetHitbox().Intersects(GetHitbox()) && GameObjectManager.Contains(this))
            {
                GameObjectManager.RemoveGameObject(this);
                Player.GetInventory().AddItem(Enums.Item.StepLadder);
            }
        }
        public Point GetLocation()
        {
            return Position;
        }

    }
}
