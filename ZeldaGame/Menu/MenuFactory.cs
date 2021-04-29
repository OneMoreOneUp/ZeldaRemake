//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using ZeldaGame.Interfaces;

namespace ZeldaGame.HUD
{
    class MenuFactory
    {
        private Texture2D MenuSpriteSheet;

        public static MenuFactory Instance { get; } = new MenuFactory();
        private readonly XmlDocument SpriteData = new XmlDocument();
        private readonly XmlDocument PointData = new XmlDocument();

        /// <summary>
        /// Private consuctor so that only one instance is created.
        /// </summary>
        private MenuFactory()
        {
            //This is intentionally empty
        }

        public void LoadAllTextures(ContentManager content)
        {
            MenuSpriteSheet = content.Load<Texture2D>("SpriteSheets/HUD&PauseScreen");
            SpriteData.Load(@"../../../Menu/MenuSpriteData.xml");
            PointData.Load(@"../../../Menu/MenuPointData.xml");
        }

        private static void AddFrame(XmlNode frame, Dictionary<int, Rectangle> IconMap, int count)
        {
            IFormatProvider format = CultureInfo.CurrentCulture;

            int x = int.Parse(frame.ChildNodes[0].InnerXml, format);
            int y = int.Parse(frame.ChildNodes[1].InnerXml, format);
            int width = int.Parse(frame.ChildNodes[2].InnerXml, format);
            int height = int.Parse(frame.ChildNodes[3].InnerXml, format);

            IconMap.Add(count, new Rectangle(x, y, width, height));
        }


        public IMenuSprite CreateSprite(String name, int VariableSpriteInitializedValue = 0)
        {
            // Get the xml node corresponding to the given name
            XmlNode node = SpriteData.DocumentElement.SelectSingleNode("sprite[@name = \"" + name + "\"]");

            // Variables for creating the sprite
            IMenuSprite menuItem;
            Dictionary<int, Rectangle> IconMap = new Dictionary<int, Rectangle>();

            // Iterate over all the frame nodes 
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode frameNode = node.ChildNodes[i];
                AddFrame(frameNode, IconMap, i);
            }

            // Create the sprite object
            if (IconMap.Count == 1)
            {
                Rectangle srcRect = IconMap.Values.ToList<Rectangle>()[0];
                menuItem = new StaticMenuSprite(MenuSpriteSheet, srcRect);
            }
            else
            {
                menuItem = new VariableMenuSprite(MenuSpriteSheet, IconMap, VariableSpriteInitializedValue);
            }

            return menuItem;
        }

        public Point GetPoint(string name)
        {
            // Get the xml node corresponding to the given name
            XmlNode node = PointData.DocumentElement.SelectSingleNode("point[@name = \"" + name + "\"]");
            IFormatProvider format = CultureInfo.CurrentCulture;

            int x = int.Parse(node.ChildNodes[0].InnerXml, format);
            int y = int.Parse(node.ChildNodes[1].InnerXml, format);

            return new Point(x, y);
        }

        public List<Point> GetPointCollection(String name)
        {
            // Get the xml node corresponding to the given name
            XmlNode node = PointData.DocumentElement.SelectSingleNode("pointcollection[@name = \"" + name + "\"]");

            List<Point> pointCollection = new List<Point>();

            // Iterate over all the frame nodes 
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                XmlNode pointNode = node.ChildNodes[i];
                IFormatProvider format = CultureInfo.CurrentCulture;

                int x = int.Parse(pointNode.ChildNodes[0].InnerXml, format);
                int y = int.Parse(pointNode.ChildNodes[1].InnerXml, format);

                pointCollection.Add(new Point(x, y));
            }
            return pointCollection;
        }
    }
}
