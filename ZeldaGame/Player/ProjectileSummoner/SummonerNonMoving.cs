using System;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.PlayerProjectiles;

namespace ZeldaGame.Player
{
    internal class SummonerNonMoving
    {
        internal Func<Direction, bool> UseLifePotion, UseBomb, UseFood, UseSecondLifePotion;

        public SummonerNonMoving(IAdventurePlayer player, PlayerProjectileDataManager data)
        {
            PlayerInventory inventory = player.GetInventory();

            UseLifePotion = delegate (Direction direction)
            {
                bool used = false;
                if (inventory.ItemAmount(Item.LifePotion) > 0)
                {
                    inventory.SetHealth(inventory.GetMaxHealth());      //Set health to max
                    inventory.RemoveItem(Item.LifePotion);
                    used = true;
                }
                return used;
            };

            UseBomb = delegate (Direction direction)
            {
                bool used = false;
                if (inventory.ItemAmount(Item.Bomb) > 0)
                {
                    PlayerProjectileManager.AddPlayerProjectile(new Bomb(direction, player, data));
                    inventory.RemoveItem(Item.Bomb);
                    used = true;
                }
                return used;
            };

            UseFood = delegate (Direction direction)
            {
                bool used = false;
                if (inventory.ItemAmount(Item.Food) > 0)
                {
                    PlayerProjectileManager.AddPlayerProjectile(new Food(direction, player, data));
                    inventory.RemoveItem(Item.Food);
                    used = true;
                }
                return used;
            };

            UseSecondLifePotion = delegate (Direction direction)
            {
                bool used = false;
                if (inventory.ItemAmount(Item.SecondLifePotion) > 0)
                {
                    PlayerInventory inventory = player.GetInventory();
                    inventory.SetHealth(inventory.GetMaxHealth());      //Set health to max
                    //TODO: Cure Link of Jinx
                    inventory.RemoveItem(Item.SecondLifePotion);
                    inventory.AddItem(Item.LifePotion);                 //Second life potion becomes life potion on use
                    used = true;
                }
                return used;
            };
        }
    }
}
