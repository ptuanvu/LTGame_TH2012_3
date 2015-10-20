using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter2_CreativeMenu_Release
{
    public static class GLOBAL
    {
        public static ContentManager Content;
        public static int curDepth = 0;
        public static Vector2 centreCamera;
        public static float scale;
        public static MapCell selected;
        public static int curSolid = 0;

        public static Rectangle ScreenToWorld(Rectangle sour)
        {
            Rectangle result = new Rectangle();
            result.X = (int)((sour.X - (int)centreCamera.X) / scale);
            result.Y = (int)((sour.Y - (int)centreCamera.Y) / scale);
            result.Width = (int)(sour.Width * scale);
            result.Height = (int)(sour.Height * scale);
            return result;
        }

        public static bool IsSelected(MouseState e, Rectangle cellPosition)
        {
            if (e.X > cellPosition.X && e.Y > cellPosition.Y && e.X < cellPosition.X + cellPosition.Width && e.Y < cellPosition.Y + cellPosition.Height)
            {
                return true;
            }

            return false;
        }
    }
}