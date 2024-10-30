using System.Runtime.Serialization;

namespace shapes
{
    public partial class Shapes : Form
    {
        Drawfield df;
        DisplayObject[] objectList;

        public static int fieldX1 = 200, fieldY1 = 100;
        public static int fieldX2 = 1000, fieldY2 = 900;
        public static int borderThickness = 10;
        public Shapes()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            //this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            Rectangle workArea = Screen.GetWorkingArea(this);

            //fieldX2 = DisplayRectangle.Width;
            //fieldY2 = DisplayRectangle.Height;

            df = new Drawfield(fieldX1, fieldY1, fieldX2, fieldY2, borderThickness, 255, 10, 60);
            (df, objectList) = Generator.generateDrawField(fieldX1, fieldY1, fieldX2, fieldY2, borderThickness);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            df.Draw(e.Graphics);
            foreach (DisplayObject obj in objectList)
            {
                obj.Draw(e.Graphics);
            }
        }
    }
}
