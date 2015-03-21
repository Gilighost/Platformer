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
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

namespace Platformer
{
    class Enemy2 : Enemy
    {
        int direction;

       
        public Enemy2(Vector2 coordinates)
        {
            Position = ConvertUnits.ToSimUnits(new Vector2(coordinates.X * 64, coordinates.Y * 64));
        }

        public override void BuildComponent(World world, Texture2D texture1)
        {
            Texture = texture1;
            

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, Position);

            Body.UserData = "enemy";
            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = false;

            Body.Friction = 10f;

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            direction = -1;
            
            Color = Color.White;
        }

        public override void Update()
        {
            if (isAlive)
            {
                direction = random.Next(0, 2);
                if(direction == 0)
                {
                    direction = -1;
                }

                if (Body.LinearVelocity.X == 0 && Body.LinearVelocity.Y == 0)
                {
                    Body.ApplyLinearImpulse(new Vector2(1f * direction, -1.5f));
                    Body.ApplyTorque(10 * direction);
                }

            }
            base.Update();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
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
            }

            base.Draw(spriteBatch);
        }
    }
    
}
