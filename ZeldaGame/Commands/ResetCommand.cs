// ResetCommand Class
//
// @author 

using ZeldaGame.GameObjectHandler;
using ZeldaGame.GameState;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;

namespace ZeldaGame.Commands
{
    class ResetCommand : ICommand
    {
        private readonly Game1 game;

        public ResetCommand(Game1 game)
        {
            this.game = game;
        }

        public bool Execute()
        {
            //Prevents low health sound from looping
            foreach (IAdventurePlayer player in PlayerManager.GetPlayers())
            {
                PlayerInventory inventory = player.GetInventory();
                inventory.SetHealth(inventory.GetMaxHealth());
            }

            int resetLevelNum = this.game.CurrentLevel.LevelNum;
            this.game.LevelStartIndex = resetLevelNum;
            GameStateManager.Reset();

            RemoveAll();
            UnPauseAll();

            game.ResetGame();
            return true;
        }

        private static void RemoveAll()
        {
            PlayerManager.RemoveAllPlayers();
            ItemManager.RemoveAllItems();
            BlockManager.RemoveAllBlocks();
            EnemyManager.RemoveAllEnemies();
            EnemyProjectileManager.RemoveAllEnemyProjectiles();
            PlayerProjectileManager.RemoveAllPlayerProjectiles();
            GameObjectManager.RemoveAllGameObjects();
        }

        private static void UnPauseAll()
        {
            PlayerManager.UnPause();
            ItemManager.UnPause();
            BlockManager.UnPause();
            EnemyManager.UnPause();
            EnemyProjectileManager.UnPause();
            PlayerProjectileManager.UnPause();
        }
    }
}
