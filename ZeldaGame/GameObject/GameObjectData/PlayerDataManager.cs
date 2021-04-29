using System.Globalization;
using System.Xml;
using static ZeldaGame.Player.AdventurePlayerInventory;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class PlayerDataManager : GameObjectDataManager
    {
        public PlayerDataManager(string file) : base(file)
        {

        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Player, name, "layer"), CultureInfo.InvariantCulture);
        }

        public int GetBaseHealth(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Player, name, "base_health"), CultureInfo.InvariantCulture);
        }

        public int GetBaseCurrency(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Player, name, "base_currency"), CultureInfo.InvariantCulture);
        }

        public float GetVelocity(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Player, name, "velocity"), CultureInfo.InvariantCulture);
        }

        public int GetInjureDuration(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Player, name, "injure_duration"), CultureInfo.InvariantCulture);
        }

        public int GetInjureKnockback(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Player, name, "injure_knockback"), CultureInfo.InvariantCulture);
        }

        public int GetPlayerAnimationFramerate(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Player, name, "animation_framerate"), CultureInfo.InvariantCulture);
        }

        public void GetSwordAttributes(Slot1Item sword, out int damage, out int length)
        {
            string xmlInput = "data/swords/" + sword.ToString();
            XmlNode node = gameObjectData.SelectSingleNode(xmlInput);

            damage = int.Parse(node.SelectSingleNode("damage").InnerText, CultureInfo.InvariantCulture);
            length = int.Parse(node.SelectSingleNode("length").InnerText, CultureInfo.InvariantCulture);
        }

        public void GetPlayerHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Player, name, out width, out height);
        }
    }
}
