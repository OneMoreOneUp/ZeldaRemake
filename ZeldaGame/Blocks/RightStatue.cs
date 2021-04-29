// Right statue block class
// Author : Jared Lawson.524

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Blocks
{
    public class RightStatue : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly bool rigid;
        private readonly int width, height;
        private readonly float layer;

        public RightStatue(Point position, int level, BlockDataManager data, int variant = 1, bool rigid = true)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            sprite = SpriteFactory.Instance.CreateSprite(name + variant, Direction.Null, level);
            this.position = position;
            this.rigid = rigid;
            this.layer = data.GetLayer(name);
            data.GetBlockHitbox(name, out width, out height);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public bool IsRigid()
        {
            return rigid;
        }
    }
}
