using System.Globalization;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class EnemyDataManager : GameObjectDataManager
    {
        public EnemyDataManager(string file) : base(file)
        {

        }

        public int GetHealth(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "health"), CultureInfo.InvariantCulture);
        }

        public float GetVelocity(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "velocity"), CultureInfo.InvariantCulture);
        }

        public int GetDamage(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "damage"), CultureInfo.InvariantCulture);
        }

        public int GetDirectionChange(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "direction_change_time"), CultureInfo.InvariantCulture);
        }

        public int GetEnemyAttackDuration(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "attack_duration"), CultureInfo.InvariantCulture);
        }

        public int GetAnimationFramerate(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "animation_framerate"), CultureInfo.InvariantCulture);
        }

        public float GetSpeedup(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "speedup"), CultureInfo.InvariantCulture);
        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "layer"), CultureInfo.InvariantCulture);
        }

        public void GetEnemyHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Enemy, name, out width, out height);
        }

        public int GetVerticalAttackDistance(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "vertical_attack_distance"), CultureInfo.InvariantCulture);
        }

        public int GetHorizontalAttackDistance(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Enemy, name, "horizontal_attack_distance"), CultureInfo.InvariantCulture);
        }
    }
}
