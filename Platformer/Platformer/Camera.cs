using System;
using System.IO;
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
    class Camera
    {
        // Distance away from the tracking body
        private float offsetX;

        // Body to center the camera on
        private Body trackingBody;

        public Matrix TransformationMatrix { get; private set; }

        // X value of the target
        public float CenterPointTarget { get; set; }

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

                // If tracking body is not located in the center
                // of the view (half screen width + current offset)
                if (ConvertUnits.ToDisplayUnits(trackingBody.Position.X) !=
                    halfScreenWidth + offsetX)
                {
                    offsetX = MathHelper.Clamp(
                        ConvertUnits.ToDisplayUnits(trackingBody.Position.X) -
                        halfScreenWidth, 0, halfScreenWidth);
                }
            }

            // Move scene
            TransformationMatrix = Matrix.CreateTranslation(-offsetX, 0, 0);
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
