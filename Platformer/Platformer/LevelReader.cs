using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Platformer
{
    public class LevelReader : Game1
    {

        StreamReader reader;
        List<String> textFileLevelNames;



        public void getLevelFromFile()
        {
            reader = new StreamReader(@"Levels\level1.txt");
        }
    }
}
