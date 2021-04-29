using System.Globalization;
using ZeldaGame.Enums;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class BlockDataManager : GameObjectDataManager
    {
        public BlockDataManager(string file) : base(file)
        {

        }

        public void GetBlockHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Block, name, out width, out height);
        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(GameObjectType.Block, name, "layer"), CultureInfo.InvariantCulture);
        }

        public void GetWallHitbox(string name, out int width, out int height, WallType type)
        {
            GetHitbox(Enums.GameObjectType.Block, name, out width, out height, type.ToString());
        }

        public void GetDoorDirectionHitbox(string name, WallType door, out int width, out int height, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    GetNorthDoorHitbox(name, door, out width, out height);
                    break;
                case Direction.South:
                    GetSouthDoorHitbox(name, door, out width, out height);
                    break;
                case Direction.East:
                    GetEastDoorHitbox(name, door, out width, out height);
                    break;
                default:
                    GetWestDoorHitbox(name, door, out width, out height);
                    break;
            }
        }

        private void GetNorthDoorHitbox(string name, WallType door, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Block, name, out width, out height, door.ToString(), "north");
        }

        private void GetSouthDoorHitbox(string name, WallType door, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Block, name, out width, out height, door.ToString(), "south");
        }

        private void GetEastDoorHitbox(string name, WallType door, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Block, name, out width, out height, door.ToString(), "east");
        }

        private void GetWestDoorHitbox(string name, WallType door, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Block, name, out width, out height, door.ToString(), "west");
        }
    }
}
