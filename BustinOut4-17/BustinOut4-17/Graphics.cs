using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    class Graphics
    {
        private static Texture2D arrow, arrow2, point, graphicsScreen, graphicsScreen2, screen;
        private static Vector2 arrowPosition = new Vector2(170, 660);
        private static Vector2 arrowPosition2 = new Vector2(650, 405);
        private static Vector2 arrowPosition3 = new Vector2(900, 405);
        private static Vector2 position = arrowPosition;
        private static int option = 0, fromTitle = 0;
        private bool isFullScreen = false;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            graphicsScreen = Content.Load<Texture2D>("Images/Menus/Graphics2");
            graphicsScreen2 = Content.Load<Texture2D>("Images/Menus/Graphics");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
            arrow2 = Content.Load<Texture2D>("Images/Objects/arrow2");
            screen = graphicsScreen;
            point = arrow;
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(screen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(point, position, Color.White);
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            ctrl.setStates();

            if (ctrl.Select())
            {
                if (option == 0)
                {
                    BustinOutGame.setState(3, 1);
                    option = 0;
                    position = arrowPosition;
                }
                else if (option == 1)
                {
                    if (isFullScreen == true)
                    {
                        isFullScreen = false;
                        screen = graphicsScreen;
                        graphics.ToggleFullScreen();
                    }
                }
                else 
                {
                    if (isFullScreen == false)
                    {
                        isFullScreen = true;
                        screen = graphicsScreen2;
                        graphics.ToggleFullScreen();
                    }
                }
            }

            if (ctrl.Back())
            {
                BustinOutGame.setState(3, 1);
            }

            if (ctrl.Up())
            {
                if (option == 0)
                {
                    option = 1;
                }
                else
                {
                    option = 0;
                }

                setPosition();
            }

            if (ctrl.Down())
            {
                if (option == 1 || option == 2)
                {
                    option = 0;
                }
                else
                {
                    option = 1;
                }

                setPosition();
            }

            if (ctrl.Right() || ctrl.Left())
            {
                if (option == 1)
                {
                    option = 2;
                }
                else if(option == 2)
                {
                    option = 1;
                }

                setPosition();
            }
        }

        private void setPosition()
        {
            if (option == 0)
            {
                position = arrowPosition;
                point = arrow;
            }
            else if (option == 1)
            {
                position = arrowPosition2;
                point = arrow2;
            }
            else
            {
                position = arrowPosition3;
                point = arrow2;
            }
        }

        public static int FromTitle
        {
            get { return fromTitle; }
            set { fromTitle = value; }
        }
    }
}
