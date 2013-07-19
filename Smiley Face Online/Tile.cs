using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Smiley_Face_Online
{
    class Tile
    {
        public bool solid;
        public float speedMultiplier;
        public byte tileID;
        public Image tex;
        public Size imageSize;

        public Tile(Image tex = null, bool solid = true, float speedMultiplier = 0F, Size? imageSize = null)
        {
            this.tileID = TileManager.getNextID();

            this.solid = solid;
            this.speedMultiplier = speedMultiplier;

            this.tex = tex;

            if (tex == null)
                tex = Properties.Resources.clear;

            if (imageSize == null)
                this.imageSize = new Size(16, 16);
            else
                this.imageSize = imageSize.Value;

            TileManager.addTileType(this);
        }
    }

    class TileManager
    {
        static Size defaultSize = new Size(16, 16);

        static List< Tile> tiles = new List< Tile>();

        public static byte addTileType(Tile tile)
        {
            if (tiles.Count != 256)
            {
                tiles.Add(tile);
                return (byte)(tiles.Count - 1);
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot add more than 256 tile types.");
            }
        }

        public static byte getIndexforTile(Tile tile)
        {
            return (byte)tiles.IndexOf(tile);
        }

        public static byte getNextID()
        {
            return (byte)tiles.Count;
        }

        /// <summary>
        /// Renders a tile instance
        /// </summary>
        /// <param name="tile">The tile to render</param>
        /// <param name="e">The PaintEventArgs to render with</param>
        /// <param name="p">The point to render at</param>
        public static void renderTile(byte ID, PaintEventArgs e, PointF p)
        {
            if (tiles.ElementAt(ID).imageSize == defaultSize)
            {
                e.Graphics.DrawImage(tiles.ElementAt(ID).tex, new RectangleF(p, new SizeF(16, 16)));
            }
            else
            {
                PointF offset = new Point((tiles.ElementAt(ID).imageSize.Width - 16) / 2, (tiles.ElementAt(ID).imageSize.Height - 16) / 2);

                e.Graphics.DrawImage(tiles.ElementAt(ID).tex, new RectangleF(new PointF(p.X - offset.X, p.Y - offset.Y), tiles.ElementAt(ID).imageSize));
            }
        }
    }
}
