// SelectSingleplayerCommand Class
//
// @author Brian Sharp

using ZeldaGame.GameState;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class SelectSingleplayerCommand : ICommand
    {
        private readonly Game1 game;

        public SelectSingleplayerCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            game.isMultiplayer = false;
            game.InitializeGameState();

            foreach (IController controller in game.Controllers)
            {
                controller.SetUpSingleplayer(game.Player1);
            }

            GameStateManager.DisplayTitleScreen = false;

            return true;
        }
    }
}
