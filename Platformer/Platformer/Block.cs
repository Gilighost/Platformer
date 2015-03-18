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
        private Color myColor;
        public Block(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));           
        }

        public override void BuildComponent(World world, Texture2D texture, Random random)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);
            //Body.SleepingAllowed = false;
            //Body.Position = Position;
            Body.BodyType = BodyType.Static;
            Body.Friction = 1f;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            int red = -1, green = -1, blue = -1;
            int i = random.Next(1, 25);
            if (i <= 9)
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
            if (i >= 17 || i == 1)
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

            myColor = new Color(red, green, blue);
        }

        public override void Update(VisualizationData visData)
        {
            //change component data based on music visualization input

            base.Update(visData);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                             ConvertUnits.ToDisplayUnits(Body.Position),
                             null,
                             myColor,
                             Body.Rotation,
                             Origin,
                             1f,
                             SpriteEffects.None,
                             0f);
            
            base.Draw(spriteBatch);
        }
    }
}
