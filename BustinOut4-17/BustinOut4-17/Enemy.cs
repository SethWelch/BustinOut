using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//This looks similar to the previous AnimatedSprite but
//instead of animating in the same class it passes
//the frame into the Animation class to do the animating

//Enemy class will have ai instead of controls

namespace BustinOutMegaMan
{
    public class Enemy
    {
        Texture2D enemySprite;
        Vector2 position;
        int currentFrame;
        float aliveFrameInterval, deathFrameInterval, timer;
        string spriteSet;
        EnemyType type;
        Rectangle bounds;
        bool isAlive;
        bool hasDied;

        Animation aliveAnimation = new Animation();
        Animation deathAnimation = new Animation();

        FaceDirection direction;

        public List<Projectiles> enemyProjectiles;

        int moveSpeed;

        enum EnemyType
        {
            BackAndForth = 1,
            RunLeft = 2,
        }

        enum FaceDirection
        {
            Left = -1,
            Right = 1,
        }


        public Enemy(int enemyType, string spriteName, int initX, int initY, int dirInterval, int speed)
        {
            position = new Vector2(initX, initY);
            aliveAnimation.Initialize(position, 10);

            currentFrame = 0;
            aliveFrameInterval = 150;
            deathFrameInterval = 300;
            type = (EnemyType)enemyType;
            spriteSet = spriteName;
            moveSpeed = speed;
            isAlive = true;
            hasDied = false;
            direction = FaceDirection.Left;

        }

        public void LoadContent(ContentManager Content)
        {
            enemySprite = Content.Load<Texture2D>("Images/" + spriteSet);
            aliveAnimation.animationImage = enemySprite;
            bounds = aliveAnimation.bounds;
            enemySprite = Content.Load<Texture2D>("Images/explosion");
            deathAnimation.animationImage = enemySprite;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //animation and ai for enemies when alive
            if (isAlive)
            {
                if (timer > aliveFrameInterval)
                {
                    currentFrame++;

                    if (currentFrame > 9)
                        currentFrame = 0;

                    timer -= aliveFrameInterval;
                }


                switch ((int)type)
                {
                    // back and forth 
                    case 1:
                        //turn the enemy around if they reach an edge of the screen or platform
                        if (!willBeOnFirmGround(new Rectangle((int)position.X, (int)position.Y, 1, 1)) || position.X <= 40 || position.X >= 1600)
                        {
                            direction = (FaceDirection)(-(int)direction);
                        }
                        position.X += (int)direction * moveSpeed;


                        break;
                    // just runs left type
                    case 2:
                        //turn the enemy around on the left side of the screen
                        if (position.X - 30 <= 40)
                        {
                            direction = (FaceDirection)(-(int)direction);
                        }

                        position.X += (int)direction * moveSpeed;

                        break;
                }

                //check if the enemy should be falling
                if (!IsOnFirmGround(new Rectangle((int)position.X - aliveAnimation.spriteWidth, (int)position.Y - aliveAnimation.spriteHeight,
                        aliveAnimation.spriteWidth, aliveAnimation.spriteHeight)))
                {
                    affectWithGravity();
                }

                if (Board.CurrentBoard.bumpedIntoBlock(new Rectangle((int)position.X - aliveAnimation.spriteWidth, (int)position.Y - 20, aliveAnimation.spriteWidth, 1)))
                {
                    direction = (FaceDirection)(-(int)direction);
                }

                aliveAnimation.Direction = (int)direction;
                aliveAnimation.CurrentFrame = currentFrame;
                aliveAnimation.Position = position;
                aliveAnimation.Update(gameTime);
            }
            else
            {
                deathAnimation.Position = position;

                if (timer > deathFrameInterval)
                {
                    if (currentFrame < 4)
                    {
                        currentFrame++;
                        deathAnimation.CurrentFrame = currentFrame;
                        deathAnimation.Update(gameTime);
                    }


                    timer -= deathFrameInterval;
                }
                if (currentFrame > 3)
                    hasDied = !hasDied;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                aliveAnimation.Draw(spriteBatch);
            }
            else
            {

                deathAnimation.Draw(spriteBatch);
            }
        }

        //this method creates the gravity for the enemies
        private void affectWithGravity()
        {
            position += Vector2.UnitY * 9f;
        }

        //this method checks to see if the enemy should fall
        public bool IsOnFirmGround(Rectangle rect)
        {
            Rectangle onePixelLower = rect;
            onePixelLower.Offset(0, 1);

            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        //This method checks ahead of the enemy to see if he will fall
        private bool willBeOnFirmGround(Rectangle rect)
        {
            Rectangle onePixelLower = rect;

            if (direction == FaceDirection.Left)
            {
                onePixelLower.Offset(-aliveAnimation.spriteWidth, 0);
            }
            else
            {
                onePixelLower.Offset(2, 0);
            }


            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        public void checkCollisions(List<Projectiles> bullets)
        {

            foreach (Projectiles b in bullets.ToArray())
            {
                if (BustinOutGame.megaman.Position.X < position.X)
                {
                    if (b.Position.X > position.X - aliveAnimation.spriteWidth)
                    {
                        if (b.Position.Y < position.Y && b.Position.Y > position.Y - aliveAnimation.spriteHeight)
                        {
                            bullets.RemoveAt(bullets.IndexOf(b));
                            isAlive = false;
                            currentFrame = 0;
                        }
                    }
                }
                else
                {
                    if (b.Position.X < position.X)
                    {
                        if (b.Position.Y < position.Y && b.Position.Y > position.Y - aliveAnimation.spriteHeight)
                        {
                            bullets.RemoveAt(bullets.IndexOf(b));
                            isAlive = false;
                            currentFrame = 0;
                        }
                    }
                }
            }
        }

        public bool isDead()
        {
            return hasDied;
        }
    }
}
