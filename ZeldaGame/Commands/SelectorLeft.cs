using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectorLeft : ICommand
    {
        private readonly Game1 game;
        public SelectorLeft(Game1 game)
        {
            this.game = game;
        }
        public bool Execute()
        {
            if (game.menuPlayer)
            {
                game.MenuPlayer1.GiveCommand(Direction.West);
            }
            else
            {
                game.MenuPlayer2.GiveCommand(Direction.West);
            }
            return true;
        }
    }
}
