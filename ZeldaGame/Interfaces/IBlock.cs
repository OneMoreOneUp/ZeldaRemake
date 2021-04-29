namespace ZeldaGame.Interfaces
{
    public interface IBlock : IGameObject
    {
        /// <summary>
        /// Returns true if the block is solid and cannot be passed through
        /// </summary>
        /// <returns>T/F iff the block is rigid</returns>
        public bool IsRigid();
    }
}