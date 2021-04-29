// MovePlayerLeftCommand Class
//
// @author 

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class MovePlayerLeftCommand : ICommand
    {
        private readonly IAdventurePlayer player;

        public MovePlayerLeftCommand(IAdventurePlayer player)
        {
            this.player = player;
        }

        public bool Execute()
        {
            player.GoLeft();
            return true;
        }
    }
}
