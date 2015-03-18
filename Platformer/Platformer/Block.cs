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
        private int timer, rInc, gInc, bInc;

        public Block(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
            timer = random.Next(0, 601);
        }

        public override void BuildComponent(World world, Texture2D texture)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);
            //Body.SleepingAllowed = false;
            //Body.Position = Position;
            Body.BodyType = BodyType.Static;
            Body.Friction = 1f;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            
            int red = random.Next(0, 256), green = random.Next(0, 256), blue = random.Next(0, 256);

            GenerateColor();
        }

        private void GenerateColor()
        {
            int red = -1, green = -1, blue = -1;

            int i = random.Next(1, 25);

            if(i <= 9)
            {
                red = 255;
            }
            else if (i >= 13 && i <= 21)
            {
                red = 0;
            }
            
            if (i >= 9 && i <= 17)
            {
                green = 255;
            }
            else if (i <= 5 || i >= 21)
            {
                green = 0;
            }
            if (i >=17 || i == 1)
            {
                blue = 255;
            }
            else if (i >= 5 && i <= 13)
            {
                blue = 0;
            }

            i = random.Next(0, 4);

            if (red == -1)
            {
                red = i * 64;
            }
            else if (green == -1)
            {
                green = i * 64;
            }
            else if (blue == -1)
            {
                blue = i * 64;
            }

            Color = new Color(red, green, blue);

        }

        public override void Update(VisualizationData visData)
        {
            //change component data based on music visualization input
            //GenerateColor(); //WARNING EPILEPSY

            if (timer > 0)
            {
                timer--;
            }
            else
            {

                int red = (int)Color.R, green = (int)Color.G, blue = (int)Color.B;

                if (red <= 0)
                {
                    red = 0;
                    rInc = random.Next(1,4);
                }
                else if (red >= 255)
                {
                    red = 255;
                    rInc = random.Next(-3, 0);
                }

                if (green <= 0)
                {
                    green = 0;
                    gInc = random.Next(1, 4);
                }
                else if (green >= 255)
                {
                    green = 255;
                    gInc = random.Next(-3, 0);
                }

                if (blue <= 0)
                {
                    blue = 0;
                    bInc = random.Next(1, 4);
                }
                else if (blue >= 255)
                {
                    blue = 255;
                    bInc = random.Next(-3, 0);
                }

                red += rInc;
                green += gInc;
                blue += bInc;

                Color = new Color(red, green, blue);
            }
            base.Update(visData);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                             ConvertUnits.ToDisplayUnits(Body.Position),
                             null,
                             Color,
                             Body.Rotation,
                             Origin,
                             1f,
                             SpriteEffects.None,
                             0f);
            
            base.Draw(spriteBatch);
        }
    }
}
