using System;

namespace BustinOutMegaMan
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static BustinOutGame game = new BustinOutGame();

        public static BustinOutGame BustinOutGame
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        static void Main(string[] args)
        {
                //Locks framerate
                game.IsFixedTimeStep = true;

                game.Run();
        }

        public static void gameExit()
        {
                game.Exit();
        }
    }
#endif
}

