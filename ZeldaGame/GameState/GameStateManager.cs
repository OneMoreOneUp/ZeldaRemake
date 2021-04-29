// Manager for change of game states
// Author: Matthew Crabtree

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using ZeldaGame.Enums;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;
using ZeldaGame.Sprites;

namespace ZeldaGame.GameState
{
    public static class GameStateManager
    {
        public static Texture2D Pixel { get; set; }
        public static bool DisplayTitleScreen { get; set; }

        private static bool lose, win, displayGameOver, nextLevel, isTransitioning;
        private static float gameOverOpacity, transitionOpacity;
        private const string gameOverText = "Game Over", resetHelpText = "Press 'R' to Reset.", winText = "You Win!";
        private const int textLineLength = 19;
        private static ISprite gameOver, resetHelp, winScreen, titleScreen;
        private static Action LoadNextRoom;

        public static void Reset()
        {
            lose = false;
            win = false;
            nextLevel = false;
            displayGameOver = false;
            isTransitioning = false;
            gameOverOpacity = 0;
            transitionOpacity = 0;
        }

        public static void GameOver()
        {
            //Kill all the players
            List<IAdventurePlayer> players = new List<IAdventurePlayer>(PlayerManager.GetPlayers());
            foreach (IAdventurePlayer player in players)
            {
                if (player is DamagedLink dl)
                {
                    dl.RemoveDecorator();
                    dl.GetDecoratedPlayer().TakeDamage(int.MaxValue, Enums.Direction.Null);
                }
                else
                {
                    player.TakeDamage(int.MaxValue, Enums.Direction.Null);
                }
            }

            //Stop music and enemies, play death sound
            MediaPlayer.Stop();
            EnemyManager.Pause();
            SoundFactory.Instance.GetSound(Sound.LinkDeath).Play();

            //Set end screen font
            gameOver = SpriteFactory.Instance.CreateFontSprite(gameOverText, textLineLength);
            resetHelp = SpriteFactory.Instance.CreateFontSprite(resetHelpText, textLineLength);
            for (int i = 0; i < gameOverText.Length; i++) gameOver.UpdateFrame();
            for (int i = 0; i < resetHelpText.Length; i++) resetHelp.UpdateFrame();

            lose = true;
        }

        public static void TitleScreen()
        {
            DisplayTitleScreen = true;
            titleScreen = SpriteFactory.Instance.CreateSprite("TitleScreen");
        }

        public static void GameWin()
        {
            //Set win screen font
            winScreen = SpriteFactory.Instance.CreateFontSprite(winText, textLineLength);
            for (int i = 0; i < winText.Length; i++) winScreen.UpdateFrame();

            //Stop music
            MediaPlayer.Stop();
            SoundFactory.Instance.GetSound(Sound.DungeonCleared).Play();

            win = true;
        }

        public static void NextLevel(Action loadNextRoom)
        {
            //Heal all players
            List<IAdventurePlayer> players = new List<IAdventurePlayer>(PlayerManager.GetPlayers());
            foreach (IAdventurePlayer player in players)
            {
                PlayerInventory inventory = player.GetInventory();
                inventory.SetHealth(inventory.GetMaxHealth());
            }

            LoadNextRoom = loadNextRoom;

            nextLevel = true;
            isTransitioning = true;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) throw new ArgumentNullException(nameof(spriteBatch));

            if (win) DrawWin(spriteBatch);
            if (lose) DrawLose(spriteBatch);
            if (nextLevel) DrawNextLevel(spriteBatch);
            if (DisplayTitleScreen) DrawTitleScreen(spriteBatch);
        }

        public static void Update()
        {
            if (lose) UpdateLose();
            if (nextLevel) UpdateNextLevel();
        }

        private static void DrawTitleScreen(SpriteBatch spriteBatch)
        {
            titleScreen.Draw(spriteBatch, new Point(Game1.DefaultWindowWidth / 2, Game1.DefaultWindowHeight / 2), Color.White, 0f);
        }

        private static void DrawWin(SpriteBatch spriteBatch)
        {
            winScreen.Draw(spriteBatch, new Point((Game1.DefaultWindowWidth / 2) - (SpriteFactory.FontWidth * winText.Length / 2), Game1.DefaultWindowHeight / 2), Color.White, 0.0f);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Game1.DefaultWindowWidth, Game1.DefaultWindowHeight), new Rectangle(0, 0, 1, 1), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, 0.095f);
        }

        private static void DrawLose(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Game1.DefaultWindowWidth, Game1.DefaultWindowHeight), new Rectangle(0, 0, 1, 1), Color.Red * 0.7f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.11f);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Game1.DefaultWindowWidth, Game1.DefaultWindowHeight), new Rectangle(0, 0, 1, 1), Color.Black * gameOverOpacity, 0.0f, Vector2.Zero, SpriteEffects.None, 0.095f);
            if (displayGameOver)
            {
                gameOver.Draw(spriteBatch, new Point((Game1.DefaultWindowWidth / 2) - (SpriteFactory.FontWidth * gameOverText.Length / 2), Game1.DefaultWindowHeight / 2), Color.White, 0.0f);
                resetHelp.Draw(spriteBatch, new Point((Game1.DefaultWindowWidth / 2) - (SpriteFactory.FontWidth * resetHelpText.Length / 2), (Game1.DefaultWindowHeight / 2) + 2 * SpriteFactory.FontHeight), Color.White, 0.0f);
            }
        }

        private static void DrawNextLevel(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Game1.DefaultWindowWidth, Game1.DefaultWindowHeight), new Rectangle(0, 0, 1, 1), Color.Black * transitionOpacity, 0.0f, Vector2.Zero, SpriteEffects.None, 0.095f);
        }

        private static void UpdateLose()
        {
            gameOverOpacity += 0.01f;
            if (!displayGameOver && gameOverOpacity > 1f)
            {
                displayGameOver = true;
            }
        }

        private static void UpdateNextLevel()
        {
            if (isTransitioning && transitionOpacity > 1f)
            {
                LoadNextRoom.Invoke();
                isTransitioning = false;
            }
            else if (isTransitioning)
            {
                transitionOpacity += 0.1f;
            }
            else if (transitionOpacity > 0f)
            {
                transitionOpacity -= 0.1f;
            }
            else
            {
                nextLevel = false;
            }
        }
    }
}
