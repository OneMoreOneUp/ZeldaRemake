using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.PlayerProjectiles;

namespace ZeldaGame.Player
{
    internal class SummonerMoving
    {
        private readonly IAdventurePlayer player;
        private readonly PlayerProjectileDataManager data;
        private IPlayerProjectile lastSwordBeam;

        public SummonerMoving(IAdventurePlayer player, PlayerProjectileDataManager data)
        {
            this.data = data;
            this.player = player;
        }

        public void SummonSwordBeam(Direction direction)
        {
            PlayerInventory inventory = player.GetInventory();
            int currentHealth = inventory.GetHealth();
            int maxHealth = inventory.GetMaxHealth();

            if (currentHealth == maxHealth && (lastSwordBeam == null || !PlayerProjectileManager.Contains(lastSwordBeam)))
            {
                lastSwordBeam = new SwordBeam(direction, player, data);
                PlayerProjectileManager.AddPlayerProjectile(lastSwordBeam);
            }
        }

        public bool UseArrow(Direction direction)
        {
            bool used = false;
            PlayerInventory inventory = player.GetInventory();
            if (inventory.GetBalance() > 0)
            {
                PlayerProjectileManager.AddPlayerProjectile(new Arrow(direction, player, data));
                inventory.RemoveBalance(1);                                                                              //Arrows cost 1 rupee to shoot
                used = true;
            }
            return used;
        }

        public bool UseSilverArrow(Direction direction)
        {
            PlayerProjectileManager.AddPlayerProjectile(new Arrow(direction, player, data));
            return true;
        }

        public bool UseBlueCandle(Direction direction)
        {
            PlayerProjectileManager.AddPlayerProjectile(new Flame(direction, player, data));
            return true;
        }

        public bool UseRedCandle(Direction direction)
        {
            PlayerProjectileManager.AddPlayerProjectile(new Flame(direction, player, data));
            return true;
        }

        public bool UseMagicalRod(Direction direction)
        {
            PlayerProjectileManager.AddPlayerProjectile(new MagicalRodBeam(direction, player, data));
            return true;
        }

        public bool UseWoodenBoomerang(Direction direction)
        {
            bool used = false;
            PlayerInventory inventory = player.GetInventory();
            if (inventory.ItemAmount(Item.WoodenBoomerang) > 0)
            {
                PlayerProjectileManager.AddPlayerProjectile(new WoodenBoomerang(direction, player, data));
                inventory.RemoveItem(Item.WoodenBoomerang);
                used = true;
            }
            return used;
        }

        public bool UseMagicalBoomerang(Direction direction)
        {
            bool used = false;
            PlayerInventory inventory = player.GetInventory();
            if (inventory.ItemAmount(Item.MagicalBoomerang) > 0)
            {
                PlayerProjectileManager.AddPlayerProjectile(new MagicalBoomerang(direction, player, data));
                inventory.RemoveItem(Item.MagicalBoomerang);
                used = true;
            }
            return used;
        }
    }
}
