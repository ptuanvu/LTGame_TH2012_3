using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter2_CreativeMenu_Release
{
    public class Camera
    {
        public Matrix transform;
        public Viewport view;
        public Point location;
        Vector2 centre;
        MouseState preState;
        float scale = 1f;

        public Camera(Viewport newView)
        {
            view = newView;
            location = new Point(0, 0);
            centre = new Vector2(location.X, location.Y);
            scale = 1f;

            transform = Matrix.CreateScale(new Vector3(scale, scale, 0)) *
                    Matrix.CreateTranslation(new Vector3(centre.X, centre.Y, 0));
        }

        public void Update(GameTime gameTime, MouseState mouse, KeyboardState key)
        {

            if (mouse.ScrollWheelValue > preState.ScrollWheelValue || key.IsKeyDown(Keys.Up))
            {
                if (scale < 5f)
                    scale += 0.1f;
                preState = mouse;
            }
            else if (mouse.ScrollWheelValue < preState.ScrollWheelValue || key.IsKeyDown(Keys.Down))
            {
                if (scale > 0.1f)
                    scale -= 0.1f;
                preState = mouse;
            }


            if (preState.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
            {
                preState = mouse;
            }
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (preState.X != 0 && preState.Y != 0)
                {
                    location.X += (mouse.X - preState.X);
                    location.Y += (mouse.Y - preState.Y);
                }

                preState = mouse;

                //preState = new MouseState((int)(mouse.X + centre.X), (int)(mouse.Y + centre.Y), 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            }

            if (preState.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
            {
                //preState = mouse;
                preState = new MouseState((int)(mouse.X + centre.X), (int)(mouse.Y + centre.Y), mouse.ScrollWheelValue, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            }

            centre = new Vector2(location.X, location.Y);
            GLOBAL.centreCamera = centre;
            transform = Matrix.CreateScale(new Vector3(scale, scale, 0)) *
                    Matrix.CreateTranslation(new Vector3(centre.X, centre.Y, 0));

            GLOBAL.scale = scale;
        }


        public Vector2 WorldToScreen(Vector2 x)
        {
            return Vector2.Zero;
        }
    }
}