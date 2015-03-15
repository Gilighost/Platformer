﻿using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    public class Block : Component
    {
        public Block(World world, Texture2D texture, Vector2 position)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(texture.Width),
                ConvertUnits.ToSimUnits(texture.Height), 1f, position);

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                Color.White,
                Body.Rotation,
                Origin,
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}
