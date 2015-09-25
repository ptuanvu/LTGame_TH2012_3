using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter02_CreativeMenu
{
    public class Menu : GameObject
    {
        private Vector2 position;
        private Vector2 size;


        private List<MenuItem> _menuItems;

        #region SetAndGet
        internal List<MenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }

            set
            {
                _menuItems = value;
            }
        }

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

        #endregion

        public Menu()
        {
            this._menuItems = new List<MenuItem>();
            this.position = new Vector2(0, 0);
            this.size = new Vector2(800, 600);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MenuItem menuItem in MenuItems)
            {
                menuItem.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach(MenuItem menuItem in _menuItems)
            {
                menuItem.Update(gameTime);
            }
        }

        public void Update(GameTime gameTime, MouseState mouse)
        {
            foreach (MenuItem menuItem in _menuItems)
            {
                menuItem.Update(gameTime, mouse);
            }
        }

        public override string ObjectName()
        {
            return "Menu";
        }
    }
}
