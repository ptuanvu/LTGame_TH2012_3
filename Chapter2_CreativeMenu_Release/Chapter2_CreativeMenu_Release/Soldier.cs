using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Chapter2_CreativeMenu_Release
{
    public static class Direction
    {
        public static int UP = 0;
        public static int DOWN = 1;
        public static int LEFT = 2;
        public static int RIGHT = 3;
    }
    public class Soldier : GameObject
    {
        public static List<Texture2D> Characters;
        public static List<Rectangle> up, down, left, right;
        public static int width;
        public static int height;
        public static Rectangle desRec;
        Rectangle desRectangle;

        MapCell position;
        Point curPos;
        int curSprite = 0;
        int milisecond = 0;
        int second = 0;
        bool selected = false;
        MouseState preMouse;
        int curDirection;
        Random rand = new Random();

        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
            }
        }

        public void ChangeSprite()
        {
            if (GLOBAL.curSolid == 0)
                GLOBAL.curSolid = 1;
            else
                GLOBAL.curSolid = 0;

            width = Characters[GLOBAL.curSolid].Width / 4;
            height = Characters[GLOBAL.curSolid].Height / 4;
            float scaletemp = (float)width / 32;
            desRec = new Rectangle(0, 0, 32, (int)(height / scaletemp) + 1);
            if (true)
            {
                Rectangle step1 = new Rectangle(0, 0, width, height);
                Rectangle step2 = new Rectangle(width, 0, width, height);
                Rectangle step3 = new Rectangle(width * 2, 0, width, height);
                Rectangle step4 = new Rectangle(width * 3, 0, width, height);
                down = new List<Rectangle>();
                down.Add(step1);
                down.Add(step2);
                down.Add(step3);
                down.Add(step4);
            }
            if (true)
            {
                Rectangle step1 = new Rectangle(0, height, width, height);
                Rectangle step2 = new Rectangle(width, height, width, height);
                Rectangle step3 = new Rectangle(width * 2, height, width, height);
                Rectangle step4 = new Rectangle(width * 3, height, width, height);
                left = new List<Rectangle>();
                left.Add(step1);
                left.Add(step2);
                left.Add(step3);
                left.Add(step4);
            }
            if (true)
            {
                Rectangle step1 = new Rectangle(0, 2 * height, width, height);
                Rectangle step2 = new Rectangle(width, 2 * height, width, height);
                Rectangle step3 = new Rectangle(width * 2, 2 * height, width, height);
                Rectangle step4 = new Rectangle(width * 3, 2 * height, width, height);
                right = new List<Rectangle>();
                right.Add(step1);
                right.Add(step2);
                right.Add(step3);
                right.Add(step4);
            }
            if (true)
            {
                Rectangle step1 = new Rectangle(0, 3 * height, width, height);
                Rectangle step2 = new Rectangle(width, 3 * height, width, height);
                Rectangle step3 = new Rectangle(width * 2, 3 * height, width, height);
                Rectangle step4 = new Rectangle(width * 3, 3 * height, width, height);
                up = new List<Rectangle>();
                up.Add(step1);
                up.Add(step2);
                up.Add(step3);
                up.Add(step4);
            }

        }

        public Soldier(MapCell pos)
        {
            curDirection = rand.Next(0, 4);
            position = pos;
            curPos = new Point(position.CurRectange.X, position.CurRectange.Y + position.CurRectange.Height - desRec.Height);
            if (Characters == null)
            {
                Characters = new List<Texture2D>();

                Texture2D Character2 = GLOBAL.Content.Load<Texture2D>(@"Characters\\DoomCharSergeant");
                Characters.Add(Character2);

                Texture2D Character1 = GLOBAL.Content.Load<Texture2D>(@"Characters\\DoomCharPrivate");
                Characters.Add(Character1);
            }
                

            width = Characters[GLOBAL.curSolid].Width / 4;
            height = Characters[GLOBAL.curSolid].Height / 4;
            float scaletemp = (float)width / 32;
            desRec = new Rectangle(0, 0, 32, (int)(height/scaletemp) + 1);
            if (down == null)
            {
                Rectangle step1 = new Rectangle(0, 0, width, height);
                Rectangle step2 = new Rectangle(width, 0, width, height);
                Rectangle step3 = new Rectangle(width*2, 0, width, height);
                Rectangle step4 = new Rectangle(width*3, 0, width, height);
                down = new List<Rectangle>();
                down.Add(step1);
                down.Add(step2);
                down.Add(step3);
                down.Add(step4);
            }
            if (left == null)
            {
                Rectangle step1 = new Rectangle(0, height, width, height);
                Rectangle step2 = new Rectangle(width, height, width, height);
                Rectangle step3 = new Rectangle(width * 2, height, width, height);
                Rectangle step4 = new Rectangle(width * 3, height, width, height);
                left = new List<Rectangle>();
                left.Add(step1);
                left.Add(step2);
                left.Add(step3);
                left.Add(step4);
            }
            if (right == null)
            {
                Rectangle step1 = new Rectangle(0, 2*height, width, height);
                Rectangle step2 = new Rectangle(width, 2*height, width, height);
                Rectangle step3 = new Rectangle(width * 2, 2*height, width, height);
                Rectangle step4 = new Rectangle(width * 3, 2*height, width, height);
                right = new List<Rectangle>();
                right.Add(step1);
                right.Add(step2);
                right.Add(step3);
                right.Add(step4);
            }
            if (up == null)
            {
                Rectangle step1 = new Rectangle(0, 3*height, width, height);
                Rectangle step2 = new Rectangle(width, 3 * height, width, height);
                Rectangle step3 = new Rectangle(width * 2, 3 * height, width, height);
                Rectangle step4 = new Rectangle(width * 3, 3 * height, width, height);
                up = new List<Rectangle>();
                up.Add(step1);
                up.Add(step2);
                up.Add(step3);
                up.Add(step4);
            }
        }

        public bool IsSelected(MouseState mouse)
        {

            if (preMouse.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
            {
                //MouseState a = new MouseState((int)((mouse.X - (int)GLOBAL.centreCamera.X)/GLOBAL.scale), (int)((mouse.Y - (int)GLOBAL.centreCamera.Y) / GLOBAL.scale), 0, ButtonState.Pressed, ButtonState.Pressed, ButtonState.Pressed, ButtonState.Pressed, ButtonState.Pressed);
                Rectangle coverted = GLOBAL.ScreenToWorld(new Rectangle(mouse.X, mouse.Y, 32, desRec.Height));
                MouseState a = new MouseState(coverted.X, coverted.Y, 0, ButtonState.Pressed, ButtonState.Pressed, ButtonState.Pressed, ButtonState.Pressed, ButtonState.Pressed);

                if (GLOBAL.IsSelected(a, desRectangle))
                {
                    this.selected = true;
                    preMouse = mouse;
                    return true;
                }
                else
                {
                    this.selected = false;
                }
            }
            preMouse = mouse;
            return false;
        }

        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            //desRectangle = new Rectangle(position.CurRectange.X, position.CurRectange.Y + position.CurRectange.Height - 45, 32, 45);

            milisecond += gameTime.ElapsedGameTime.Milliseconds;
            if (milisecond > 99)
            {
                if (curSprite < 3)
                    curSprite++;
                else curSprite = 0;
                milisecond = 0;
            }

            
            if (gameTime.TotalGameTime.Seconds % 5 == 0)
            {
                if (second != gameTime.TotalGameTime.Seconds)
                    curDirection = rand.Next(0, 4);
                second = gameTime.TotalGameTime.Seconds;
            }

           // if (curDirection == Direction.UP) desRectangle = new Rectangle(curPos.X, curPos.Y--, 32, 45);
            if (curDirection == Direction.LEFT && curPos.X > 0) desRectangle = new Rectangle(curPos.X--, curPos.Y, 32, desRec.Height);
            else if (curDirection == Direction.RIGHT && curPos.X < 608) desRectangle = new Rectangle(curPos.X++, curPos.Y, 32, desRec.Height);
            else if (curDirection == Direction.UP && curPos.Y > 0) desRectangle = new Rectangle(curPos.X, curPos.Y--, 32, desRec.Height);
            else if (curDirection == Direction.DOWN && curPos.Y < 595) desRectangle = new Rectangle(curPos.X, curPos.Y++, 32, desRec.Height);

            IsSelected(mouse);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Rectangle desRectangle = new Rectangle(position.CurRectange.X, position.CurRectange.Y + position.CurRectange.Height - 45, 32, 45);

            if (curDirection == Direction.LEFT) spriteBatch.Draw(Characters[GLOBAL.curSolid], desRectangle, left[curSprite], Color.White);
            else if (curDirection == Direction.RIGHT) spriteBatch.Draw(Characters[GLOBAL.curSolid], desRectangle, right[curSprite], Color.White);
            else if (curDirection == Direction.UP) spriteBatch.Draw(Characters[GLOBAL.curSolid], desRectangle, up[curSprite], Color.White);
            else if (curDirection == Direction.DOWN) spriteBatch.Draw(Characters[GLOBAL.curSolid], desRectangle, down[curSprite], Color.White);
        }

        public override string ObjectName()
        {
            throw new NotImplementedException();
        }
    }
}