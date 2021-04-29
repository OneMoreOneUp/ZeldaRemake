namespace ZeldaGame.Interfaces
{
    public interface IItem : IGameObject
    {
        /// <summary>
        /// Updates the location of the item
        /// </summary>
        /// <param name="x">x = #x + $x</param>
        /// <param name="y">y = #y + $y</param>
        public void UpdateLocation(int x, int y);

        /// <summary>
        /// Gets the cost to pickup/purchase the item
        /// </summary>
        /// <returns>The cost to purchase this item</returns>
        public int GetCost();

        /// <summary>
        /// Have the player pick up this item
        /// </summary>
        /// <param name="player">The player to pick up the item</param>
        public void PlayerPickUp(IAdventurePlayer player);
    }
}
