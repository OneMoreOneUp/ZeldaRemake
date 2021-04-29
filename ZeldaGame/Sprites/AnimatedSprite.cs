// AnimatedSprite Class
//
// @author Brian Sharp

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Sprites
{
    class AnimatedSprite : ISprite
    {
        private readonly Texture2D spriteSheet;
        private readonly List<Rectangle> sourceRectList;
        private Rectangle drawRect;
        private int frameNumber;
        private readonly bool fixedCenter, bottomRightOrigin;

        public AnimatedSprite(Texture2D spriteSheet, List<Rectangle> sourceRectList, bool fixedCenter = false, bool bottomRightOrigin = false)
        {
            this.spriteSheet = spriteSheet;
            this.sourceRectList = sourceRectList;
            this.fixedCenter = fixedCenter;
            this.bottomRightOrigin = bottomRightOrigin;
        }

        public void Draw(SpriteBatch spriteBatch, Point position, Color color, float layer)
        {
            // create a drawRect taking into account the neccessary offset to draw the sprite centered on posX, posY
            if (fixedCenter && bottomRightOrigin)
            {
                drawRect = new Rectangle(
                    position.X - (sourceRectList[frameNumber].Width - (sourceRectList[0].Width / 2)),
                    position.Y - (sourceRectList[frameNumber].Height - (sourceRectList[0].Height / 2)),
                    sourceRectList[frameNumber].Width,
                    sourceRectList[frameNumber].Height
                );
            }
            else if (fixedCenter)
            {
                drawRect = new Rectangle(
                    position.X - (sourceRectList[0].Width / 2),
                    position.Y - (sourceRectList[0].Height / 2),
                    sourceRectList[frameNumber].Width,
                    sourceRectList[frameNumber].Height
                );
            }
            else
            {
                drawRect = new Rectangle(
                    position.X - (sourceRectList[frameNumber].Width / 2),
                    position.Y - (sourceRectList[frameNumber].Height / 2),
                    sourceRectList[frameNumber].Width,
                    sourceRectList[frameNumber].Height
                );
            }

            spriteBatch.Draw(spriteSheet, drawRect, sourceRectList[frameNumber], color, 0.0f, Vector2.Zero, SpriteEffects.None, layer);
        }

        public bool UpdateFrame()
        {
            frameNumber++;
            frameNumber %= sourceRectList.Count;

            return frameNumber == 0;
        }
    }
}