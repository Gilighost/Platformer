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

//Code from
//http://stackoverflow.com/questions/12914002/how-to-load-all-files-in-a-folder-with-xna


namespace Platformer 
{
    public static class LevelReader
    {
        public static Dictionary<int, String> LoadListContent<T>(this ContentManager contentManager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + @"\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            Dictionary<int, String> result = new Dictionary<int, String>();

            FileInfo[] files = dir.GetFiles("*.*");
            int i = 1;
            foreach (FileInfo file in files)
            {
                result.Add(i, Path.GetFileNameWithoutExtension(file.Name));
                i++;
            }
            
            return result;
        }
    }
}
