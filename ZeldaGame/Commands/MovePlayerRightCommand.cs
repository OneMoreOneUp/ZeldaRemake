// MovePlayerRightCommand Class
//
// @author 

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class MovePlayerRightCommand : ICommand
    {
        private readonly IAdventurePlayer player;

        public MovePlayerRightCommand(IAdventurePlayer player)
        {
            this.player = player;
        }

        public bool Execute()
        {
            player.GoRight();
            return true;
        }
    }
}
