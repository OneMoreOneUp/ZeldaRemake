// Stair block Class
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
    class Stair : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly bool rigid;
        private readonly int width, height;
        private readonly Room CurrentRoom;
        private Point SpawnLocation;
        private readonly float layer;

        public Stair(Point position, int level, Room room, BlockDataManager data, bool rigid = true)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);
            CurrentRoom = room;
            sprite = SpriteFactory.Instance.CreateSprite(name, Direction.Null, level);
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

        public void Transition(Point location)
        {
            SpawnLocation = location;
            CurrentRoom.Transition(Direction.Down);
            //System.Diagnostics.Debug.WriteLine("Player Location" + SpawnLocation);
        }

        public Point GetSpawnPoint()
        {
            //System.Diagnostics.Debug.WriteLine("Player Location" + SpawnLocation);
            return SpawnLocation;
        }
    }
}
