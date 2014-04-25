using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BustinOutMegaMan
{
    /// <summary>
    /// Simple code for a platformer game
    /// Created in 2013 by Jakob "xnafan" Krarup
    /// http://www.xnafan.net
    /// Distribute and reuse freely, but please leave this comment
    /// Modifed heavily by Andreas Freiburg, Seth Welch, and Phillip Peterson
    /// </summary>

    public class BustinOutGame : Game
    {
        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static TimeSpan timeRemaining = TimeSpan.FromMinutes(20.0);
        private Texture2D tileTexture1, tileTexture2, blockTexture, platformTexture, megamanTexture, bowser,
            spikesUpTexture, spikesDownTexture, spikesLeftTexture, spikesRightTexture, viewButtons, bowserText, bowserText2;
        private static Texture2D ui, prison1, prison2, prison3, prisonBoss, black, stairs, mb1, mb2, mb3, mb4, pong;
        public static AnimatedSprite megaman;
        public static bool soundBool = true, screenChange = false, wonPong = false;
        private Board board1, board2;
        private Random rnd = new Random();
        private SpriteFont debugFont;
        private static int bgNum = 0, yCorrect = 165, level = 1, height = 900, width = 1600, currentGameState = 0;
        public static int interact = 0;
        private bool debugBool = false, hasQuarter = false;
        private KeyboardState currentState, previousState;
        private static String timeString;
        Rectangle source;

        //for arrow sounds
        static SoundEffect arrowSound, arrowSelect, arrowBack;
        static SoundEffectInstance arrowSoundInstance, arrowSelectInstance, arrowBackInstance;
        float volume = .15f;

        public static int screenHeight
        {
            get { return height; }
            set { height = value; }
        }
        public static int screenWidth
        {
            get { return width; }
            set { width = value; }
        }

        public static int gameState
        {
            get { return currentGameState; }
        }

        Title titleScreen = new Title();
        Options optScn = new Options();
        Hall hallOfFame = new Hall();
        Controls control = new Controls();
        Sounds sound = new Sounds();
        Pause pause = new Pause();
        static PongGame game = new PongGame();
        Enemies enemies = new Enemies();
        Frogger frog = new Frogger();
        Graphics grph = new Graphics();
        MiniGames mini = new MiniGames();
        PlayerControls ctrl = new PlayerControls();
        DifficultySelect difficult = new DifficultySelect();
        ShowControls show = new ShowControls();


        public static List<Projectiles> LiveProjectiles;
        public Texture2D Bullet;
        private SpriteFont hudFont;

        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromMinutes(2);

        //sets all of the different game states
        private enum GameState
        {
            Title,
            Playing,
            Hall,
            Options,
            Sounds,
            Controls,
            MiniGames,
            Graphics,
            Pause,
            Pong,
            Frogger,
            Show,
            Difficulty
        }

        private static GameState mCurrentState = GameState.Title;

        public BustinOutGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = false;
        }

        protected override void LoadContent()
        {
            //Load Content for all of the different screens
            titleScreen.LoadContent(Content);
            hallOfFame.LoadContent(Content);
            optScn.LoadContent(Content);
            control.LoadContent(Content);
            sound.LoadContent(Content);
            pause.LoadContent(Content);
            game.LoadContent(Content);
            MusicPlayer.LoadContent(Content);
            enemies.LoadContent(Content);
            frog.LoadContent(Content);
            grph.LoadContent(Content);
            mini.LoadContent(Content);
            difficult.LoadContent(Content);
            show.LoadContent(Content);

            //sound effects
            arrowSound = Content.Load<SoundEffect>("Sounds/sound");
            arrowSelect = Content.Load<SoundEffect>("Sounds/select");
            arrowBack = Content.Load<SoundEffect>("Sounds/goBack");

            //creating each sound instance and setting the volumes
            arrowSoundInstance = arrowSound.CreateInstance();
            arrowSoundInstance.Volume = volume;
            arrowSelectInstance = arrowSelect.CreateInstance();
            arrowSelectInstance.Volume = volume;
            arrowBackInstance = arrowBack.CreateInstance();
            arrowBackInstance.Volume = volume;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load all of the graphics needed for in game
            viewButtons = Content.Load<Texture2D>("Images/Menus/buttonsInverted");
            tileTexture1 = Content.Load<Texture2D>("Images/Objects/block");
            pong = Content.Load<Texture2D>("Images/Objects/Pong");
            tileTexture2 = Content.Load<Texture2D>("Images/Objects/mblock");
            blockTexture = Content.Load<Texture2D>("Images/Objects/mario block");
            platformTexture = Content.Load<Texture2D>("Images/Objects/Platform");
            megamanTexture = Content.Load<Texture2D>("Images/MegaManRun");
            spikesUpTexture = Content.Load<Texture2D>("Images/Objects/Spikes Up");
            spikesDownTexture = Content.Load<Texture2D>("Images/Objects/Spikes Down");
            spikesLeftTexture = Content.Load<Texture2D>("Images/Objects/Spikes Left");
            spikesRightTexture = Content.Load<Texture2D>("Images/Objects/Spikes Right");
            bowser = Content.Load<Texture2D>("Images/bowser2");
            bowserText = Content.Load<Texture2D>("Images/bowserText");
            bowserText2 = Content.Load<Texture2D>("Images/bowserText2");
            ui = Content.Load<Texture2D>("Images/UI Overlay");

            //Load fonts for game
            hudFont = Content.Load<SpriteFont>("Fonts/Hud");
            debugFont = Content.Load<SpriteFont>("debugFont");
            
            //create the players character
            megaman = new AnimatedSprite(megamanTexture, 0, 60, 50);

            //create the levels
            board1 = new Board(1, spriteBatch, tileTexture1, blockTexture, platformTexture, spikesUpTexture, spikesDownTexture, spikesLeftTexture, spikesRightTexture, 212, 20);
            board2 = new Board(2, spriteBatch, tileTexture2, blockTexture, platformTexture, spikesUpTexture, spikesDownTexture, spikesLeftTexture, spikesRightTexture, 212, 20);

            //Set where player starts
            megaman.Position = new Vector2(100, 655);

            //Create players bullets
            LiveProjectiles = new List<Projectiles>();
            Bullet = Content.Load<Texture2D>("Images/Objects/bullet");

            prison1 = Content.Load<Texture2D>("Images/Backgrounds/PrisonBackground1");
            prison2 = Content.Load<Texture2D>("Images/Backgrounds/PrisonBackground2");
            prison3 = Content.Load<Texture2D>("Images/Backgrounds/PrisonBackground3");
            prisonBoss = Content.Load<Texture2D>("Images/Backgrounds/PrisonBoss");
            stairs = Content.Load<Texture2D>("Images/Objects/stairs");
            mb1 = Content.Load<Texture2D>("Images/Backgrounds/mb1");
            mb2 = Content.Load<Texture2D>("Images/Backgrounds/mb2");
            mb3 = Content.Load<Texture2D>("Images/Backgrounds/mb3");
            mb4 = Content.Load<Texture2D>("Images/Backgrounds/mboss");
            black = Content.Load<Texture2D>("Images/Backgrounds/background");

            //Set background for level
            if (level == 1)
            {
                Board.CurrentBoard = board1;
            }
            if (level == 2)
            {
                Board.CurrentBoard = board2;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            ManageState(gameTime);
            base.Update(gameTime);
            megaman.Update(gameTime);
            if (level == 1)
                board1.Update(gameTime);
            else
                board2.Update(gameTime);
            CheckKeyboardAndReact();
            source = new Rectangle((int)BustinOutGame.megaman.Position.X, (int)BustinOutGame.megaman.Position.Y, 60, 50);
            ctrl.setStates();

            if (!megaman.IsAlive())
                megaman.MegaManExplode(gameTime);
        }

        private void CheckKeyboardAndReact()
        {
            if (ctrl.debugToggle())
            {
                if (debugBool) debugBool = false;
                else debugBool = true;
            }

            if (ctrl.resetGame()) { RestartGame(0); }

            if (ctrl.levelChange())
            {
                if (level == 1)
                {
                    level = 2;
                    Board.CurrentBoard = board2;
                    bgNum = 0;
                    MusicPlayer.SwitchSong(4, level);
                }
                else
                {
                    level = 1;
                    Board.CurrentBoard = board1;
                    bgNum = 0;
                    MusicPlayer.SwitchSong(4, level);
                }
            }
        }

        public static void RestartGame(int from)
        {
            if (from == 0)
            {
                Board.CurrentBoard.CreateNewBoard();
                PutmegamanInBottomLeftCorner();
                LiveProjectiles.Clear();
                interact = 0;
                screenChange = true;
            }
            else if (from == 1)
            {
                game.Reset();
            }
            else
            {

            }

            timeRemaining = TimeSpan.FromMinutes(20.0);
        }

        private static void PutmegamanInBottomLeftCorner()
        {
            megaman.Position = new Vector2(30, 655);
            megaman.Movement = Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            spriteBatch.Begin();

            spriteBatch.Draw(black, new Vector2(0, 0), Color.White);

            switch (mCurrentState)
            {
                case GameState.Title:
                    {
                        titleScreen.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Hall:
                    {
                        hallOfFame.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Options:
                    {
                        optScn.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Controls:
                    {
                        control.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Sounds:
                    {
                        sound.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.MiniGames:
                    {
                        mini.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Graphics:
                    {
                        grph.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Pause:
                    {
                        pause.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Difficulty:
                    {
                        difficult.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Show:
                    {
                        show.Draw(spriteBatch, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Pong:
                    {
                        game.Draw(spriteBatch, timeRemaining, yCorrect, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height - (yCorrect * 2));

                        break;
                    }
                case GameState.Frogger:
                    {
                        frog.Draw(spriteBatch, timeRemaining);

                        break;
                    }
                case GameState.Playing:
                    {
                        base.Draw(gameTime);
                        spriteBatch.Draw(getBG(), new Vector2(0, 160), Color.White);

                        //draw the correct boards
                        if (level == 1)
                        {
                            board1.Draw();

                            if (bgNum == 0)
                            {
                                spriteBatch.Draw(stairs, new Vector2(30, 170), Color.White);
                            }
                        }
                        else
                            board2.Draw();

                        spriteBatch.Draw(ui, new Vector2(0, 0), Color.White);
                        if (debugBool) WriteDebugInformation();
                        DrawHud();

                        //---------------------Adding Bowser-----------------------//
                        if (bgNum == 3 && level == 1)
                        {
                            if (hasQuarter == true)
                            {
                                //bowser sees that the player has a quarter and challenges him
                                trigger(2);

                                //player selects continue
                                if (ctrl.shoot())
                                {
                                    //enable controls
                                    AnimatedSprite.controlsOn = true;
                                    interact++;

                                    //start pong game
                                    BustinOutGame.setState(8, 0);
                                }
                            }
                            else
                            {
                                //bowser tells player to get a quarter
                                trigger(1);

                                //player selects continue
                                if (ctrl.shoot())
                                {
                                    AnimatedSprite.controlsOn = true;
                                    interact++;
                                }

                                //Debug to set player as having a quarter
                                if (ctrl.jump())
                                {
                                    hasQuarter = true;
                                }
                            }
                        }

                        //remove wall by stairs if pong was won
                        if (wonPong == true)
                        {
                            board1.Tiles[10, 1].IsBlocked = false;
                            board1.Tiles[10, 1].Texture = null;
                            board1.Tiles[10, 2].IsBlocked = false;
                            board1.Tiles[10, 2].Texture = null;
                            board1.Tiles[10, 3].IsBlocked = false;
                            board1.Tiles[10, 3].Texture = null;
                            board1.Tiles[10, 4].IsBlocked = false;
                            board1.Tiles[10, 4].Texture = null;
                        }
                        //---------------------End Bowser-------------------------//

                        //draw all projectiles
                        for (int i = 0; i < LiveProjectiles.Count; i++)
                        {
                            spriteBatch.Draw(Bullet, LiveProjectiles[i].Position, null, Color.White, 0f, new Vector2(Bullet.Width / 2, Bullet.Height / 2), 1f, SpriteEffects.None, 0);
                        }

                        //draw all enemies
                        enemies.Draw(spriteBatch, bgNum, level);

                        //draw megaman
                        spriteBatch.Draw(megaman.Texture, megaman.Position, megaman.SourceRect, Color.White, 0f, megaman.Origin, 1.0f, SpriteEffects.None, 0);

                        break;
                    }
            }

            //adds the controls box to the menu screens
            if (mCurrentState == GameState.Title || mCurrentState == GameState.Hall || mCurrentState == GameState.MiniGames || mCurrentState == GameState.Options || mCurrentState == GameState.Controls 
                || mCurrentState == GameState.Sounds || mCurrentState == GameState.Pause)
            {
                spriteBatch.Draw(viewButtons, new Vector2(350, 775), Color.White);
            }
            spriteBatch.End();
        }

        private void DrawHud()
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f, titleSafeArea.Y);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            timeString = "" + timeRemaining.Minutes.ToString("00") + ":" + timeRemaining.Seconds.ToString("00");
            Color timeColor;
            if (timeRemaining > WarningTime ||
                (int)timeRemaining.TotalSeconds % 2 == 0)
            {
                timeColor = Color.White;
            }
            else
            {
                timeColor = Color.Red;
            }

            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        private void ManageState(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();

            //The switch statement will handle what screen the user sees
            switch (mCurrentState)
            {
                case GameState.Title:
                    {
                        titleScreen.Update(gameTime);

                        break;
                    }
                case GameState.Hall:
                    {
                        hallOfFame.Update(gameTime);

                        break;
                    }
                case GameState.Options:
                    {
                        optScn.Update(gameTime);

                        break;
                    }
                case GameState.Controls:
                    {
                        control.Update(gameTime);

                        break;
                    }
                case GameState.Sounds:
                    {
                        sound.Update(gameTime);

                        break;
                    }
                case GameState.Graphics:
                    {
                        grph.Update(gameTime, graphics);

                        break;
                    }
                case GameState.MiniGames:
                    {
                        mini.Update(gameTime);

                        break;
                    }
                case GameState.Pause:
                    {
                        pause.Update(gameTime);

                        break;
                    }
                case GameState.Difficulty:
                    {
                        difficult.Update(gameTime);

                        break;
                    }
                case GameState.Show:
                    {
                        show.Update(gameTime);

                        break;
                    }
                case GameState.Pong:
                    {
                        game.Update(gameTime);

                        //decrement remaining time
                        timeRemaining -= gameTime.ElapsedGameTime;

                        if (ctrl.Pause())
                        {
                            Pause.from = 1;
                            BustinOutGame.setState(7, 0);
                        }

                        break;
                    }
                case GameState.Frogger:
                    {
                        frog.Update(gameTime);

                        //decrement remaining time
                        timeRemaining -= gameTime.ElapsedGameTime;

                        if (ctrl.Pause())
                        {
                            Pause.from = 2;
                            BustinOutGame.setState(7, 0);
                        }

                        break;
                    }
                case GameState.Playing:
                    {
                        //decrement remaining time
                        timeRemaining -= gameTime.ElapsedGameTime;

                        if (AnimatedSprite.shooting == true)
                        {
                            Projectiles bullet = new Projectiles();

                            if (AnimatedSprite.direction == 1)
                            {
                                bullet.Position = new Vector2((megaman.Position.X + megaman.SourceRect.Width), (megaman.Position.Y + (megaman.SourceRect.Height / 2)));
                                bullet.Velocity = new Vector2(10, 0);
                                bullet.bound = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, Bullet.Width, Bullet.Height);
                            }
                            else
                            {
                                bullet.Position = new Vector2((megaman.Position.X), (megaman.Position.Y + (megaman.SourceRect.Height / 2)));
                                bullet.Velocity = new Vector2(-10, 0);
                                bullet.bound = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, Bullet.Width, Bullet.Height);
                            }
                            LiveProjectiles.Add(bullet);
                        }

                        foreach (Projectiles p in LiveProjectiles.ToArray())
                        {
                            p.Position += p.Velocity;

                            //check to see when the projectile leaves the visible part of the screen
                            if (p.Position.X < 0 || p.Position.X > graphics.GraphicsDevice.Viewport.Width || p.Position.Y < 0 || p.Position.Y > graphics.GraphicsDevice.Viewport.Height)
                            {
                                LiveProjectiles.RemoveAt(LiveProjectiles.IndexOf(p));
                            }

                            if (screenChange == true)
                            {
                                LiveProjectiles.Clear();
                                screenChange = false;
                            }
                        }

                        if (ctrl.Pause())
                        {
                            Pause.from = 0;
                            BustinOutGame.setState(7, 0);
                        }

                        enemies.Update(gameTime, bgNum, LiveProjectiles);

                        break;
                    }
            }
        }

        //prints debug info
        private void WriteDebugInformation()
        {
            string positionInText = string.Format("Position of megaman: ({0:0.0}, {1:0.0})", megaman.Position.X, megaman.Position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", megaman.Movement.X, megaman.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground? : {0}", megaman.IsOnFirmGround(source));
            string MMshooting = string.Format("Volume Level: {0}", MediaPlayer.Volume);
            string collider = string.Format("Position of collider: ({0:0.0}, {1:0.0})", megaman.SourceRect.X, megaman.SourceRect.Y);
            string isAliveText = string.Format("isAlive: {0}", megaman.IsAlive());
            DrawWithShadow(positionInText, new Vector2(10, 0));
            DrawWithShadow(movementInText, new Vector2(10, 20));
            DrawWithShadow(isOnFirmGroundText, new Vector2(10, 40));
            DrawWithShadow(collider, new Vector2(10, 80));
            DrawWithShadow(isAliveText, new Vector2(10, 100));
            DrawWithShadow(MMshooting, new Vector2(10, 60));
            DrawWithShadow("F5 for random board", new Vector2(10, 200));
            DrawWithShadow("Press +/- for volume", new Vector2(10, 220));
        }

        private void DrawWithShadow(string text, Vector2 position)
        {
            spriteBatch.DrawString(debugFont, text, position + Vector2.One, Color.Black);
            spriteBatch.DrawString(debugFont, text, position, Color.LightYellow);
        }

        //allows the screen change to change the background
        public static void BGChange(int i)
        {
            if (i == 0)
            {
                bgNum--;
            }
            else
            {
                bgNum++;
            }
        }

        //Gives the current background if called
        public static Texture2D getBG()
        {
            if (level == 1)
            {
                if (bgNum == 0)
                {
                    return prison1;
                }
                else if (bgNum == 1)
                {
                    return prison2;
                }
                else if (bgNum == 2)
                {
                    return prison3;
                }
                else
                {
                    return prisonBoss;
                }
            }
            else
            {
                if (bgNum == 0)
                {
                    return mb1;
                }
                else if (bgNum == 1)
                {
                    return mb2;
                }
                else if (bgNum == 2)
                {
                    return mb3;
                }
                else
                {
                    return mb4;
                }
            }
        }

        public static void setState(int state, int prevState)
        {
            if (prevState == 1)
            {
                //do nothing
            }
            else
            {
                MusicPlayer.SwitchSong(state, level);
            }

            if (state == 1)
            {
                mCurrentState = GameState.Title;
                currentGameState = 1;
            }
            else if (state == 2)
            {
                mCurrentState = GameState.Hall;
                currentGameState = 2;
            }
            else if (state == 3)
            {
                mCurrentState = GameState.Options;
                currentGameState = 3;
            }
            else if (state == 4)
            {
                mCurrentState = GameState.Playing;
                currentGameState = 4;
            }
            else if (state == 5)
            {
                mCurrentState = GameState.Sounds;
                currentGameState = 5;
            }
            else if (state == 6)
            {
                mCurrentState = GameState.Controls;
                currentGameState = 6;
            }
            else if (state == 7)
            {
                mCurrentState = GameState.Pause;
                currentGameState = 7;
            }
            else if (state == 8)
            {
                mCurrentState = GameState.Pong;
                currentGameState = 8;
            }
            else if (state == 9)
            {
                mCurrentState = GameState.Frogger;
                currentGameState = 9;
            }
            else if (state == 10)
            {
                mCurrentState = GameState.MiniGames;
                currentGameState = 10;
            }
            else if (state == 11)
            {
                mCurrentState = GameState.Graphics;
                currentGameState = 11;
            }
            else if(state == 12)
            {
                mCurrentState = GameState.Show;
                currentGameState = 12;
            }
            else
            {
                mCurrentState = GameState.Difficulty;
                currentGameState = 13;
            }
        }

        public static void setLevel(int lvl)
        {
            level = lvl;
        }

        //clears all of megaman's bullets
        public static void clearBullets()
        {
            foreach (Projectiles p in LiveProjectiles.ToArray())
            {
                LiveProjectiles.Clear();
            }
        }

        //plays the sounds
        public static void playArrowSound(int choice)
        {
            if (soundBool == true)
            {
                if (choice == 1)
                    arrowSoundInstance.Play();
                else if (choice == 2)
                    arrowSelectInstance.Play();
                else
                    arrowBackInstance.Play();
            }
        }

        private void trigger(int choice)
        {
            spriteBatch.Draw(pong, new Vector2(600, 425), Color.White);

            if (choice == 1)
            {
                if (interact == 0)
                {
                    AnimatedSprite.controlsOn = false;
                    spriteBatch.Draw(bowserText, new Vector2(525, 175), Color.White);
                    spriteBatch.Draw(bowser, new Vector2(800, 250), Color.White);
                }

            }
            if (choice == 2)
            {
                if(interact == 0)
                {
                    AnimatedSprite.controlsOn = false;
                    spriteBatch.Draw(bowserText2, new Vector2(525, 175), Color.White);
                    spriteBatch.Draw(bowser, new Vector2(800, 250), Color.White);
                }
            }
        }
    }
}