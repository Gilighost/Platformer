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
using FarseerPhysics.Factories;

namespace Platformer
{
    class Block : Component
    {
        public Block(Vector2 position)
        {
            Position = position;

            Random r = new Random();

            float red = 0, green = 0, blue = 0;
            int i = r.Next(1, 4);
            int j = i;
            while (j == i)
            {
                j = r.Next(1, 4);
            }

            if (i == 1)
            {
                red = 255;
            }
            else if (i == 2)
            {
                green = 255;
            }
            else
            {
                blue = 255;
            }

            if (j == 1)
            {
                red = r.Next(0, 256);
            }
            else if (j == 2)
            {
                green = r.Next(0, 256);
            }
            else
            {
                blue = r.Next(0, 256);
            }

            this.Color = new Color(red, green, blue);
        }
        public override void Update(VisualizationData visData)
        {
            //change component data based on music visualization input

            base.Update(visData);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
