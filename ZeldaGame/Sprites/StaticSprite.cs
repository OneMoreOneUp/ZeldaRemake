// StaticSprite Class
//
// @author Brian Sharp

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Sprites
{
    class StaticSprite : ISprite
    {
        private readonly Texture2D spriteSheet;
        private Rectangle sourceRect;
        private Rectangle drawRect;

        public StaticSprite(Texture2D spriteSheet, Rectangle sourceRect)
        {
            this.spriteSheet = spriteSheet;
            this.sourceRect = sourceRect;
        }

        public void Draw(SpriteBatch spriteBatch, Point position, Color color, float layer)
        {
            // create a drawRect taking into account the neccessary offset to draw the sprite centered on posX, posY
            drawRect = new Rectangle(
                position.X - (sourceRect.Width / 2),
                position.Y - (sourceRect.Height / 2),
                sourceRect.Width,
                sourceRect.Height
                );

            spriteBatch.Draw(spriteSheet, drawRect, sourceRect, color, 0.0f, Vector2.Zero, SpriteEffects.None, layer);
        }

        public bool UpdateFrame()
        {
            // Static sprite has only a single frame and would finish a cycle every frame
            return true;
        }


    }
}