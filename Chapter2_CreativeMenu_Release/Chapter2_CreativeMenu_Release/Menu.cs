using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter2_CreativeMenu_Release
{
    public class Menu : GameObject
    {
        private Vector2 position;
        private Vector2 size;
        private List<MenuItem> menuItems;

        #region Setget
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        internal List<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }

            set
            {
                menuItems = value;
            }
        }

        #endregion

        public Menu()
        {
            this.menuItems = new List<MenuItem>();
            this.position = new Vector2(0, 0);
            this.size = new Vector2(800, 600);
            this.GameState = 0;
        }

        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            foreach (MenuItem menuItem in menuItems) menuItem.Update(gameTime, mouse, keyboard);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MenuItem menuItem in menuItems) menuItem.Draw(gameTime, spriteBatch);
        }

        public override string ObjectName()
        {
            return "Menu";
        }
    }
}
