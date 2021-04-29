//Author: Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZeldaGame.Enums;
using ZeldaGame.HUD.Menu;
using ZeldaGame.Interfaces;
using ZeldaGame.Levels;
using ZeldaGame.Player;

namespace ZeldaGame.HUD
{
    class MenuDisplay : IMenuItem
    {
        private readonly HUDDisplay HUDDisplay;
        private readonly MiniMapMenuDisplay MapDisplay;
        private readonly InventoryMenu InventoryDisplay;
        public bool Transition;
        public bool Down = true;

        public MenuDisplay(Level level, AdventurePlayerInventory inventory, bool playerTwo = false)
        {
            HUDDisplay = new HUDDisplay(level, inventory, true, playerTwo);
            MapDisplay = new MiniMapMenuDisplay(level, inventory, playerTwo);
            InventoryDisplay = new InventoryMenu(inventory, playerTwo);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            HUDDisplay.Draw(spriteBatch, color, Transition);
            MapDisplay.Draw(spriteBatch, color, Transition);
            InventoryDisplay.Draw(spriteBatch, color, Transition);
        }

        public void Update()
        {
            HUDDisplay.Update();
            MapDisplay.Update();
            InventoryDisplay.Update();
        }

        //flips direction of menu travel
        public void FlipDirection()
        {
            Down = !Down;
        }

        /*
         * 
            //only change position if in transition
            Transition = CheckMovement(trans);
                if(Transition)
                {
                    //if in transition and down, add x (moves down)
                    //if in transition and not down, subtract x (moves up)
                    HUDDisplay.TransitionDisplay(Down);
                    MiniMapMenuDisplay.TransitionDisplay(Down);
                    InventoryMenu.TransitionDisplay(Down);
                }
         * 
         * 
         */

        //this function handles transitioning all menu sections into view
        public void TransitionDisplay()
        {
            //if down, add x (moves down)
            //if not down, subtract x (moves up)
            HUDDisplay.TransitionDisplay(Down);
            MapDisplay.TransitionDisplay(Down);
            InventoryDisplay.TransitionDisplay(Down);
        }

        public void TransitionFrames(bool menuPlayer)
        {
            MapDisplay.TransitionFrame(menuPlayer);
            InventoryDisplay.TransitionFrame(menuPlayer);
        }

        public void SetMenu(bool menuPlayer)
        {
            MapDisplay.SetPoint(menuPlayer);
            InventoryDisplay.SetPoint(menuPlayer);
        }

        public void GiveCommand(Direction direction, bool select = false)
        {
            InventoryDisplay.ReceiveCommand(direction, select);
        }

        public void ChangeLevel(Level level, bool playerTwo)
        {
            HUDDisplay.ChangeLevel(level);
            MapDisplay.ChangeLevel(level, playerTwo);
        }

        public void SetPlayerTwoMenu()
        {
            MapDisplay.SetPlayerTwoMap();
            InventoryDisplay.SetPlayerTwoInv();
        }

        public void ResetPlayerTwo()
        {
            MapDisplay.ResetPlayerTwo();
            InventoryDisplay.ResetPlayerTwo();
        }
    }
}
