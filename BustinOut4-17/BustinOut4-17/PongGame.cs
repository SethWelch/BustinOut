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

//Beginning code taken from: http://www.freewebs.com/campelmxna/XNATutorials/XNATut4.htm

namespace BustinOutMegaMan
{
    public class PongGame
    {
        Texture2D ball1, ball2, paddle, paddle2, background;
        Ball ball;
        Paddle playerone, playertwo;
        Rectangle playerrectangle, computerrectangle, ballrectangle;
        int posSpeed = 16, negSpeed = -16, ballSpeed = 17, p1Score = 0, p2Score = 0, direction = 1, height, width;
        Point start = new Point(800, 450);
        String p1 = "MegaMan", p2 = "Bowser", time = "20:00";
        SpriteFont font, font2;
        Vector2 fontPos = new Vector2(550, 10), fontPos2 = new Vector2(875, 10), scorePos = new Vector2(640, 50), scorePos2 = new Vector2(950, 50), timerPos = new Vector2(700, 400);
        SoundEffect blip;

        PlayerControls ctrl = new PlayerControls();


        //public static TimeSpan timeRemaining = BustinOutGame.timeRemaining;

        public void LoadContent(ContentManager Content)
        {
            ball1 = Content.Load<Texture2D>("Images/Objects/ball") as Texture2D;
            ball2 = Content.Load<Texture2D>("Images/Objects/fire") as Texture2D;
            paddle = Content.Load<Texture2D>("Images/bowser");
            paddle2 = Content.Load<Texture2D>("Images/mega_man_pong");
            background = Content.Load<Texture2D>("Images/Backgrounds/PongBG");
            font = Content.Load<SpriteFont>("Fonts/font");
            font2 = Content.Load<SpriteFont>("Fonts/font2");
            blip = Content.Load<SoundEffect>("Sounds/blip");

            ball = new Ball(start);

            height = BustinOutGame.screenHeight;
            width = BustinOutGame.screenWidth;

            playerone = new Paddle(new Vector2(20, height / 2));
            playertwo = new Paddle(new Vector2(1500, height / 2));
            playerrectangle = new Rectangle((int)playerone.Position.X, (int)playerone.Position.Y, paddle2.Width, paddle2.Height);
            computerrectangle = new Rectangle((int)playertwo.Position.X, (int)playertwo.Position.Y, paddle.Width, paddle.Height);
            ballrectangle = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball1.Width, ball1.Height);

            ball.xSpeed = ballSpeed;
            ball.ySpeed = ballSpeed;
        }

        public void Update(GameTime gameTime)
        {
            ctrl.setStates();

            //decrement remaining time
            //timeRemaining -= gameTime.ElapsedGameTime;

            if (ctrl.moveUp())
            {
                playerone.ySpeed = negSpeed;
            }
            else if (ctrl.moveDown())
            {
                playerone.ySpeed = posSpeed;
            }
            else
            {
                playerone.ySpeed = 0;
            }

            if (ctrl.Pause())
            {
                BustinOutGame.setState(7, 0);
            }

            playerone.UpdatePaddle();
            playertwo.UpdatePaddle();
            playerrectangle.X = (int)playerone.Position.X;
            playerrectangle.Y = (int)playerone.Position.Y;
            computerrectangle.X = (int)playertwo.Position.X;
            computerrectangle.Y = (int)playertwo.Position.Y;
            ballrectangle.X = (int)ball.Position.X;
            ballrectangle.Y = (int)ball.Position.Y;
            ball.UpdateBall();

            if (ball.Position.X <= -100)
            {
                ball.Position = start;
                ball.xSpeed = posSpeed;
                p2Score++;
                direction = 0;
            }
            else if (ball.Position.X + ball1.Width >= width - 5)
            {
                ball.Position = start;
                ball.xSpeed = negSpeed;
                p1Score++;
                direction = 1;
            }
            if (ball.Position.Y + ball1.Height > height)
            {
                ball.ySpeed *= -1;
            }
            else if (ball.Position.Y < 0)
            {
                ball.ySpeed *= -1;
            }


            if (playerone.Position.Y < 0)
            {
                playerone.Position.Y = 0;
            }
            else if (playerone.Position.Y + paddle.Height > height)
            {
                playerone.Position.Y = height - paddle.Height;
            }
            if (ballrectangle.Intersects(playerrectangle))
            {
                ball.Position.X = playerrectangle.Right;
                ball.xSpeed *= -1;
                direction = 0;
                blip.Play();
            }
            else if (ballrectangle.Intersects(computerrectangle))
            {
                ball.Position.X = computerrectangle.Left - ball1.Width;
                ball.xSpeed *= -1;
                direction = 1;
                blip.Play();
            }
            if (ball.Position.Y + ball1.Height / 2 > playertwo.Position.Y + paddle.Height / 2)
            {
                playertwo.ySpeed = posSpeed;
            }
            else if (ball.Position.Y + ball1.Height / 2 < playertwo.Position.Y + paddle.Height / 2)
            {
                playertwo.ySpeed = negSpeed;
            }

            if (playertwo.Position.Y < 0)
            {
                playertwo.Position.Y = 0;
            }
            else if (playertwo.Position.Y + paddle.Height > height)
            {
                playertwo.Position.Y = height - paddle.Height;
            }
        }


        public void Draw(SpriteBatch spriteBatch, TimeSpan timeRemaining)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            spriteBatch.DrawString(font, p1, fontPos, Color.LightGreen);
            spriteBatch.DrawString(font, p2, fontPos2, Color.LightGreen);
            spriteBatch.DrawString(font, ("" + p1Score), scorePos, Color.LightGreen);
            spriteBatch.DrawString(font, ("" + p2Score), scorePos2, Color.LightGreen);

            time = "" + timeRemaining.Minutes.ToString("00") + ":" + timeRemaining.Seconds.ToString("00");

            spriteBatch.DrawString(font2, time, timerPos, Color.LightGreen);

            spriteBatch.Draw(paddle2, playerone.Position, Color.White);

            if (direction == 0)
                spriteBatch.Draw(ball1, new Vector2(ball.Position.X, ball.Position.Y), Color.White);
            else
                spriteBatch.Draw(ball2, new Vector2(ball.Position.X, ball.Position.Y), Color.White);

            spriteBatch.Draw(paddle, playertwo.Position, Color.White);
        }
    }
}

namespace BustinOutMegaMan
{
    public class Ball
    {
        public Point Position;
        public int xSpeed;
        public int ySpeed;

        public Ball(Point position)
        {
            Position = position;
        }

        public void UpdateBall()
        {
            Position.X += xSpeed;
            Position.Y += ySpeed;
        }
    }
}

namespace BustinOutMegaMan
{
    public class Paddle
    {
        public Vector2 Position;
        public int ySpeed;

        public Paddle(Vector2 position)
        {
            Position = position;
        }

        public void UpdatePaddle()
        {
            Position.Y += ySpeed;
        }
    }
}

