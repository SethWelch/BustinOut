using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;

/*======================================================Frogger.cs======================================================================
 * Seth Welch
 * 3-31-14
 * 
 * 
 * This is my recreation of frogger.  The projectiles idea is from our Megaman shooting, but none of the code has been copied from
 * another source.  This will work as one of the boss fights.
 * ===================================================================================================================================*/


namespace BustinOutMegaMan
{
    class Frogger
    {
        Texture2D mLeft, mRight, mman, background, background2, background3, block, block2, truck, truck2, truck3, truck4, car1, car2, light, light1, light2;
        Rectangle playerrectangle;
        int posX = 7, posY = 0, xMax = 19, yMax = 13, counter = 0, edge = 400, lightning = 1, lightY = 8, set = 0, point = 0, froggerState = 0, winning = 5;
        static Vector2 originalPos = new Vector2(570, 845);
        Vector2 playerPos = originalPos;
        Vector2 truckPos = new Vector2(-400, 785), truckPos2 = new Vector2(2000, 555), truckPos3 = new Vector2(-400, 675), truckPos4 = new Vector2(2000, 615);
        Vector2 carPos1 = new Vector2(-400, 500), carPos2 = new Vector2(2000, 725);
        Vector2 lightPos1 = new Vector2(0, 385), lightPos2 = new Vector2(0, 325), lightPos3 = new Vector2(0, 270), lightPos4 = new Vector2(0, 215);
        Vector2 lightPos5 = new Vector2(0, 160), timerPos = new Vector2(700, 0), deathPos = new Vector2(1200, 0);
        String time = "20:00";

        public List<Projectiles> trucks, trucks2, blocks, score, cars;

        PlayerControls ctrl = new PlayerControls();
        SpriteFont font;

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts/font2");
            background = Content.Load<Texture2D>("Images/Backgrounds/froggerBG");
            background2 = Content.Load<Texture2D>("Images/Menus/FroggerIntro");
            background3 = Content.Load<Texture2D>("Images/Menus/winner");
            mLeft = Content.Load<Texture2D>("Images/mman");
            mRight = Content.Load<Texture2D>("Images/mman2");
            block = Content.Load<Texture2D>("Images/Objects/blocked");
            block2 = Content.Load<Texture2D>("Images/Objects/blocked2");
            truck = Content.Load<Texture2D>("Images/Objects/truck");
            truck2 = Content.Load<Texture2D>("Images/Objects/truck2");
            truck3 = Content.Load<Texture2D>("Images/Objects/truck3");
            truck4 = Content.Load<Texture2D>("Images/Objects/truck4");
            car1 = Content.Load<Texture2D>("Images/Objects/car1");
            car2 = Content.Load<Texture2D>("Images/Objects/car2");
            light1 = Content.Load<Texture2D>("Images/Objects/lightning1");
            light2 = Content.Load<Texture2D>("Images/Objects/lightning2");
            mman = mLeft;
            light = light1;

            //Creates lists for the vehicles and rocks
            trucks = new List<Projectiles>();
            trucks2 = new List<Projectiles>();
            cars = new List<Projectiles>();
            blocks = new List<Projectiles>();
            score = new List<Projectiles>();

