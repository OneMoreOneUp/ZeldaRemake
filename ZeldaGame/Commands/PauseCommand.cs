// Pause Command Class
//
// @author Benjamin J Nagel, Matthew Crabtree
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
namespace ZeldaGame.Commands
{
    class PauseCommand : ICommand
    {
        private bool isPaused;
        private readonly Game1 game;

        public PauseCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            if (isPaused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
            return true;
        }

        private void Pause()
        {
            BlockManager.Pause();
            EnemyManager.Pause();
            EnemyProjectileManager.Pause();
            GameObjectManager.Pause();
            ItemManager.Pause();
            PlayerManager.Pause();
            PlayerProjectileManager.Pause();

            foreach (IController controller in game.Controllers)
            {
                controller.Pause();
            }

            isPaused = true;
        }

        private void UnPause()
        {
            BlockManager.UnPause();
            EnemyManager.UnPause();
            EnemyProjectileManager.UnPause();
            GameObjectManager.UnPause();
            ItemManager.UnPause();
            PlayerManager.UnPause();
            PlayerProjectileManager.UnPause();

            foreach (IController controller in game.Controllers)
            {
                controller.UnPause();
            }

            isPaused = false;
        }
    }
}
