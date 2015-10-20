using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chapter2_CreativeMenu_Release
{
    public class Layer
    {
        private string _name;
        private int _width = 0;
        private int _height = 0;
        private List<int> _data;

        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        public List<int> Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
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

        public Layer(string name, int width, int height, List<int> data)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.Data = data;
        }
    }
}