using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectInMenu : ICommand
    {
        private readonly Game1 game;
        private bool playerMenu;
        public SelectInMenu(Game1 game, bool player)
        {
            this.game = game;
            playerMenu = player;
        }
        public bool Execute()
        {
            if (game.MenuActive && playerMenu == game.menuPlayer)
            {
                if (game.menuPlayer)
                {
                    game.MenuPlayer1.GiveCommand(Direction.Null, true);
                }
                else
                {
                    game.MenuPlayer2.GiveCommand(Direction.Null, true);
                }
                game.TransitionMenu(playerMenu);

            }

            return true;
        }
    }
}
