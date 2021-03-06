//Brick block Class
// Author : Benjamin J Nagel
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

    class HalfDivider : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly bool rigid;
        private bool moveAble, moving;
        private readonly int width, height;
        private int movedAmountX, movedAmountY;
        private readonly Room currentRoom;
        private Direction direction;
        public HalfDivider(Point position, int level, Room room, BlockDataManager data, bool rigid = true, bool moveAble = false)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            sprite = SpriteFactory.Instance.CreateSprite(name, Direction.Null, level);
            this.position = position;
            this.rigid = rigid;
            this.moveAble = moveAble;
            currentRoom = room;
            data.GetBlockHitbox(name, out width, out height);
        }
        public void Update(GameTime gameTime)
        {
            if (moving)
            {
                Move();
                if (movedAmountX >= width || movedAmountY >= height)
                {
                    moving = false;
                    moveAble = false;
                    currentRoom.movedBlock = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, 0.5f);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public bool IsRigid()
        {
            return rigid;
        }

        private void Move()
        {
            switch (direction)
            {
                case Direction.North:
                    position.Y += 1;
                    movedAmountY++;
                    break;
                case Direction.South:
                    position.Y -= 1;
                    movedAmountY++;
                    break;
                case Direction.West:
                    position.X += 1;
                    movedAmountX++;
                    break;
                default:
                    position.X -= 1;
                    movedAmountX++;
                    break;
            }
        }

        public void LinkCollision(Direction direction)
        {
            if (moveAble)
            {
                moving = true;
                this.direction = direction;
            }
        }
    }
}
