using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter02_CreativeMenu
{
    class Ship : GameObject
    {
        private Vector2 position;
        private Vector2 size;
        private Color color;
        private bool isClicked = false;
        private bool isMoveIn = false;
        private Texture2D ship2D;
        private MouseState previousMouseState;

        //Advanced movement
        private Vector2 spriteOrigin;
        private float rotation = 0.0f;
        private int degrees = 0;
        private Vector2 spriteVelocity;

        private const float tangentialVelocity = 5f;
        private float friction = 0.1f;

        //Click Handler
        public delegate void ClickHandler(object sender, MyMenuItemEventArgs e);
        public event ClickHandler Click;

        //KeyBoard Handler
        private KeyboardState previousKeyBoardState;
        private int KeyBoardTimeHold = 0;

        //Bullets
        private List<Bullet> bullets;
        private const int BULLET_SPACES = 2;

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

        public Texture2D Ship2D
        {
            get
            {
                return ship2D;
            }

            set
            {
                ship2D = value;
            }
        }

        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets) bullet.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(ship2D, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, rotation, Vector2.Zero, SpriteEffects.None, 0);
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
            return "GameObject";
        }

        private int _down = 0;
        private bool zoom = true;


        public Ship(Texture2D ship)
        {
            this.ship2D = ship;
            this.position = new Vector2(0, 0);
            this.size = new Vector2(50, 100);
            this.color = Color.White;
            this.bullets = new List<Bullet>();
        }

        public void Update(GameTime gameTime, MouseState mouse)
        {
            if (IsMouseMoveIn(mouse))
            {
                IsMoveIn = true;
            }
            else
            {
                IsMoveIn = false;
            }

            if (previousMouseState.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released && IsMouseMoveIn(previousMouseState))
            {
                //if (Click != null)
                //{
                //    this.Click(this, new MyMenuItemEventArgs(mouse));
                //}

                this.isClicked = true;
            }
            else
            {
                this.isClicked = false;
            }
            previousMouseState = mouse;
        }

        internal void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            foreach (Bullet bullet in bullets) bullet.Update(gameTime);
            
            position = spriteVelocity + position;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                spriteVelocity.X = (float)Math.Sin(rotation) * tangentialVelocity;
                spriteVelocity.Y = -(float)Math.Cos(rotation) * tangentialVelocity;
            }
            else if (spriteVelocity != Vector2.Zero)
            {
                float i = spriteVelocity.X;
                float j = spriteVelocity.Y;

                spriteVelocity.X = i -= friction * i;
                spriteVelocity.Y = j -= friction * j;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                spriteVelocity.X = (float)Math.Sin(rotation) * tangentialVelocity;
                spriteVelocity.Y = (float)Math.Cos(rotation) * tangentialVelocity;
            }
            else if (spriteVelocity != Vector2.Zero)
            {
                float i = spriteVelocity.X;
                float j = spriteVelocity.Y;

                spriteVelocity.X = i -= friction * i;
                spriteVelocity.Y = j -= friction * j;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
               
                Bullet bullet = new Bullet(position, this.rotation, this.color, gameTime);

                if (bullets.Count > 0)
                {
                    if (bullets[bullets.Count - 1].IsPosibleToAddNewBullet(gameTime))
                    {
                        bullets.Add(bullet);
                    }
                    //bullets.Add(bullet);
                }
                else bullets.Add(bullet);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                rotation += 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                rotation -= 0.1f;
            }


        }
    }
}
