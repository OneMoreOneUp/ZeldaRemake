using System.Collections.Generic;
using ZeldaGame.Enums;

namespace ZeldaGame.Player
{
    public class AdventurePlayerInventory : PlayerInventory
    {
        public enum Slot1Item
        {
            Nothing,
            WoodSword = Item.WoodSword,
            WhiteSword = Item.WhiteSword,
            MagicalSword = Item.MagicalSword
        }

        public enum Slot2Item
        {
            Nothing,
            WoodenBoomerang = Item.WoodenBoomerang, MagicalBoomerang = Item.MagicalBoomerang,
            Bow = Item.Bow, BowSilverArrow = Item.SilverArrow, BowArrow = Item.Arrow,
            BlueCandle = Item.BlueCandle, RedCandle = Item.RedCandle,
            Food = Item.Food, Bomb = Item.Bomb, Flute = Item.Flute, MagicalRod = Item.MagicalRod,
            LifePotion = Item.LifePotion, SecondLifePotion = Item.SecondLifePotion
        }

        public enum Shield
        {
            WoodShield = Item.WoodShield,
            MagicalShield = Item.MagicalShield
        }

        private Slot1Item slot1;
        private Slot2Item slot2;
        private Shield shield;

        public AdventurePlayerInventory
            (int balance = 0, int currentHealth = 6, int maxHealth = 6, Dictionary<Item, int> starting = null,
            Slot1Item slot1 = Slot1Item.Nothing, Slot2Item slot2 = Slot2Item.Nothing, Shield shield = Shield.WoodShield)
            : base(balance, currentHealth, maxHealth, starting)
        {
            this.slot1 = slot1;
            this.slot2 = slot2;
            this.shield = shield;
        }

        public void SetSlot1(Slot1Item slot1Item)
        {
            this.slot1 = slot1Item;
        }

        public Slot1Item GetSlot1()
        {
            return this.slot1;
        }

        public void SetSlot2(Slot2Item slot2Item)
        {
            this.slot2 = slot2Item;
        }

        public Slot2Item GetSlot2()
        {
            return this.slot2;
        }

        public void SetShield(Shield shield)
        {
            this.shield = shield;
        }

        public Shield GetShield()
        {
            return this.shield;
        }
    }
}
