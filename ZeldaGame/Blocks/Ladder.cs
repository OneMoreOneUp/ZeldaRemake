// Ladder block Class
// Author : Jared Lawson.524

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Sprites;

namespace ZeldaGame.Blocks
{
    class Ladder : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly bool rigid, transitionable;
        private readonly int width, height;
        private readonly Room CurrentRoom;
        private readonly float layer;

        public Ladder(Point position, Room room, BlockDataManager data, bool rigid = true, bool transitionable = false)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.rigid = rigid;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name, Direction.Null, 1);
            data.GetBlockHitbox(name, out width, out height);
            CurrentRoom = room;
            this.transitionable = transitionable;
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

        public void Transition()
        {
            if (transitionable) CurrentRoom.Transition(Direction.Up);
        }
    }
}
