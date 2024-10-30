using shapes;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace movement
{
    public partial class Form1 : Form
    {
        public static int fieldX1 = 200, fieldY1 = 100;
        public static int fieldX2 = 1400, fieldY2 = 900;
        public static int borderThickness = 5;
        public static bool runing = true;
        private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private static int FPS = 100;
        private static int timeInterval = 1000 / FPS;
        public static int BORDER_1;
        public static int BORDER_2;
        private static Game MainGame;
        private static bool accelerated = true;

        private string ToBgr(Color c) => $"{c.B:X2}{c.G:X2}{c.R:X2}";

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        const int DWWMA_CAPTION_COLOR = 35;
        const int DWWMA_BORDER_COLOR = 34;
        const int DWMWA_TEXT_COLOR = 36;
        public void CustomWindow(Color borderColor, IntPtr handle)
        {
            IntPtr hWnd = handle;
            //Change border color
            int[] border = [int.Parse(ToBgr(borderColor), System.Globalization.NumberStyles.HexNumber)];
            DwmSetWindowAttribute(hWnd, 34, border, 4);
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;

            //AllocConsole();
            timer.Tick += new EventHandler(MoveObjects);
            timer.Interval = timeInterval;

            Rectangle workArea = Screen.GetWorkingArea(this);
            BORDER_1 = DisplayRectangle.Width;
            BORDER_2 = DisplayRectangle.Height;
            Drawfield field;
            DisplayObject[] objects;
            (field, objects) = Generator.generateDrawField(fieldX1, fieldY1, fieldX2, fieldY2, borderThickness, accelerated, true);


            // Initializing Game object
            MainGame = new Game(field, accelerated);
            foreach (DisplayObject obj in objects)
            {
                MainGame.AddObject(obj);
            }
            timer.Enabled = true;
        }

        private void MoveObjects(Object myObject, EventArgs myEventArgs)
        {
            if (runing)
            {
                MainGame.MoveObjects();
                Invalidate();
            }
            else
            {
                runing = false;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (runing)
            {
                MainGame.DrawGame(e.Graphics, DisplayRectangle.Width, DisplayRectangle.Height);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    runing = false;
                    break;
                case Keys.E:
                    runing = true;
                    break;
            }
        }
    }
}