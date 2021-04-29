/*
 * Flame block class
 * Author: Ben Nagel, Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Blocks
{
    public class Flame : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private int lastFrame;
        private readonly int timeTillNewFrame = 140;
        private readonly bool rigid;
        private readonly int width, height;
        private readonly float layer;

        public Flame(Point position, BlockDataManager data, bool rigid = true)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.rigid = rigid;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetBlockHitbox(name, out width, out height);
        }

        public void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            lastFrame += gametime.ElapsedGameTime.Milliseconds;
            if (lastFrame > timeTillNewFrame)
            {
                lastFrame = 0;
                sprite.UpdateFrame();
            }
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
