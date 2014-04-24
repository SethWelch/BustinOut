using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

/*======================================================DifficultySelect.cs============================================================
 * Seth Welch
 * 4-21-14
 * 
 * This is the screen after selecting new game.  It allows the user to choose which difficulty they want to play at.
 * ===================================================================================================================================*/

namespace BustinOutMegaMan
{
    class DifficultySelect
    {
        private static Texture2D arrow, arrow2, pointer, optionScreen;
        private static Vector2 arrowPosition = new Vector2(50, 300);
        private static Vector2 arrowPosition2 = new Vector2(50, 485);
        private static Vector2 arrowPosition3 = new Vector2(170, 660);
        private static Vector2 position = arrowPosition;
        private int location = 0;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            optionScreen = Content.Load<Texture2D>("Images/Menus/DifficultySelect");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
            arrow2 = Content.Load<Texture2D>("Images/Objects/arrow2");
            pointer = arrow2;
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(optionScreen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(pointer, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (ctrl.Up())
            {
                PositionUp();
            }

            if (ctrl.Down())
            {
                PositionDown();
            }

            if (ctrl.Select())
            {
                //Easy
                if(location == 0)
                {
                    BustinOutGame.setLevel(1);
                    BustinOutGame.setState(12, 1);
                    BustinOutGame.timeRemaining = TimeSpan.FromMinutes(40.0);
                }
                
                //Normal
                else if (location == 1)
                {
                    BustinOutGame.setLevel(1);
                    BustinOutGame.setState(12, 1);
                    BustinOutGame.timeRemaining = TimeSpan.FromMinutes(20.0);
                }

                //Go back to title screen
                else
                {
                    BustinOutGame.setState(1, 1);
                }
            }

            if (ctrl.Back())
            {
                BustinOutGame.setState(1, 1);
            }
        }

        //Move the arrow up
        private void PositionUp()
        {
            if (location == 0)
            {
                location = 2;
            }
            else
            {
                location--;
            }

            SetPosition();
        }

        //Move the arrow down
        private void PositionDown()
        {
            if (location == 2)
            {
                location = 0;
            }
            else
            {
                location++;
            }

            SetPosition();
        }

        //set where the arrow is at
        private void SetPosition()
        {
            if (location == 0)
            {
                position = arrowPosition;
                pointer = arrow2;
            }
            else if (location == 1)
            {
                position = arrowPosition2;
                pointer = arrow2;
            }
            else
            {
                position = arrowPosition3;
                pointer = arrow;
            }
        }
    }
}