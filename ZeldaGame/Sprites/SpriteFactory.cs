// SpriteFactory Class
//
// @author Brian Sharp and Benjamin J Nagel

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using ZeldaGame.Enums;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Sprites
{
    class SpriteFactory
    {
        private Texture2D DungeonEnemySheet, BossSheet, ItemSheet, NPCSpriteSheet, FontSpriteSheet, TitleSpriteSheet;
        private List<Texture2D> LinkSheets, DungeonTileSheets;
        private readonly XmlDocument SpriteData;
        private readonly XmlDocument FontData;
        private const int fontWidth = 7, fontHeight = 7;

        public static int FontWidth { get { return fontWidth; } }
        public static int FontHeight { get { return fontHeight; } }

        public static SpriteFactory Instance { get; } = new SpriteFactory();
        /// <summary>
        /// Private consuctor so that only one instance is created.
        /// </summary>
        private SpriteFactory()
        {
            SpriteData = new XmlDocument();
            FontData = new XmlDocument();
        }

        public void LoadAllTextures(ContentManager content)
        {
            DungeonEnemySheet = content.Load<Texture2D>("SpriteSheets/DungeonEnemySpriteSheet");
            BossSheet = content.Load<Texture2D>("SpriteSheets/BossSpriteSheet");
            ItemSheet = content.Load<Texture2D>("SpriteSheets/ItemSpriteSheet");
            NPCSpriteSheet = content.Load<Texture2D>("SpriteSheets/NPCSpriteSheet");
            FontSpriteSheet = content.Load<Texture2D>("SpriteSheets/FontSpriteSheet");
            TitleSpriteSheet = content.Load<Texture2D>("SpriteSheets/TitleSpriteSheet");

            LinkSheets = new List<Texture2D>
            {
                content.Load<Texture2D>("SpriteSheets/LinkSpriteSheet"),
                content.Load<Texture2D>("SpriteSheets/WhiteLinkSpriteSheet"),
                content.Load<Texture2D>("SpriteSheets/RedLinkSpriteSheet")
            };

            DungeonTileSheets = new List<Texture2D>
            {
                content.Load<Texture2D>("SpriteSheets/Dungeon1TileSheet"),
                content.Load<Texture2D>("SpriteSheets/Dungeon2TileSheet"),
                content.Load<Texture2D>("SpriteSheets/Dungeon3TileSheet"),
                content.Load<Texture2D>("SpriteSheets/Dungeon4TileSheet"),
                content.Load<Texture2D>("SpriteSheets/Dungeon5TileSheet"),
                content.Load<Texture2D>("SpriteSheets/Dungeon6TileSheet"),
                content.Load<Texture2D>("SpriteSheets/Dungeon7TileSheet"),
            };

            SpriteData.Load(@"../../../Sprites/SpriteData.xml");
            FontData.Load(@"../../../Sprites/FontData.xml");
        }

        private Texture2D GetSpriteSheet(string name, int level)
        {
            Texture2D spriteSheet = null;

            switch (name)
            {
                case "DungeonEnemySheet":
                    spriteSheet = DungeonEnemySheet;
                    break;
                case "BossSheet":
                    spriteSheet = BossSheet;
                    break;
                case "ItemSheet":
                    spriteSheet = ItemSheet;
                    break;
                case "NPCSpriteSheet":
                    spriteSheet = NPCSpriteSheet;
                    break;
                case "LinkSheet":
                    spriteSheet = LinkSheets[level - 1];
                    break;
                case "DungeonTileSheet":
                    spriteSheet = DungeonTileSheets[level - 1];
                    break;
                case "TitleSpriteSheet":
                    spriteSheet = TitleSpriteSheet;
                    break;
                default:
                    // Add error for improperly formated XML
                    break;
            }

            return spriteSheet;
        }

        private static void AddFrame(XmlNode frame, List<Rectangle> frameList)
        {
            IFormatProvider format = CultureInfo.CurrentCulture;

            int x = int.Parse(frame.ChildNodes[0].InnerXml, format);
            int y = int.Parse(frame.ChildNodes[1].InnerXml, format);
            int width = int.Parse(frame.ChildNodes[2].InnerXml, format);
            int height = int.Parse(frame.ChildNodes[3].InnerXml, format);

            frameList.Add(new Rectangle(x, y, width, height));
        }

        public ISprite CreateSprite(string name, Direction facing = Direction.Null, int level = 1)
        {
            // Get the xml node corresponding to the given name
            XmlNode node = SpriteData.DocumentElement.SelectSingleNode("sprite[@name = \"" + name + "\"]");

            // Variables for creating the sprite
            ISprite sprite;
            List<Rectangle> frameList = new List<Rectangle>();
            bool fixedCenter = false, bottomRightOrigin = false;

            // Set the spriteSheet to be used
            Texture2D spriteSheet = GetSpriteSheet(node.FirstChild.InnerText, level);

            // Iterate over all the frame nodes 
            for (int i = 1; i < node.ChildNodes.Count; i++)
            {
                XmlNode frameNode = node.ChildNodes[i];
                XmlAttribute direction = (XmlAttribute)frameNode.Attributes.GetNamedItem("direction");

                // Only add frame nodes that have no direction or are matching the given direction
                if (facing == Direction.Null || direction.Value == facing.ToString())
                {
                    AddFrame(frameNode, frameList);

                    XmlAttribute fixedCenterAttribute = (XmlAttribute)frameNode.Attributes.GetNamedItem("fixedCenter");
                    fixedCenter = fixedCenterAttribute != null && fixedCenterAttribute.Value == "true";

                    XmlAttribute bottomRightOriginAttribute = (XmlAttribute)frameNode.Attributes.GetNamedItem("bottomRightOrigin");
                    bottomRightOrigin = bottomRightOriginAttribute != null && bottomRightOriginAttribute.Value == "true";
                }
            }

            // Create the sprite object
            if (frameList.Count == 1)
            {
                sprite = new StaticSprite(spriteSheet, frameList[0]);
            }
            else
            {
                sprite = new AnimatedSprite(spriteSheet, frameList, fixedCenter, bottomRightOrigin);
            }

            return sprite;
        }

        public ISprite CreateFontSprite(string name, int maxCharactersPerLine)
        {
            // Variables for creating the sprite
            List<Rectangle> frameList = new List<Rectangle>();
            List<StaticSprite> spriteList = new List<StaticSprite>();

            // Iterate over the characters of the string
            for (int i = 0; i < name.Length; i++)
            {
                XmlNode node = FontData.DocumentElement.SelectSingleNode("sprite[@name = \"" + name.ToUpper(CultureInfo.CurrentCulture)[i] + "\"]");

                if (node != null)
                {
                    AddFrame(node.FirstChild, frameList);

                    spriteList.Add(new StaticSprite(FontSpriteSheet, frameList[^1]));
                }
            }

            return new FontSprite(spriteList, name, fontWidth, fontHeight, maxCharactersPerLine);
        }
    }
}

