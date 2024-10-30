using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace menu
{
    public partial class InputForm : Form
    {
        public InputForm(string[] args, int[] args2)
        {
            InitializeComponent();
            label1.Text = args[0] + ":";
            label2.Text = args[1] + ":";
            textBox1.Text = args2[0].ToString();
            textBox2.Text = args2[1].ToString();
        }

        public int InputValue1, InputValue2;

        private void button1_Click(object sender, EventArgs e)
        {
            InputValue1 = Int32.Parse(textBox1.Text);
            InputValue2 = Int32.Parse(textBox2.Text);
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
