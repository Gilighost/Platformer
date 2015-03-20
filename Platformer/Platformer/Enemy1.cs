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
    class Enemy1 : Enemy
    {
        int direction;
        int speed;

        public Enemy1(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
        }

        public override void BuildComponent(World world, Texture2D texture)
        {
            Texture = texture;

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);
            Body.BodyType = BodyType.Dynamic;

            Body.IgnoreGravity = true;

            Body.Friction = 0f;

            Body.OnCollision += changeDirection;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            direction = -1;
            speed = random.Next(1, 2);
            Body.ApplyLinearImpulse(new Vector2(speed * direction, 0));
            Color = Color.White;
        }

        private bool changeDirection(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {

            direction *= -1;
            Body.ApplyLinearImpulse(new Vector2(speed * direction, 0));
            
            return true;
        }

        public override void Update()
        {
            //Body.Position = new Vector2(Body.Position.X + ConvertUnits.ToSimUnits(speed * direction), Body.Position.Y);

            

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
