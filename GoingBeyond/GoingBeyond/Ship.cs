using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoingBeyond
{
    class Ship
    {
        public Model Model;
        public Matrix[] transform;

        //Model position
        public Vector3 Position = Vector3.Zero;

        public Vector3 Velocity = Vector3.Zero;

        public Matrix RotationMatrix = Matrix.Identity;
        private float rotation = 0.0f;

        //amplifies controller speed input
        private const float VelocityScale = 5.0f;

        public float Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }

                if (rotation != newVal)
                {
                    rotation = newVal;
                    RotationMatrix =
       Matrix.CreateRotationY(rotation);
                }
            }
        }

        public void Update(KeyboardState currentKeyState)
        {
            Vector3 modelVelocityAdd = Vector3.Zero;

            if (currentKeyState.IsKeyDown(Keys.A))
                Rotation += 0.10f;
            else if (currentKeyState.IsKeyDown(Keys.D))
                Rotation -= 0.10f;
            //Velocity += RotationMatrix.Forward * 1.0f;


            modelVelocityAdd.X = -(float)Math.Sin(Rotation);
            modelVelocityAdd.Z = -(float)Math.Cos(Rotation);

            if (currentKeyState.IsKeyDown(Keys.W))
            {

                modelVelocityAdd *= 5;
            }
            //Velocity += RotationMatrix. * 1.0f;
            Velocity += modelVelocityAdd;

        }

    }
}
