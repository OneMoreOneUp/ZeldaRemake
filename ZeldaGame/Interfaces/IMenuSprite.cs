using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaGame.Interfaces
{
    public interface IMenuSprite
    {
        public void Draw(SpriteBatch spriteBatch, Point position, Color color);

        public void Update(int variableSpriteName);

    }
}
