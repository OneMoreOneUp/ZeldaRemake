using System.Globalization;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class ItemDataManager : GameObjectDataManager
    {
        public ItemDataManager(string file) : base(file)
        {

        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Item, name, "layer"), CultureInfo.InvariantCulture);
        }

        public float GetVelocity(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Item, name, "velocity"), CultureInfo.InvariantCulture);
        }

        public int GetDirectionChange(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Item, name, "direction_change_time"), CultureInfo.InvariantCulture);
        }

        public int GetAnimationFramerate(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Item, name, "animation_framerate"), CultureInfo.InvariantCulture);
        }

        public void GetItemHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Item, name, out width, out height);
        }

        public int GetEffectDuration(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Item, name, "effect_duration"), CultureInfo.InvariantCulture);
        }
    }
}
