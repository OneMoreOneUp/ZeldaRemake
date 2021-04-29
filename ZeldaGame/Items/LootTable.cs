// Created By: Benjamin J Nagel / Anakin Matthew
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
namespace ZeldaGame.Items
{
    // Code Based off this https://gamedev.stackexchange.com/questions/162976/how-do-i-create-a-weighted-collection-and-then-pick-a-random-element-from-it
    public class LootTable
    {
        private readonly Dictionary<Enums.Item, double> Loottable;
        private double TotalPercentage;
        private readonly Random random = new Random();

        public LootTable()
        {
            Loottable = new Dictionary<Enums.Item, double>();
            TotalPercentage = 0;
            AddItemToLootTable(Enums.Item.Heart, 0.05);
            AddItemToLootTable(Enums.Item.Fairy, 0.05);
            AddItemToLootTable(Enums.Item.Rupee, 0.05);
            AddItemToLootTable(Enums.Item.FiveRupee, 0.05);
            AddItemToLootTable(Enums.Item.Bomb, 0.05);
            AddItemToLootTable(Enums.Item.Clock, 0.05);
            AddItemToLootTable(Enums.Item.Key, 0.05);
        }
        public void AddItemToLootTable(Enums.Item item, double percent)
        {
            if (!Loottable.ContainsKey(item))
            {
                TotalPercentage += percent;
                Loottable.Add(item, TotalPercentage);

            }
        }
        public IItem GetDropItem(Point position, ItemDataManager itemDataManager)
        {
            Enums.Item? itemPicked = null;
            double drop = random.NextDouble() * TotalPercentage;
            foreach (KeyValuePair<Enums.Item, double> loot in Loottable)
            {
                if (loot.Value >= drop)
                {
                    itemPicked = loot.Key;
                    break;
                }

            }
            IItem item = FindItem(itemPicked, position, itemDataManager);
            return item;
        }
        private static IItem FindItem(Enums.Item? item, Point position, ItemDataManager itemDataManager)
        {
            return item switch
            {
                Enums.Item.Heart => new Items.Heart(position, itemDataManager),
                Enums.Item.Food => new Items.Fairy(position, itemDataManager),
                Enums.Item.Rupee => new Items.BlueRupee(position, itemDataManager),
                Enums.Item.FiveRupee => new Items.RedRupee(position, itemDataManager),
                Enums.Item.Bomb => new Items.Bomb(position, itemDataManager),
                Enums.Item.Clock => new Items.Clock(position, itemDataManager),
                Enums.Item.Key => new Items.Key(position, itemDataManager),
                _ => null,
            };
        }
    }
}