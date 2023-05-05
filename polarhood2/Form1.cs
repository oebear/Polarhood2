using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace polarhood2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ticker1 = textBox1.Text;
            string pythonFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\price1.py";
            string arguments = ticker1; // replace with your own arguments

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "python"; // set the filename to the Python interpreter
            startInfo.Arguments = $"{pythonFilePath} {arguments}"; // set the arguments for the Python script
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            List<string> outputLines = new List<string>();
            string? line;
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                outputLines.Add(line);
            }
            process.WaitForExit();
            label8.Text = outputLines[1];
            label7.Text = outputLines[0];
            outputLines.RemoveRange(0, 2);
            foreach (string a in outputLines)
            {
                textBox2.Text += a;

            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ticker = textBox1.Text;
            int ammount = Decimal.ToInt32(numericUpDown1.Value);
            string name = label8.Text;
            decimal price = decimal.Parse(label7.Text, CultureInfo.InvariantCulture);
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            using (FileStream fs = new FileStream(path1, FileMode.Append))
            {
                using (StreamWriter w = new StreamWriter(fs))
                {
                    w.WriteLine(ticker);
                    w.WriteLine(name);
                    w.WriteLine(price);
                    w.WriteLine(ammount);
                    w.Close();
                }
                fs.Close();
            }
            panel4reflesh();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // values
            int ticker1 = 0;
            int distance1 = 13;
            int distance2 = 12;
            int lines1 = 0;
            // path of the txt file where all your stocks and their values and stored
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            // checking if the file exist and if it does it displays its values and if not it creates it
            if (File.Exists(path1))
            {
                string[] stockdata = File.ReadAllLines(path1);
                foreach (string a in stockdata)
                {
                    lines1 += 1;

                }
                int stockammount = lines1 / 4;
                for (int i = stockammount; i > 0; i--)
                {
                    // creating buttons and labels to my stocks section
                    Label ticker = new Label();
                    ticker.Text = stockdata[ticker1];
                    ticker.Location = new Point(13, distance1);
                    ticker.Size = new(60, 22);
                    ticker.Tag = stockdata[ticker1];
                    panel4.Controls.Add(ticker);

                    Button sell2 = new Button();
                    sell2.Text = "Sell";
                    sell2.Location = new Point(100, distance2);
                    sell2.Click += sell2_Click;
                    sell2.Tag = stockdata[ticker1];
                    panel4.Controls.Add(sell2);

                    distance1 += 26;
                    distance2 += 26;
                    ticker1 += 4;
                }
            }
            else
            {
                using (FileStream fs = File.Create(path1))
                {

                }
            }


        }

        private void sell2_Click(object? sender, EventArgs e)
        {
            // This code will execute when any of the sell buttons are clicked.

            if (sender is Button sell2 && sell2.Tag is string ticker)
            {

                // Remove the ticker from the file
                RemoveTickerFromFile(ticker);

            }



        }
        private void panel4reflesh()
        {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            // clears the panel of old values
            panel4.Controls.Clear();
            // values
            int ticker1 = 0;
            int distance1 = 13;
            int distance2 = 12;
            int lines1 = 0;
            string[] stockdata = File.ReadAllLines(filePath);
            foreach (string a in stockdata)
            {
                lines1 += 1;

            }
            int stockammount = lines1 / 4;
            for (int i = stockammount; i > 0; i--)
            {
                // creating buttons and labels to my stocks section
                Label ticker = new Label();
                ticker.Text = stockdata[ticker1];
                ticker.Location = new Point(13, distance1);
                ticker.Size = new(60, 22);
                ticker.Tag = stockdata[ticker1];
                panel4.Controls.Add(ticker);

                Button sell2 = new Button();
                sell2.Text = "Sell";
                sell2.Location = new Point(100, distance2);
                sell2.Click += sell2_Click;
                sell2.Tag = stockdata[ticker1];
                panel4.Controls.Add(sell2);

                distance1 += 26;
                distance2 += 26;
                ticker1 += 4;
            }
        }

        private void RemoveTickerFromFile(string tickerToRemove)
        {
            // Load the tickers from the file
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            List<string> tickers = File.ReadAllLines(filePath).ToList();

            // Remove the ticker from the list
            int position = tickers.IndexOf(tickerToRemove);
            int position2 = position + 3;
            int count = position2 - position;
            MessageBox.Show(tickers[position]);
            MessageBox.Show(tickers[position2]);
            tickers.RemoveRange(position, count);

            // Write the updated list of tickers back to the file
            File.WriteAllLines(filePath, tickers);
            // call a panel4 reflesh
            panel4reflesh();
        }
    }
}