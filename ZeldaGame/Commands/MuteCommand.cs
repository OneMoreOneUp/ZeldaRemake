// Sets the volume to 0% or last volume before mute
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class MuteCommand : ICommand
    {
        public static float lastVolume;

        public MuteCommand()
        {

        }

        public bool Execute()
        {
            if (SoundEffect.MasterVolume > 0f)
            {
                lastVolume = SoundEffect.MasterVolume;
                SoundEffect.MasterVolume = 0f;
                MediaPlayer.Volume = SoundEffect.MasterVolume;
            }
            else
            {
                SoundEffect.MasterVolume = lastVolume;
                MediaPlayer.Volume = SoundEffect.MasterVolume;
            }

            return true;
        }
    }
}
