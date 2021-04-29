// IController Interface
//
// @author Brian Sharp

namespace ZeldaGame.Interfaces
{
    public interface IController
    {
        public void GetInput();

        public void ToggleInMenu();

        public void Pause();

        public void UnPause();

        public void SetUpSingleplayer(IAdventurePlayer player);

        public void SetUpMultiplayer(IAdventurePlayer player1, IAdventurePlayer player2);
    }
}
