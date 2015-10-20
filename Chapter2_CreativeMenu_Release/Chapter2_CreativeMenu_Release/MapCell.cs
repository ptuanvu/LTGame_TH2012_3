using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chapter2_CreativeMenu_Release
{
    public class MapCell 
    {
        Rectangle sourceRectange;
        Rectangle curRectange;
        int _depth = 0;
        TileSet _tileSet;
        Point _position;

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

        public Rectangle SourceRectange
        {
            get
            {
                return sourceRectange;
            }

            set
            {
                sourceRectange = value;
            }
        }

        public TileSet TileSet
        {
            get
            {
                return _tileSet;
            }

            set
            {
                _tileSet = value;
            }
        }

        public Rectangle CurRectange
        {
            get
            {
                return curRectange;
            }

            set
            {
                curRectange = value;
            }
        }

        public Point Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.TileSet != null)
                spriteBatch.Draw(this.TileSet.Texture, this.curRectange, this.sourceRectange, Color.White);
        }
    }
}