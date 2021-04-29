// Water block Class
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
    public class Water : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly bool rigid;
        private readonly int width, height;
        private readonly float layer;
        private readonly BlockDataManager blockData;
        public Water(Point position, int level, BlockDataManager data, bool rigid = true)
        {
            string name = NameLookupTable.GetName(this);
            blockData = data ?? throw new ArgumentNullException(nameof(data));
            sprite = SpriteFactory.Instance.CreateSprite(name, Direction.Null, level);
            this.position = position;
            this.rigid = rigid;
            this.layer = data.GetLayer(name);
            data.GetBlockHitbox(name, out width, out height);
        }

        public void Update(GameTime gameTime)
        {
            //
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
        public Point GetLocation()
        {
            return position;
        }
        public BlockDataManager GetData()
        {
            return blockData;
        }
    }
}
