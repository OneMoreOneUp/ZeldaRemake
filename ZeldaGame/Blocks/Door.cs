/*
 * Door block class
 * Author: Michael Frank, Brian Sharp
 */

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
    class Door : IBlock
    {
        private Point position;
        private ISprite sprite;
        private readonly bool rigid = true;
        private int width, height;
        private readonly int level;
        private readonly Direction direction;
        private WallType wallType;
        private readonly Room currentRoom;
        private readonly BlockDataManager data;
        private readonly float layer;

        public Door(Point position, Direction direction, Enums.WallType wallType, int level, Room currentRoom, BlockDataManager data, bool rigid = true)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.level = level;
            this.direction = direction;
            this.wallType = wallType;
            this.currentRoom = currentRoom;
            sprite = SpriteFactory.Instance.CreateSprite(wallType.ToString(), direction, level);
            this.rigid = rigid;
            this.layer = data.GetLayer(name);

            data.GetDoorDirectionHitbox(name, wallType, out width, out height, direction);
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
            //Doors are static
        }

        public void ChangeType(WallType wallType)
        {
            string name = NameLookupTable.GetName(this);
            this.wallType = wallType;
            sprite = SpriteFactory.Instance.CreateSprite(wallType.ToString(), direction, level);
            data.GetDoorDirectionHitbox(name, wallType, out width, out height, direction);
        }

        public WallType GetWallType()
        {
            return this.wallType;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public bool LinkCollision(bool hasKey, bool hasBossKey)
        {
            switch (wallType)
            {
                case WallType.LockedDoor:
                case WallType.BossDoor:
                    return KeyCollision(hasKey, hasBossKey);
                case WallType.OpenDoor:
                case WallType.BombedDoor:
                    return true;
                default:
                    return false;

            }
        }

        public void BombCollision()
        {
            if (wallType == WallType.BombableDoor)
            {
                ChangeType(WallType.BombedDoor);
                currentRoom.BombedOppositeDoor(direction);
                // Need to add type: BombedDoor, BombableDoor should be using NoDoor Sprite
                // Changed the sprites so that BombedDoor and BombableDoor use correct sprite
            }
        }

        public bool KeyCollision(bool hasKey, bool hasBossKey)
        {
            if (wallType == WallType.LockedDoor)
            {
                if (hasKey)
                {
                    ChangeType(WallType.OpenDoor);
                    currentRoom.UnlockOppositeDoor(direction);
                }
                return hasKey;
            }
            else
            {
                if (hasBossKey)
                {
                    ChangeType(WallType.OpenDoor);
                    currentRoom.UnlockOppositeDoor(direction);
                }
                return hasBossKey;
            }
        }

        public void Transition()
        {
            currentRoom.Transition(direction);
        }

        public bool IsRigid()
        {
            return rigid;
        }
    }
}
