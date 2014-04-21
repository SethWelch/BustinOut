using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

/*========================================================ShowControls.cs==============================================================
 * Seth Welch
 * 4-21-14
 * 
 * This screen just shows the player the controls after selecting their difficulty, but before playing the game.
 * ===================================================================================================================================*/

namespace BustinOutMegaMan
{
    class ShowControls
    {
        private static Texture2D optionScreen;

        PlayerControls ctrl = new PlayerControls();

        public void LoadContent(ContentManager Content)
        {
            optionScreen = Content.Load<Texture2D>("Images/Menus/ShowControls");
        }

        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(optionScreen, new Rectangle(0, y, Width, Height), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (ctrl.Select())
            {
                BustinOutGame.setLevel(1);
                BustinOutGame.setState(4, 0);
            }
        }
    }
}
