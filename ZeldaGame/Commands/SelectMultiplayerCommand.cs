// SelectMultiplayerCommand Class
//
// @author Brian Sharp

using ZeldaGame.GameState;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectMultiplayerCommand : ICommand
    {
        private readonly Game1 game;

        public SelectMultiplayerCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            game.isMultiplayer = true;
            if (game.setSize)
            {
                game.UpdateScreen();
                game.setSize = false;
            }
            game.InitializeGameState();

            foreach (IController controller in game.Controllers)
            {
                controller.SetUpMultiplayer(game.Player1, game.Player2);
            }

            GameStateManager.DisplayTitleScreen = false;

            return true;
        }
    }
}
