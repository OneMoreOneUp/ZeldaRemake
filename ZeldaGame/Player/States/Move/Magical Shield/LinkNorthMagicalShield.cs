// Link moving up with a magical shield class
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
    public class LinkNorthMagicalShield : ILinkState
    {
        private readonly Link link;
        private readonly float speed;
        private int animationIndex;
        private readonly int animationSpeed, variant;
        private GameTime gameTime;

        public LinkNorthMagicalShield(Link link, int variant = 1)
        {
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            this.speed = link.GetVelocity();
            this.gameTime = new GameTime();
            this.animationSpeed = link.GetAnimFramerate();
            this.variant = variant;
            link.SetSprite(SpriteFactory.Instance.CreateSprite(NameLookupTable.GetName(this), Direction.North, variant));
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public void GoUp()
        {
            link.UpdateLocation(0, (int)(-1 * gameTime.ElapsedGameTime.TotalMilliseconds * speed));
            animationIndex++;
            animationIndex %= animationSpeed;
            if (animationIndex == 0) link.UpdateSprite();
        }
        public void GoDown()
        {
            link.UpdateLocation(0, (int)(gameTime.ElapsedGameTime.TotalMilliseconds * speed));
            link.SetState(new LinkSouthMagicalShield(link, variant));
        }
        public void GoLeft()
        {
            link.UpdateLocation((int)(-1 * gameTime.ElapsedGameTime.TotalMilliseconds * speed), 0);
            link.SetState(new LinkWestMagicalShield(link, variant));
        }
        public void GoRight()
        {
            link.UpdateLocation((int)(gameTime.ElapsedGameTime.TotalMilliseconds * speed), 0);
            link.SetState(new LinkEastMagicalShield(link, variant));
        }

        public void UseSlot1(ProjectileSummoner itemHandler, Slot1Item slot1)
        {
            if (itemHandler == null) throw new ArgumentNullException(nameof(itemHandler));

            switch (slot1)
            {
                case Slot1Item.Nothing:
                    //Cannot attack
                    break;
                case Slot1Item.WoodSword:
                    link.SetState(new LinkNorthMagicalShieldWoodSword(link, variant));
                    break;
                case Slot1Item.WhiteSword:
                    link.SetState(new LinkNorthMagicalShieldWhiteSword(link, variant));
                    break;
                case Slot1Item.MagicalSword:
                    link.SetState(new LinkNorthMagicalShieldMagicalSword(link, variant));
                    break;
                default:
                    Trace.WriteLine("[ERROR] Used unimplemented sword from: " + this.GetType().Name);
                    break;
            }

            itemHandler.UseSlot1Item(slot1, Direction.North);
        }

        public void UseSlot2(ProjectileSummoner itemHandler, Slot2Item slot2)
        {
            if (itemHandler == null) throw new ArgumentNullException(nameof(itemHandler));

            if (itemHandler.UseSlot2Item(slot2, Direction.North))
            {
                if (slot2 == Slot2Item.MagicalRod)
                {
                    link.SetState(new LinkNorthMagicalShieldMagicalRod(link, variant));
                }
                else
                {
                    link.SetState(new LinkNorthUseItem(link, variant));
                }
            }
        }

        public bool CanBlock(Direction fromDirection)
        {
            return fromDirection == Direction.North;
        }

        public Rectangle GetMeleeHitbox(int meleeLength)
        {
            return Rectangle.Empty;
        }
    }
}
