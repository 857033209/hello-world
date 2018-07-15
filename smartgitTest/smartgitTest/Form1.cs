using smartgitTest.class1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smartgitTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "hello world!";
            label1.Text = "hello world1!";
            label1.Text = "1!";
            label1.Text = "hd!";
            label1.Text = "dforld!";
            label1.Text = "hsdfsfssswweggggrld!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = test1.changestr2();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f = new Form2();
            f.Show();
        }
    }
}
