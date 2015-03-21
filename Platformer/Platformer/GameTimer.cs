using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace Platformer
{
    class GameTimer
    {
        public const int TIME_TO_COMPLETE = 45;

        public double timeLeft { get; set; }
        public double timeTaken { get; set; }

        public GameTimer()
        {
 
            timeLeft = TIME_TO_COMPLETE;
            timeTaken = 0;
        }

        public void resetTimer()
        {

            timeLeft = TIME_TO_COMPLETE;
            timeTaken = 0;
        }

        public void Update(GameTime gameTime)
        {

            timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
            timeTaken += gameTime.ElapsedGameTime.TotalSeconds;
            timeLeft = Math.Round(timeLeft, 2);
 
        }
    }
}
