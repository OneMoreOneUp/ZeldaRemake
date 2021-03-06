using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using ZeldaGame.Commands;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Controllers
{
    class KeyboardController : IController
    {
        private Dictionary<Keys, ICommand> nonHoldKeyMap;
        private readonly Dictionary<Keys, ICommand> player1HoldKeyMap;
        private readonly Dictionary<Keys, ICommand> player2HoldKeyMap;
        private readonly Dictionary<Keys, ICommand> menuKeyMap;
        private bool inMenu, paused;
        private KeyboardState oldState;
        private readonly Game1 game;

        public KeyboardController(Game1 game)
        {
            oldState = Keyboard.GetState();
            this.game = game;
            nonHoldKeyMap = new Dictionary<Keys, ICommand>();
            player1HoldKeyMap = new Dictionary<Keys, ICommand>();
            player2HoldKeyMap = new Dictionary<Keys, ICommand>();
            menuKeyMap = new Dictionary<Keys, ICommand>();

            nonHoldKeyMap.Add(Keys.A, new SelectSingleplayerCommand(game));
            nonHoldKeyMap.Add(Keys.B, new SelectMultiplayerCommand(game));
            nonHoldKeyMap.Add(Keys.Q, new ExitCommand(game));
            nonHoldKeyMap.Add(Keys.Escape, new ExitCommand(game));
        }

        public void SetUpMultiplayer(IAdventurePlayer player1, IAdventurePlayer player2)
        {
            SetUpGeneralCommands();

            //Player 1 movement
            player1HoldKeyMap.Add(Keys.W, new MovePlayerUpCommand(player1));
            player1HoldKeyMap.Add(Keys.S, new MovePlayerDownCommand(player1));
            player1HoldKeyMap.Add(Keys.A, new MovePlayerLeftCommand(player1));
            player1HoldKeyMap.Add(Keys.D, new MovePlayerRightCommand(player1));

            //Player 1 items
            nonHoldKeyMap.Add(Keys.J, new UsePlayerSlot1Command(player1));
            nonHoldKeyMap.Add(Keys.K, new UsePlayerSlot2Command(player1));

            //Player 1 menu controls
            nonHoldKeyMap.Add(Keys.Enter, new OpenMenu(game, true));
            menuKeyMap.Add(Keys.Enter, new SelectInMenu(game, true));

            //Player 2 movement
            player2HoldKeyMap.Add(Keys.Up, new MovePlayerUpCommand(player2));
            player2HoldKeyMap.Add(Keys.Down, new MovePlayerDownCommand(player2));
            player2HoldKeyMap.Add(Keys.Left, new MovePlayerLeftCommand(player2));
            player2HoldKeyMap.Add(Keys.Right, new MovePlayerRightCommand(player2));

            //Player 2 menu controls
            nonHoldKeyMap.Add(Keys.RightShift, new OpenMenu(game, false));
            menuKeyMap.Add(Keys.RightShift, new SelectInMenu(game, false));
        }

        public void SetUpSingleplayer(IAdventurePlayer player)
        {
            SetUpGeneralCommands();

            //Player movement
            player1HoldKeyMap.Add(Keys.W, new MovePlayerUpCommand(player));
            player1HoldKeyMap.Add(Keys.S, new MovePlayerDownCommand(player));
            player1HoldKeyMap.Add(Keys.A, new MovePlayerLeftCommand(player));
            player1HoldKeyMap.Add(Keys.D, new MovePlayerRightCommand(player));
            player1HoldKeyMap.Add(Keys.Up, new MovePlayerUpCommand(player));
            player1HoldKeyMap.Add(Keys.Down, new MovePlayerDownCommand(player));
            player1HoldKeyMap.Add(Keys.Left, new MovePlayerLeftCommand(player));
            player1HoldKeyMap.Add(Keys.Right, new MovePlayerRightCommand(player));

            //Player items
            nonHoldKeyMap.Add(Keys.J, new UsePlayerSlot1Command(player));
            nonHoldKeyMap.Add(Keys.K, new UsePlayerSlot2Command(player));

            //Menu controls
            nonHoldKeyMap.Add(Keys.Enter, new OpenMenu(game, true));
            menuKeyMap.Add(Keys.Enter, new SelectInMenu(game, true));
        }

        private void SetUpGeneralCommands()
        {
            nonHoldKeyMap = new Dictionary<Keys, ICommand>
            {

                //Debug commands
                { Keys.F1, new PreviousRoomCommand(game) },
                { Keys.F2, new NextRoomCommand(game) },
                { Keys.F3, new ToggleHitboxesCommand(game) },

                //General commands
                { Keys.Escape, new ExitCommand(game) },
                { Keys.Q, new ExitCommand(game) },
                { Keys.R, new ResetCommand(game) },
                { Keys.OemOpenBrackets, new VolumeDownCommand() },
                { Keys.OemCloseBrackets, new VolumeUpCommand() },
                { Keys.P, new MuteCommand() },
                { Keys.N, new PauseCommand(game) }
            };

            //Menu Control
            menuKeyMap.Add(Keys.A, new SelectorLeft(game));
            menuKeyMap.Add(Keys.S, new SelectorDown(game));
            menuKeyMap.Add(Keys.D, new SelectorRight(game));
            menuKeyMap.Add(Keys.W, new SelectorUp(game));
            menuKeyMap.Add(Keys.Left, new SelectorLeft(game));
            menuKeyMap.Add(Keys.Down, new SelectorDown(game));
            menuKeyMap.Add(Keys.Right, new SelectorRight(game));
            menuKeyMap.Add(Keys.Up, new SelectorUp(game));
        }

        //Returns the command corresponding to the user's input, returns null when there is not input or when input doesn't map to a command.
        public void GetInput()
        {
            KeyboardState newState = Keyboard.GetState();

            Keys[] pressedKeys = newState.GetPressedKeys();
            bool player1Hold = false, player2Hold = false;

            foreach (Keys key in pressedKeys)
            {
                if (inMenu)
                {
                    if (menuKeyMap.ContainsKey(key) && oldState.IsKeyUp(key))
                    {
                        menuKeyMap[key].Execute();
                    }
                }
                else
                {
                    if (nonHoldKeyMap.ContainsKey(key) && oldState.IsKeyUp(key))
                    {
                        nonHoldKeyMap[key].Execute();
                    }
                    else if (!player1Hold && player1HoldKeyMap.ContainsKey(key) && !paused)
                    {
                        player1HoldKeyMap[key].Execute();
                        player1Hold = true;
                    }
                    else if (!player2Hold && player2HoldKeyMap.ContainsKey(key) && !paused)
                    {
                        player2HoldKeyMap[key].Execute();
                        player2Hold = true;
                    } 
                }
            }

            oldState = newState;
        }

        public void Pause()
        {
            paused = true;
        }

        public void ToggleInMenu()
        {
            inMenu = !inMenu;
        }

        public void UnPause()
        {
            paused = false;
        }
    }
}
