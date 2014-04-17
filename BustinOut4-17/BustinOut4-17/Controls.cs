using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    class Controls
    {
        private static Texture2D arrow, crtlScrn;
        private static Vector2 arrowPosition = new Vector2(130, 660), position = arrowPosition;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            crtlScrn = Content.Load<Texture2D>("Images/Menus/Controls Screen 2");
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(crtlScrn, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(arrow, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (ctrl.Select())
            {
                BustinOutGame.setState(3, 1);
            }

            if (ctrl.Back())
            {
                BustinOutGame.setState(3, 1);
            }
        }
    }
}
