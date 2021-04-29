// Text block Class
// Author : Brian Sharp

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Interfaces;
using ZeldaGame.Sprites;

namespace ZeldaGame.Blocks
{
    class Text : IBlock
    {
        private readonly ISprite sprite;
        private Point position;
        private readonly bool rigid;
        private bool doneDrawingText;

        public Text(Point position, string text, int textMaxCharactersPerLine, bool rigid = true)
        {
            this.sprite = SpriteFactory.Instance.CreateFontSprite(text, textMaxCharactersPerLine);
            this.position = position;
            this.rigid = rigid;
            this.doneDrawingText = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!doneDrawingText)
            {
                doneDrawingText = sprite.UpdateFrame();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, 0f);
        }

        public Rectangle GetHitbox()
        {
            return Rectangle.Empty;
        }

        public bool IsRigid()
        {
            return rigid;
        }
    }
}
