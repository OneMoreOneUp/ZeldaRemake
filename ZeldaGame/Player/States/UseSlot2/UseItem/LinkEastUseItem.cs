// Link using an item to the right
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Player.States
{
    public class LinkEastUseItem : ILinkState
    {
        private readonly Link link;
        private readonly Shield shield;
        private int animationIndex;
        private readonly int animationSpeed, variant;
        public LinkEastUseItem(Link link, int variant = 1)
        {
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            this.shield = link.GetInventory().GetShield();
            this.variant = variant;
            link.SetSprite(SpriteFactory.Instance.CreateSprite(NameLookupTable.GetName(this), Direction.East, variant));
            this.animationSpeed = 5 * link.GetAnimFramerate();
        }

        public void Update(GameTime gameTime)
        {
            animationIndex++;
            animationIndex %= animationSpeed;
            if (animationIndex == 0 && link.UpdateSprite())
            {
                if (this.shield == Shield.WoodShield)
                {
                    link.SetState(new LinkEastWoodShield(link, variant));
                }
                else if (this.shield == Shield.MagicalShield)
                {
                    link.SetState(new LinkEastMagicalShield(link, variant));
                }
                else
                {
                    Trace.WriteLine("[ERROR] Unimplemented shield, stuck in use item state.");
                }
            }
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
    }
}