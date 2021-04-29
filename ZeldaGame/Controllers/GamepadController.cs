// GamepadController Class
//
// @author Jared Lawson

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using ZeldaGame.Commands;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Controllers
{
    class GamepadController : IController
    {
        //Gamepad Key dictionary
        private Dictionary<Buttons, ICommand> nonHoldMap, holdMap, menuMap;
        private bool inMenu, paused;
        private GamePadState oldState;
        private readonly Game1 game;
        private bool isMultiplayer;

        public GamepadController(Game1 game)
        {
            this.game = game;

            nonHoldMap = new Dictionary<Buttons, ICommand>
                {
                    //select singleplayer of multiplayer
                    { Buttons.A, new SelectSingleplayerCommand(game) },
                    { Buttons.B, new SelectMultiplayerCommand(game) }
                };
        }

        public void SetUpSingleplayer(IAdventurePlayer player)
        {
            menuMap = new Dictionary<Buttons, ICommand>
                {
                    { Buttons.DPadDown, new SelectorDown(game) },
                    { Buttons.DPadLeft, new SelectorLeft(game) },
                    { Buttons.DPadRight, new SelectorRight(game) },
                    { Buttons.DPadUp, new SelectorUp(game) },
                    { Buttons.X, new SelectInMenu(game, !isMultiplayer) }
                };

            nonHoldMap = new Dictionary<Buttons, ICommand>
                {
                    //Debug commands
                    {Buttons.LeftShoulder, new PreviousRoomCommand(game) },
                    {Buttons.RightShoulder, new NextRoomCommand(game) },
                    {Buttons.RightStick, new ToggleHitboxesCommand(game) },

                    //General commands
                    {Buttons.Start, new PauseCommand(game) },
                    {Buttons.Y, new ResetCommand(game) },
                    {Buttons.Back, new ExitCommand(game) },

                    //Menu controls
                    {Buttons.X, new OpenMenu(game, !isMultiplayer)},

                    //player attacks
                    { Buttons.A, new UsePlayerSlot1Command(player) },
                    { Buttons.B, new UsePlayerSlot2Command(player) }
                };
            holdMap = new Dictionary<Buttons, ICommand>
                {
                    //player movement
                    { Buttons.DPadDown, new MovePlayerDownCommand(player) },
                    { Buttons.DPadLeft, new MovePlayerLeftCommand(player) },
                    { Buttons.DPadRight, new MovePlayerRightCommand(player) },
                    { Buttons.DPadUp, new MovePlayerUpCommand(player) },
                    { Buttons.LeftThumbstickDown, new MovePlayerDownCommand(player) },
                    { Buttons.LeftThumbstickLeft, new MovePlayerLeftCommand(player) },
                    { Buttons.LeftThumbstickRight, new MovePlayerRightCommand(player) },
                    { Buttons.LeftThumbstickUp, new MovePlayerUpCommand(player) }
                };
        }

        public void SetUpMultiplayer(IAdventurePlayer player1, IAdventurePlayer player2)
        {
            isMultiplayer = true;
            SetUpSingleplayer(player2);
        }

        //Returns the command corresponding to the user's input, returns null when there is not input or when input doesn't map to a command.
        public void GetInput()
        {
            //Get gamepad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if (inMenu)
            {
                GetMenuInput(gamepadState);
            }
            else if (!paused)
            {
                GetPlayerInput(gamepadState);
            }
            oldState = gamepadState;
        }

        private void GetMenuInput(GamePadState gamepadState)
        {
            if (gamepadState.DPad.Down == ButtonState.Pressed && oldState.DPad.Down != ButtonState.Pressed)
            {
                menuMap[Buttons.DPadDown].Execute();
            }
            else if (gamepadState.DPad.Left == ButtonState.Pressed && oldState.DPad.Left != ButtonState.Pressed)
            {
                menuMap[Buttons.DPadLeft].Execute();
            }
            else if (gamepadState.DPad.Right == ButtonState.Pressed && oldState.DPad.Right != ButtonState.Pressed)
            {
                menuMap[Buttons.DPadRight].Execute();
            }
            else if (gamepadState.DPad.Up == ButtonState.Pressed && oldState.DPad.Up != ButtonState.Pressed)
            {
                menuMap[Buttons.DPadUp].Execute();
            }
            else if (gamepadState.Buttons.X == ButtonState.Pressed && oldState.Buttons.X != ButtonState.Pressed)
            {
                menuMap[Buttons.X].Execute();
            }
        }

        private void GetPlayerInput(GamePadState gamepadState)
        {
            GetPlayerMovement(gamepadState);
            GetPlayerItems(gamepadState);
            GetGeneral(gamepadState);
        }

        private void GetGeneral(GamePadState gamepadState)
        {
            if (gamepadState.Buttons.Back == ButtonState.Pressed && oldState.Buttons.Back != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.Back].Execute();
            }
            else if (gamepadState.Buttons.Y == ButtonState.Pressed && oldState.Buttons.Y != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.Y].Execute();
            }
            else if (gamepadState.Buttons.X == ButtonState.Pressed && oldState.Buttons.X != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.X].Execute();
            }
            else if (gamepadState.Buttons.RightStick == ButtonState.Pressed && oldState.Buttons.RightStick != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.RightStick].Execute();
            }
            else if (gamepadState.Buttons.LeftShoulder == ButtonState.Pressed && oldState.Buttons.LeftShoulder != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.LeftShoulder].Execute();
            }
            else if (gamepadState.Buttons.RightShoulder == ButtonState.Pressed && oldState.Buttons.RightShoulder != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.RightShoulder].Execute();
            }
            else if (gamepadState.Buttons.Start == ButtonState.Pressed && oldState.Buttons.Start != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.Start].Execute();
            }
        }

        private void GetPlayerItems(GamePadState gamepadState)
        {
            if (gamepadState.Buttons.A == ButtonState.Pressed && oldState.Buttons.A != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.A].Execute();
            }
            else if (gamepadState.Buttons.B == ButtonState.Pressed && oldState.Buttons.B != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.B].Execute();
            }
            else if (gamepadState.Buttons.Start == ButtonState.Pressed && oldState.Buttons.Start != ButtonState.Pressed)
            {
                nonHoldMap[Buttons.Start].Execute();
            }
        }

        private void GetPlayerMovement(GamePadState gamepadState)
        {
            if (gamepadState.DPad.Down == ButtonState.Pressed)
            {
                holdMap[Buttons.DPadDown].Execute();
            }
            else if (gamepadState.ThumbSticks.Left.X < -0.5f)
            {
                holdMap[Buttons.LeftThumbstickLeft].Execute();
            }
            else if (gamepadState.DPad.Left == ButtonState.Pressed)
            {
                holdMap[Buttons.DPadLeft].Execute();
            }
            else if (gamepadState.ThumbSticks.Left.X > 0.5f)
            {
                holdMap[Buttons.LeftThumbstickRight].Execute();
            }
            else if (gamepadState.DPad.Right == ButtonState.Pressed)
            {
                holdMap[Buttons.DPadRight].Execute();
            }
            else if (gamepadState.ThumbSticks.Left.Y < -0.5f)
            {
                holdMap[Buttons.LeftThumbstickDown].Execute();
            }
            else if (gamepadState.DPad.Up == ButtonState.Pressed)
            {
                holdMap[Buttons.DPadUp].Execute();
            }
            else if (gamepadState.ThumbSticks.Left.Y > 0.5f)
            {
                holdMap[Buttons.LeftThumbstickUp].Execute();
            }
        }

        public void ToggleInMenu()
        {
            inMenu = !inMenu;
        }

        public void Pause()
        {
            paused = true;
        }

        public void UnPause()
        {
            paused = false;
        }
    }
}
