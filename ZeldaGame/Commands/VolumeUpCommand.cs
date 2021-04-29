// Turns the volume up 5%
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class VolumeUpCommand : ICommand
    {
        public VolumeUpCommand()
        {

        }

        public bool Execute()
        {
            if (SoundEffect.MasterVolume <= 1f - 0.05f)
            {
                SoundEffect.MasterVolume += 0.05f;
            }
            else
            {
                SoundEffect.MasterVolume = 1f;
            }
            MediaPlayer.Volume = SoundEffect.MasterVolume;
            MuteCommand.lastVolume = SoundEffect.MasterVolume;

            return true;
        }
    }
}
