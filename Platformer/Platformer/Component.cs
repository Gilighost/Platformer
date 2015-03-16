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
    abstract class Component
    {
        public Body Body { get; protected set; }

        public Texture2D Texture { get; protected set; }

        public Vector2 Position { get; protected set; }

        public Vector2 Origin { get; protected set; }

        public Color Color { get; protected set; }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                ConvertUnits.ToDisplayUnits(Position),
                null,
                Color,
                Body.Rotation,
                Origin,
                1f,
                SpriteEffects.None,
                0f);
        }

        public virtual void CreateComponent(World world, Texture2D texture)
        {
            Texture = texture;
 
            Color = Color.White;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Body = BodyFactory.CreateRectangle(world,
                 ConvertUnits.ToSimUnits(texture.Width),
                 ConvertUnits.ToSimUnits(texture.Height), 1f, Position);
        }

        public virtual void Update(VisualizationData visData)
        {

        }
    }
}
