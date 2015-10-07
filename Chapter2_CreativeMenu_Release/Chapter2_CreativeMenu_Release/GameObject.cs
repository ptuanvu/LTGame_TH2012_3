using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter2_CreativeMenu_Release
{
    public abstract class GameObject
    {
        public abstract void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract string ObjectName();
    }
}
