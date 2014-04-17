using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace BustinOutMegaMan
{
    public class Pause
    {
        private static Texture2D pauseScreen, arrow;
        private static Vector2 arrowPosition = new Vector2(150, 400), arrowPosition2 = new Vector2(150, 465), arrowPosition3 = new Vector2(150, 525), arrowPosition4 = new Vector2(200, 590),
            arrowPosition5 = new Vector2(150, 660);
        private static Vector2 position = arrowPosition;
        private static int option = 0, whereFrom = 0;

        PlayerControls ctrl = new PlayerControls();

        public static int from
        {
            get { return whereFrom; }
            set { whereFrom = value; }
        }

        public void LoadContent(ContentManager Content)
        {
            pauseScreen = Content.Load<Texture2D>("Images/Menus/Pause");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
        }


        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            //handles if unpaused, where the game returns to
            if (ctrl.Back() && whereFrom == 0)
            {
                BustinOutGame.setState(4, 0);
            }
            else if (ctrl.Back() && whereFrom == 1)
            {
                BustinOutGame.setState(8, 0);
            }
            else if (ctrl.Back() && whereFrom == 2)
            {
                BustinOutGame.setState(9, 0);
            }


            if (ctrl.Down())
            {
                if (option == 4)
                {
                    option = 0;
                }
                else
                {
                    option++;
                }
            }

            if (ctrl.Up())
            {
                if (option == 0)
                {
                    option = 4;
                }
                else
                {
                    option--;
                }
            }

            if (ctrl.Select())
            {
                if (option == 0 && whereFrom == 0)
                {
                    BustinOutGame.setState(4, 0);
                }
                else if (option == 0 && whereFrom == 1)
                {
                    BustinOutGame.setState(8, 0);
                }
                else if (option == 0 && whereFrom == 2)
                {
                    BustinOutGame.setState(9, 0);
                }
                else if (option == 1)
                {
                    BustinOutGame.setState(3, 0);
                    Options.FromTitle = 1;
                }
                else if (option == 2)
                {
                    BustinOutGame.RestartGame();
                    BustinOutGame.setState(4, 0);
                    BustinOutGame.setLevel(1);
                }
                else if (option == 3)
                {
                    BustinOutGame.RestartGame();
                    BustinOutGame.setState(1, 0);
                    BustinOutGame.setLevel(0);
                }
                else
                {
                    Program.gameExit();
                }

                option = 0;
            }

            SetPosition();
        }
        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(pauseScreen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(arrow, position, Color.White);
        }

        private void SetPosition()
        {
            if (option == 0)
            {
                position = arrowPosition;
            }
            else if (option == 1)
            {
                position = arrowPosition2;
            }
            else if (option == 2)
            {
                position = arrowPosition3;
            }
            else if (option == 3)
            {
                position = arrowPosition4;
            }
            else
            {
                position = arrowPosition5;
            }
        }
    }
}
