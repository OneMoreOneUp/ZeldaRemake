// Link moving down with a wooden shield class
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameState;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Player
{
    public class LinkDead : IAdventurePlayer
    {
        private readonly IAdventurePlayer decoratedLink;
        private int frames;
        private readonly int changeSpriteFrame = 4;          //How many frames each sprite of link's "death spin" last
        private int numSpins = 6;                            //How many times the death spin should cycle.
        private int spinIndex;                               //Current index of death spin
        private readonly ISprite[] deathSpin;

        private readonly Color[] colorSpin;


        public LinkDead(IAdventurePlayer link, int variant = 1)
        {
            this.decoratedLink = link;
            string name = NameLookupTable.GetName(this);

            this.deathSpin = new ISprite[]
            {
                SpriteFactory.Instance.CreateSprite(name, Direction.South, variant),
                SpriteFactory.Instance.CreateSprite(name, Direction.West, variant),
                SpriteFactory.Instance.CreateSprite(name, Direction.North, variant),
                SpriteFactory.Instance.CreateSprite(name, Direction.East, variant)
            };

            this.colorSpin = new Color[]
            {
                Color.AntiqueWhite,
                Color.Red,
                Color.Blue,
                Color.Green,
            };

            SoundFactory.Instance.GetSound(Sound.LinkHurtAndEraseSaveData).Play();

            GameStateManager.GameOver();
        }

        public void Update(GameTime gameTime)
        {
            frames = (frames + 1) % changeSpriteFrame;
            if (frames == 0)
            {
                if (numSpins > 0)
                {
                    UpdateSpinCycle();
                }
            }
        }

        private void UpdateSpinCycle()
        {
            spinIndex = (spinIndex + 1) % deathSpin.Length;
            if (spinIndex == 0) numSpins--; //Completed full spin
        }

        //You cannot do anything while you are dead
        public void GoUp()
        {

        }
        public void GoDown()
        {

        }
        public void GoLeft()
        {

        }
        public void GoRight()
        {

        }

        public Rectangle GetMeleeHitbox()
        {
            return Rectangle.Empty;
        }

        public bool CanBlock(Direction fromDirection)
        {
            return false;
        }

        public void TakeDamage(int amount, Direction fromDireciton)
        {
            //Dead player cannot take damage
        }

        public void UseSlot1()
        {
            //Dead player cannot use items
        }

        public void UseSlot2()
        {
            //Dead player cannot use items
        }

        public void UpdateLocation(int x, int y)
        {
            this.decoratedLink.UpdateLocation(x, y);
        }

        public Point GetLocation()
        {
            return this.decoratedLink.GetLocation();
        }

        public void SetLocation(Point position)
        {
            this.decoratedLink.SetLocation(position);
        }

        public AdventurePlayerInventory GetInventory()
        {
            return this.decoratedLink.GetInventory();
        }

        public Rectangle GetHitbox()
        {
            return this.decoratedLink.GetHitbox();
        }

        public Rectangle GetPlayerHitbox()
        {
            return this.decoratedLink.GetPlayerHitbox();
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (spriteBatch == null) throw new ArgumentNullException(nameof(spriteBatch));

            deathSpin[spinIndex].Draw(spriteBatch, this.decoratedLink.GetLocation(), colorSpin[spinIndex], 0.1f);
        }

        public int GetDamage()
        {
            return 0;
        }

        public void HoldItem(IItem item)
        {
            //Link cannot hold items
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
