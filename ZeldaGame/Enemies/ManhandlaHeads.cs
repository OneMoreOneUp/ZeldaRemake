/*
 * Creates ManhandlaHead
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ZeldaGame.EnemyProjectiles;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
namespace ZeldaGame.Enemies
{
    public class ManhandlaHeads : BaseEnemy
    {
        private readonly ManHandlaController Controller;
        private readonly int animFramerate;
        private int health, AttackTimer, currentFrame;
        private readonly EnemyDataManager data;
        private readonly string name;

        public ManhandlaHeads(Point position, EnemyDataManager data, Direction direction, ManHandlaController controller)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            Controller = controller;
            name = NameLookupTable.GetName(this);
            this.position = position;
            this.layer = data.GetLayer(name);
            health = data.GetHealth(name);
            damage = data.GetDamage(name);
            animFramerate = data.GetAnimationFramerate(name);
            data.GetEnemyHitbox(name, out width, out height);
            sprite = SpriteFactory.Instance.CreateSprite(name, direction);
            AttackTimer = 0;
            data.GetEnemyHitbox(name, out width, out height);
        }
        public override void Update(GameTime gametime)
        {
            currentFrame = (currentFrame + 1) % animFramerate;
            if (currentFrame == 0) sprite.UpdateFrame();

            if (stunDuration <= 0)
            {
                AttackTimer++;
                if (AttackTimer > 25 * 6)
                {
                    SpawnFireballs();
                }
            }
            else
            {
                stunDuration--;
            }
        }

        public override void TakeDamage(Direction damagedFrom, int amount)
        {
            health -= amount;
            if (health > 0)
            {
                EnemyManager.AddEnemy(new DamagedEnemy(this, damagedFrom, false));
                EnemyManager.RemoveEnemy(this);
            }
            else
            {
                EnemyManager.AddEnemy(new EnemyDeath(position, data));
                EnemyManager.RemoveEnemy(this);
                Controller.RemoveHead(this);
            }
        }

        public override void ChangeDirection()
        {
            // left Empty intentionally 
        }

        public override void ChangeDirection(Direction notDirection)
        {
            Controller.ChangeDirection(notDirection);
        }

        private void SpawnFireballs()
        {
            float angle = GetPlayerAngle();
            EnemyProjectileDataManager projectileData = new EnemyProjectileDataManager(data.GetXPath());
            EnemyProjectileManager.AddEnemyProjectile(new Fireball(position, -1, 0, angle, this, projectileData));
            AttackTimer = 0;
            SoundFactory.Instance.GetSound(Sound.BossScream3).Play();
        }

        private float GetPlayerAngle()
        {
            List<IAdventurePlayer> players = PlayerManager.GetPlayers();
            if (players.Count == 0) return 0;

            Random random = new Random();
            IAdventurePlayer player = players[random.Next(0, players.Count)];
            Point location = player.GetLocation();

            double x = position.X - location.X;
            double y = position.Y - location.Y;

            return (float)Math.Atan2(y, x);
        }

        public void UpdateHeadLocation(int x, int y)
        {
            position.X += x;
            position.Y += y;
        }

        public override void UpdateLocation(int X, int Y)
        {
            this.Controller.UpdateLocation(X, Y);
        }
    }
}
