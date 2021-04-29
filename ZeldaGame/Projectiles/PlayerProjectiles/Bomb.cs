// Player bomb class
// Author : Michael Frank, Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.PlayerProjectiles
{
    class Bomb : IPlayerProjectile
    {
        private Point position;
        private readonly ISprite IntactSprite;
        private readonly ISprite ExplosionSprite;
        private readonly IAdventurePlayer owner;
        private int fuseTime, explosionTime, currentAnimFrame;
        private readonly int damage, intactWidth, intactHeight, explodeWidth, explodeHeight, animFramerate;
        private readonly float layer;

        public Bomb(Direction direction, IAdventurePlayer owner, PlayerProjectileDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = owner.GetLocation();
            this.owner = owner;
            this.layer = data.GetLayer(name);
            IntactSprite = SpriteFactory.Instance.CreateSprite(name);
            ExplosionSprite = SpriteFactory.Instance.CreateSprite("BombExplosion");

            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetBombAttributes(out fuseTime, out explosionTime);
            data.GetExplosiveHitbox(name, out intactWidth, out intactHeight, true);
            data.GetExplosiveHitbox(name, out explodeWidth, out explodeHeight, false);
            CalculateLocation(data.GetSummonDistance(name), direction);

            SoundFactory.Instance.GetSound(Sound.BombDrop).Play();
        }

        private void CalculateLocation(int distance, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    position.Y -= distance;
                    break;
                case Direction.South:
                    position.Y += distance;
                    break;
                case Direction.West:
                    position.X -= distance;
                    break;
                default:
                    position.X += distance;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (IsIntact())
            {
                IntactSprite.Draw(spriteBatch, position, color, layer);
            }
            else if (explosionTime > 0)
            {
                ExplosionSprite.Draw(spriteBatch, position, color, 0.3f);
                ExplosionSprite.Draw(spriteBatch, position + new Point(intactWidth, 0), color, 0.3f);
                ExplosionSprite.Draw(spriteBatch, position + new Point(-1 * intactWidth, 0), color, 0.3f);
                ExplosionSprite.Draw(spriteBatch, position + new Point(0, intactHeight), color, 0.3f);
                ExplosionSprite.Draw(spriteBatch, position + new Point(0, -1 * intactHeight), color, 0.3f);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsIntact())
            {
                fuseTime--;
            }
            else if (explosionTime > 0)
            {
                explosionTime--;

                currentAnimFrame = (currentAnimFrame + 1) % animFramerate;
                if (currentAnimFrame == 0) this.ExplosionSprite.UpdateFrame();
            }
            else
            {
                PlayerProjectileManager.RemovePlayerProjectile(this);
            }
        }

        public Rectangle GetHitbox()
        {
            if (IsIntact())
            {
                return new Rectangle(position.X - (intactWidth / 2), position.Y - (intactHeight / 2), intactWidth, intactHeight);
            }
            else
            {
                return new Rectangle(position.X - (explodeWidth / 2), position.Y - (explodeHeight / 2), explodeWidth, explodeHeight);
            }
        }

        public bool IsIntact()
        {
            return fuseTime > 0;
        }

        public bool IsOwner(IAdventurePlayer player)
        {
            return this.owner == player;
        }

        public void CollideRigid()
        {
            //Bomb does nothing when colliding with a rigid object
        }

        public int GetDamage()
        {
            return damage;
        }
    }
}
