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
            string pythonFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\price1.py"; // location of py file

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "python"; // set the filename to the Python interpreter
            startInfo.Arguments = $"{pythonFilePath} {ticker1}"; // set the arguments for the Python script
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
               
                textBox2.Text += Environment.NewLine + Environment.NewLine + a;

            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            // values
            string ticker = textBox1.Text;
            int ammount = Decimal.ToInt32(numericUpDown1.Value);
            string ammount3 = (numericUpDown1.Value).ToString();
            string name = label8.Text;
            int change = 0;
            decimal price = decimal.Parse(label7.Text, CultureInfo.InvariantCulture);
            decimal price2 = price * ammount;
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            string path2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data2.txt";
            string[] mymoney = File.ReadAllLines(path2);
            List<string> check = File.ReadAllLines(path1).ToList();
            decimal mymoney2 = Decimal.Parse(mymoney[0], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("de-DE"));
            int variable = 0;
            decimal change2 = 0;


            if (check.Contains(ticker) == true && mymoney2 >= price2 && price2 != 0)
            {

                int position = check.IndexOf(ticker);
                if (decimal.Parse(check[position + 4]) != 0)
                {
                    change2 = decimal.Parse(check[position + 3]) * decimal.Parse(check[position + 4]);
                }
                decimal price3 = decimal.Parse(check[position + 2]) * decimal.Parse(check[position + 3]) + change2;
                decimal price4 = price3 + price2;
                decimal ammount2 = price4 / (Decimal.Parse(check[position + 3]) + Decimal.Parse(ammount3));
                int ammount4 = ammount + Convert.ToInt32(check[position + 3]);
                check[position + 3] = ammount4.ToString();
                check[position + 2] = price.ToString();
                check[position + 4] = "0";
                Debug.WriteLine(ammount2);
                Debug.WriteLine(price4);
                Debug.WriteLine(price3);
                Debug.WriteLine(change2);
                Debug.WriteLine(price);
                Debug.WriteLine(ammount4);
                MessageBox.Show(price3.ToString());
                using (FileStream b = new FileStream(path1, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(b))
                    {
                        foreach (string a in check)
                        {
                            w.WriteLine(a);

                        }
                        w.Close();
                    }
                    b.Close();

                }
                variable = 1;
                panel4reflesh();
            }

            if (mymoney2 >= price2 && price2 != 0 && variable == 0)
            {
                // saving stock values
                using (FileStream fs = new FileStream(path1, FileMode.Append))
                {

                    using (StreamWriter w = new StreamWriter(fs))
                    {
                        w.WriteLine(ticker);
                        w.WriteLine(name);
                        w.WriteLine(price);
                        w.WriteLine(ammount);
                        w.WriteLine(change);
                        w.Close();
                    }
                    fs.Close();
                }
                // recalculating balance
                decimal mymoney3 = mymoney2 - price2; // new balance value
                using (FileStream fs = new FileStream(path2, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs))
                    {
                        w.WriteLine(mymoney3);
                        w.Close();
                    }
                    fs.Close();
                }
                label11.Text = mymoney3.ToString() + "$"; // new balance value on label
                // refleshing My stocks panel
                panel4reflesh();
            }
            if (mymoney2 <= price2 && price2 == 0 && variable == 0)
            {
                MessageBox.Show("you are too poor or you bought 0 stock");
            }




        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // path of the txt file where all your stocks and their values and stored
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            string path2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data2.txt";

            // checking if the file exist and if not it creates it
            if (File.Exists(path1))
            {


            }
            else
            {
                using (FileStream fs = File.Create(path1))
                {
                    fs.Close();
                }
            }
            if (File.Exists(path2))
            {
                string[] cash = File.ReadAllLines(path2);
                label11.Text = cash[0] + "$";
            }
            else
            {
                using (FileStream fs2 = File.Create(path2))
                {
                    fs2.Close();
                }
                File.WriteAllText(path2, "1000");
            }
            panel4reflesh();


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
            // path to saved stocks file
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            // clears the panel of old values
            panel4.Controls.Clear();
            // values for places of buttons
            int ticker1 = 0;
            int price1 = 2;
            int ammount1 = 3;
            int change1 = 4;
            int distance1 = 13;
            int distance2 = 11;
            int lines1 = 0;
            // loop that creates elements for each stock
            string[] stockdata = File.ReadAllLines(filePath);
            foreach (string a in stockdata)
            {
                lines1 += 1;

            }

            int stockammount = lines1 / 5;
            for (int i = stockammount; i > 0; i--)
            {
                // creating buttons and labels to my stocks section
                Label ticker = new Label();
                ticker.Text = stockdata[ticker1];
                ticker.Location = new Point(2, distance1);
                ticker.Size = new(60, 25);
                ticker.Tag = stockdata[ticker1];
                panel4.Controls.Add(ticker);

                Label price = new Label();
                price.Text = stockdata[price1];
                price.Location = new Point(60, distance1);
                price.Size = new(55, 25);
                price.Tag = stockdata[ticker1];
                panel4.Controls.Add(price);

                Label change = new Label();
                change.Text = stockdata[change1];
                change.Location = new Point(110, distance1);
                change.Size = new(60, 25);
                change.Tag = stockdata[ticker1];
                panel4.Controls.Add(change);

                PictureBox pictureBox1 = new PictureBox();
                pictureBox1.Size = new(20, 20);
                pictureBox1.Location = new Point(175, distance1);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                if (Decimal.Parse(stockdata[change1], CultureInfo.CreateSpecificCulture("de-DE")) > 0)
                {
                    pictureBox1.Image = Properties.Resources.icons8_up_30;
                }
                if (Decimal.Parse(stockdata[change1], CultureInfo.CreateSpecificCulture("de-DE")) < 0)
                {
                    pictureBox1.Image = Properties.Resources.icons8_down_30;
                }
                panel4.Controls.Add(pictureBox1);

                Label ammount = new Label();
                ammount.Text = stockdata[ammount1];
                ammount.Location = new Point(195, distance1);
                ammount.Size = new(25, 25);
                ammount.Tag = stockdata[ticker1];
                panel4.Controls.Add(ammount);

                Button sell2 = new Button();
                sell2.Text = "Sell";
                sell2.Location = new Point(220, distance2);
                sell2.Size = new(60, 25);
                sell2.Click += sell2_Click;
                sell2.Tag = stockdata[ticker1];
                panel4.Controls.Add(sell2);
                ticker1 += 5;
                price1 += 5;
                ammount1 += 5;
                change1 += 5;
                distance1 += 26;
                distance2 += 26;
            }
        }

        private async void Panel4change()
        {
            int lines1 = 0;
            int ticker = 0;
            int change = 4;
            int price = 2;
            // path to saved stocks file
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            // loop that creates elements for each stock
            string[] stockdata = File.ReadAllLines(filePath);
            foreach (string a in stockdata)
            {
                lines1 += 1;

            }
            int stockammount = lines1 / 5;
            for (int i = stockammount; i > 0; i--)
            {
                string pythonFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\price2.py"; // location of py file

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "python"; // set the filename to the Python interpreter
                startInfo.Arguments = $"{pythonFilePath} {stockdata[ticker]}"; // set the arguments for the Python script
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                string pricenew = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                decimal priceold = Decimal.Parse(stockdata[price], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("de-DE"));
                decimal pricenew2 = Decimal.Parse(pricenew, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                decimal change2 = pricenew2 - priceold;
                string change3 = change2.ToString();
                stockdata[change] = change3;
                ticker += 5;
                change += 5;
                price += 5;
                await Task.Delay(5000);
            }
            File.WriteAllLines(filePath, stockdata);
            panel4reflesh();

        }

        private void RemoveTickerFromFile(string tickerToRemove)
        {
            // Load the tickers from the file and balance values
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data1.txt";
            string filePath2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data2.txt";
            string balanceString = File.ReadAllText(filePath2).Trim();
            decimal balance = Decimal.Parse(balanceString, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("de-DE"));
            List<string> tickers = File.ReadAllLines(filePath).ToList();
            int position = tickers.IndexOf(tickerToRemove);
            // update balance
            decimal change = Decimal.Parse(tickers[position + 4], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.CreateSpecificCulture("de-DE"));
            decimal count = Decimal.Parse(tickers[position + 3], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("de-DE"));
            decimal price = Decimal.Parse(tickers[position + 2], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("de-DE"));
            decimal balance2 = balance + price * count + change * count;
            Debug.WriteLine(change * count);
            Debug.WriteLine(price * count);
            File.WriteAllText(filePath2, balance2.ToString());
            label11.Text = balance2.ToString();
            // Remove the ticker from the list         
            tickers.RemoveRange(position, 5);
            // Write the updated list of tickers back to the file
            File.WriteAllLines(filePath, tickers);
            // call a panel4 reflesh
            panel4reflesh();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Panel4change();
        }
    }
}