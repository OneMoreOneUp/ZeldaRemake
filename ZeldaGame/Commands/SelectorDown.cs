using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectorDown : ICommand
    {
        private readonly Game1 game;
        public SelectorDown(Game1 game)
        {
            this.game = game;
        }
        public bool Execute()
        {
            if (game.menuPlayer)
            {
                game.MenuPlayer1.GiveCommand(Direction.South);
            }
            else
            {
                game.MenuPlayer2.GiveCommand(Direction.South);
            }
            return true;
        }
    }
}
