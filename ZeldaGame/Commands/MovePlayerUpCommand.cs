// MovePlayerUpCommand Class
//
// @author 

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class MovePlayerUpCommand : ICommand
    {
        private readonly IAdventurePlayer player;

        public MovePlayerUpCommand(IAdventurePlayer player)
        {
            this.player = player;
        }

        public bool Execute()
        {
            player.GoUp();
            return true;
        }
    }
}
