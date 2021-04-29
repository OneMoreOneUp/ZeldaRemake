using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Sprites;

namespace ZeldaGame.Projectiles.Particles
{
    class WallMaster : IGameObject
    {
        private Point position;
        private readonly Point finalPosition;
        private readonly ISprite sprite;
        private readonly ISprite linkSprite;
        private readonly float layer, velocity;
        private int currentFrame, directionScalar;
        private readonly Room room;
        private List<IAdventurePlayer> players;
        private readonly IEnemy wallMaster;

        public WallMaster(Point position, IAdventurePlayer player, Direction direction, EnemyDataManager data, Room room, IEnemy Wallmaster)
        {

            if (data == null) throw new ArgumentNullException(nameof(data));
            if (player == null) throw new ArgumentNullException(nameof(data));
            this.room = room;
            string name = NameLookupTable.GetName(this);
            this.wallMaster = Wallmaster;
            sprite = SpriteFactory.Instance.CreateSprite(name, direction);
            linkSprite = player.GetSprite();
            this.position = position;
            this.directionScalar = 1;
            this.finalPosition = LevelHandler.LinkDungeonStart();
            velocity = data.GetVelocity(name);
            layer = data.GetLayer(name);
            SaveState();
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
            linkSprite.Draw(spriteBatch, position, color, layer + 0.000001f);
        }

        public Rectangle GetHitbox()
        {
            return Rectangle.Empty;
        }

        public void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));
            //Check if returned to level start
            if (directionScalar < 0 && this.position.Y <= this.finalPosition.Y)
            {
                LoadState();
            }

            //Movement
            currentFrame++;
            if (currentFrame % 2 == 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                UpdateLocation(0, directionScalar * movementVal);
            }

        }

        public void UpdateLocation(int X, int Y)
        {
            if (position.Y + Y > Game1.DefaultWindowHeight)
            {
                directionScalar = -1;
                this.position.X = finalPosition.X;
                EnemyManager.AddEnemy(wallMaster);
                room.ReturnToStart();
            }
            else
            {
                position.X += X;
                position.Y += Y;
            }
        }

        private void SaveState()
        {
            players = new List<IAdventurePlayer>(PlayerManager.GetPlayers());
            PlayerManager.RemoveAllPlayers();
            EnemyManager.RemoveEnemy(wallMaster);
            EnemyManager.Pause();
        }

        private void LoadState()
        {
            GameObjectManager.RemoveGameObject(this);
            PlayerManager.AddPlayer(players);
            EnemyManager.UnPause();
            foreach (IAdventurePlayer player in players)
            {
                player.SetLocation(this.finalPosition);
            }
        }
    }
}
