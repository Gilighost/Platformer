using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

//Dictionary code from
//http://stackoverflow.com/questions/12914002/how-to-load-all-files-in-a-folder-with-xna


namespace Platformer 
{
    public static class LevelReader
    {

        public static Dictionary<int, char[][]> levelContent;
        
        public static void LoadLevelContent<T>(this ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + @"\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            levelContent = new Dictionary<int, char[][]>();

            FileInfo[] files = dir.GetFiles("*.*");

            int key = 1;

            foreach (FileInfo file in files)
            {
                int counter = 0;
                char[][] symbols = new char[File.ReadLines(file.FullName).Count()][];
                foreach (string line in File.ReadLines(file.FullName))
                {
                    symbols[counter] = new char[line.Length];
                    symbols[counter] = line.ToArray();
                    
                    counter++;
                }
                levelContent.Add(key, symbols);
                key++;
            }
        }
    }
}
