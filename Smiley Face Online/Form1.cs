using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Input.Manipulations;
using System.Runtime.InteropServices;
using System.IO;

namespace Smiley_Face_Online
{
    public partial class frm_game : Form
    {
        GameLevel testLevel;
        Timer ticker = new Timer();
        PointF winPos = new PointF(56,4);
        SizeF winSize = new SizeF(640, 480);
        float scrollSpeed = 4F;

        public frm_game()
        {
            InitializeComponent();
            DoubleBuffered = true;

            ticker.Interval = (int)(1000F / 60F);
            ticker.Tick += new EventHandler(GameUpdate);
            ticker.Start();
        }

        private void frm_game_Load(object sender, EventArgs e)
        {
            testLevel = new GameLevel("Map Test", 800, 600);

            Tile tree = new Tile(Properties.Resources.tree);
            Tile grass = new Tile(Properties.Resources.grass);

            testLevel.setTile(1, new Point(2,2));

            Stream s = File.OpenWrite("saveGame.sfl");
            testLevel.SaveToStream(s);
            s.Close();
        }

        private void GameUpdate(object o, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Keys.Left))
            {
                winPos.X -= scrollSpeed;
            }
            if (Keyboard.IsKeyDown(Keys.Right))
            {
                winPos.X += scrollSpeed;
            }
            if (Keyboard.IsKeyDown(Keys.Up))
            {
                winPos.Y -= scrollSpeed;
            }
            if (Keyboard.IsKeyDown(Keys.Down))
            {
                winPos.Y += scrollSpeed;
            }

            this.Text = "Smiley Face Wars Online (" + testLevel.Name + ") " + winPos;

            Invalidate();
        }

        private void frm_game_Paint(object sender, PaintEventArgs e)
        {
            testLevel.Render(e, new RectangleF(winPos, winSize));
        }

        public abstract class Keyboard
        {
            [Flags]
            private enum KeyStates
            {
                None = 0,
                Down = 1,
                Toggled = 2
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]

            private static extern short GetKeyState(int keyCode);

            private static KeyStates GetKeyState(Keys key)
            {
                KeyStates state = KeyStates.None;

                short retVal = GetKeyState((int)key);

                //If the high-order bit is 1, the key is down
                //otherwise, it is up.
                if ((retVal & 0x8000) == 0x8000)
                    state |= KeyStates.Down;

                //If the low-order bit is 1, the key is toggled.
                if ((retVal & 1) == 1)
                    state |= KeyStates.Toggled;

                return state;
            }

            public static bool IsKeyDown(Keys key)
            {
                return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
            }

            public static bool IsKeyToggled(Keys key)
            {
                return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
            }
        }
    }
}
