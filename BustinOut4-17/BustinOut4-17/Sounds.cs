using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace BustinOutMegaMan
{
    class Sounds
    {
        private static Texture2D point, arrow, arrow2, sound1, sound2, sound3, sound4, sound5, sound6, soundScreen;
        private static Vector2 arrowPosition = new Vector2(150, 660);
        private static Vector2 arrowPosition2 = new Vector2(520, 480);
        private static Vector2 arrowPosition3 = new Vector2(520, 350);
        private static Vector2 arrowPosition4 = new Vector2(720, 480);
        private static Vector2 arrowPosition5 = new Vector2(720, 350);
        private static Vector2 arrowPosition6 = new Vector2(910, 350);
        private static Vector2 position = arrowPosition;
        private static int option = 0, optionX = 0, optionY = 0;
        private static int musicV = 1, soundV = 1;

        PlayerControls ctrl = new PlayerControls();

        //load the images
        public void LoadContent(ContentManager Content)
        {
            sound1 = Content.Load<Texture2D>("Images/Menus/Sound screen");
            sound2 = Content.Load<Texture2D>("Images/Menus/Sound screen 2");
            sound3 = Content.Load<Texture2D>("Images/Menus/Sound screen 3");
            sound4 = Content.Load<Texture2D>("Images/Menus/Sound screen 4");
            sound5 = Content.Load<Texture2D>("Images/Menus/Sound screen 5");
            sound6 = Content.Load<Texture2D>("Images/Menus/Sound screen 6");
            soundScreen = sound2;
            arrow = Content.Load<Texture2D>("Images/Objects/arrow");
            arrow2 = Content.Load<Texture2D>("Images/Objects/arrow2");
            point = arrow;
        }

        //draw all of the images into the game
        public void Draw(SpriteBatch spriteBatch, int y, int Width, int Height)
        {
            spriteBatch.Draw(soundScreen, new Rectangle(0, y, Width, Height), Color.White);

            spriteBatch.Draw(point, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            //this if statement handles if the user selects a new volume
            if (ctrl.Select())
            {
                if (option == 0)
                {
                    optionX = 0;
                    position = arrowPosition;
                    BustinOutGame.setState(3, 1);
                }
                else if (option == 1)
                {
                    soundV = 0;
                }
                else if (option == 2)
                {
                    musicV = 0;
                }
                else if (option == 3)
                {
                    soundV = 1;
                }
                else if (option == 4)
                {
                    musicV = 1;
                }
                else if (option == 5)
                {
                    musicV = 2;
                }

                UpdateImage();
            }

            if (ctrl.Back())
            {
                BustinOutGame.setState(3, 1);
            }

            //handles moving the pointer up
            if (ctrl.Up())
            {
                if (optionY == 2  && optionX == 0)
                {
                    optionY = 0;
                }
                else if (optionY == 2 && optionX == 1)
                {
                    optionY = 0;
                    optionX = 0;
                }
                else if (optionY == 2 && optionX == 2)
                {
                    optionY = 0;
                    optionX = 0;
                }
                else
                {
                    optionY++;
                }

                SetOption();
            }

            //handles moving the pointer down
            if (ctrl.Down())
            {
                if (optionY == 0 && optionX == 0)
                {
                    optionY = 2;
                }
                else if (optionY == 1 && optionX == 1)
                {
                    optionY = 0;
                    optionX = 0;
                }
                else if (optionY == 2 && optionX == 2)
                {
                    optionY = 0;
                    optionX = 0;
                }
                else
                {
                    optionY--;
                }

                SetOption();
            }

            //handles moving the pointer right
            if (ctrl.Right())
            {
                if (optionX == 0 && optionY == 0)
                {
                    //do nothing
                }
                else if((optionX == 1 && optionY == 1) || (optionX == 2 && optionY == 2))
                {
                    optionX = 0;
                }
                else
                {
                    optionX++;
                }

                SetOption();
            }

            //handles moving the pointer left
            if (ctrl.Left())
            {
                if (optionX == 0 && optionY == 0)
                {
                    //do nothing
                }
                else if (optionX == 0 && optionY == 1)
                {
                    optionX = 1;
                }
                else if (optionX == 0 && optionY == 2)
                {
                    optionX = 2;
                }
                else
                {
                    optionX--;
                }

                SetOption();
            }
        }

        //This method finds out which button is being moved to
        private void SetOption()
        {
            if (optionX == 0)
            {
                if (optionY == 0)
                {
                    option = 0;
                }
                else if (optionY == 1)
                {
                    option = 1;
                }
                else
                {
                    option = 2;
                }
                SetPosition();
            }
            else if (optionX == 1)
            {
                if (optionY == 1)
                {
                    option = 3;
                }
                else
                {
                    option = 4;
                }
                SetPosition();
            }
            else
            {
                option = 5;
                SetPosition();
            }
        }

        //This method will move the arrow and change to the left or right arrow
        public void SetPosition()
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
            else if (option == 2)
            {
                position = arrowPosition3;
                point = arrow2;
            }
            else if (option == 3)
            {
                position = arrowPosition4;
                point = arrow2;
            }
            else if (option == 4)
            {
                position = arrowPosition5;
                point = arrow2;
            }
            else
            {
                position = arrowPosition6;
                point = arrow2;
            }
        }

        //this method changes the image to what was selected and calls the volume changing methods
        private void UpdateImage()
        {
            if (option == 1)
            {
                if (musicV == 0)
                {
                    soundScreen = sound4;
                    BustinOutGame.soundBool = false;
                }
                else if (musicV == 1)
                {
                    soundScreen = sound5;
                    BustinOutGame.soundBool = false;
                }
                else
                {
                    soundScreen = sound6;
                    BustinOutGame.soundBool = false;
                }
            }
            if (option == 2)
            {
                if (soundV == 0)
                {
                    soundScreen = sound4;
                    MusicPlayer.VolumeOff();
                }
                else
                {
                    soundScreen = sound3;
                    MusicPlayer.VolumeOff();
                }
            }
            if (option == 3)
            {
                if (musicV == 0)
                {
                    soundScreen = sound3;
                    BustinOutGame.soundBool = true;
                    BustinOutGame.playArrowSound(2);
                }
                else if (musicV == 1)
                {
                    soundScreen = sound2;
                    BustinOutGame.soundBool = true;
                    BustinOutGame.playArrowSound(2);
                }
                else
                {
                    soundScreen = sound1;
                    BustinOutGame.soundBool = true;
                    BustinOutGame.playArrowSound(2);
                }
            }
            if (option == 4)
            {
                if (soundV == 0)
                {
                    soundScreen = sound5;
                    MusicPlayer.VolumeLow();
                }
                else
                {
                    soundScreen = sound2;
                    MusicPlayer.VolumeLow();
                }
            }
            if (option == 5)
            {
                if (soundV == 0)
                {
                    soundScreen = sound6;
                    MusicPlayer.VolumeMax();
                }
                else
                {
                    soundScreen = sound1;
                    MusicPlayer.VolumeMax();
                }
            }
        }
    }
}