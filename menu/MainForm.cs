using oop3.DisplayObjects;
using oop3.Menu;
using oop3.Utilities;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace oop2
{
    public partial class MainForm : Form
    {
        public static int fieldX1 = 0, fieldY1 = 0;
        public static int fieldX2 = 1450, fieldY2 = 900;
        public static int borderThickness = 5;

        private readonly bool movementEnabled = false;
        private static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private static int FPS = 60;
        private static int timeInterval = 1000 / FPS;

        private static Game MainGame;
        private static bool accelerated = false;


        private bool MenuEditMode = false;
        private bool _ItemEditMode = false;
        private bool ItemEditMode
        {
            get => _ItemEditMode;
            set {
                _ItemEditMode = value;
                if (_ItemEditMode) { 

                    ItemTemplate = TemplateGenerator.TemplateFromMenuItem(ChosenItem);
                    ItemEdit = TemplateGenerator.GenItemEdit(ItemTemplate, ChosenItem);
                }
            }
        }

        private bool MoveMode = false;
        private Point lastPoint = new Point();
        private Menu MainMenu;
        private Menu ItemEdit;
        private Menu MenuEdit;
        private TemplateGenerator MenuTemplate;
        private TemplateGenerator ItemTemplate;
        private MenuItem? ChosenItem;


        public MainForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            WindowState = FormWindowState.Normal;

            if (movementEnabled)
            {
                timer.Tick += new EventHandler(MoveObjects);
                timer.Interval = timeInterval;
                timer.Enabled = true;
            }

            Rectangle workArea = Screen.GetWorkingArea(this);

            // make right bottom the second point
            fieldX2 = DisplayRectangle.Width;
            fieldY2 = DisplayRectangle.Height;

            GameField field;
            DisplayObject[] objects;

            (field, objects) = ObjectGenerator.generateDrawField(fieldX1, fieldY1, fieldX2, fieldY2, borderThickness, accelerated);
            field.isAccelerated = false;

            MainMenu = new Menu(10, 10);
            (MenuEdit, MenuTemplate) = TemplateGenerator.GenMenuEdit(MainMenu);
            ChosenItem = null;


            field.AddObject(MainMenu);

            // Initializing Game object
            MainGame = new Game(field);

        }

        private void MoveObjects(Object myObject, EventArgs myEventArgs)
        {
            MainGame.MoveObjects(timeInterval);
            Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            MainGame.DrawGame(e.Graphics);
            if (MenuEditMode)
            {
                int x, y;
                (x, y) = MainGame.GetClientOffset();
                e.Graphics.TranslateTransform(x, y);


                if (ItemEditMode)
                {
                    // draw item edit menu
                    ItemEdit.Draw(e.Graphics);
                    if (ChosenItem != null) { 
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 5),
                                new Rectangle(ChosenItem.view.frameX1, ChosenItem.view.frameY1,
                                ChosenItem.view.frameX2 - ChosenItem.view.frameX1,
                                ChosenItem.view.frameY2 - ChosenItem.view.frameY1)); 
                    }
                }
                else { 
                    // draw menu edit menu
                    MenuEdit.Draw(e.Graphics);
                    if (ChosenItem != null)
                    {
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red), 5),
                            new Rectangle(ChosenItem.view.frameX1, ChosenItem.view.frameY1,
                            ChosenItem.view.frameX2 - ChosenItem.view.frameX1,
                            ChosenItem.view.frameY2 - ChosenItem.view.frameY1));
                    }
                }
                //ChosenItem?.Draw(e.Graphics);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.R:
                    MenuEditMode = !MenuEditMode;
                    Invalidate();
                    break;
                case Keys.E:
                    if (ChosenItem != null) { 
                        ItemEditMode = !ItemEditMode;
                        Invalidate();
                    }
                    break;
            }
        }


        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
            if (MenuEditMode)
            {
                int offsX, offsY;
                (offsX, offsY) = MainGame.GetClientOffset();
                if (ItemEditMode)
                {
                    if (!ItemEdit.Click(x - offsX, y - offsY)) {
                        if (ChosenItem.Click(x - offsX, y - offsY)) {
                            MoveMode = true;
                            lastPoint = new Point(x, y);
                        }  
                    } 
                }
                else { 
                    // check editing menu click
                    if (MenuEdit.Click(x - offsX, y - offsY))
                    {
                        // deselect. or do I need to?
                        // holy moly ravioli
                        if (MenuEdit.lastClickedItem.GetStringDisplay() == "Remove")
                        {
                            if (MainMenu.lastClickedItem != null)
                            {
                                MenuItem item = MainMenu.lastClickedItem;
                                while (item.lastClickedItem != null)
                                {
                                    item = item.lastClickedItem;
                                }
                                ChosenItem = item;
                            }
                            else
                            {
                                ChosenItem = null;
                            }
                            ItemEditMode = false;
                        }
                        // else, the clicked item remains the same
                    }
                    else
                    {
                        MainGame.HandleClick(x, y);
                        // some menu item has been chosen
                        if (MainMenu.lastClickedItem != null)
                        {
                            MenuItem item = MainMenu.lastClickedItem;
                            while (item.lastClickedItem != null)
                            {
                                item = item.lastClickedItem;
                            }
                            ChosenItem = item;
                            MoveMode = true;
                            lastPoint = new Point(x, y);    
                        }
                        else
                        {
                            ChosenItem = null;
                            ItemEditMode = false;
                        }
                    }
                }
            }

            else
            {
                MainGame.HandleClick(x, y);
            }
            Invalidate();
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            MoveMode = false;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (ItemEditMode && MoveMode) {
                ChosenItem.ShiftObject(e.Location.X - lastPoint.X, e.Location.Y - lastPoint.Y);
                lastPoint = e.Location;
                Invalidate();
            }
        }
    }
}
