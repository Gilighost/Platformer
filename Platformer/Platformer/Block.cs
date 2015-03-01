using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class Block
    {
        private Texture2D sprite;
        private Body body;
        private Vector2 origin;

        public Texture2D Sprite { get { return sprite; } }
        public Vector2 Origin { get { return origin; } }
        public float Rotation
        {
            get
            {
                return body.Rotation;
            }
        }

        public Block(World world, Texture2D texture, Vector2 position)
        {
            sprite = texture;
            origin = position;
            body = BodyFactory.CreateRectangle(
                world, 
                ConvertUnits.ToSimUnits(32f), 
                ConvertUnits.ToSimUnits(32f), 
                1f, 
                origin);
        }
    }
}
