using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using ZeldaGame.Collisions;
using ZeldaGame.Commands;
using ZeldaGame.Controllers;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.GameState;
using ZeldaGame.HUD;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Player;
using ZeldaGame.Sprites;

namespace ZeldaGame
{
    class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CollisionDetector collisionDetector;
        private float scale;
        private Texture2D pixel;
        public MenuDisplay MenuPlayer1, MenuPlayer2;
        private HUDDisplay hudPlayer1, hudPlayer2;
        public bool menuPlayer = true;
        public bool menuOpen;
        public bool setSize = true;

        public bool MenuTrans { get; set; }
        public bool MenuActive { get; set; }
        public List<IController> Controllers { get; set; }
        public bool DrawHitboxes { get; set; }
        public IAdventurePlayer Player1 { get; set; }
        public IAdventurePlayer Player2 { get; set; }
        public MenuDisplay Menu { get; set; }

        private readonly int defaultPlayerX = 127 / 2, defaultPlayerY = 135 / 2;
        public static int DefaultWindowWidth = 256, DefaultWindowHeight = 232;

        //both these set to public so reset command can adjust the reset level based on current player level
        public Level CurrentLevel { get; set; }
        public int LevelStartIndex { get; set; }

        //this is public so that the controller can set it from the title screen
        public bool isMultiplayer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        protected override void Initialize()
        {
            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            collisionDetector = new CollisionDetector(pixel);
            GameStateManager.Pixel = pixel;
            LevelStartIndex = 1;

            MediaPlayer.IsRepeating = true;

            _graphics.PreferredBackBufferWidth = DefaultWindowWidth * 2;
            _graphics.PreferredBackBufferHeight = DefaultWindowHeight * 2;
            _graphics.ApplyChanges();

            scale = 2f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFactory.Instance.LoadAllTextures(Content);
            SoundFactory.Instance.LoadAllSounds(Content);
            MenuFactory.Instance.LoadAllTextures(Content);

            InitializeTitleState();
        }

