using System.Globalization;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class PlayerProjectileDataManager : GameObjectDataManager
    {
        public PlayerProjectileDataManager(string file) : base(file)
        {

        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, name, "layer"), CultureInfo.InvariantCulture);
        }

        public float GetVelocity(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, name, "velocity"), CultureInfo.InvariantCulture);
        }

        public int GetDamage(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, name, "damage"), CultureInfo.InvariantCulture);
        }

        public int GetAttackDistance(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, name, "attack_distance"), CultureInfo.InvariantCulture);
        }

        public int GetSummonDistance(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, name, "summon_distance"), CultureInfo.InvariantCulture);
        }

        public int GetAnimationFramerate(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, name, "animation_framerate"), CultureInfo.InvariantCulture);
        }

        public void GetBombAttributes(out int fuseTime, out int explosionTime)
        {
            fuseTime = int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, "Bomb", "fuse_time"), CultureInfo.InvariantCulture);
            explosionTime = int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, "Bomb", "explosion_time"), CultureInfo.InvariantCulture);
        }

        public void GetFoodAttributes(out int spoilTime)
        {
            spoilTime = int.Parse(GetAttribute(Enums.GameObjectType.PlayerProjectile, "Food", "spoil_time"), CultureInfo.InvariantCulture);
        }

        public void GetPlayerProjectileHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.PlayerProjectile, name, out width, out height);
        }

        public void GetVertHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.PlayerProjectile, name, out width, out height, "vertical");
        }

        public void GetHorHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.PlayerProjectile, name, out width, out height, "horizontal");
        }

        public void GetExplosiveHitbox(string name, out int width, out int height, bool isIntact)
        {
            if (isIntact)
            {
                GetHitbox(Enums.GameObjectType.PlayerProjectile, name, out width, out height, "intact");
            }
            else
            {
                GetHitbox(Enums.GameObjectType.PlayerProjectile, name, out width, out height, "explosion");
            }
        }
    }
}
