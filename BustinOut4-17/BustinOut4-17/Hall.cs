using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    class Hall
    {
        private static Texture2D arrow, optionScreen;
        private static Vector2 arrowPosition = new Vector2(170, 630);
        private static Vector2 position = arrowPosition;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            optionScreen = Content.Load<Texture2D>("Images/Menus/Hall of fame");
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
                BustinOutGame.setState(1, 1);
            }

            if (ctrl.Back())
            {
                BustinOutGame.setState(1, 1);
            }
        }
    }
}
