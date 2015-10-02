using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter02_CreativeMenu
{
    public class Bullet : GameObject
    {
        private static int SHOOTING1 = 0;
        private static int SHOOTING2 = 1;
        private static int SHOOTING3 = 2;
        private static int FLYING = 3;
        private static int OVER1 = 4;
        private static int OVER2 = 5;
        private static int OVER3 = 6;
        private GameTime timeStart;
        private int mili = 0;
        private int second = 0;
        private List<Texture2D> bullets = new List<Texture2D>();
        private Ship ship;
        private int state = 0;

        private Vector2 position;
        private Vector2 size;
        private Color color;
        //Advanced movement
        private Vector2 spriteOrigin;
        private float rotation = 0.0f;
        private int degrees = 0;
        private Vector2 spriteVelocity;

        //Bullet Change
        

        private const float tangentialVelocity = 10f;
        private float friction = 0.1f;

        public override string ObjectName()
        {
            return "Bullet";
        }

        public override void Update(GameTime gameTime)
        {
            if (state >= 6)
            {
                return;
            }

            if (gameTime.TotalGameTime.Seconds - second > 1 && state < 6)
            {
                state++;
            }
            
            UpdateBulletShape(this.state);

            position = spriteVelocity + position;

            spriteVelocity.X = (float)Math.Sin(rotation) * tangentialVelocity;
            spriteVelocity.Y = (float)Math.Cos(rotation) * tangentialVelocity;

            if (spriteVelocity != Vector2.Zero)
            {
                float i = spriteVelocity.X;
                float j = -spriteVelocity.Y;

                spriteVelocity.X = i -= friction * i;
                spriteVelocity.Y = j -= friction * j;
            }
        }

        private void UpdateBulletShape(int state)
        {
            if (state < 4)
                state++;
        }


        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (state > 7)
            {
                state = 6;
            }
            spriteBatch.Draw(bullets[state], new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), null, color, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }

        private Vector2 SyncBullet(Vector2 p)
        {
            Vector2 spriteVelocity2;
            float tangentialVelocity2 = 19f;

            spriteVelocity2.Y = (float)Math.Sin(rotation) * tangentialVelocity2;
            spriteVelocity2.X = (float)Math.Cos(rotation) * tangentialVelocity2;

            p = spriteVelocity2 + p;
            return p;
        }

        public Bullet(Vector2 position, float p, Color color, GameTime gameTime)
        {
            // TODO: Complete member initialization
            this.bullets = Textures.bullets;
            this.rotation = p;
            this.color = color;
            this.size.X = 15;
            this.size.Y = 35;
            this.position = SyncBullet(position);
            second = gameTime.TotalGameTime.Seconds;
            mili = gameTime.TotalGameTime.Milliseconds;
        }

        public bool IsPosibleToAddNewBullet(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Math.Abs(gameTime.TotalGameTime.Seconds - second) > 0 || (Math.Abs(gameTime.TotalGameTime.Seconds - second) == 0 && Math.Abs(gameTime.TotalGameTime.Milliseconds - mili) > 500))
            {
                return true;
            }

            return false;
        }
    }
}
