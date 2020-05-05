using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace Akira
{
    class Musica
    {
        public static void Play(Song gameplayMusic)

        {
            try
            {
                MediaPlayer.Play(gameplayMusic);
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }
        }
    }
