// Link player class
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Player
{
    public class DamagedLink : IAdventurePlayer
    {
        private readonly IAdventurePlayer decoratedLink;
        private int damagedTimer;                                                                                        //How many frames link will remain in damaged decorator
        private int knockbackTimer = 12;                                                                                  //Over how many frames link will be knocked back
        private readonly Color[] colorCycle = new Color[] { Color.Red, Color.Green, Color.Blue, Color.White };           //Cycle of colors to overlay on link
        private readonly int changeColorFrame = 4;                                                                       //How many frames until color changes
        private int colorIndex;                                                                                          //Current index of color to overlay on link from $colorCycle
        private readonly int knockback;                                                                                  //How many pixels to be knockbacked per frame
        private readonly Direction damagedFrom;

        public DamagedLink(IAdventurePlayer decoratedLink, Direction damagedFrom, PlayerDataManager data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            this.decoratedLink = decoratedLink;
            this.damagedFrom = damagedFrom;
            string name = NameLookupTable.GetName(this.decoratedLink);
            SoundFactory.Instance.GetSound(Sound.LinkHurtAndEraseSaveData).Play();
            this.damagedTimer = data.GetInjureDuration(name);
            this.knockback = data.GetInjureKnockback(name);
        }

        public void Update(GameTime gameTime)
        {
            //Knockback link
            if (knockbackTimer > 0)
            {
                knockbackTimer--;
                this.GetKnockedBack();
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

            decoratedLink.Update(gameTime);
        }

        public void RemoveDecorator()
        {
            PlayerManager.RemovePlayer(this);
            PlayerManager.AddPlayer(decoratedLink);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            decoratedLink.Draw(spriteBatch, colorCycle[colorIndex]);
        }

        public void GoUp()
        {
            decoratedLink.GoUp();
        }
        public void GoDown()
        {
            decoratedLink.GoDown();
        }
        public void GoLeft()
        {
            decoratedLink.GoLeft();
        }
        public void GoRight()
        {
            decoratedLink.GoRight();
        }

        public void TakeDamage(int amount, Direction fromDirection)
        {
            //Damaged Link should not take damage
        }

        public void UseSlot1()
        {
            decoratedLink.UseSlot1();
        }

        public void UseSlot2()
        {
            decoratedLink.UseSlot2();
        }

        public Rectangle GetHitbox()
        {
            return decoratedLink.GetHitbox();
        }

        public Rectangle GetMeleeHitbox()
        {
            return decoratedLink.GetMeleeHitbox();
        }

        public Rectangle GetPlayerHitbox()
        {
            return decoratedLink.GetPlayerHitbox();
        }

        public int GetDamage()
        {
            return decoratedLink.GetDamage();
        }

        public void UpdateLocation(int x, int y)
        {
            decoratedLink.UpdateLocation(x, y);
        }

        public void SetLocation(Point position)
        {
            decoratedLink.SetLocation(position);
        }

        public Point GetLocation()
        {
            return decoratedLink.GetLocation();
        }

        private void GetKnockedBack()
        {
            switch (damagedFrom)
            {
                case Enums.Direction.North:
                    UpdateLocation(0, knockback);
                    break;
                case Enums.Direction.South:
                    UpdateLocation(0, -1 * knockback);
                    break;
                case Enums.Direction.West:
                    UpdateLocation(knockback, 0);
                    break;
                case Enums.Direction.East:
                    UpdateLocation(-1 * knockback, 0);
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unimplemented direction, " + damagedFrom.ToString() + " , in " + this.GetType().Name);
                    break;
            }
        }

        public AdventurePlayerInventory GetInventory()
        {
            return decoratedLink.GetInventory();
        }

        public bool CanBlock(Direction fromDirection)
        {
            return this.decoratedLink.CanBlock(fromDirection);
        }

        public void HoldItem(IItem item)
        {
            RemoveDecorator();
            decoratedLink.HoldItem(item);
        }

        public IAdventurePlayer GetDecoratedPlayer()
        {
            return this.decoratedLink;
        }

        public ISprite GetSprite()
        {
            return this.decoratedLink.GetSprite();
        }
    }
}
