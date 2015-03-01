using FarseerPhysics;
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

    class Player
    {
        //Constants
        //*******************************************************
        private const String playerSpriteName = @"Images/record_98px";
        //*******************************************************

        private Body circleBody;
        private Texture2D circleSprite;
        private Vector2 circleOrigin;
        private Vector2 circlePosition;


        //movement
        private KeyboardState oldKeyState;
        

        public Player(World world, ContentManager Content, Vector2 position)
        {
            circlePosition = position;


            circleSprite = Content.Load<Texture2D>(playerSpriteName);
            circleOrigin = new Vector2(circleSprite.Width / 2f, circleSprite.Height / 2f);
            circleBody = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(96 / 2f), 1f, circlePosition);
            circleBody.BodyType = BodyType.Dynamic;

            circleBody.Restitution = 0.3f;
            circleBody.Friction = 0.5f;
        }

        public void getPlayerMovement()
        {
            KeyboardState state = Keyboard.GetState();
            circleBody.Awake = true;

            if (state.IsKeyDown(Keys.Left))
                circleBody.ApplyTorque(-5);

            if (state.IsKeyDown(Keys.Right))
                circleBody.ApplyTorque(5);

            if (state.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up))
                circleBody.ApplyLinearImpulse(new Vector2(0, -5));

            oldKeyState = state;

        }

        public void LoadContent(ContentManager Content, World world)
        {

           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(circleSprite, ConvertUnits.ToDisplayUnits(circleBody.Position), null, Color.White, circleBody.Rotation, circleOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
