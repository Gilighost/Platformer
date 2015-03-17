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

            Body = BodyFactory.CreateCircle(world, Texture.Height / 2, 1f, Position);
            Body.Position = Position;
            Body.BodyType = BodyType.Dynamic;

            Body.Restitution = 0.3f;
            Body.Friction = 0.5f;
            
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Color = Color.White;
        }
        public override void Update(VisualizationData visData)
        {
            // get movement

            KeyboardState state = Keyboard.GetState();
            Body.Awake = true;

            if (state.IsKeyDown(Keys.Left))
                Body.ApplyTorque(-10);

            if (state.IsKeyDown(Keys.Right))
                Body.ApplyTorque(10);

            if (state.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up))
                Body.ApplyLinearImpulse(new Vector2(0, -10));

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
