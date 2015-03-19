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
    class Player : Component
    {
        private KeyboardState oldKeyState;

        public Player(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
        }

        public override void BuildComponent(World world, Texture2D texture)
        {
            Texture = texture;

            Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(Texture.Height / 2), 1f, Position);
            //Body.SleepingAllowed = false;
            Body.BodyType = BodyType.Dynamic;

            Body.Friction = 10f;

            Body.OnCollision += Body_OnCollision;
            
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Color = Color.White;
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (true)
            {

            }
            
            return true;
        }
        public override void Update()
        {
            // get movement

            KeyboardState state = Keyboard.GetState();


            if (state.IsKeyDown(Keys.Left))
            {
                if (Body.LinearVelocity.X > -5)
                {
                    Body.ApplyForce(new Vector2(-20, 0));
                }   
            }
            else
            {
                if (Body.LinearVelocity.X < 0)
                {
                    //Body.ApplyForce(new Vector2(5, 0));
                }
            }

            if (state.IsKeyDown(Keys.Right))
            {
               if (Body.LinearVelocity.X < 5)
               {
                   Body.ApplyForce(new Vector2(20, 0));
               }
            }
            else
            {
                if (Body.LinearVelocity.X > 0)
                {
                   // Body.ApplyForce(new Vector2(-5, 0));
                }
            }

            if (state.IsKeyDown(Keys.Up))
            {
                if (oldKeyState.IsKeyUp(Keys.Up))
                {
                    Body.ApplyLinearImpulse(new Vector2(0, -3));
                }
            }

            oldKeyState = state;

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
