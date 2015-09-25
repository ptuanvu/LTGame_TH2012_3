using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter02_CreativeMenu
{
    public class MenuItem : GameObject
    {
        private Vector2 position;
        private Vector2 size;
        private Color color;
        private bool isClicked = false;
        private bool isMoveIn = false;
        private Texture2D buttonNormal;
        private Texture2D buttonClicked;
        private MouseState previousMouseState;

        //Click Handler
        public delegate void ClickHandler(object sender, MyMenuItemEventArgs e);
        public event ClickHandler Click;

        #region SetAndGet

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

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public Texture2D ButtonNormal
        {
            get
            {
                return buttonNormal;
            }

            set
            {
                buttonNormal = value;
            }
        }

        public Texture2D ButtonClicked
        {
            get
            {
                return buttonClicked;
            }

            set
            {
                buttonClicked = value;
            }
        }

        public bool IsClicked
        {
            get
            {
                return isClicked;
            }

            set
            {
                isClicked = value;
            }
        }

        public bool IsMoveIn
        {
            get
            {
                return isMoveIn;
            }

            set
            {
                isMoveIn = value;
            }
        }

        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isMoveIn)
            {
                spriteBatch.Draw(this.buttonClicked, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            } else
            {
                spriteBatch.Draw(this.ButtonNormal, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            } 
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public bool IsMouseMoveIn(MouseState mouse)
        {
            Rectangle objectRectange = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRectange = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (objectRectange.Intersects(mouseRectange))
            {
                return true;
            }
            return false;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public override string ObjectName()
        {
            return "MenuItem";
        }

        private int _down = 0;
        private bool zoom = true;

        public void Update(GameTime gameTime, MouseState mouse)
        {
            if (IsMouseMoveIn(mouse))
            {
                IsMoveIn = true;
            } else
            {
                IsMoveIn = false;
            }

            if (previousMouseState.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released && IsMouseMoveIn(previousMouseState))
            {
                if (Click != null)
                {
                    this.Click(this, new MyMenuItemEventArgs(mouse));
                }

                this.isClicked = true;
            }
            else
            {
                this.isClicked = false;
            }
            previousMouseState = mouse;
        }

        public MenuItem(Texture2D buttonNormal, Texture2D buttonClicked)
        {
            this.ButtonNormal = buttonNormal;
            this.buttonClicked = buttonClicked;
            this.position = new Vector2(0, 0);
            this.size = new Vector2(300, 100);
            this.color = Color.White;
        }
    }

    public class MyMenuItemEventArgs : EventArgs
    {
        public MyMenuItemEventArgs(object sender)
        {

        }
    }
}   