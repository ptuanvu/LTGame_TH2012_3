using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter2_CreativeMenu_Release
{
    internal class MenuItem : GameObject
    {
        private Vector2 position;
        private Vector2 size;
        private Color color;
        private Texture2D buttonNormal;
        private Texture2D buttonClicked;
        private MouseState previousMouseState;
        private bool isMoveIn = false;
        private bool isClicked = false;

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

        public MouseState PreviousMouseState
        {
            get
            {
                return previousMouseState;
            }

            set
            {
                previousMouseState = value;
            }
        }

        #endregion
        //Click Handler
        public delegate void ClickHandler(object sender, MyMenuItemEventArgs e);
        public event ClickHandler Click;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isMoveIn)
            {
                spriteBatch.Draw(this.buttonClicked, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            }
            else
            {
                spriteBatch.Draw(this.ButtonNormal, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
            }
        }

        public override string ObjectName()
        {
            return "MenuItem";
        }

        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            

            if (IsMouseMoveIn(mouse))
            {
                isMoveIn = true;
            }
            else
            {
                isMoveIn = false;
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

        public bool IsMouseMoveIn(MouseState mouse)
        {
            Rectangle objectRectange = new Rectangle((int)(position.X + GLOBAL.centreCamera.X), (int)(position.Y + GLOBAL.centreCamera.Y), (int)size.X, (int)size.Y);
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

        public MenuItem(Texture2D buttonNormal, Texture2D buttonClicked)
        {
            this.ButtonNormal = buttonNormal;
            this.ButtonClicked = buttonClicked;
            this.Position = new Vector2(0, 0);
            this.Size = new Vector2(300, 100);
            this.Color = Color.White;
        }

    }

    public class MyMenuItemEventArgs : EventArgs
    {
        public MyMenuItemEventArgs(object sender)
        {

        }
    }
}