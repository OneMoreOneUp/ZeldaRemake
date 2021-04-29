// ExitCommand Class
//
// @author Brian Sharp

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class ToggleHitboxesCommand : ICommand
    {
        private readonly Game1 game;

        public ToggleHitboxesCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            game.DrawHitboxes = !game.DrawHitboxes;
            return true;
        }
    }
}
