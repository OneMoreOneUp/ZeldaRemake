using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaGame.Interfaces
{
    interface IMenuItem
    {
        public void Draw(SpriteBatch spriteBatch, Color color);

        public void Update();
    }
}
