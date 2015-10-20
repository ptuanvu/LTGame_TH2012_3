using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Input;

namespace Chapter2_CreativeMenu_Release
{
    public class MapRow {
        public List<MapCell> Columns = new List<MapCell>();
    }

    public class ImageSize
    {
        public int Witdh { get; set; }
        public int Height { get; set; }
    }

    public class TileMap : GameObject
    {
        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 50;
        public int MapHeight = 50;
        private Texture2D mouseMap;
        private Texture2D slopeMaps;
        private uint _numberOfColumns;
        private uint _numberOfRows;
        private uint _cellSize;
        List<TileSet> tileSet;
        List<Layer> layers;
        Texture2D _chooser;

        public Texture2D Chooser
        {
            get
            {
                return _chooser;
            }

            set
            {
                _chooser = value;
            }
        }

        public TileMap(Texture2D mouseMap, Texture2D slopeMap)
        {
            this.mouseMap = mouseMap;
            this.slopeMaps = slopeMaps;
            this.GameState = 2;
            _chooser = GLOBAL.Content.Load<Texture2D>(@"Maps\\Chooser");

            Stream mapcontent = new FileStream("Content/Maps/mapcontent.tmx", FileMode.Open, FileAccess.Read);
            XElement document;

            document = XElement.Load(mapcontent);
            _numberOfColumns = (uint)document.Attribute("width");
            _numberOfRows = (uint)document.Attribute("height");
            _cellSize = (uint)document.Attribute("tilewidth");
            tileSet = GetTileSet(document);
            layers = GetLayer(document);
            WorldToMapCell();
        }

        public void WorldToMapCell()
        {
           // Layer lay = layers[0];
            foreach(Layer lay in layers)
            {
                int depth = GLOBAL.curDepth++;
                int dem = 0;

                for (int i = 0; i < lay.Height; i++)
                {
                    MapRow row = new MapRow();
                    for (int j = 0; j < lay.Width; j++)
                    {
                        MapCell cell = FindTextureByGidData(lay.Data[dem]);
                        cell.Position = new Point(j, i);
                        Rectangle curRectange = new Rectangle(j * 32, i * 32, 32, 32);
                        cell.CurRectange = curRectange;
                        cell.Depth = depth;
                        dem++;
                        row.Columns.Add(cell);
                    }
                    Rows.Add(row);
                }
            }

        }

        private MapCell FindTextureByGidData(int v)
        {
            MapCell result = new MapCell();
            foreach (TileSet ts in tileSet)
            {
                if (ts.Firstgrid <= v && (ts.Firstgrid + ts.Tilecount) >= v)
                {
                    result.TileSet = ts;

                    int dem = ts.Firstgrid;
                    for(int i = 0; i < ts.ImgHeight.First()/32; i++)
                        for (int j = 0;  j < ts.ImgWidth.First() / 32; j++)
                        {
                            if (dem == v)
                            {
                                Rectangle sourRectange = new Rectangle(j * 32, i * 32,  32, 32);
                                result.SourceRectange = sourRectange;
                            }
                            dem++;
                        }
                }
            }
            return result;
        }

        private List<TileSet> GetTileSet(XElement document)
        {
            var tileset = from cust in document.Descendants("tileset")
                          select new TileSet(Convert.ToInt32(cust.Attribute("firstgid").Value), cust.Attribute("name").Value, Convert.ToInt32(cust.Attribute("tilewidth").Value), Convert.ToInt32(cust.Attribute("tileheight").Value), Convert.ToInt32(cust.Attribute("tilecount").Value),
                          new List<int>(
                              from img in cust.Descendants("image")
                              select Convert.ToInt32(img.Attribute("width").Value)
                              ),
                          new List<int>(
                              from img in cust.Descendants("image")
                              select Convert.ToInt32(img.Attribute("height").Value)
                              )
                          );
            return tileset.ToList();
        }

        private List<Layer> GetLayer(XElement document)
        {
            var layer = from lay in document.Descendants("layer")
                        select new Layer(lay.Attribute("name").Value, Convert.ToInt32(lay.Attribute("width").Value), Convert.ToInt32(lay.Attribute("height").Value),
                        new List<int>(
                            from grid in lay.Descendants("tile")
                            select Convert.ToInt32(grid.Attribute("gid").Value)
                            )
                        );
            return layer.ToList();
        }

        public MapCell GetSelectedMapCell(MouseState e)
        {
            MapCell result = new MapCell();
            foreach(MapRow row in Rows)
            {
                foreach(MapCell cell in row.Columns)
                {
                    //Rectangle cellPosition = GetCellPosition(cell.CurRectange);
                    Rectangle cellPos = GetCellPosition(cell);
                    if (GLOBAL.IsSelected(e, cellPos))
                    {
                        if (result.Depth <= cell.Depth && cell.TileSet != null)
                            result = cell;
                    }
                }
            }

            if (result.TileSet != null)
                return result;
            else return null;
 
        }

        private Rectangle GetCellPosition(MapCell c)
        {

            Rectangle result = new Rectangle();

            int width = (int)(c.CurRectange.Width * GLOBAL.scale);
            int height = (int)(c.CurRectange.Height * GLOBAL.scale);

            result.X = width * c.Position.X + (int)GLOBAL.centreCamera.X;
            result.Y = height * c.Position.Y + (int)GLOBAL.centreCamera.Y;
            result.Width = width;
            result.Height = height;
            
            //result.X = curRectange.X + (int)GLOBAL.centreCamera.X;
            //result.Y = curRectange.Y + (int)GLOBAL.centreCamera.Y;
            //result.Width = (int)(curRectange.Width * GLOBAL.scale);
            //result.Height = (int)(curRectange.Height * GLOBAL.scale);
            return result;

        }

        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
            GLOBAL.selected = GetSelectedMapCell(mouse);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(MapRow row in Rows)
            {
                foreach(MapCell cell in row.Columns)
                {
                    cell.Draw(gameTime, spriteBatch);
                }
            }

            if (GLOBAL.selected != null)
                spriteBatch.Draw(this._chooser, GLOBAL.selected.CurRectange, Color.White);
        }

        public override string ObjectName()
        {
            throw new NotImplementedException();
        }
    }
}