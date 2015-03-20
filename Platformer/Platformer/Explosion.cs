using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Explosion
    {
        private bool isActive;
        private Vector2 position;
        private float rotation;
        private Texture2D texture;

        // Goes from 0 (start explosion) to 1 (end)
        private float transitionValue;

        // Length of the explosion 
        private readonly TimeSpan timeToLive = TimeSpan.FromSeconds(0.7);

        public Explosion(Texture2D texture)
        {
            isActive = false;
            this.texture = texture;
        }

        /// <summary>
        /// Start an explosion
        /// </summary>
        /// <param name="body">exploded body</param>
        public void Activate(Body body)
        {
            position = ConvertUnits.ToDisplayUnits(body.Position);
            rotation = body.Rotation;
            transitionValue = 0f;
            isActive = true;
        }

        private void UpdateTransition(GameTime elapsedGameTime)
        {
            // Scale transition based on how much time has elapsed
            float transitionDelta = (float)(elapsedGameTime.ElapsedGameTime.TotalMilliseconds /
                                            timeToLive.TotalMilliseconds);
            transitionValue += transitionDelta;

            // Did we reach the end of the transition?
            if (transitionValue >= 1)
            {
                transitionValue = 1f;
                isActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime elapsedGameTime)
        {
            if (isActive)
            {
                UpdateTransition(elapsedGameTime);

                // Progressively decrease color and alpha
                int colorChannel = 255 - (int)(transitionValue * 255);
                int alphaCannel = 160 - (int)(transitionValue * 160);
                Color color = new Color(colorChannel, colorChannel, colorChannel, alphaCannel);

                // Make explosion get larger
                float scale = 0.75f + transitionValue;

                spriteBatch.Draw(texture, position, null, color,
                    rotation,
                    new Vector2(texture.Width / 2, texture.Height / 2),  // origin
                    scale,
                    SpriteEffects.None, 0);
            }
        }
    }
}

