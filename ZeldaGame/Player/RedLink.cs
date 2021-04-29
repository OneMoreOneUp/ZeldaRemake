// Link player class
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework;
using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player.States;

namespace ZeldaGame.Player
{
    public class RedLink : Link
    {
        public RedLink(Point position, AdventurePlayerInventory inventory, PlayerDataManager data, bool pickup = false, IItem item = null)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            this.inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            this.projectileSummoner = new ProjectileSummoner(this, new PlayerProjectileDataManager(data.GetXPath()));

            this.position = position;
            this.inventory = inventory;
            string name = NameLookupTable.GetName(this);

            data.GetPlayerHitbox(name, out width, out height);
            this.velocity = data.GetVelocity(name);
            this.layer = data.GetLayer(name);
            this.animFramerate = data.GetPlayerAnimationFramerate(name);

            SetInitialState(inventory.GetShield(), pickup, item);
        }

        public override void TakeDamage(int amount, Direction fromDirection)
        {
            if (state is LinkPickUpItem) return;    //Ignore damage when picking up items

            amount = (int)Math.Ceiling(amount / 4.0);

            int currentHealth = inventory.GetHealth();
            if (currentHealth <= amount && amount > 0 && !damaged)
            {
                damaged = true;
                inventory.SetHealth(0);
                PlayerManager.AddPlayer(new LinkDead(this));
                PlayerManager.RemovePlayer(this);
            }
            else if (amount > 0 && !damaged)
            {
                damaged = true;
                inventory.SetHealth(currentHealth - amount);
                PlayerManager.AddPlayer(new DamagedLink(this, fromDirection, this.data));
                PlayerManager.RemovePlayer(this);
            }
        }

        public override void HoldItem(IItem item)
        {
            this.state = new LinkPickUpItem(this, item, new ItemDataManager(data.GetXPath()), 3);
        }
    }
}
