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
        private int colorInc;

        public Block(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
        }

        public override void BuildComponent(World world, Texture2D texture)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);
            //Body.SleepingAllowed = false;
            //Body.Position = Position;
            Body.BodyType = BodyType.Static;
            Body.Friction = 10f;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

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

            i = random.Next(1, 3);

            if (i == 1)
            {
                colorInc = random.Next(1, 4);
            }
            else
            {
                colorInc = random.Next(-3, 0);
            }
        }

        public override void Update()
        {
            int red = (int)Color.R, green = (int)Color.G, blue = (int)Color.B;

            if (red > 0 && red < 255)
            {
                if (red + colorInc >= 0)
                {
                    if (red + colorInc <= 255)
                    {
                        red += colorInc;
                    }
                    else
                    {
                        red = 255;
                    }
                }
                else
                {
                    red = 0;
                }
            }
            else if (green > 0 && green < 255)
            {
                if (green + colorInc >= 0)
                {
                    if (green + colorInc <= 255)
                    {
                        green += colorInc;
                    }
                    else
                    {
                        green = 255;
                    }
                }
                else
                {
                    green = 0;
                }
            }
            else if (blue > 0 && blue < 255)
            {
                if (blue + colorInc >= 0)
                {
                    if (blue + colorInc <= 255)
                    {
                        blue += colorInc;
                    }
                    else
                    {
                        blue = 255;
                    }
                }
                else
                {
                    blue = 0;
                }
            }
            else
            {
                int i;
                bool newTransitionChosen = false; ;

                while (!newTransitionChosen)
                {
                    i = random.Next(1,4);

                    if (i == 1)
                    {
                        if (red == 0 && (green == 0 || blue == 0))
                        {
                            newTransitionChosen = true;
                            colorInc = random.Next(1, 4);
                            red += colorInc;
                        }
                        else if (red == 255 && (green == 255 || blue == 255))
                        {
                            newTransitionChosen = true;
                            colorInc = random.Next(-3, 0);
                            red += colorInc;
                        }
                    }
                    else if (i == 2)
                    {
                        if (green == 0 && (red == 0 || blue == 0))
                        {
                            newTransitionChosen = true;
                            colorInc = random.Next(1, 4);
                            green += colorInc;
                        }
                        else if (green == 255 && (red == 255 || blue == 255))
                        {
                            newTransitionChosen = true;
                            colorInc = random.Next(-3, 0);
                            green += colorInc;
                        }
                    }
                    else if (i == 3)
                    {
                        if (blue == 0 && (green == 0 || red == 0))
                        {
                            newTransitionChosen = true;
                            colorInc = random.Next(1, 4);
                            blue += colorInc;
                        }
                        else if (blue == 255 && (green == 255 || red == 255))
                        {
                            newTransitionChosen = true;
                            colorInc = random.Next(-3, 0);
                            blue += colorInc;
                        }
                    }
                }
            }

            Color = new Color(red, green, blue);

            base.Update();
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
