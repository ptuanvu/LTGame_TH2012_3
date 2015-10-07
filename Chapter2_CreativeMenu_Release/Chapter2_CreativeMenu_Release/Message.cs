using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter2_CreativeMenu_Release
{
    public class DisplayMessage : GameObject
    {
        private string messagge;
        private TimeSpan displayTime;
        private int currentIndex;
        private Vector2 position;
        private string drawMessage;
        private Color drawColor;
        private SpriteFont font;
        private string name;

        public DisplayMessage(string message, TimeSpan displayTime, Vector2 position, Color color, SpriteFont font, string name)
        {
            this.messagge = message;
            this.displayTime = displayTime;
            this.currentIndex = 0;
            this.position = position;
            this.drawMessage = string.Empty;
            this.drawColor = color;
            this.font = font;
            this.name = name;

        }

        public string Messagge
        {
            get
            {
                return messagge;
            }

            set
            {
                messagge = value;
            }
        }

        public TimeSpan DisplayTime
        {
            get
            {
                return displayTime;
            }

            set
            {
                displayTime = value;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }

            set
            {
                currentIndex = value;
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

        public string DrawMessage
        {
            get
            {
                return drawMessage;
            }

            set
            {
                drawMessage = value;
            }
        }

        public Color DrawColor
        {
            get
            {
                return drawColor;
            }

            set
            {
                drawColor = value;
            }
        }

        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            displayTime -= gameTime.ElapsedGameTime;

            if (this.displayTime <= TimeSpan.Zero)
            {
                this.displayTime = TimeSpan.FromSeconds(1.0);
                this.currentIndex = 0;
                this.drawMessage = "";
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.drawMessage += this.messagge[this.currentIndex].ToString();
            spriteBatch.DrawString(this.font, this.drawMessage, this.position, this.drawColor);

            if (this.currentIndex != this.messagge.Length - 1)
            {
                this.currentIndex++;
            } 
        }

        public override string ObjectName()
        {
            return this.name;
        }
    }
}
