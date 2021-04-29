using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.Sprites;

namespace ZeldaGame.Player
{
    public class PlayerInventory
    {
        private int balance;
        private int health;
        private int maxHealth;
        private Dictionary<Item, int> inventory;
        private readonly SoundEffectInstance lowHealth;

        public PlayerInventory(int balance = 0, int health = 20, int maxHealth = 20, Dictionary<Item, int> starting = null)
        {
            this.balance = balance;
            this.health = health;
            this.maxHealth = maxHealth;
            if (starting == null)
            {
                inventory = new Dictionary<Item, int>();
            }
            else
            {
                inventory = starting;
            }

            //Low health loop sound
            lowHealth = SoundFactory.Instance.GetSound(Sound.LinkLowHealth).CreateInstance();
            lowHealth.IsLooped = true;
        }

        public void AddItem(Item item, int amount = 1)
        {
            if (amount <= 0) throw new ArgumentException("Cannot add a negative value or zero to inventory.");

            if (!inventory.TryAdd(item, amount))
            {
                inventory[item] += amount;
            }
        }

        public void ClearInventory()
        {
            balance = 0;
            inventory = new Dictionary<Item, int>();
        }

        public void RemoveItem(Item item, int amount = 1)
        {
            if (amount <= 0) throw new ArgumentException("Cannot remove a negative value or zero from inventory.");

            int currentVal = ItemAmount(item);
            if (amount > currentVal) throw new ArgumentException("Cannot remove more items than the inventory contains.");

            inventory[item] -= amount;
        }

        public int ItemAmount(Item item)
        {
            inventory.TryGetValue(item, out int currentVal);
            return currentVal;                                      //currentVal will be 0 if item is not in inventory
        }

        public void AddBalance(int amount)
        {
            balance += amount;
            SoundFactory.Instance.GetSound(Sound.RefillHealthOrRupeeCountChange).Play();
        }

        public void RemoveBalance(int amount)
        {
            if (amount > balance) throw new ArgumentException("Cannot remove more than current balance.");
            balance -= amount;
            SoundFactory.Instance.GetSound(Sound.RefillHealthOrRupeeCountChange).Play();
        }

        public int GetBalance()
        {
            return balance;
        }

        public void SetHealth(int amount)
        {
            //Play heal sound
            if (amount > this.health) SoundFactory.Instance.GetSound(Sound.RefillHealthOrRupeeCountChange).Play();

            this.health = amount;

            //Play low health sound
            if (health > 0 && health <= 2)
            {
                lowHealth.Play();
            }
            else
            {
                lowHealth.Stop();
            }
        }

        public int GetHealth()
        {
            return this.health;
        }

        public void SetMaxHealth(int amount)
        {
            if (amount > this.maxHealth) SoundFactory.Instance.GetSound(Sound.RefillHealthOrRupeeCountChange).Play();
            this.maxHealth = amount;
        }

        public int GetMaxHealth()
        {
            return this.maxHealth;
        }
    }
}
