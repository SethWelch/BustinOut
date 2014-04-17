using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    class Options
    {
        private static Texture2D arrow, optionScreen;
        private static Vector2 arrowPosition = new Vector2(170, 475);
        private static Vector2 arrowPosition2 = new Vector2(170, 530);
        private static Vector2 arrowPosition3 = new Vector2(170, 605);
        private static Vector2 arrowPosition4 = new Vector2(170, 665);
        private static Vector2 position = arrowPosition;
        private static int option = 0, fromTitle = 0;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            optionScreen = Content.Load<Texture2D>("Images/Menus/Options Menu");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(optionScreen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(arrow, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (ctrl.Select())
            {
                if (option == 0)
                {
                    BustinOutGame.setState(11, 1);
                }
                else if (option == 1)
                {
                    BustinOutGame.setState(5, 1);
                }
                else if (option == 2)
                {
                    BustinOutGame.setState(6, 1);
                }
                else
                {
                    if (fromTitle == 0)
                    {
                        BustinOutGame.setState(1, 1);
                    }
                    else
                    {
                        BustinOutGame.setState(7, 0);
                        FromTitle = 0;
                    }
                }
                option = 0;
                position = arrowPosition;
            }

            if (ctrl.Back())
            {
                if (fromTitle == 0)
                {
                    BustinOutGame.setState(1, 1);
                }
                else
                {
                    BustinOutGame.setState(7, 0);
                    FromTitle = 0;
                }
            }

            if (ctrl.Up())
            {
                if (option == 0)
                {
                    option = 3;
                }
                else
                {
                    option--;
                }

                setPosition();
            }

            if (ctrl.Down())
            {
                if (option == 3)
                {
                    option = 0;
                }
                else
                {
                    option++;
                }

                setPosition();
            }
        }

        private void setPosition()
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
            else
            {
                position = arrowPosition4;
            }
        }

        public static int FromTitle
        {
            get { return fromTitle; }
            set { fromTitle = value; }
        }
    }
}
