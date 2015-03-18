﻿using System;
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
    class Goal : Component
    {
        private Color myColor;

         public Goal(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
        }

        public override void BuildComponent(World world, Texture2D texture, Random random)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);
            //Body.SleepingAllowed = false;
            Body.BodyType = BodyType.Static;
            Body.Friction = 0.5f;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            myColor = Color.White;

            Body.IsSensor = true;
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
