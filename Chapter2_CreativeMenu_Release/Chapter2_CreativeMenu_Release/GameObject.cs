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
        private int _depth = 0;
        private int _gameState;

        public int Depth
        {
            get
            {
                return _depth;
            }

            set
            {
                _depth = value;
            }
        }

        public int GameState
        {
            get
            {
                return _gameState;
            }

            set
            {
                _gameState = value;
            }
        }

        public abstract void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract string ObjectName();
    }
}
