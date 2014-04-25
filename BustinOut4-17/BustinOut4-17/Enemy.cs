using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private GraphicsDeviceManager graphics;

        Texture2D enemySprite;
        Vector2 position;
        int currentFrame;
        float aliveFrameInterval, deathFrameInterval, timer;
        float shootTimer, shootInterval;
        int shootSpeed;
        string spriteSet;
        EnemyType type;
        Rectangle bounds;
        bool isAlive;
        bool hasDied;

        int pit;

        Animation aliveAnimation = new Animation();
        Animation deathAnimation = new Animation();

        FaceDirection direction;

        public List<Projectiles> enemyProjectiles;

        Texture2D bullet;

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
            deathFrameInterval = 150;
            shootInterval = 1200;
            type = (EnemyType)enemyType;
            spriteSet = spriteName;
            moveSpeed = speed;
            isAlive = true;
            hasDied = false;
            direction = FaceDirection.Left;
            pit = 860;

            enemyProjectiles = new List<Projectiles>();
            shootSpeed = 10;
        }

        public void LoadContent(ContentManager Content)
        {
            enemySprite = Content.Load<Texture2D>("Images/" + spriteSet);
            aliveAnimation.animationImage = enemySprite;
            bounds = aliveAnimation.bounds;
            enemySprite = Content.Load<Texture2D>("Images/explosion");
            deathAnimation.animationImage = enemySprite;
            bullet = Content.Load<Texture2D>("Images/Objects/bullet");
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

                if (position.Y > pit)
                {
                    isAlive = false;
                    currentFrame = 0;
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

            shootBullets(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
                aliveAnimation.Draw(spriteBatch);
            else
                deathAnimation.Draw(spriteBatch);

            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                spriteBatch.Draw(bullet, enemyProjectiles[i].Position, null, Color.White, 0f, new Vector2(bullet.Width / 2, bullet.Height / 2), 1f, SpriteEffects.None, 0);
            }

        }

        //this method creates the gravity for the enemies
        private void affectWithGravity()
        {
            if (isAlive)
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
            if (isAlive)
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

            if (BustinOutGame.megaman.IsAlive())
            {
                foreach (Projectiles b in enemyProjectiles.ToArray())
                {
                    if (b.Position.X > BustinOutGame.megaman.Position.X && b.Position.X < BustinOutGame.megaman.Position.X + BustinOutGame.megaman.SourceRect.Width &&
                        b.Position.Y > BustinOutGame.megaman.Position.Y && b.Position.Y < BustinOutGame.megaman.Position.Y + BustinOutGame.megaman.SourceRect.Height)
                    {
                        BustinOutGame.megaman.isAlive = false;
                        enemyProjectiles.RemoveAt(enemyProjectiles.IndexOf(b));
                    }

                }
            }
            

        }

        private void shootBullets(GameTime gameTime)
        {
            //if player is to the left and on the same level and
            //if the enemy is facing them shoot a bullet

            shootTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Projectiles enemyBullet = new Projectiles();

            if (BustinOutGame.megaman.Position.X < position.X && direction == FaceDirection.Left)
            {
                if (BustinOutGame.megaman.Position.Y <= position.Y && BustinOutGame.megaman.Position.Y >= position.Y - aliveAnimation.spriteHeight)
                {
                    enemyBullet.Position = new Vector2((position.X - aliveAnimation.spriteWidth), (position.Y - (aliveAnimation.spriteHeight / 2)));
                    enemyBullet.Velocity = new Vector2(shootSpeed * (int)direction, 0);
                    enemyBullet.bound = new Rectangle((int)enemyBullet.Position.X, (int)enemyBullet.Position.Y, bullet.Width, bullet.Height);

                    if (shootTimer > shootInterval && isAlive)
                    {
                        enemyProjectiles.Add(enemyBullet);
                        shootTimer -= shootInterval;
                    }

                }

            }

            if (BustinOutGame.megaman.Position.X > position.X && direction == FaceDirection.Right)
            {
                if (BustinOutGame.megaman.Position.Y <= position.Y && BustinOutGame.megaman.Position.Y >= position.Y - aliveAnimation.spriteHeight)
                {
                    enemyBullet.Position = new Vector2(position.X, (position.Y - (aliveAnimation.spriteHeight / 2)));
                    enemyBullet.Velocity = new Vector2(shootSpeed * (int)direction, 0);
                    enemyBullet.bound = new Rectangle((int)enemyBullet.Position.X, (int)enemyBullet.Position.Y, bullet.Width, bullet.Height);

                    if (shootTimer > shootInterval && isAlive)
                    {
                        enemyProjectiles.Add(enemyBullet);
                        shootTimer -= shootInterval;
                    }

                }

            }

            foreach (Projectiles p in enemyProjectiles.ToArray())
            {
                p.Position += p.Velocity;

                //check to see when the projectile leaves the visible part of the screen
                if (p.Position.X < 0 || p.Position.X > 1600)
                {
                    enemyProjectiles.RemoveAt(enemyProjectiles.IndexOf(p));
                }

                if (BustinOutGame.screenChange == true)
                {
                    enemyProjectiles.Clear();
                    BustinOutGame.screenChange = false;
                }
            }

            

        }


        public bool isDead()
        {
            return hasDied;
        }
    }
}