            //create the rocks as projectiles with no velocity
            for (int i = 0; i < 10; i++)
            {
                Projectiles rock = new Projectiles();
                Projectiles rock2 = new Projectiles();

                rock.Position = new Vector2(i * 160, 105);
                rock.Velocity = new Vector2(0, 0);
                blocks.Add(rock);

                if (i % 2 != 0)
                {
                    rock2.Position = new Vector2(rock.Position.X + 80, 105);
                    rock2.Velocity = new Vector2(0, 0);
                    blocks.Add(rock2);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            if (froggerState == 0)
            {
                if (ctrl.Select())
                {
                    froggerState = 1;
                }
            }
            if (froggerState == 1)
            {
                //create this rectangle in update since the x and y will change
                playerrectangle = new Rectangle((int)playerPos.X, (int)playerPos.Y, mLeft.Width, mLeft.Height);

                //checks if player won
                if (point == winning)
                {
                    froggerState = 2;
                }

                //If the player wants to move up
                if (ctrl.Up())
                {
                    if (posY == yMax)
                    {
                        //do nothing
                    }
                    else if (CollisionWalls(1) == true)
                    {
                        //do nothing
                    }
                    else
                    {
                        playerPos.Y = playerPos.Y - 57;
                        posY++;
                    }
                }

                //If the player wants to move down
                if (ctrl.Down())
                {
                    if (posY == 0)
                    {
                        //do nothing
                    }
                    else if (CollisionWalls(2) == true)
                    {
                        //do nothing
                    }
                    else
                    {
                        playerPos.Y = playerPos.Y + 57;
                        posY--;
                    }
                }

                //If the player wants to move left
                if (ctrl.Left())
                {
                    mman = mLeft;

                    if (posX == 0)
                    {
                        //do nothing
                    }
                    else if (CollisionWalls(3) == true)
                    {
                        //do nothing
                    }
                    else
                    {
                        playerPos.X = playerPos.X - 80;
                        posX--;
                    }
                }

                //If the player wants to move right
                if (ctrl.Right())
                {
                    mman = mRight;

                    if (posX == xMax)
                    {
                        //do nothing
                    }
                    else if (CollisionWalls(4) == true)
                    {
                        //do nothing
                    }
                    else
                    {
                        playerPos.X = playerPos.X + 80;
                        posX++;
                    }
                }

                //pause the game
                if (ctrl.Pause())
                {
                    BustinOutGame.setState(7, 0);
                }

                Console.WriteLine("" + posX);

                //Blocks the area that the player just scored from
                if (posY == yMax)
                {
                    Projectiles blocker = new Projectiles();
                    blocker.Position = new Vector2(posX * 80, 105);
                    blocker.Velocity = new Vector2(0, 0);

                    resetPos(false);

                    score.Add(blocker);
                    point++;
                }

                //adds the trucks with an increment that decides how many will show up
                if (counter == 0 || (counter % 60) == 0)
                {
                    Projectiles vehicle = new Projectiles();
                    Projectiles vehicle2 = new Projectiles();
                    Projectiles vehicle3 = new Projectiles();
                    Projectiles vehicle4 = new Projectiles();

                    vehicle.Position = truckPos;
                    vehicle.Velocity = new Vector2(10, 0);
                    trucks.Add(vehicle);

                    vehicle2.Position = truckPos2;
                    vehicle2.Velocity = new Vector2(-10, 0);
                    trucks.Add(vehicle2);

                    vehicle3.Position = truckPos3;
                    vehicle3.Velocity = new Vector2(8, 0);
                    trucks2.Add(vehicle3);

                    vehicle4.Position = truckPos4;
                    vehicle4.Velocity = new Vector2(-8, 0);
                    trucks2.Add(vehicle4);

                }

                //adds the cars with an increment that decides how many will show up
                if (counter == 0 || (counter % 120) == 0)
                {
                    Projectiles vehicle5 = new Projectiles();
                    Projectiles vehicle6 = new Projectiles();

                    vehicle5.Position = carPos1;
                    vehicle5.Velocity = new Vector2(5, 0);
                    cars.Add(vehicle5);

                    vehicle6.Position = carPos2;
                    vehicle6.Velocity = new Vector2(-5, 0);
                    cars.Add(vehicle6);
                }

                //Controls the electricity bar that megaman must pass at the end
                if (light == light1)
                {
                    light = light2;
                }
                else
                {
                    light = light1;
                }

                if (counter == 0 || (counter % 50) == 0)
                {
                    if (set == 0)
                    {
                        if (lightning < 5)
                        {
                            lightning++;
                        }
                        else
                        {
                            set = 1;
                        }
                    }
                    if (set == 1)
                    {
                        if (lightning > 1)
                        {
                            lightning--;
                        }
                        else
                        {
                            set = 2;
                        }
                    }

                    if (set == 2)
                    {
                        if (lightning < 5)
                        {
                            lightning++;
                        }
                        else
                        {
                            lightning = 0;
                            set = 0;
                        }
                    }
                }

                counter++;

                //reset the counter so it doesnt get infinitly big
                if (counter >= 400)
                {
                    counter = 0;
                }

                //checks for collisions with vehicles
                CollisionVehicles();

                //Checks if player hits lightning
                CollisionLightning();

                //Remove vehicles from list when they hit the screen edges
                foreach (Projectiles p in trucks.ToArray())
                {
                    p.Position += p.Velocity;

                    //check to if the trucks leave the screen
                    if (p.Position.X < -edge || p.Position.X > BustinOutGame.screenWidth + edge)
                    {
                        trucks.RemoveAt(trucks.IndexOf(p));
                    }
                }

                foreach (Projectiles p in trucks2.ToArray())
                {
                    p.Position += p.Velocity;

                    //check to if the trucks leave the screen
                    if (p.Position.X < -edge || p.Position.X > BustinOutGame.screenWidth + edge)
                    {
                        trucks2.RemoveAt(trucks2.IndexOf(p));
                    }
                }

                foreach (Projectiles p in cars.ToArray())
                {
                    p.Position += p.Velocity;

                    //check to if the trucks leave the screen
                    if (p.Position.X < -edge || p.Position.X > BustinOutGame.screenWidth + edge)
                    {
                        cars.RemoveAt(cars.IndexOf(p));
                    }
                }
            }
            if (froggerState == 2)
            {
                if (ctrl.Select())
                {
                    BustinOutGame.wonFrogger = true;
                    BustinOutGame.setState(4, 0);

                    froggerState = 0;
                    score.Clear();
                    point = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, TimeSpan timeRemaining, int y, int Width, int Height)
        {
            if (froggerState == 0)
            {
                spriteBatch.Draw(background2, new Rectangle(0, y, Width, Height), Color.White);
            }
            if (froggerState == 1)
            {
                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                //set the timer
                time = "" + timeRemaining.Minutes.ToString("00") + ":" + timeRemaining.Seconds.ToString("00");

                //draw timer
                spriteBatch.DrawString(font, time, timerPos, Color.White);

                spriteBatch.DrawString(font, "Deaths: " + BustinOutGame.deathCounter, deathPos, Color.White);

                //draw the blocks at the top
                for (int i = 0; i < blocks.Count; i++)
                {
                    spriteBatch.Draw(block, blocks[i].Position, Color.White);
                }

                //draw where megaman has scored
                for (int i = 0; i < score.Count; i++)
                {
                    spriteBatch.Draw(block2, score[i].Position, Color.White);
                }

                //draw the first set of vehicles
                for (int i = 0; i < trucks.Count; i++)
                {
                    spriteBatch.Draw(truck, trucks[i].Position, Color.White);

                    if ((i + 1) < trucks.Count)
                    {
                        spriteBatch.Draw(truck2, trucks[i + 1].Position, Color.White);
                        i++;
                    }
                }

                //draw the second set of vehicles
                for (int i = 0; i < trucks2.Count; i++)
                {
                    spriteBatch.Draw(truck3, trucks2[i].Position, Color.White);

                    if ((i + 1) < trucks2.Count)
                    {
                        spriteBatch.Draw(truck4, trucks2[i + 1].Position, Color.White);
                        i++;
                    }
                }

                //draw the third set of vehicles
                for (int i = 0; i < cars.Count; i++)
                {
                    spriteBatch.Draw(car2, cars[i].Position, Color.White);

                    if ((i + 1) < cars.Count)
                    {
                        spriteBatch.Draw(car1, cars[i + 1].Position, Color.White);
                        i++;
                    }
                }

                //draw the electricity bar
                spriteBatch.Draw(light, getLightningState(lightning), Color.White);

                spriteBatch.Draw(mman, playerPos, Color.White);
            }
            if (froggerState == 2)
            {
                spriteBatch.Draw(background3, new Rectangle(0, y, Width, Height), Color.White);
            }
        }

        //check if the player collides with the blocks
        private Boolean CollisionWalls(int a)
        {
            bool answer = false;
            Rectangle test;

            if (a == 1)
            {
                test = new Rectangle((int)playerPos.X, (int)playerPos.Y - 57, mLeft.Width, mLeft.Height);
            }
            else if (a == 2)
            {
                test = new Rectangle((int)playerPos.X, (int)playerPos.Y + 57, mLeft.Width, mLeft.Height);
            }
            else if (a == 3)
            {
                test = new Rectangle((int)playerPos.X - 80, (int)playerPos.Y, mLeft.Width, mLeft.Height);
            }
            else
            {
                test = new Rectangle((int)playerPos.X + 80, (int)playerPos.Y, mLeft.Width, mLeft.Height);
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                Rectangle testBlock = new Rectangle((int)blocks[i].Position.X, (int)blocks[i].Position.Y, block.Width, block.Height);

                if (test.Intersects(testBlock))
                {
                    answer = true;
                }
            }

            for (int j = 0; j < score.Count; j++)
            {
                Rectangle testBlock2 = new Rectangle((int)score[j].Position.X, (int)score[j].Position.Y, block2.Width, block2.Height);

                if (test.Intersects(testBlock2))
                {
                    answer = true;
                }
            }

            return answer;
        }

        //Check and react if the vehicles collide with the player
        private void CollisionVehicles()
        {
            for (int i = 0; i < trucks.Count; i++)
            {
                Rectangle test = new Rectangle((int)trucks[i].Position.X, (int)trucks[i].Position.Y, truck.Width, truck.Height);

                if (test.Intersects(playerrectangle) == true)
                {
                    resetPos(true);
                }
           }

            for (int i = 0; i < trucks2.Count; i++)
            {
                Rectangle test2 = new Rectangle((int)trucks2[i].Position.X, (int)trucks2[i].Position.Y, truck3.Width, truck3.Height);

                if (test2.Intersects(playerrectangle) == true)
                {
                    resetPos(true);
                }
            }

            for (int i = 0; i < cars.Count; i++)
            {
                Rectangle test3 = new Rectangle((int)cars[i].Position.X, (int)cars[i].Position.Y, car1.Width, car1.Height);

                if (test3.Intersects(playerrectangle) == true)
                {
                    resetPos(true);
                }
            }
        }

        //checks if the player collides with the lightning bar
        private void CollisionLightning()
        {
            if (posY == lightY)
            {
                resetPos(true);
            }
        }

        //sets the character position to the beginning spot
        private void resetPos(bool died)
        {
            if (died == true)
            {
                BustinOutGame.deathCounter++;
            }

            playerPos = originalPos;
            posX = 7;
            posY = 0;
        }

        //gets the postion of the lightning for drawing
        private Vector2 getLightningState(int i)
        {
            Vector2 currentPosition;

            if (i == 1)
            {
                currentPosition = lightPos1;
                lightY = 8;
            }
            else if (i == 2)
            {
                currentPosition = lightPos2;
                lightY = 9;
            }
            else if (i == 3)
            {
                currentPosition = lightPos3;
                lightY = 10;
            }
            else if (i == 4)
            {
                currentPosition = lightPos4;
                lightY = 11;
            }
            else
            {
                currentPosition = lightPos5;
                lightY = 12;
            }

            return currentPosition;
        }

    }
}
