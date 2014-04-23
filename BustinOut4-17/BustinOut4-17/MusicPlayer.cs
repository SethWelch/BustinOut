using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BustinOutMegaMan
{
    class MusicPlayer 
    {
        private static Song title, prison, mario, song;
        private static float volume = .15f;

        public static void LoadContent(ContentManager Content)
        {
            title = Content.Load<Song>("Music/Title");
            prison = Content.Load<Song>("Music/Prison");
            mario = Content.Load<Song>("Music/Mario");
            MusicPlayer.Volume(volume);
            MediaPlayer.Play(title);
            MediaPlayer.IsRepeating = true;
        }

        public static void SwitchSong(int state, int level)
        {
            if (state == 1 || state == 2 || state == 3 || state == 5 || state == 6)
            {
                song = title;
            }
            else if (state == 4 && level == 1)
            {
                song = prison;
            }
            else if (state == 4 && level == 2)
            {
                song = mario;
            }
            else
            {
                song = null;
            }

            MediaPlayer.Stop();
            Music();
        }

        public static void Music()
        {
            if (song == null)
            {

            }
            else
            {
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
            }
        }

        public static void Pause()
        {
            MediaPlayer.Pause();
        }

        public static void Resume()
        {
            MediaPlayer.Resume();
        }

        //stop the music
        public static void Stop()
        {
            MediaPlayer.Stop();
        }

        //set the volume
        public static void Volume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        //turn up the volume
        public static void VolumeUp()
        {
            MediaPlayer.Volume += .01f;
        }

        //turn down the volume
        public static void VolumeDown()
        {
            MediaPlayer.Volume -= .01f;
        }

        //volume off setting
        public static void VolumeOff()
        {
            MediaPlayer.Volume = 0f;
        }

        //low volume setting
        public static void VolumeLow()
        {
            MediaPlayer.Volume = .15f;
        }

        //max volume setting
        public static void VolumeMax()
        {
            MediaPlayer.Volume = .3f;
        }
    }
}
