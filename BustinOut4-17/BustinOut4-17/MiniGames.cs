using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    class MiniGames
    {
        private static Texture2D arrow, miniGameScreen;
        private static Vector2 arrowPosition = new Vector2(170, 430);
        private static Vector2 arrowPosition2 = new Vector2(170, 490);
        private static Vector2 arrowPosition3 = new Vector2(200, 545);
        private static Vector2 arrowPosition4 = new Vector2(170, 600);
        private static Vector2 arrowPosition5 = new Vector2(170, 660);
        private static Vector2 position = arrowPosition;
        private static int option = 0, fromTitle = 0;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            miniGameScreen = Content.Load<Texture2D>("Images/Menus/Mini Game");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(miniGameScreen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(arrow, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (ctrl.Select())
            {
                if (option == 0)
                {
                    BustinOutGame.setState(8, 0);
                }
                else if (option == 1)
                {
                    BustinOutGame.setState(9, 0);
                }
                else if (option == 2)
                {

                }
                else if (option == 3)
                {
                    
                }
                else
                {
                    BustinOutGame.setState(1, 1);
                }
                option = 0;
                position = arrowPosition;
            }

            if (ctrl.Back())
            {
                BustinOutGame.setState(1, 1);
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

                setPosition();
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
            else if (option == 3)
            {
                position = arrowPosition4;
            }
            else
            {
                position = arrowPosition5;
            }
        }

        public static int FromTitle
        {
            get { return fromTitle; }
            set { fromTitle = value; }
        }
    }
}
