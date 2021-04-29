// MovePlayerDownCommand Class
//
// @author 

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class MovePlayerDownCommand : ICommand
    {
        private readonly IAdventurePlayer player;

        public MovePlayerDownCommand(IAdventurePlayer player)
        {
            this.player = player;
        }

        public bool Execute()
        {
            player.GoDown();
            return true;
        }
    }
}
