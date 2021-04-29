using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectorRight : ICommand
    {
        private readonly Game1 game;
        public SelectorRight(Game1 game)
        {
            this.game = game;
        }
        public bool Execute()
        {
            if (game.menuPlayer)
            {
                game.MenuPlayer1.GiveCommand(Direction.East);
            }
            else
            {
                game.MenuPlayer2.GiveCommand(Direction.East);
            }
            return true;
        }
    }
}
