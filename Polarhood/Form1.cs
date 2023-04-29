using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polarhood
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ticker1 = textBox1.Text;
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (StreamWriter writer = new StreamWriter(path1 + "\\stockprice.txt."))
            {
                writer.WriteLine(ticker1+ ":NASDAQ");

            }
            
        }
    }
}
