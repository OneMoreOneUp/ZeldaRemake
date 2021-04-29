using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class GameObjectDataManager
    {
        internal readonly XmlDocument gameObjectData = new XmlDocument();
        private readonly string xPath;

        internal GameObjectDataManager(string file)
        {
            gameObjectData.Load(file);
            xPath = file;
        }

        internal void GetHitbox(Enums.GameObjectType objectType, string name, out int width, out int height, String version = null, String version2 = null)
        {
            string xmlInput = "data/" + GetTypeString(objectType) + "/object[@name ='" + name + "']/hitbox";
            if (version != null)
            {
                xmlInput += "[@version = '" + version + "'";
                if (version2 != null) xmlInput += " and @version2 = '" + version2 + "'";
                xmlInput += "]";
            }
            XmlNode node = gameObjectData.SelectSingleNode(xmlInput);

            width = int.Parse(node.SelectSingleNode("width").InnerText, CultureInfo.InvariantCulture);
            height = int.Parse(node.SelectSingleNode("height").InnerText, CultureInfo.InvariantCulture);
        }

        internal string GetAttribute(Enums.GameObjectType objectType, string name, string attribute)
        {
            string xmlInput = "data/" + GetTypeString(objectType) + "/object[@name ='" + name + "']/attributes/" + attribute;
            XmlNode node = gameObjectData.SelectSingleNode(xmlInput);
            return node.InnerText;
        }

        internal static string GetTypeString(Enums.GameObjectType objectType)
        {
            string type = "";

            switch (objectType)
            {
                case Enums.GameObjectType.Item:
                    type = "items";
                    break;
                case Enums.GameObjectType.Block:
                    type = "blocks";
                    break;
                case Enums.GameObjectType.Player:
                    type = "players";
                    break;
                case Enums.GameObjectType.PlayerProjectile:
                    type = "player_projectiles";
                    break;
                case Enums.GameObjectType.EnemyProjectile:
                    type = "enemy_projectiles";
                    break;
                case Enums.GameObjectType.Enemy:
                    type = "enemies";
                    break;
                case Enums.GameObjectType.Particle:
                    type = "particles";
                    break;
                default:
                    Trace.WriteLine("[ERROR] Unknown object type in GameObjectData: " + objectType);
                    break;
            }

            return type;
        }

        public string GetXPath()
        {
            return xPath;
        }
    }
}
