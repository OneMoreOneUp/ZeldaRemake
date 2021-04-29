// Link using an item to the right
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Items;
using ZeldaGame.Sprites;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Player.States
{
    public class LinkPickUpItem : ILinkState
    {
        private readonly Link link;
        private int animationIndex;
        private readonly int animationSpeed, variant;
        private double soundDuration;
        private readonly IItem holdable, holding;

        public LinkPickUpItem(Link link, IItem item, ItemDataManager data, int variant = 1)
        {
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            if (data == null) throw new ArgumentNullException(nameof(data));
            link.SetSprite(SpriteFactory.Instance.CreateSprite(NameLookupTable.GetName(this), Direction.Null, variant));
            this.animationSpeed = 5 * link.GetAnimFramerate();
            this.variant = variant;
            this.holding = item;

            SoundEffect fanfare = SoundFactory.Instance.GetSound(Sound.NewItemFanFare);
            fanfare.Play();
            soundDuration = fanfare.Duration.TotalMilliseconds;
            this.holdable = new Holdable(link.GetLocation() + new Point(0, -10), item, data);

            ItemManager.AddItem(this.holdable);
            ItemManager.Pause();
            EnemyManager.Pause();
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime == null) throw new ArgumentNullException(nameof(gameTime));

            animationIndex++;
            if (animationIndex == animationSpeed) link.UpdateSprite();

            soundDuration -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (soundDuration <= 0) StopHolding();
        }

        private void StopHolding()
        {
            Shield shield = link.GetInventory().GetShield();
            if (shield == Shield.MagicalShield)
            {
                link.SetState(new LinkSouthMagicalShield(link, variant));
            }
            else
            {
                link.SetState(new LinkSouthWoodShield(link, variant));
            }

            if (holding is TriForcePiece tfp) tfp.Transition();

            ItemManager.RemoveItem(this.holdable);
            ItemManager.UnPause();
            EnemyManager.UnPause();
        }

        public void GoUp()
        {
            //Link cannot move while attacking
        }
        public void GoDown()
        {
            //Link cannot move while attacking
        }
        public void GoLeft()
        {
            //Link cannot move while attacking
        }
        public void GoRight()
        {
            //Link cannot move while attacking
        }

        public void UseSlot1(ProjectileSummoner itemHandler, Slot1Item slot1)
        {
            //Link cannot use items while attacking
        }

        public void UseSlot2(ProjectileSummoner itemHandler, Slot2Item slot2)
        {
            //Link cannot use items while attacking
        }

        public bool CanBlock(Direction fromDirection)
        {
            return false;
        }

        public Rectangle GetMeleeHitbox(int meleeLength)
        {
            return Rectangle.Empty;
        }

        public void UpdateItemLocation(int x, int y)
        {
            this.holdable.UpdateLocation(x, y);
        }
    }
}