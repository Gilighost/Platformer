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

namespace Platformer
{
    class Camera
    {
        // Distance away from the tracking body
        private Vector2 offset;

        // Body to center the camera on
        private Body trackingBody;

        public Matrix TransformationMatrix { get; private set; }

        // Create a singleton
        public static readonly Camera Current = new Camera();

        private Camera()
        {
            // Private so that it can't be instantiated outside
            // the class
        }

        public void Update()
        {
            if (trackingBody != null)
            {
                float halfScreenWidth = Game1.HalfScreenWidth;
                float halfScreenHeight = Game1.HalfScreenHeight;

                // If tracking body is not located in the center
                // of the view (half screen width + current offset)
                if (ConvertUnits.ToDisplayUnits(trackingBody.Position.X) !=
                    halfScreenWidth)
                {
                    if (ConvertUnits.ToDisplayUnits(trackingBody.Position.X) > halfScreenWidth)
                    {
                        offset.X = ConvertUnits.ToDisplayUnits(trackingBody.Position.X) - halfScreenWidth;
                    }
                    else
                    {
                        offset.X = halfScreenWidth - ConvertUnits.ToDisplayUnits(trackingBody.Position.X);
                    }

                }
                if (ConvertUnits.ToDisplayUnits(trackingBody.Position.Y) !=
                    halfScreenHeight)
                {
                    if (ConvertUnits.ToDisplayUnits(trackingBody.Position.Y) > halfScreenHeight)
                    {
                        offset.Y = ConvertUnits.ToDisplayUnits(trackingBody.Position.Y) - halfScreenHeight;
                    }
                    else
                    {
                        offset.Y = halfScreenHeight - ConvertUnits.ToDisplayUnits(trackingBody.Position.Y);
                    }

                }
            }

            // Move scene
            TransformationMatrix = Matrix.CreateTranslation(-offset.X, -offset.Y, 0);
        }

        public void StartTracking(Body body)
        {
            trackingBody = body;
        }

        public void StopTracking()
        {
            trackingBody = null;
        }
    }
}
