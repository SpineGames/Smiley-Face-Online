using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Smiley_Face_Online
{
    class GameLevel
    {
        string mapName = "Unknown map";
        public string Name
        {
            get { return mapName; }
        }
        static Version v = new Version(1, 0, 0, 0);
        public static Version version
        {
            get { return v; }
        }
        Size mapSize;
        byte[,] tiles;

        public GameLevel(string mapName, int width, int height)
        {
            this.mapName = mapName;
            tiles = new byte[width, height];
            mapSize = new Size(width, height);
        }

        /// <summary>
        /// Sets the tile at the coord to the specified ID
        /// </summary>
        /// <param name="ID">The tile ID to set to</param>
        /// <param name="coord">The co-ordinate to set</param>
        public void setTile(byte ID, Point coord)
        {
            tiles[coord.X, coord.Y] = ID;
        }

        public void Render(PaintEventArgs e, RectangleF window)
        {
            int minX = window.Left / 16F > 0 ? (int)(window.Left / 16F) : 0;
            int minY = window.Top / 16F > 0 ? (int)(window.Top / 16F) : 0;
            int maxX = (int)(window.Right / 16F) + 1 < mapSize.Width ? (int)(window.Right / 16F) + 1 : mapSize.Width;
            int MaxY = (int)(window.Bottom / 16F) + 1 < mapSize.Height ? (int)(window.Bottom / 16F) + 1 : mapSize.Height;

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < MaxY; y++)
                {
                    TileManager.renderTile(tiles[x, y], e, new PointF((x * 16) - window.Left, (y * 16) - window.Top));
                }
            }
        }

        public void SaveToStream(Stream stream)
        {
            BinaryWriter w = new BinaryWriter(stream);

            w.Write(v.ToString());
            w.Write(mapName);

            w.Write(mapSize.Width);
            w.Write(mapSize.Height);

            for (int x = 0; x < mapSize.Width; x++)
            {
                for (int y = 0; y < mapSize.Height; y++)
                {
                    w.Write(tiles[x, y]);
                }
            }

            w.Dispose();
        }

        public static GameLevel LoadFromStream(Stream stream)
        {
            BinaryReader r = new BinaryReader(stream);

            string v = r.ReadString();
            if (version == Version.Parse(v))
            {
                string name = r.ReadString();
                int width = r.ReadInt32();
                int height = r.ReadInt32();

                GameLevel level = new GameLevel(name, width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        level.setTile(r.ReadByte(), new Point(x, y));
                    }
                }

                return level;
            }
            else
            {
                stream.Close();
                r.Close();
                throw new ArgumentException("The level version does not match the current level version. Aborting load.");
            }
        }
    }
}
