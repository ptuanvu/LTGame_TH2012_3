using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter02_CreativeMenu
{
    public static class Textures
    {
        public static List<Texture2D> bullets;
        public static void AddBullet(List<Texture2D> bullet)
        {
            bullets = bullet;
        }
    }
}
