// ExitCommand Class
//
// @author Brian Sharp

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class ExitCommand : ICommand
    {
        private readonly Game1 game;

        public ExitCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            game.Exit();
            return true;
        }
    }
}
