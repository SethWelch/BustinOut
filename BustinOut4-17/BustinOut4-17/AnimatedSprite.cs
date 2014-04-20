using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BustinOutMegaMan
{
    public class AnimatedSprite
    {
        Texture2D spriteTexture;
        float timer = 0f, deathTimer = 0f, interval = 150f, startY, countDuration = .2f, currentTime = 0f, gravity = .65f,
            jumpPower = 20, frictionOnGround = .08f, frictionInAir = .06f, runSpeed = .5f, bulletSpeed = .2f;
        int currentFrame = 0, spriteWidth = 60, spriteHeight = 50, maxRightSideOfScreen = 1544, maxLeftSideOfScreen = -46,
            startX = 30, boardScreenSize = 1590, pitDepth = 765;
        public static int direction = 1;
        Rectangle sourceRect;
        Vector2 position, origin;
        bool jumping, running, isAlive = true;
        public static bool shooting = false;

        //for megaman
        public Vector2 Movement { get; set; }
        public static Rectangle mman { get; set; }
        private Vector2 oldPosition;

        //test
        public static Rectangle onePixelLower;

        PlayerControls ctrl = new PlayerControls();

        public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.spriteTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.jumping = false;
            this.running = false;
        }

        public void Update(GameTime gameTime)
        {
            AffectWithGravity();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            WrapAcrossScreenIfNeeded();
            CheckForPitDeath(gameTime);
            HandleSpriteMovement(gameTime);
        }

        public void AnimateRight(GameTime gameTime)
        {
            if (jumping)
            {
                currentFrame = 9;
            }
            else
            {
                if ((ctrl.doingNothing()))
                {
                    currentFrame = 4;
                }

                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > interval)
                {
                    currentFrame++;

                    if (currentFrame > 7)
                    {
                        currentFrame = 4;
                    }
                    timer = 0f;
                }
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            if (jumping)
            {
                currentFrame = 8;
            }
            else
            {
                if (ctrl.doingNothing())
                {
                    currentFrame = 0;
                }

                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timer > interval)
                {
                    currentFrame++;

                    if (currentFrame > 3)
                    {
                        currentFrame = 0;
                    }
                    timer = 0f;
                }
            }
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
           
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            mman = new Rectangle((int)BustinOutGame.megaman.Position.X, (int)BustinOutGame.megaman.Position.Y, (int)BustinOutGame.megaman.spriteWidth, BustinOutGame.megaman.spriteHeight);

            if (jumping && isAlive)
            {
                //Figures out sprite for jumping in either direction
                if (direction == 0)
                {
                    BustinOutGame.megaman.spriteHeight = 60;
                    currentFrame = 8;
                }
                else
                {
                    BustinOutGame.megaman.spriteHeight = 60;
                    currentFrame = 9;
                }
                if (position.Y >= startY)
                {
                    BustinOutGame.megaman.spriteHeight = 50;
                    jumping = false;
                    //figures out the direction to have character facing after landing
                    if (direction == 0)
                        currentFrame = 10;
                    else
                        currentFrame = 11;
                }
            }

            else
            {
                BustinOutGame.megaman.spriteHeight = 50;
                if (ctrl.jump() && IsOnFirmGround(mman) && jumping == false)
                {
                    jumping = true;
                    startY = position.Y;
                    Movement = -Vector2.UnitY * jumpPower;
                }
            }

            // If shooting key is held down then he'll stay in the shooting frame
            if (shooting && isAlive)
            {
                currentTime += bulletSpeed;//(float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentTime >= countDuration)
                {
                    currentTime -= countDuration;
                    shooting = false;
                }
            }

            //Animate Right Movement
            if (ctrl.moveRight() && isAlive)
            {
                running = true;
                AnimateRight(gameTime);
                Movement += Vector2.UnitX * runSpeed;

                //sets direction to have character facing when landing
                direction = 1;
            }

            // Animate Left Movement
            if (ctrl.moveLeft() && isAlive)
            {
                running = true;
                AnimateLeft(gameTime);
                Movement -= Vector2.UnitX * runSpeed;

                //sets direction to have character facing when landing
                direction = 0;
            }

            // If both left and right keys are pressed stop moving, seems to always face left
            // but the main part of the guy not moving works
            if (ctrl.moveStop() && isAlive)
            {
                if (direction == 0)
                    if (jumping)
                    {
                        BustinOutGame.megaman.spriteHeight = 60;
                        currentFrame = 8;
                    }
                    else
                    {
                        BustinOutGame.megaman.spriteHeight = 50;
                        currentFrame = 10;
                    }
                else
                    if (jumping)
                    {
                        BustinOutGame.megaman.spriteHeight = 60;
                        currentFrame = 9;
                    }
                    else
                    {
                        BustinOutGame.megaman.spriteHeight = 50;
                        currentFrame = 11;
                    }
            }

            //If just standing around then have appropriate standing sprites.
            if (!(ctrl.doNothing() || jumping || shooting) && isAlive)
            {
                running = false;

                if (direction == 0)
                    currentFrame = 10;
                else
                    currentFrame = 11;
            }

            // Based shooting flag OP
            if (ctrl.shoot() && isAlive)
            {
                shooting = true;

                if (running == false && direction == 1)
                {
                    currentFrame = 12;
                }
                else if (running == false && direction == 0)
                {
                    currentFrame = 13;
                }
                else if (running == true && direction == 1)
                {
                    currentFrame = 14;
                }
                else
                {
                    currentFrame = 15;
                }
            }

            //origin = new Vector2(sourceRect.Width/80, sourceRect.Height/80);
            origin = new Vector2(sourceRect.Width / 25, sourceRect.Height / 35);
        }

        //applies gravity
        private void AffectWithGravity()
        {   
            if (isAlive)
                Movement += Vector2.UnitY * gravity;
        }

        //apply friction, friction is stronger when mm is on the ground
        private void SimulateFriction()
        {
            if (IsOnFirmGround(mman))
            {
                //apply ground friction
                Movement -= Movement * Vector2.One * frictionOnGround;
            }
            else
            {
                //apply in-air friction
                Movement -= Movement * Vector2.One * frictionInAir;
            }
        }

        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, SourceRect);
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            //player position
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        //checks if the character is on the ground, used to check if they can jump
        public bool IsOnFirmGround(Rectangle rect)
        {
            onePixelLower = rect;
            onePixelLower.Offset(0, 1);

            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        //stops the character from moving when they are blocked
        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = Position - oldPosition;

            if (lastMovement.X == 0) //if the character's x position is the same as the x position from the previous frame
            {
                Movement *= Vector2.UnitY; // then multiply it by Vector2.UnitY (short for Vector2(0,1); (the X movement is * 0)
                //so that movement along the x-axis is not applied
            }
            if (lastMovement.Y == 0)
            {
                Movement *= Vector2.UnitX; //so that movement along the y-axis is not applied
                jumping = false;
            }
        }

        //wraps megaman to other side of screen when he leaves the screen area
        private void WrapAcrossScreenIfNeeded()
        {
            //right side of screen
            if (Position.X > maxRightSideOfScreen)
            {
                //checks megamans y position to make sure he stays at the same height
                int posY = (int)Position.Y;
                Position = new Vector2(startX, posY);
                //then moves the screen
                MoveBoardRight();
            }

            //left side of screen
            if (Position.X < maxLeftSideOfScreen)
            {
                int posY = (int)Position.Y;
                Position = new Vector2(maxRightSideOfScreen, posY);
                MoveBoardLeft();
            }
        }

        //moves the board when needed
        private void MoveBoardRight()
        {
            BustinOutGame.screenChange = true;

            BustinOutGame.BGChange(1);

            for (int x = 0; x < Board.CurrentBoard.Columns; x++)
                for (int y = 0; y < Board.CurrentBoard.Rows; y++)
                    Board.CurrentBoard.Tiles[x, y].Position -= new Vector2(boardScreenSize, 0);
        }

        private void MoveBoardLeft()
        {
            BustinOutGame.screenChange = true;

            BustinOutGame.BGChange(0);

            for (int x = 0; x < Board.CurrentBoard.Columns; x++)
                for (int y = 0; y < Board.CurrentBoard.Rows; y++)
                    Board.CurrentBoard.Tiles[x, y].Position += new Vector2(boardScreenSize, 0);
        }

        //checks if megaman falls down a hole and puts him back at start
        private void CheckForPitDeath(GameTime gameTime)
        {
            if (Position.X > 500)
            {
                BustinOutGame.clearBullets();

                MegaManExplode(gameTime);
            }

            if (Position.Y > 850)
                Position = new Vector2(startX, startY);
        }


        private void MegaManExplode(GameTime gameTime)
        {
            isAlive = false;
            
            deathTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if (currentFrame <= 15)
            
                //deathTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                currentFrame = 16;

                if (deathTimer > interval * 25)
                {
                    currentFrame++;

                   deathTimer = 0f;
                } 
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Texture2D Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public Board Board
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public BustinOutGame BustinOutGame
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public MovementWrapper MovementWrapper
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
