using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObjectHandler
{
    public static class GameObjectManager
    {
        private static List<IGameObject> gameObjects = new List<IGameObject>();
        private static bool paused;
        private static double pauseDuration;

        public static void AddGameObject(IGameObject gameObject)
        {
            if (!gameObjects.Contains(gameObject))
            {
                gameObjects.Add(gameObject);
            }
            else
            {
                Trace.WriteLine("[WARNING] GameObject could not be added to game objects. It already exists.");
            }
        }

        public static void AddGameObject(List<IGameObject> gameObjects)
        {
            if (gameObjects == null) throw new ArgumentNullException(nameof(gameObjects));

            foreach (IGameObject gameObject in gameObjects)
            {
                AddGameObject(gameObject);
            }
        }

        public static void RemoveGameObject(IGameObject gameObject)
        {
            if (gameObjects.Contains(gameObject))
            {
                gameObjects.Remove(gameObject);
            }
            else
            {
                Trace.WriteLine("[WARNING] GameObject could not be removed from game objects. It does not exist.");
            }
        }

        public static void RemoveGameObject(List<IGameObject> gameObjects)
        {
            if (gameObjects == null) throw new ArgumentNullException(nameof(gameObjects));

            foreach (IGameObject gameObject in gameObjects)
            {
                RemoveGameObject(gameObject);
            }
        }

        public static List<IGameObject> GetGameObjects()
        {
            return gameObjects;
        }

        public static void RemoveAllGameObjects()
        {
            gameObjects = new List<IGameObject>();
        }

        public static void UpdateAll(GameTime gametime)
        {
            if (gametime == null) throw new ArgumentNullException(nameof(gametime));

            if (!paused)
            {
                List<IGameObject> copyProjectiles = new List<IGameObject>(gameObjects);
                foreach (IGameObject gameObject in copyProjectiles)
                {
                    gameObject.Update(gametime);
                }
            }
            else if (pauseDuration > 0)
            {
                pauseDuration -= gametime.ElapsedGameTime.TotalSeconds;
                if (pauseDuration <= 0) UnPause();
            }
        }

        public static void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (IGameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch, Color.White);
            }
        }

        public static bool Contains(IGameObject gameObject)
        {
            return gameObjects.Contains(gameObject);
        }

        public static void Pause(double duration = 0)
        {
            paused = true;
            pauseDuration = duration;
        }

        public static void UnPause()
        {
            paused = false;
        }
    }
}
