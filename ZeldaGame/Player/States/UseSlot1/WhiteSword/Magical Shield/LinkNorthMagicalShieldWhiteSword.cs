// Link attacking Up with a white sword class with magical shield
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Player.States
{
    public class LinkNorthMagicalShieldWhiteSword : ILinkState
    {
        private readonly Link link;
        private int animationIndex;
        private readonly int animationSpeed, variant;
        public LinkNorthMagicalShieldWhiteSword(Link link, int variant = 1)
        {
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            this.variant = variant;
            link.SetSprite(SpriteFactory.Instance.CreateSprite(NameLookupTable.GetName(this), Direction.North, variant));
            this.animationSpeed = link.GetAnimFramerate();
            PlaySlashSound();
        }

        private void PlaySlashSound()
        {
            AdventurePlayerInventory inventory = link.GetInventory();
            if (inventory.GetSlot1() != Slot1Item.Nothing)
            {
                if (inventory.GetHealth() == inventory.GetMaxHealth())
                {
                    SoundFactory.Instance.GetSound(Sound.LinkCombinedSword).Play();
                }
                else
                {
                    SoundFactory.Instance.GetSound(Sound.LinkSwordSlash).Play();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            animationIndex++;
            animationIndex %= animationSpeed;
            if (animationIndex == 0 && link.UpdateSprite()) link.SetState(new LinkNorthMagicalShield(link, variant));
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
            Rectangle hitbox = link.GetPlayerHitbox();
            hitbox.Y -= meleeLength;
            hitbox.Height = meleeLength;
            return hitbox;
        }
    }
}