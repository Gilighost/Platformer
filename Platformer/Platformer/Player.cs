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

        private bool airborne;
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

            Body.Restitution = 0f;
            Body.Friction = 1f;
            
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Color = Color.White;
        }
        public override void Update(VisualizationData visData)
        {
            // get movement

            KeyboardState state = Keyboard.GetState();

            if (Body.LinearVelocity.Y != 0)
            {
                airborne = true;
            }
            else
            {
                airborne = false;
            }

            if (state.IsKeyDown(Keys.Left) && Body.AngularVelocity > -20)
            {
                //Body.ApplyTorque(-5);
                if (Body.LinearVelocity.X > 0)
                {
                    Body.ApplyForce(new Vector2(-3, 0));
                }

                if (airborne)
                {
                    Body.ApplyForce(new Vector2(-2, 0));
                }
                else
                {
                    Body.ApplyForce(new Vector2(-4, 0));
                }
            }

            if (state.IsKeyDown(Keys.Right) && Body.AngularVelocity < 20)
            {
                //Body.ApplyTorque(5);

                if (Body.LinearVelocity.X < 0)
                {
                    Body.ApplyForce(new Vector2(3, 0));
                }

                if (airborne)
                {
                    Body.ApplyForce(new Vector2(2, 0));
                }
                else
                {
                    Body.ApplyForce(new Vector2(4, 0));
                }

            }

            if (state.IsKeyDown(Keys.Up) && !airborne)
            {
                Body.ApplyLinearImpulse(new Vector2(0, -4));
            }

            //if (airborne && state.IsKeyDown(Keys.Left))
            //{
            //    Body.ApplyForce(new Vector2(-3, 0));
            //}
            //else if (airborne && state.IsKeyDown(Keys.Right))
            //{
            //    Body.ApplyForce(new Vector2(3, 0));
            //}

            oldKeyState = state;

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
