using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class OpenMenu : ICommand
    {
        private readonly Game1 game;
        private bool playerMenu;
        public OpenMenu(Game1 game, bool player)
        {
            this.game = game;
            playerMenu = player;

        }
        public bool Execute()
        {
            if (!game.MenuTrans && !game.MenuActive)
            {
                game.MenuTransition(playerMenu);

                foreach (IController controller in game.Controllers)
                {
                    controller.ToggleInMenu();
                }

            }
            return true;
        }
    }
}
