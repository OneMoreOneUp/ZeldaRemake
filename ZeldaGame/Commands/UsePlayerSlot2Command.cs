// UsePlayerSlot1Command Class
//
// @author 

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class UsePlayerSlot2Command : ICommand
    {
        private readonly IAdventurePlayer player;

        public UsePlayerSlot2Command(IAdventurePlayer player)
        {
            this.player = player;
        }

        public bool Execute()
        {
            player.UseSlot2();
            return true;
        }
    }
}
