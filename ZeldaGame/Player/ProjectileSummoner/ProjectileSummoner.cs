using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.Interfaces;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.Player
{
    public class ProjectileSummoner
    {
        private readonly Dictionary<Slot1Item, Action<Direction>> slot1Summons;
        private readonly Dictionary<Slot2Item, Func<Direction, bool>> slot2Summons;
        private readonly Action<Direction> nothingAct = (Direction direction) => { };
        private readonly Func<Direction, bool> nothingFunc = (Direction direction) => { return false; };

        public ProjectileSummoner(IAdventurePlayer player, PlayerProjectileDataManager data)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            SummonerMoving movingSummoner = new SummonerMoving(player, data);
            SummonerNonMoving nonMovingSummoner = new SummonerNonMoving(player, data);

            slot1Summons = new Dictionary<Slot1Item, Action<Direction>>()
            {
                {Slot1Item.Nothing, nothingAct },
                {Slot1Item.WoodSword, movingSummoner.SummonSwordBeam },
                {Slot1Item.WhiteSword, movingSummoner.SummonSwordBeam },
                {Slot1Item.MagicalSword, movingSummoner.SummonSwordBeam },
            };

            slot2Summons = new Dictionary<Slot2Item, Func<Direction, bool>>()
            {
                {Slot2Item.Nothing, nothingFunc },
                {Slot2Item.Bow, nothingFunc },
                {Slot2Item.BowArrow, movingSummoner.UseArrow },
                {Slot2Item.BlueCandle, movingSummoner.UseBlueCandle },
                {Slot2Item.Bomb, nonMovingSummoner.UseBomb },
                {Slot2Item.WoodenBoomerang, movingSummoner.UseWoodenBoomerang },
                {Slot2Item.Food, nonMovingSummoner.UseFood },
                {Slot2Item.MagicalBoomerang, movingSummoner.UseMagicalBoomerang },
                {Slot2Item.MagicalRod, movingSummoner.UseMagicalRod },
                {Slot2Item.RedCandle, movingSummoner.UseRedCandle },
                {Slot2Item.BowSilverArrow, movingSummoner.UseSilverArrow },
                {Slot2Item.LifePotion, nonMovingSummoner.UseLifePotion },
                {Slot2Item.SecondLifePotion, nonMovingSummoner.UseSecondLifePotion }
            };
        }

        public void UseSlot1Item(Slot1Item slot1, Direction direction)
        {
            if (slot1Summons.TryGetValue(slot1, out Action<Direction> summon))
            {
                summon.Invoke(direction);
            }
            else
            {
                Trace.WriteLine("[ERROR] Unimplemented item to summon: " + slot1.ToString());
            }
        }

        public bool UseSlot2Item(Slot2Item slot2, Direction direction)
        {
            if (slot2Summons.TryGetValue(slot2, out Func<Direction, bool> summon))
            {
                return summon.Invoke(direction);
            }
            else
            {
                Trace.WriteLine("[ERROR] item to summon: " + slot2.ToString());
                return false;
            }
        }
    }
}
