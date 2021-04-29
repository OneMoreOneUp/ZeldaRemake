// PreviousRoomCommand Class
//
// @author Jared Lawson

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class PreviousRoomCommand : ICommand
    {
        private readonly Game1 game;

        public PreviousRoomCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            game.CurrentLevel.TransitionThoughList(false);
            return true;
        }
    }
}
