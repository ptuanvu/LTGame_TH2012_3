using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoingBeyond4
{
    class Ship
    {
        public Model Model;
        public Matrix[] transform;

        //Model position
        public Vector3 Position = Vector3.Zero;

        public Vector3 Velocity = Vector3.Zero;

        public Matrix RotationMatrix = Matrix.Identity;
        private float rotation;

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
                    RotationMatrix = Matrix.CreateRotationY(rotation);
                }
            }
        }

        public void Update(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                rotation -= 0.1f;
            } else if (keyboard.IsKeyDown(Keys.D))
            {
                rotation += 0.1f;
            }

            Velocity += RotationMatrix.Forward * 1.0f;
        }

    }
}
