using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.EnemyProjectiles;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Sprites;

namespace ZeldaGame.Enemies
{
    public class Wizzrobe : CardinalEnemy
    {
        private readonly int animFramerate, distanceToChangeDirection;
        private int currentFrame, health;
        private bool hidden;
        public Wizzrobe(Point position, EnemyDataManager data, string color)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            name = NameLookupTable.GetName(this);
            this.position = position;
            this.layer = data.GetLayer(name);
            health = data.GetHealth(name);
            velocity = data.GetVelocity(name);
            damage = data.GetDamage(name);
            distanceToChangeDirection = data.GetDirectionChange(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
            hidden = false;
            name = color + name;
            ChangeDirection();
        }


        public override void Update(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            //Update sprite animation
            currentFrame++;
            currentFrame %= animFramerate;
            if (currentFrame == 0)
            {
                sprite.UpdateFrame();

            }

            //Change direction and attack
            if (currentDistance >= distanceToChangeDirection)
            {
                hidden = !hidden;
                ChangeDirection();
                if (!hidden)
                {
                    SummonMagicalBeam();
                }
            }

            //Movement 
            if (currentFrame % 2 == 0 && stunDuration <= 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                Move(movementVal);
            }

            //Update stun duration
            if (stunDuration > 0) stunDuration--;
        }


        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            health -= amount;
            if (health >= 0)
            {
                EnemyManager.AddEnemy(new DamagedEnemy(this, damagedFrom, true));
                EnemyManager.RemoveEnemy(this);
            }
            else
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
            }
        }
        public override void ChangeDirection()
        {
            Random random = new Random();
            int rndNumber = random.Next(1, 5);
            switch (rndNumber)
            {
                case 1:
                    facing = Direction.East;
                    break;
                case 2:
                    facing = Direction.West;
                    break;
                case 3:
                    facing = Direction.North;
                    break;
                case 4:
                    facing = Direction.South;
                    break;

            }
            currentDistance = 0;        //Resets time since last turn
            CheckSpriteFacing();
        }

        private void CheckSpriteFacing()
        {
            if (facing == Direction.South)
            {
                sprite = SpriteFactory.Instance.CreateSprite(name, Direction.East);
            }
            else
            {
                sprite = SpriteFactory.Instance.CreateSprite(name, facing);
            }
        }
        private void SummonMagicalBeam()
        {
            Point boomerangPos = position;
            int distance = ((width + height) / 2) + 2;                      //Distance from the enemy to summon the boomerang
            switch (facing)
            {
                case Direction.North:
                    boomerangPos.Y -= distance;
                    break;
                case Direction.South:
                    boomerangPos.Y += distance;
                    break;
                case Direction.West:
                    boomerangPos.X -= distance;
                    break;
                case Direction.East:
                    boomerangPos.X += distance;
                    break;
            }

            EnemyProjectileManager.AddEnemyProjectile(new MagicalRodBeam(position, facing, this, new EnemyProjectileDataManager(data.GetXPath())));
        }
        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (!hidden || !EnemyManager.Contains(this))
            {
                sprite.Draw(spriteBatch, position, color, layer);
            }
        }
    }
}
