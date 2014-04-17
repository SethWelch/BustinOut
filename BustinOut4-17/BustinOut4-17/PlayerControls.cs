using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/*======================================================PlayerControls.cs======================================================================
 * Seth Welch
 * 4-13-14
 * 
 * This is where all of our games controls are now handled.  It is set up for both controller and keyboard.
 * ===================================================================================================================================*/


namespace BustinOutMegaMan
{
    public class PlayerControls
    {
        static KeyboardState currentState, previousState;
        static GamePadState currentGP, previousGP;

        //Gets and sets what the keyboard and controller states currently are
        public void setStates()
        {
            previousState = currentState;
            previousGP = currentGP;
            currentState = Keyboard.GetState();
            currentGP = GamePad.GetState(PlayerIndex.One);
        }

        //player selects an option in the menus
        public bool Select()
        {
            if (previousState.IsKeyDown(Keys.Space) == true && currentState.IsKeyDown(Keys.Space) == false || previousState.IsKeyDown(Keys.Enter) == true && currentState.IsKeyDown(Keys.Enter) == false)
                return true;
            else if (previousGP.Buttons.A == ButtonState.Released && currentGP.Buttons.A == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player moves up in the menus
        public bool Up()
        {
            if (previousState.IsKeyDown(Keys.W) == true && currentState.IsKeyDown(Keys.W) == false || previousState.IsKeyDown(Keys.Up) == true && currentState.IsKeyDown(Keys.Up) == false)
                return true;
            else if (previousGP.DPad.Up == ButtonState.Released && currentGP.DPad.Up == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player moves down in the menus
        public bool Down()
        {
            if (previousState.IsKeyDown(Keys.S) == true && currentState.IsKeyDown(Keys.S) == false || previousState.IsKeyDown(Keys.Down) == true && currentState.IsKeyDown(Keys.Down) == false)
                return true;
            else if (previousGP.DPad.Down == ButtonState.Released && currentGP.DPad.Down == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player moves left in the menus
        public bool Left()
        {
            if (previousState.IsKeyDown(Keys.A) == true && currentState.IsKeyDown(Keys.A) == false || previousState.IsKeyDown(Keys.Left) == true && currentState.IsKeyDown(Keys.Left) == false)
                return true;
            else if (previousGP.DPad.Left == ButtonState.Released && currentGP.DPad.Left == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player moves right in the menus
        public bool Right()
        {
            if (previousState.IsKeyDown(Keys.D) == true && currentState.IsKeyDown(Keys.D) == false || previousState.IsKeyDown(Keys.Right) == true && currentState.IsKeyDown(Keys.Right) == false)
                return true;
            else if (previousGP.DPad.Right == ButtonState.Released && currentGP.DPad.Right == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player moves to previous screen in the menus
        public bool Back()
        {
            if (previousState.IsKeyDown(Keys.Escape) == true && currentState.IsKeyDown(Keys.Escape) == false)
                return true;
            else if (previousGP.Buttons.B == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player pauses the game
        public bool Pause()
        {
            if (previousState.IsKeyDown(Keys.Escape) == true && currentState.IsKeyDown(Keys.Escape) == false || previousState.IsKeyDown(Keys.Enter) == true && currentState.IsKeyDown(Keys.Enter) == false)
                return true;
            else if (previousGP.Buttons.Start == ButtonState.Released && currentGP.Buttons.Start == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player exits the game on title screen 
        public bool Exit()
        {
            if (previousState.IsKeyDown(Keys.Escape) == true && currentState.IsKeyDown(Keys.Escape) == false)
                return true;
            else if (previousGP.Buttons.B == ButtonState.Released && currentGP.Buttons.B == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game and not pressing anything
        public bool doingNothing()
        {
            if (currentState != previousState)
                return true;
            else if (currentGP != previousGP)
                return true;
            else
                return false;
        }

        //player is in game jumping
        public bool jump()
        {
            if (currentState.IsKeyDown(Keys.Space) == true && previousState.IsKeyDown(Keys.Space) == false)// || currentGamePadState.Buttons.A == ButtonState.Pressed)
                return true;
            else if (previousGP.Buttons.A == ButtonState.Released && currentGP.Buttons.A == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game moving up
        public bool moveUp()
        {
            if (currentState.IsKeyDown(Keys.W) == true)
                return true;
            else if (previousGP.DPad.Up == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game moving down
        public bool moveDown()
        {
            if (currentState.IsKeyDown(Keys.S) == true)
                return true;
            else if (previousGP.DPad.Down == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game moving right
        public bool moveRight()
        {
            if (currentState.IsKeyDown(Keys.D) == true)
                return true;
            else if (previousGP.DPad.Right == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game moving left
        public bool moveLeft()
        {
            if (currentState.IsKeyDown(Keys.A) == true)
                return true;
            else if (previousGP.DPad.Left == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game and pressing both left and right
        public bool moveStop()
        {
            if (currentState.IsKeyDown(Keys.A) && currentState.IsKeyDown(Keys.D))
                return true;
            else
                return false;
        }

        //player is in game and not pressing either direction
        public bool doNothing()
        {
            if (currentState.IsKeyDown(Keys.A) || currentState.IsKeyDown(Keys.D))
                return true;
            else if (currentGP.DPad.Left == ButtonState.Pressed || currentGP.DPad.Right == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        //player is in game and shooting
        public bool shoot()
        {
            if (currentState.IsKeyDown(Keys.NumPad0) == true && previousState.IsKeyUp(Keys.NumPad0) == true)
                return true;
            else if (previousGP.Buttons.X == ButtonState.Released && currentGP.Buttons.X == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool levelChange()
        {
            if (previousState.IsKeyDown(Keys.L) == true && currentState.IsKeyDown(Keys.L) == false)
                return true;
            else
                return false;
        }

        public bool debugToggle()
        {
            if (previousState.IsKeyDown(Keys.F1) == true && currentState.IsKeyDown(Keys.F1) == false)
                return true;
            else
                return false;
        }

        public bool resetGame()
        {
            if (currentState.IsKeyDown(Keys.F5) == true)
                return true;
            else
                return false;
        }
    }
}
