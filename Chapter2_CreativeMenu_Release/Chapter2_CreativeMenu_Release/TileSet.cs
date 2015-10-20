using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Chapter2_CreativeMenu_Release
{
    public class TileSet
    {
        private int _firstgrid = 0;
        private string _name = "";
        private int _tilewidth = 0;
        private int _tileheight = 0;
        private int _tilecount = 0;
        List<int> _imgWidth;
        List<int> _imgHeight;
        private IEnumerable<Vector2> _imgSize;
        private Texture2D _texture;

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }

            set
            {
                _texture = value;
            }
        }

        public int Firstgrid
        {
            get
            {
                return _firstgrid;
            }

            set
            {
                _firstgrid = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public int Tilewidth
        {
            get
            {
                return _tilewidth;
            }

            set
            {
                _tilewidth = value;
            }
        }

        public int Tileheight
        {
            get
            {
                return _tileheight;
            }

            set
            {
                _tileheight = value;
            }
        }

        public int Tilecount
        {
            get
            {
                return _tilecount;
            }

            set
            {
                _tilecount = value;
            }
        }


        public IEnumerable<Vector2> ImgSize
        {
            get
            {
                return _imgSize;
            }

            set
            {
                _imgSize = value;
            }
        }

        public List<int> ImgHeight
        {
            get
            {
                return _imgHeight;
            }

            set
            {
                _imgHeight = value;
            }
        }

        public List<int> ImgWidth
        {
            get
            {
                return _imgWidth;
            }

            set
            {
                _imgWidth = value;
            }
        }

        public TileSet(int firstgrid, string name, int tilewidth, int tileheight, int tilecount, List<int> ImgWidth, List<int> ImgHeight)
        {
            this.Firstgrid = firstgrid;
            this.Name = name;
            this.Tilecount = tilecount;
            this.Tilewidth = tilewidth;
            this.Tileheight = tileheight;
            this.ImgWidth = ImgWidth;
            this.ImgHeight = ImgHeight;
            string assetname = "Maps\\";
            string filename = assetname + name;
            this._texture = GLOBAL.Content.Load<Texture2D>(@filename);
        }
    }
}