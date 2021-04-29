//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ZeldaGame.Interfaces;

namespace ZeldaGame.HUD
{
    class StaticMenuSprite : IMenuSprite
    {
        private readonly Texture2D spriteSheet;
        private Rectangle sourceRect;
        private Rectangle drawRect;
        public StaticMenuSprite(Texture2D spriteSheet, Rectangle sourceRect)
        {
            this.spriteSheet = spriteSheet;
            this.sourceRect = sourceRect;
        }

        public void Draw(SpriteBatch spriteBatch, Point position, Color color)
        {
            // create a drawRect taking into account the neccessary offset to draw th
            drawRect = new Rectangle(
                position.X,
                position.Y,
                sourceRect.Width,
                sourceRect.Height
                );
            spriteBatch.Draw(spriteSheet, drawRect, sourceRect, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.05f);
        }

        public void Update(int variableSpriteName)
        {
            //Not used by Static Menu Sprites
        }

        public void UpdateFrame(int value)
        {
            throw new NotImplementedException();
        }
    }
}
