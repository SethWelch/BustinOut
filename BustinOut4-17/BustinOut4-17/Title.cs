using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    public class Title
    {
        private static Texture2D arrow, titleScreen, sure, screen;

        //Title Screen
        private static Vector2 arrowPosition = new Vector2(225, 415);
        private static Vector2 arrowPosition2 = new Vector2(225, 480);
        private static Vector2 arrowPosition3 = new Vector2(225, 540);
        private static Vector2 arrowPosition4 = new Vector2(225, 610);
        private static Vector2 arrowPosition5 = new Vector2(225, 675);

        private static Vector2 position = arrowPosition;

        //Are you sure
        private static Vector2 arrowPosition6 = new Vector2(900, 440);
        private static Vector2 arrowPosition7 = new Vector2(900, 490);

        private static int option = 0;
        private static bool title = true;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            titleScreen = Content.Load<Texture2D>("Images/Menus/Title Screen");
            sure = Content.Load<Texture2D>("Images/Menus/Title Screen Quit Game");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
            screen = titleScreen;
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(screen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(arrow, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (ctrl.Select())
            {
                if (option == 0)
                {
                    BustinOutGame.setState(4, 0);
                    BustinOutGame.setLevel(1);
                }
                else if (option == 1)
                {
                    option = 0;
                    position = arrowPosition;
                    BustinOutGame.setState(10, 1);
                }
                else if(option == 2)
                {
                    option = 0;
                    position = arrowPosition;
                    BustinOutGame.setState(2, 1);
                }
                else if(option == 3) 
                {
                    option = 0;
                    position = arrowPosition;
                    BustinOutGame.setState(3, 1);
                }
                else if(option == 4)
                {
                    //asks if the player is sure they want to quit;
                    screen = sure;
                    title = false;
                    position = arrowPosition6;
                    option = 5;
                }
                else if (option == 5)
                {
                    Program.gameExit();
                }
                else
                {
                    screen = titleScreen;
                    position = arrowPosition;
                    option = 0;
                    title = true;
                }
            }

            if (ctrl.Up())
            {
                if (title == true)
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
                else
                {
                    if (option == 5)
                    {
                        option = 6;
                    }
                    else
                    {
                        option--;
                    }
                }
            }

            if (ctrl.Down())
            {
                if (title == true)
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
                else
                {
                    if (option == 6)
                    {
                        option = 5;
                    }
                    else
                    {
                        option++;
                    }
                }
            }

            if (ctrl.Exit())
            {
                Program.game.Exit();
            }
            SetPosition();
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
                else if (option == 4)
                {
                    position = arrowPosition5;
                }
                else if (option == 5)
                {
                    position = arrowPosition6;
                }
                else
                {
                    position = arrowPosition7;
                }
        }
    }
}
