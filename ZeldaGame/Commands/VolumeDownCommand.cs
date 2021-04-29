// Turns the volume down 5%
//
// @author Matthew Crabtree

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class VolumeDownCommand : ICommand
    {
        public VolumeDownCommand()
        {

        }

        public bool Execute()
        {
            if (SoundEffect.MasterVolume >= 0.05f)
            {
                SoundEffect.MasterVolume -= 0.05f;
            }
            else
            {
                SoundEffect.MasterVolume = 0f;
            }
            MediaPlayer.Volume = SoundEffect.MasterVolume;
            MuteCommand.lastVolume = SoundEffect.MasterVolume;

            return true;
        }
    }
}
