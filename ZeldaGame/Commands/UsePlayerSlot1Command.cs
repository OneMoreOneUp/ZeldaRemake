// UsePlayerSlot1Command Class
//
// @author 

using ZeldaGame.Interfaces;

namespace ZeldaGame.Commands
{
    class UsePlayerSlot1Command : ICommand
    {
        private readonly IAdventurePlayer player;

        public UsePlayerSlot1Command(IAdventurePlayer player)
        {
            this.player = player;
        }

        public bool Execute()
        {
            player.UseSlot1();
            return true;
        }
    }
}
