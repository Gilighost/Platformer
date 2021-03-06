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
        

         public Goal(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
        }

        public override void BuildComponent(World world, Texture2D texture)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);
            Body.UserData = "goal";
            Body.BodyType = BodyType.Static;
            Body.Friction = 0.5f;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Color = Color.White;

            Body.IsSensor = true;
        }

        public override void Update()
        {
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
