/*
 * Detects and handles collisions between any game objects.
 * 
 * Author: Matthew Crabtree
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Collisions
{
    public class CollisionDetector
    {
        private List<IGameObject> gameObjects;
        private readonly Texture2D pixel;

        public CollisionDetector(Texture2D pixel)
        {
            this.pixel = pixel;
        }

        public void DetectCollisions()
        {
            GetGameObjects();
            SweepAndPrune();
        }

        private void GetGameObjects()
        {
            gameObjects = new List<IGameObject>();
            gameObjects.AddRange(PlayerManager.GetPlayers());
            gameObjects.AddRange(EnemyManager.GetEnemies());
            gameObjects.AddRange(ItemManager.GetItems());
            gameObjects.AddRange(EnemyProjectileManager.GetEnemyProjectiles());
            gameObjects.AddRange(PlayerProjectileManager.GetPlayerProjectiles());
            gameObjects.AddRange(BlockManager.GetBlocks());
        }

        private void SweepAndPrune()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    DetectCollision(gameObjects[i], gameObjects[j]);
                }
            }
        }

        private static void DetectCollision(IGameObject object1, IGameObject object2)
        {
            Rectangle object1Rect = object1.GetHitbox();
            Rectangle object2Rect = object2.GetHitbox();

            if (object1Rect.Intersects(object2Rect))
            {
                CollisionHandlerMatcher.MatchCollisionHandler(object1, object2);
            }
        }

        /// <summary>
        /// Calculates the direction that rect1 intersected with rect2 from.
        /// </summary>
        /// <param name="rect1">The rectangle that got collided with</param>
        /// <param name="rect2">The rectangle that is colliding</param>
        /// <param name="intersection">The intersection between the two rectangles</param>
        /// <returns>The direction that $rect2 was intersected by $rect1 from</returns>
        internal static Direction GetCollisionDirection(Rectangle rect1, Rectangle rect2, Rectangle intersection)
        {
            if (rect1.Y < rect2.Y && intersection.Height < intersection.Width)
            {
                return Direction.North;
            }
            else if (rect1.Y > rect2.Y && intersection.Height < intersection.Width)
            {
                return Direction.South;
            }
            else if (rect1.X < rect2.X && intersection.Height > intersection.Width)
            {
                return Direction.West;
            }
            else
            {
                return Direction.East;
            }
        }

        public void DrawColliders(SpriteBatch spriteBatch)
        {
            foreach (IGameObject gameObject in gameObjects)
            {
                DrawCollider(gameObject, spriteBatch, pixel);
            }
        }

        private static void DrawCollider(IGameObject gameObject, SpriteBatch spriteBatch, Texture2D borderPixel)
        {
            if (gameObject is IAdventurePlayer p)
            {
                DrawColliderBorders(spriteBatch, p.GetPlayerHitbox(), Color.Green, borderPixel);
                DrawColliderBorders(spriteBatch, p.GetMeleeHitbox(), Color.Blue, borderPixel);
            }
            else if (gameObject is IPlayerProjectile)
            {
                DrawColliderBorders(spriteBatch, gameObject.GetHitbox(), Color.Blue, borderPixel);
            }
            else if (gameObject is IBlock)
            {
                //DrawColliderBorders(spriteBatch, gameObject.GetHitbox(), Color.Orange, borderPixel);
            }
            else if (gameObject is IEnemy || gameObject is IEnemyProjectile)
            {
                DrawColliderBorders(spriteBatch, gameObject.GetHitbox(), Color.Red, borderPixel);
            }
            else if (gameObject is IItem)
            {
                DrawColliderBorders(spriteBatch, gameObject.GetHitbox(), Color.Yellow, borderPixel);
            }
        }

        private static void DrawColliderBorders(SpriteBatch spriteBatch, Rectangle collider, Color color, Texture2D borderPixel)
        {
            //Based on: https://stackoverflow.com/questions/2795741/displaying-rectangles-in-game-window-with-xna

            spriteBatch.Draw(borderPixel, new Rectangle(collider.Left, collider.Top, 1, collider.Height), color); // Left
            spriteBatch.Draw(borderPixel, new Rectangle(collider.Right, collider.Top, 1, collider.Height), color); // Right
            spriteBatch.Draw(borderPixel, new Rectangle(collider.Left, collider.Top, collider.Width, 1), color); // Top
            spriteBatch.Draw(borderPixel, new Rectangle(collider.Left, collider.Bottom, collider.Width, 1), color); // Bottom
        }
    }
}