        protected override void Update(GameTime gameTime)
        {

            collisionDetector.DetectCollisions();
            GameStateManager.Update();
            if (!GameStateManager.DisplayTitleScreen)
            {
                if (!MenuActive)
                {
                    BlockManager.UpdateAll(gameTime);
                    ItemManager.UpdateAll(gameTime);
                    EnemyManager.UpdateAll(gameTime);
                    PlayerProjectileManager.UpdateAll(gameTime);
                    EnemyProjectileManager.UpdateAll(gameTime);
                    PlayerManager.UpdateAll(gameTime);
                    GameObjectManager.UpdateAll(gameTime);
                    if (CurrentLevel.transitionRoom)
                    {
                        TransitionManager.UpdateFrame();
                    }
                    else
                    {
                        CurrentLevel.Update(gameTime);
                    }

                    hudPlayer1.Update();
                    if (isMultiplayer)
                    {
                        hudPlayer2.Update();
                    }

                    if (MenuTrans)
                    {
                        TransitionManager.UpdateFrame();
                        MenuTrans = TransitionManager.CheckTransitionMenu();
                        if (!MenuTrans)
                        {

                            if (menuPlayer)
                            {
                                MenuPlayer1.SetMenu(menuPlayer);
                            }
                            else
                            {
                                MenuPlayer2.SetMenu(menuPlayer);
                            }
                            TransitionFinished(menuPlayer);
                            TransitionManager.ResetTransition();
                        }
                    }
                }
                else
                {
                    if (menuPlayer)
                    {
                        MenuPlayer1.Update();
                    }
                    else
                    {
                        MenuPlayer2.Update();
                    }
                    hudPlayer1.Update();
                    if (isMultiplayer)
                    {
                        hudPlayer2.Update();

                    }
                }
            }

            foreach (IController controller in Controllers)
            {
                controller.GetInput();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Matrix matrix = Matrix.CreateScale(scale, scale, 1.0f);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, matrix);
            GameStateManager.Draw(_spriteBatch);
            if (!GameStateManager.DisplayTitleScreen)
            {
                if (!MenuActive)
                {
                    if (CurrentLevel.transitionRoom)
                    {
                        TransitionManager.DrawRoomTransition(_spriteBatch, CurrentLevel);

                        hudPlayer1.Draw(_spriteBatch, Color.White);

                        if (isMultiplayer)
                        {
                            hudPlayer2.Draw(_spriteBatch, Color.White);
                        }
                    }
                    else if (MenuTrans)
                    {
                        if (menuPlayer)
                        {
                            TransitionManager.DrawMenuTransition(_spriteBatch, MenuPlayer1, menuPlayer, hudPlayer1);
                            if (isMultiplayer)
                            {
                                hudPlayer2.Draw(_spriteBatch, Color.White);
                            }
                        }
                        else if (isMultiplayer && !menuPlayer)
                        {
                            TransitionManager.DrawMenuTransition(_spriteBatch, MenuPlayer2, menuPlayer, hudPlayer2);
                            hudPlayer1.Draw(_spriteBatch, Color.White);
                        }
                    }
                    else
                    {
                        //if no roomTransition, or menu transition draw hud
                        hudPlayer1.Draw(_spriteBatch, Color.White);

                        if (isMultiplayer)
                        {
                            hudPlayer2.Draw(_spriteBatch, Color.White);
                        }
                    }
                    if (DrawHitboxes) collisionDetector.DrawColliders(_spriteBatch);
                    BlockManager.DrawAll(_spriteBatch);
                    ItemManager.DrawAll(_spriteBatch);
                    EnemyManager.DrawAll(_spriteBatch, Color.White);
                    PlayerManager.DrawAll(_spriteBatch, Color.White);
                    PlayerProjectileManager.DrawAll(_spriteBatch);
                    EnemyProjectileManager.DrawAll(_spriteBatch);
                    GameObjectManager.DrawAll(_spriteBatch);

                }
                else
                {
                    if (isMultiplayer)
                    {
                        if (menuPlayer)
                        {
                            MenuPlayer1.Draw(_spriteBatch, Color.White);
                            hudPlayer2.Draw(_spriteBatch, Color.White);
                        }
                        else
                        {
                            MenuPlayer2.Draw(_spriteBatch, Color.White);
                            hudPlayer1.Draw(_spriteBatch, Color.White);
                        }
                    }
                    else
                    {
                        MenuPlayer1.Draw(_spriteBatch, Color.White);
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            scale = (1.0f * Window.ClientBounds.Width) / (DefaultWindowWidth);

            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = (int)(scale * (DefaultWindowHeight));
            _graphics.ApplyChanges();
        }

        private void InitializeTitleState()
        {
            GameStateManager.TitleScreen();
            MediaPlayer.Play(SoundFactory.Instance.GetSong(Enums.Sound.Intro));
            Controllers = new List<IController> { new KeyboardController(this), new MouseController(), new GamepadController(this) };
        }

        public void InitializeGameState()
        {
            CreatePlayers();
            CreateLevels();
            CreateMenuSystem();
            MediaPlayer.Stop();
            MediaPlayer.Play(SoundFactory.Instance.GetSong(Enums.Sound.Dungeon));
        }

        public void UpdateScreen()
        {
            DefaultWindowHeight += 56;
            _graphics.PreferredBackBufferHeight = DefaultWindowHeight * 2;
            _graphics.ApplyChanges();
        }

        public void ResetGame()
        {
            Controllers = new List<IController> { new KeyboardController(this), new MouseController(), new GamepadController(this) };
            if (isMultiplayer)
            {
                new SelectMultiplayerCommand(this).Execute();
            }
            else
            {
                new SelectSingleplayerCommand(this).Execute();
            }
        }

        private void CreatePlayers()
        {
            PlayerDataManager playerData = new PlayerDataManager("../../../GameObject/GameObjectData/GameObjectData.xml");
            int baseHealth = playerData.GetBaseHealth("Link");
            int baseCurrency = playerData.GetBaseCurrency("Link");

            AdventurePlayerInventory Player1Inventory = new AdventurePlayerInventory(baseCurrency, baseHealth, baseHealth, null, AdventurePlayerInventory.Slot1Item.WoodSword);
            Player1 = new GreenLink(new Point(2 * defaultPlayerX, 2 * defaultPlayerY), Player1Inventory, playerData);
            PlayerManager.AddPlayer(Player1);

            if (isMultiplayer)
            {
                AdventurePlayerInventory Player2Inventory = new AdventurePlayerInventory(baseCurrency, baseHealth, baseHealth, null, AdventurePlayerInventory.Slot1Item.WoodSword);
                Player2 = new GreenLink(new Point(2 * defaultPlayerX, 2 * defaultPlayerY), Player2Inventory, playerData);
                PlayerManager.AddPlayer(Player2);
            }
        }

        private void CreateMenuSystem()
        {
            MenuPlayer1 = new MenuDisplay(CurrentLevel, Player1.GetInventory());

            hudPlayer1 = new HUDDisplay(CurrentLevel, Player1.GetInventory());

            if (Player2 != null)
            {
                MenuPlayer2 = new MenuDisplay(CurrentLevel, Player2.GetInventory(), true);
                hudPlayer2 = new HUDDisplay(CurrentLevel, Player2.GetInventory(), false, true);

                hudPlayer2.SetPlayerTwo();

                CurrentLevel.AddHUD2(hudPlayer2);
            }

            CurrentLevel.AddHUD(hudPlayer1);
            CurrentLevel.AddMenu(MenuPlayer1);
        }

        private void CreateLevels()
        {
            CurrentLevel = new Level(Player1, LevelStartIndex);

            CurrentLevel.BuildMap();
            CurrentLevel.StartLevel();
            if (Player2 != null)
            {
                CurrentLevel.AddPlayer((Link)Player2);
            }
        }


        public void MenuTransition(bool player)
        {
            //this flips menu trans from false to true
            //or true to false
            MenuTrans = true;

            if (player)
            {
                menuPlayer = true;
                MenuPlayer1.Transition = true;

            }
            else if (isMultiplayer && !player)
            {
                menuPlayer = false;
                MenuPlayer2.Transition = true;
            }

            //set menuTrans to true since menu is in transition
            BlockManager.Pause();
            ItemManager.Pause();
            GameObjectManager.Pause();
            EnemyManager.Pause();
            PlayerManager.Pause();
            PlayerProjectileManager.Pause();
            EnemyProjectileManager.Pause();
        }

        //flips whether or not menu is in transition
        //menu is in transition when player hits ENTER
        public void TransitionMenu(bool player)
        {

            MenuActive = false;
            MenuTrans = true;

            if (player)
            {
                //changes direction display moves
                MenuPlayer1.FlipDirection();
                MenuPlayer1.Transition = true;
            }
            else
            {
                //changes direction display moves
                MenuPlayer2.FlipDirection();
                MenuPlayer2.Transition = true;
            }
        }

        private void TransitionFinished(bool player)
        {

            MenuTrans = false;
            if (player)
            {
                MenuPlayer1.Transition = false;
            }
            else
            {
                MenuPlayer2.Transition = false;
            }
            //controls the update/draws

            if (!menuOpen)
            {
                MenuActive = true;
                menuOpen = true;
            }
            else
            {
                if (player)
                {
                    MenuPlayer1.FlipDirection();
                }
                else
                {
                    MenuPlayer2.FlipDirection();
                }
                //MenuDisplay.SetGameHud();
                BlockManager.UnPause();
                ItemManager.UnPause();
                GameObjectManager.UnPause();
                EnemyManager.UnPause();
                PlayerManager.UnPause();
                PlayerProjectileManager.UnPause();
                EnemyProjectileManager.UnPause();
                menuOpen = false;

                foreach (IController controller in Controllers)
                {
                    controller.ToggleInMenu();
                }
            }

            //controls the transitions
        }
    }
}