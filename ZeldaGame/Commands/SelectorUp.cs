using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectorUp : ICommand
    {
        private readonly Game1 game;
        public SelectorUp(Game1 game)
        {
            this.game = game;
        }
        public bool Execute()
        {
            if (game.menuPlayer)
            {
                game.MenuPlayer1.GiveCommand(Direction.North);
            }
            else
            {
                game.MenuPlayer2.GiveCommand(Direction.North);
            }
            return true;
        }
    }
}
