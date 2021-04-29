//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using ZeldaGame.Interfaces;

namespace ZeldaGame.HUD
{
    class VariableMenuSprite : IMenuSprite
    {

        private readonly Texture2D spriteSheet;
        private readonly Dictionary<int, Rectangle> DisplayMap;
        private Rectangle sourceRect;
        private Rectangle drawRect;
        private readonly int scale;

        public VariableMenuSprite(Texture2D spriteSheet, Dictionary<int, Rectangle> map, int InitialSprite = 0)
        {
            this.spriteSheet = spriteSheet;
            DisplayMap = map;
            map.TryGetValue(InitialSprite, out sourceRect);
            scale = 1;
        }

        public void Draw(SpriteBatch spriteBatch, Point position, Color color)
        {
            // create a drawRect taking into account the neccessary offset to draw the sprite centered on posX, posY
            drawRect = new Rectangle(
                position.X,
                position.Y,
                sourceRect.Width * scale,
                sourceRect.Height * scale
                );

            spriteBatch.Draw(spriteSheet, drawRect, sourceRect, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.04f);
        }

        public void Update(int variableSpriteName)
        {
            DisplayMap.TryGetValue(variableSpriteName, out sourceRect);
        }

        public void UpdateFrame(int value)
        {
            throw new NotImplementedException();
        }
    }
}
