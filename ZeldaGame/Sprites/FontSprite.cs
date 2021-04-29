// FontSprite Class
//
// @author Brian Sharp

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Sprites
{
    class FontSprite : ISprite
    {
        private readonly List<StaticSprite> spriteList;
        private int numberDrawn;
        private readonly int charactersPerLine, charWidth, charHeight;
        public string text;

        public FontSprite(List<StaticSprite> spriteList, string text, int charWidth, int charHeight, int charactersPerLine)
        {
            this.spriteList = spriteList;
            this.charactersPerLine = charactersPerLine;
            this.text = text;
            this.charHeight = charHeight;
            this.charWidth = charWidth;

            numberDrawn = 0;
        }

        // Position is the center point of the first character of the text to be drawn
        public void Draw(SpriteBatch spriteBatch, Point position, Color color, float layer)
        {
            int charactersInLine = 0;

            // Iterate over all the text that is already drawn
            for (int i = 0; i < numberDrawn; i++)
            {
                // Draw the sprite
                spriteList[i].Draw(spriteBatch, position, color, layer);

                charactersInLine++;

                // Update the draw position of the next sprite 
                // End of line isn't reached so increase the X position
                if (charactersInLine < charactersPerLine)
                {
                    position.X += charWidth + 1;
                }
                // End of line was reached and there are enough characters to fill another full line
                else if (((spriteList.Count - i) / charactersPerLine) > 0)
                {
                    position.X -= (charWidth + 1) * (charactersPerLine - 1);
                    position.Y += charHeight + 1;

                    charactersInLine = 0;
                }
                // End of line was reached and there isn't enough characters to fill another full line
                else
                {
                    int extraSpaces = ((charactersPerLine - (spriteList.Count - i)) / 2) + 1;
                    position.X -= (charWidth + 1) * (charactersPerLine - extraSpaces - 1);
                    position.Y += charHeight + 1;

                    charactersInLine = 0;
                }
            }
        }

        public bool UpdateFrame()
        {
            if (numberDrawn != spriteList.Count)
            {
                numberDrawn++;
                return false;
            }

            return true;
        }
    }
}
