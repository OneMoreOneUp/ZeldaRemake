/*
 * Creates a damaged enemy
 * 
 * Author: Benjamin J Nagel
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
namespace ZeldaGame.Enemies
{
    public class DamagedEnemy : IEnemy
    {
        private readonly IEnemy decoratedEnemy;
        private int damagedTimer = 32;                                                                                  //How many frames link will remain in damaged decorator
        private int knockbackTimer = 6;                                                                                  //Over how many frames link will be knocked back
        private readonly Color[] colorCycle = new Color[] { Color.Red, Color.Green, Color.Blue, Color.White };            //Cycle of colors to overlay on link
        private readonly int changeColorFrame = 4;                                                                       //How many frames until color changes
        private int colorIndex;                                                                                          //Current index of color to overlay on link from $colorCycle
        private readonly int knockback = 4;                                                                                      //How many pixels to be knockbacked per frame
        private readonly Direction damagedFrom;
        private readonly bool getKnockedBack;

        public DamagedEnemy(IEnemy decoratedEnemy, Direction damagedFrom, bool getKnockedBack)
        {
            this.decoratedEnemy = decoratedEnemy;
            this.damagedFrom = damagedFrom;
            this.getKnockedBack = getKnockedBack;
            SoundFactory.Instance.GetSound(Sound.EnemyHit).Play();
        }
        public void Update(GameTime gameTime)
        {
            //Knockback link
            if (getKnockedBack && knockbackTimer > 0)
            {
                knockbackTimer--;
                GetKnockedBack();
            }

            //Remove decorator if timer ended
            damagedTimer--;
            if (damagedTimer == 0)
            {
                RemoveDecorator();
            }

            // Adjust which color to overlay
            if (damagedTimer % changeColorFrame == 0)
            {
                colorIndex++;
                colorIndex %= colorCycle.Length;
            }

            decoratedEnemy.Update(gameTime);
        }
        public void TakeDamage(Direction directionFrom, int amount)
        {
            // does not get damaged
        }
        private void RemoveDecorator()
        {
            EnemyManager.RemoveEnemy(this);
            EnemyManager.AddEnemy(decoratedEnemy);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            decoratedEnemy.Draw(spriteBatch, colorCycle[colorIndex]);
        }
        public void UpdateLocation(int X, int Y)
        {
            decoratedEnemy.UpdateLocation(X, Y);
        }
        public Rectangle GetHitbox()
        {
            return decoratedEnemy.GetHitbox();
        }
        private void GetKnockedBack()
        {
            switch (damagedFrom)
            {
                case Direction.North:
                    UpdateLocation(0, knockback);
                    break;
                case Direction.South:
                    UpdateLocation(0, -1 * knockback);
                    break;
                case Direction.West:
                    UpdateLocation(knockback, 0);
                    break;
                case Direction.East:
                    UpdateLocation(-1 * knockback, 0);
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unimplemented direction, " + damagedFrom.ToString() + " , in " + GetType().Name);
                    break;
            }
        }
        public Point GetLocation()
        {
            return this.decoratedEnemy.GetLocation();
        }
        public void ChangeDirection(Direction notDirection)
        {
            this.decoratedEnemy.ChangeDirection(notDirection);
        }
        public void ChangeDirection()
        {
            //left blank on purpose
        }

        public int GetDamage()
        {
            return 0;
        }

        public IEnemy GetDecoratedEnemy()
        {
            return this.decoratedEnemy;
        }

        public void Stun(int duration)
        {
            this.decoratedEnemy.Stun(duration);
        }
    }
}
