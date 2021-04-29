using System.Globalization;

namespace ZeldaGame.GameObject.GameObjectData
{
    public class ParticleDataManager : GameObjectDataManager
    {
        public ParticleDataManager(string file) : base(file)
        {

        }

        public float GetLayer(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Particle, name, "layer"), CultureInfo.InvariantCulture);
        }

        public float GetVelocity(string name)
        {
            return float.Parse(GetAttribute(Enums.GameObjectType.Particle, name, "velocity"), CultureInfo.InvariantCulture);
        }

        public int GetDuration(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Particle, name, "duration"), CultureInfo.InvariantCulture);
        }

        public int GetAnimationFramerate(string name)
        {
            return int.Parse(GetAttribute(Enums.GameObjectType.Particle, name, "animation_framerate"), CultureInfo.InvariantCulture);
        }
        public void GetParticleHitbox(string name, out int width, out int height)
        {
            GetHitbox(Enums.GameObjectType.Particle, name, out width, out height);
        }
    }
}
