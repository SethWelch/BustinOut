using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BustinOutMegaMan
{
    public class Board
    {
        public Tile[,] Tiles { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D Background { get; set; }
        public Texture2D TileTexture { get; set; }
        public Texture2D BlockTexture { get; set; }
        public Texture2D PlatformTexture { get; set; }
        public Texture2D SpikesUpTexture { get; set; }
        public Texture2D SpikesDownTexture { get; set; }
        public Texture2D SpikesLeftTexture { get; set; }
        public Texture2D SpikesRightTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random _rnd = new Random();
        public static Board CurrentBoard { get; set; }
        public Vector2 Movement { get; set; }
        public Vector2 Position { get; set; }

        //Used to decide where pitfalls will be
        public int[] pitsLevel1 = new int[2] {30,75};
        public int[] pits = new int[2];
        private int pitSize = 5;
        private int level = 1;


        public Board(int level, SpriteBatch spritebatch, Texture2D tileTexture, Texture2D blockTexture, Texture2D platformTexture,
             Texture2D spikesUpTexture, Texture2D spikesDownTexture, Texture2D spikesLeftTexture, Texture2D spikesRightTexture, int columns, int rows)
        {
            this.level = level;
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            BlockTexture = blockTexture;
            PlatformTexture = platformTexture;
            SpikesUpTexture = spikesUpTexture;
            SpikesDownTexture = spikesDownTexture;
            SpikesLeftTexture = spikesLeftTexture;
            SpikesRightTexture = spikesRightTexture;
            SpriteBatch = spritebatch;
            Tiles = new Tile[Columns, Rows];
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left)) { Movement -= Vector2.UnitX * .5f; }
            if (keyboardState.IsKeyDown(Keys.Right)) { Movement += Vector2.UnitX * 0.5f; }
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        public void CreateNewBoard()
        {
            InitializeAllTilesAndBuildLevel();
            BuildLevel();
            SetAllBorderTilesBlocked();
            SetUnblockedTiles();
        }

        //Sets where the blocks wont exist
        private void SetUnblockedTiles()
        {

            if (level == 1)
            {
                pits = pitsLevel1;

                //This loop will create the pitfalls
                for (int i = 0; i < pits.Length; i++)
                {
                    int x = pits[i];

                    for (int j = 0; j < pitSize; j++)
                    {
                        Tiles[x, 19].IsBlocked = false;
                        x++;
                    }
                }
            }
        }

        private void InitializeAllTilesAndBuildLevel()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    //moves the board down
                    int r = y + 5;

                    Vector2 tilePosition = new Vector2(x * TileTexture.Width, r * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, false);
                }
            }
        }

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    { Tiles[x, y].IsBlocked = true; }
                }
            }
        }

        private void BuildLevel()
        {
            if (level == 2)
            {
                    //-------------------------------------------------Screen 1----------------------------------------------------------------
                    //-------------------------------------------------0<X<~48-------------------------------------------------------------------                

                    
                    Tiles[5, 5].IsBlocked = true;
                    Tiles[5, 5].Texture = BlockTexture;
                    Tiles[10,5].IsBlocked = true;
                    Tiles[10, 5].Texture = BlockTexture;
                    Tiles[15, 8].IsBlocked = true;
                    Tiles[15, 8].Texture = BlockTexture;
                    Tiles[20, 5].IsBlocked = true;
                    Tiles[20, 5].Texture = BlockTexture;
                    Tiles[25, 5].IsBlocked = true;
                    Tiles[25, 5].Texture = BlockTexture;
                    Tiles[30, 5].IsBlocked = true;
                    Tiles[30, 5].Texture = BlockTexture;
                    Tiles[35, 8].IsBlocked = true;
                    Tiles[35, 8].Texture = BlockTexture;
                    Tiles[40, 5].IsBlocked = true;
                    Tiles[40, 5].Texture = BlockTexture;
                    Tiles[45, 8].IsBlocked = true;
                    Tiles[45, 8].Texture = BlockTexture;
                    Tiles[50, 5].IsBlocked = true;
                    Tiles[50, 5].Texture = BlockTexture;


                    Tiles[26, 13].IsBlocked = true;
                    Tiles[26, 13].Texture = BlockTexture;
                    Tiles[39, 13].IsBlocked = true;
                    Tiles[39, 13].Texture = BlockTexture;
                    Tiles[38, 12].IsBlocked = true;
                    

                    //-----------------------------------------------Screen 2--------------------------------------------------------------------
                    //-----------------------------------------------~48<X<~102----------------------------------------------------------------------
                    Tiles[55, 5].IsBlocked = true;
                    Tiles[55, 5].Texture = BlockTexture;
                    Tiles[60, 5].IsBlocked = true;
                    Tiles[60, 5].Texture = BlockTexture;
                    Tiles[65, 5].IsBlocked = true;
                    Tiles[65, 5].Texture = BlockTexture;
                    Tiles[70, 5].IsBlocked = true;
                    Tiles[70, 5].Texture = BlockTexture;
                    Tiles[75, 5].IsBlocked = true;
                    Tiles[75, 5].Texture = BlockTexture;
                    Tiles[80, 5].IsBlocked = true;
                    Tiles[80, 5].Texture = BlockTexture;
                    Tiles[85, 5].IsBlocked = true;
                    Tiles[85, 5].Texture = BlockTexture;
                    Tiles[90, 5].IsBlocked = true;
                    Tiles[90, 5].Texture = BlockTexture;
                    Tiles[95, 5].IsBlocked = true;
                    Tiles[95, 5].Texture = BlockTexture;
                    Tiles[100,5].IsBlocked = true;
                    Tiles[100, 5].Texture = BlockTexture;
                    
                    //-----------------------------------------------Screen 3--------------------------------------------------------------------
                    //-----------------------------------------------102<X<154-------------------------------------------------------------------

                    
               

                    //---------------------------------------------Boss Screen------------------------------------------------------------------
                    //---------------------------------------------~155<X<205-------------------------------------------------------------------

                    Tiles[160, 5].IsBlocked = true;
                    Tiles[160, 5].Texture = PlatformTexture;
                    Tiles[165, 5].IsBlocked = true;
                    Tiles[165, 5].Texture = PlatformTexture;
                    Tiles[170, 5].IsBlocked = true;
                    Tiles[170, 5].Texture = PlatformTexture;
                    Tiles[175, 5].IsBlocked = true;
                    Tiles[175, 5].Texture = PlatformTexture;
                    Tiles[180, 5].IsBlocked = true;
                    Tiles[180, 5].Texture = PlatformTexture;
                    //Tiles[185, 4].IsBlocked = true;
                    //Tiles[185, 4].Texture = PlatformTexture;
                    //Tiles[194, 5].IsBlocked = true;
                    //Tiles[194, 5].Texture = PlatformTexture;
                    //Tiles[191, 8].IsBlocked = true;
                    //Tiles[193, 11].IsBlocked = true;
                    //Tiles[193, 11].Texture = PlatformTexture;
                    Tiles[187, 6].IsBlocked = true;
                    Tiles[191, 8].IsBlocked = true;
                    Tiles[195, 10].IsBlocked = true;
                    Tiles[199, 12].IsBlocked = true;
                    Tiles[202, 14].IsBlocked = true;
                    Tiles[206, 16].IsBlocked = true;
                    Tiles[206, 16].Texture = PlatformTexture;
            }
            if (level == 1)
            {
                //-------------------------------------------------Screen 1----------------------------------------------------------------
                //-------------------------------------------------0<X<~48-------------------------------------------------------------------                

                Tiles[0, 5].IsBlocked = true;
                Tiles[0, 5].Texture = PlatformTexture;
                Tiles[5, 5].IsBlocked = true;
                Tiles[5, 5].Texture = PlatformTexture;
                Tiles[10, 5].IsBlocked = true;
                Tiles[10, 5].Texture = PlatformTexture;
                Tiles[15, 8].IsBlocked = true;
                Tiles[15, 8].Texture = PlatformTexture;
                Tiles[20, 5].IsBlocked = true;
                Tiles[20, 5].Texture = PlatformTexture;
                Tiles[25, 5].IsBlocked = true;
                Tiles[25, 5].Texture = PlatformTexture;
                Tiles[30, 5].IsBlocked = true;
                Tiles[30, 5].Texture = PlatformTexture;
                Tiles[35, 8].IsBlocked = true;
                Tiles[35, 8].Texture = PlatformTexture;
                Tiles[40, 5].IsBlocked = true;
                Tiles[40, 5].Texture = PlatformTexture;
                Tiles[45, 8].IsBlocked = true;
                Tiles[45, 8].Texture = PlatformTexture;
                Tiles[50, 5].IsBlocked = true;
                Tiles[50, 5].Texture = PlatformTexture;

                //create wall blocking stairs
                Tiles[10, 1].IsBlocked = true;
                Tiles[10, 1].Texture = TileTexture;
                Tiles[10, 2].IsBlocked = true;
                Tiles[10, 2].Texture = TileTexture;
                Tiles[10, 3].IsBlocked = true;
                Tiles[10, 3].Texture = TileTexture;
                Tiles[10, 4].IsBlocked = true;
                Tiles[10, 4].Texture = TileTexture;

                Tiles[26, 13].IsBlocked = true;
                Tiles[26, 13].Texture = SpikesLeftTexture;
                Tiles[39, 13].IsBlocked = true;
                Tiles[39, 13].Texture = SpikesRightTexture;
                Tiles[38, 12].IsBlocked = true;
                Tiles[38, 12].Texture = SpikesUpTexture;
                Tiles[27, 12].IsBlocked = true;
                Tiles[27, 12].Texture = SpikesUpTexture;
                
                Tiles[28, 14].IsBlocked = true;
                Tiles[28, 14].Texture = SpikesDownTexture;
                //Tiles[29, 14].IsBlocked = true;
                //Tiles[29, 14].Texture = SpikesDownTexture;
                Tiles[35, 14].IsBlocked = true;
                Tiles[35, 14].Texture = SpikesDownTexture;
                Tiles[36, 14].IsBlocked = true;
                Tiles[36, 14].Texture = SpikesDownTexture;
                Tiles[32, 12].IsBlocked = true;
                Tiles[32, 12].Texture = BlockTexture;
                Tiles[26, 13].IsBlocked = true;
                Tiles[26, 13].Texture = PlatformTexture;
                Tiles[34, 13].IsBlocked = true;
                Tiles[34, 13].Texture = PlatformTexture;

                //-----------------------------------------------Screen 2--------------------------------------------------------------------
                //-----------------------------------------------~48<X<~102----------------------------------------------------------------------
                Tiles[55, 5].IsBlocked = true;
                Tiles[55, 5].Texture = PlatformTexture;
                Tiles[60, 5].IsBlocked = true;
                Tiles[60, 5].Texture = PlatformTexture;
                Tiles[65, 5].IsBlocked = true;
                Tiles[65, 5].Texture = PlatformTexture;
                Tiles[70, 5].IsBlocked = true;
                Tiles[70, 5].Texture = PlatformTexture;
                Tiles[75, 5].IsBlocked = true;
                Tiles[75, 5].Texture = PlatformTexture;
                Tiles[80, 5].IsBlocked = true;
                Tiles[80, 5].Texture = PlatformTexture;
                Tiles[85, 5].IsBlocked = true;
                Tiles[85, 5].Texture = PlatformTexture;
                Tiles[90, 5].IsBlocked = true;
                Tiles[90, 5].Texture = PlatformTexture;
                Tiles[95, 5].IsBlocked = true;
                Tiles[95, 5].Texture = PlatformTexture;
                Tiles[100, 5].IsBlocked = true;
                Tiles[100, 5].Texture = PlatformTexture;
                Tiles[105, 5].IsBlocked = true;
                Tiles[105, 5].Texture = PlatformTexture;

                Tiles[74, 12].IsBlocked = true;
                Tiles[74, 12].Texture = PlatformTexture;
                Tiles[74, 13].IsBlocked = true;
                Tiles[74, 13].Texture = SpikesDownTexture;
                Tiles[79, 12].IsBlocked = true;
                Tiles[79, 12].Texture = PlatformTexture;
                Tiles[83, 13].IsBlocked = true;
                Tiles[83, 13].Texture = SpikesDownTexture;
                Tiles[85, 14].IsBlocked = true;
                Tiles[85, 14].Texture = PlatformTexture;
                Tiles[90, 16].IsBlocked = true;
                Tiles[90, 16].Texture = PlatformTexture;
                Tiles[78, 8].IsBlocked = true;
                Tiles[78, 8].Texture = BlockTexture;
                //-----------------------------------------------Screen 3--------------------------------------------------------------------
                //-----------------------------------------------102<X<154-------------------------------------------------------------------

                Tiles[110, 5].IsBlocked = true;
                Tiles[110, 5].Texture = PlatformTexture;
                Tiles[115, 8].IsBlocked = true;
                Tiles[115, 8].Texture = PlatformTexture;
                //Tiles[120, 4].IsBlocked = true;
                //Tiles[120, 4].Texture = PlatformTexture;
                Tiles[125, 8].IsBlocked = true;
                Tiles[125, 8].Texture = PlatformTexture;
                //Tiles[130, 4].IsBlocked = true;
                //Tiles[130, 4].Texture = PlatformTexture;
                Tiles[135, 8].IsBlocked = true;
                Tiles[135, 8].Texture = PlatformTexture;
                //Tiles[140, 4].IsBlocked = true;
                //Tiles[140, 4].Texture = PlatformTexture;
                Tiles[145, 8].IsBlocked = true;
                Tiles[145, 8].Texture = PlatformTexture;
                Tiles[150, 5].IsBlocked = true;
                Tiles[150, 5].Texture = PlatformTexture;
                Tiles[155, 5].IsBlocked = true;
                Tiles[155, 5].Texture = PlatformTexture;

                Tiles[115, 18].IsBlocked = true;
                Tiles[115, 18].Texture = SpikesUpTexture;
                Tiles[120, 18].IsBlocked = true;
                Tiles[120, 18].Texture = SpikesUpTexture;
                Tiles[125, 18].IsBlocked = true;
                Tiles[125, 18].Texture = SpikesUpTexture;
                Tiles[130, 18].IsBlocked = true;
                Tiles[130, 18].Texture = SpikesUpTexture;
                Tiles[135, 18].IsBlocked = true;
                Tiles[135, 18].Texture = SpikesUpTexture;
                Tiles[140, 18].IsBlocked = true;
                Tiles[140, 18].Texture = SpikesUpTexture;
                Tiles[145, 18].IsBlocked = true;
                Tiles[145, 18].Texture = SpikesUpTexture;
                Tiles[150, 18].IsBlocked = true;
                Tiles[150, 18].Texture = SpikesUpTexture;
                Tiles[155, 18].IsBlocked = true;
                Tiles[155, 18].Texture = SpikesUpTexture;

                //---------------------------------------------Boss Screen------------------------------------------------------------------
                //---------------------------------------------~155<X<205-------------------------------------------------------------------

                Tiles[160, 5].IsBlocked = true;
                Tiles[160, 5].Texture = PlatformTexture;
                Tiles[165, 5].IsBlocked = true;
                Tiles[165, 5].Texture = PlatformTexture;
                Tiles[170, 5].IsBlocked = true;
                Tiles[170, 5].Texture = PlatformTexture;
                Tiles[175, 5].IsBlocked = true;
                Tiles[175, 5].Texture = PlatformTexture;
                Tiles[180, 5].IsBlocked = true;
                Tiles[180, 5].Texture = PlatformTexture;
                //Tiles[185, 4].IsBlocked = true;
                //Tiles[185, 4].Texture = PlatformTexture;
                //Tiles[194, 5].IsBlocked = true;
                //Tiles[194, 5].Texture = PlatformTexture;
                //Tiles[191, 8].IsBlocked = true;
                //Tiles[193, 11].IsBlocked = true;
                //Tiles[193, 11].Texture = PlatformTexture;
                Tiles[187, 6].IsBlocked = true;
                Tiles[191, 8].IsBlocked = true;
                Tiles[195, 10].IsBlocked = true;
                Tiles[199, 12].IsBlocked = true;
                Tiles[202, 14].IsBlocked = true;
                Tiles[206, 16].IsBlocked = true;
                Tiles[206, 16].Texture = PlatformTexture;
            }
        }

        public void Draw()
        {
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }
        }

        //checks to see if the passed in rectangle collides with any of the spikes tiles
        public bool HitSpike(Rectangle rectangleToCheck)
        {
            foreach (var tile in Board.CurrentBoard.Tiles)
            {
                if ((tile.Texture == SpikesDownTexture || tile.Texture == SpikesUpTexture || tile.Texture == SpikesLeftTexture || tile.Texture == SpikesRightTexture) && 
                    new Rectangle((int)tile.Position.X, (int)tile.Position.Y, (int)tile.Texture.Width, // creates a rectangle based off of the tile to check if it collides
                        (int)tile.Texture.Height).Intersects(rectangleToCheck)) 
                    return true;    
            }
            return false;
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (var tile in Tiles)
            {
                if (tile.IsBlocked && new Rectangle((int)tile.Position.X, (int)tile.Position.Y, (int)tile.Texture.Width, (int)tile.Texture.Height).Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

        public bool bumpedIntoBlock(Rectangle rectangleToCheck)
        {
            foreach (var tile in Tiles)
            {
                if (tile.IsBlocked && new Rectangle((int)tile.Position.X, (int)tile.Position.Y, (int)tile.Texture.Width, (int)tile.Texture.Height).Intersects(rectangleToCheck))
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.IsDiagonalMove)
            {
                int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.OneStep.X * Vector2.UnitX * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

                Vector2 remainingVerticalMovement = wrapper.OneStep.Y * Vector2.UnitY * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
            }

            return wrapper.FurthestAvailableLocationSoFar;
        }
    }
}