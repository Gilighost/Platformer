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

        public static List<Component> ReadInLevelComponents(World world, int levelKey)
        {
            List<Component> levelComponents = new List<Component>();
            for (int i = 0; i < LevelReader.levelContent[levelKey].Length; i++) //lines
            {
                for (int j = 0; j < LevelReader.levelContent[levelKey][i].Length; j++) //characters
                {

                    if (LevelReader.levelContent[levelKey][i][j] == 'P')
                    {
                        Player player = new Player(world, Game1.playerTexture, new Vector2(j, i));
                        levelComponents.Add(player);
                    }

                    if (LevelReader.levelContent[levelKey][i][j] == '#')
                    {
                        Component block = new Block(world, Game1.blockTexture, new Vector2(j,i));
                        levelComponents.Add(block);
                    }
                }
            }
            return levelComponents;
        }
    }
}
