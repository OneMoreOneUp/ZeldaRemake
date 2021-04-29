using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using ZeldaGame.Player.States;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Player
{
    public abstract class Link : IAdventurePlayer
    {
        internal ILinkState state;
        internal ISprite sprite;
        internal Point position;
        internal PlayerDataManager data;
        internal ProjectileSummoner projectileSummoner;
        internal AdventurePlayerInventory inventory;
        internal int meleeLength, damage, width, height, animFramerate;
        internal float velocity, layer;
        internal bool damaged;

        protected internal void SetInitialState(Shield shield, bool pickup, IItem item)
        {
            if (pickup && item != null)
            {
                state = new LinkPickUpItem(this, item, new ItemDataManager(data.GetXPath()));
            }
            else if (shield == Shield.MagicalShield)
            {
                state = new LinkNorthMagicalShield(this);
            }
            else
            {
                state = new LinkNorthWoodShield(this);
            }
        }

        public void Update(GameTime gameTime)
        {
            state.Update(gameTime);
            damaged = false;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
        }

        public void GoUp()
        {
            state.GoUp();
        }
        public void GoDown()
        {
            state.GoDown();
        }
        public void GoLeft()
        {
            state.GoLeft();
        }
        public void GoRight()
        {
            state.GoRight();
        }

        public void UseSlot1()
        {
            data.GetSwordAttributes(this.inventory.GetSlot1(), out damage, out meleeLength);
            state.UseSlot1(this.projectileSummoner, this.inventory.GetSlot1());
        }

        public void UseSlot2()
        {
            state.UseSlot2(this.projectileSummoner, this.inventory.GetSlot2());
        }

        protected internal bool UpdateSprite()
        {
            return this.sprite.UpdateFrame();
        }

        protected internal void SetSprite(ISprite newSprite)
        {
            this.sprite = newSprite;
        }

        protected internal void SetState(ILinkState newState)
        {
            this.state = newState;
        }

        public AdventurePlayerInventory GetInventory()
        {
            return this.inventory;
        }

        public void UpdateLocation(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;

            if (this.state is LinkPickUpItem pui) pui.UpdateItemLocation(x, y);
        }

        public Point GetLocation()
        {
            return this.position;
        }

        public void SetLocation(Point position)
        {
            this.position = position;
        }

        public Rectangle GetMeleeHitbox()
        {
            return state.GetMeleeHitbox(meleeLength);
        }

        public int GetDamage()
        {
            return this.damage;
        }

        public bool CanBlock(Direction fromDirection)
        {
            return this.state.CanBlock(fromDirection);
        }

        protected internal int GetAnimFramerate()
        {
            return this.animFramerate;
        }

        protected internal float GetVelocity()
        {
            return this.velocity;
        }

        public Rectangle GetHitbox()
        {
            Rectangle meleeHitbox = GetMeleeHitbox();
            if (meleeHitbox.IsEmpty)
            {
                return GetPlayerHitbox();
            }
            else
            {
                return Rectangle.Union(GetPlayerHitbox(), meleeHitbox);
            }
        }

        public Rectangle GetPlayerHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }

        public abstract void TakeDamage(int amount, Direction fromDireciton);
        public abstract void HoldItem(IItem item);

        public ISprite GetSprite()
        {
            return this.sprite;
        }
    }
}
