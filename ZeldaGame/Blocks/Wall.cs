using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Blocks
{
    class Wall : IBlock
    {
        private Point position;
        private readonly ISprite sprite;
        private readonly bool rigid;
        private readonly int width, height;
        private readonly float layer;

        public Wall(Point position, Enums.WallType type, int level, BlockDataManager data, bool rigid = true)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.rigid = rigid;
            this.layer = data.GetLayer(name);
            sprite = SpriteFactory.Instance.CreateSprite(type.ToString(), Direction.Null, level);
            data.GetWallHitbox(name, out width, out height, type);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public void Update(GameTime gametime)
        {

        }

        public bool IsRigid()
        {
            return rigid;
        }
    }
}

