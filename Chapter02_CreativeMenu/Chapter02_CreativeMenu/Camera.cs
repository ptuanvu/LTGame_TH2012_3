using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter02_CreativeMenu
{
    public class Camera
    {
        public Matrix transform;
        public Viewport view;
        Vector2 centre;

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gameTime, Ship ship1)
        {
            centre = new Vector2(ship1.Position.X + (ship1.Size.X / 2) - 400, ship1.Position.Y + (ship1.Size.Y / 2) - 400);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }
    }
}
