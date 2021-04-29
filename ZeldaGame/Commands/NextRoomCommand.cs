// NextRoomCommand Class
//
// @author Jared Lawson

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class NextRoomCommand : ICommand
    {
        private readonly Game1 game;

        public NextRoomCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            game.CurrentLevel.TransitionThoughList(true);
            return true;
        }
    }
}
