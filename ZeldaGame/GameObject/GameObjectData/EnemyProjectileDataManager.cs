using System.Globalization;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class EnemyProjectileDataManager : GameObjectDataManager
    {
        public EnemyProjectileDataManager(string file) : base(file)
        {

        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.EnemyProjectile, name, "layer"), CultureInfo.InvariantCulture);
        }

        public float GetVelocity(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.EnemyProjectile, name, "velocity"), CultureInfo.InvariantCulture);
        }

        public int GetDamage(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.EnemyProjectile, name, "damage"), CultureInfo.InvariantCulture);
        }

        public int GetAttackDistance(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.EnemyProjectile, name, "attack_distance"), CultureInfo.InvariantCulture);
        }

        public int GetAnimationFramerate(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.EnemyProjectile, name, "animation_framerate"), CultureInfo.InvariantCulture);
        }

        public void GetEnemyProjectileHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.EnemyProjectile, name, out width, out height);
        }

        public void GetVertHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.EnemyProjectile, name, out width, out height, "vertical");
        }

        public void GetHorHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.EnemyProjectile, name, out width, out height, "horizontal");
        }
    }
}
