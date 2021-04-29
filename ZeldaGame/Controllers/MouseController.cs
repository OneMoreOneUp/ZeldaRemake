// MouseController Class
//
// @author Jared Lawson

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using ZeldaGame.Commands;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Controllers
{
    class MouseController : IController
    {
        private enum MouseStates { leftClick, rightClick }
        private readonly Dictionary<MouseStates, ICommand> mouseKeyMap;
        private MouseState oldState;
        private bool inMenu, paused;

        public MouseController()
        {
            mouseKeyMap = new Dictionary<MouseStates, ICommand>();
        }

        public void SetUpSingleplayer(IAdventurePlayer player)
        {
            mouseKeyMap.Add(MouseStates.leftClick, new UsePlayerSlot1Command(player));
            mouseKeyMap.Add(MouseStates.rightClick, new UsePlayerSlot2Command(player));
        }

        public void SetUpMultiplayer(IAdventurePlayer player1, IAdventurePlayer player2)
        {
            SetUpSingleplayer(player2);
        }

        //Returns the command corresponding to the user's input, returns null when there is not input or when input doesn't map to a command.
        public void GetInput()
        {
            if (paused) return;

            MouseState newState = Mouse.GetState();

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton != ButtonState.Pressed && mouseKeyMap.ContainsKey(MouseStates.leftClick))
            {
                mouseKeyMap[MouseStates.leftClick].Execute();
            }
            else if (newState.RightButton == ButtonState.Pressed && oldState.RightButton != ButtonState.Pressed && mouseKeyMap.ContainsKey(MouseStates.rightClick))
            {
                mouseKeyMap[MouseStates.rightClick].Execute();
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
