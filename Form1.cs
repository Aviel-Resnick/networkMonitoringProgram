using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Network_Monitoring_Program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            substring_split();
            InitTimer();
            Main();

        }

        public void substring_split()
        {
            int LINES = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\arp_dump.txt").Count() - 1;
            string[] DATA_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\arp_dump.txt");

            System.IO.File.WriteAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt", string.Empty);

            for (int i = 0; i < LINES; i++)
            {
                string LINE = DATA_ARRAY[i];
                int LINE_LENGTH = LINE.Count();

                if (LINE_LENGTH > 4)
                {
                    string CHAR = LINE.Substring(0, 5);

                    bool CORRECT_LINE = CHAR.Equals("  192", StringComparison.Ordinal);

                    if (CORRECT_LINE == true)
                    {
                        string MAC = LINE.Substring(24, 17);
                        System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt", string.Format("{0}{1}", MAC, Environment.NewLine));
                    }
                }
            }
        }

        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MACS_LIST.Items.Clear();
            MACS_LIST.Text = "MAC Addresses of Connected Users";
            Main();
        }

        public void Main()
        {
            int CONNECTED_MAC_COUNT = System.IO.File.ReadLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt").Count();
            int ALLOWED_MAC_COUNT = System.IO.File.ReadLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt").Count();
            string[] MACS_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt");
            string[] ALLOWED_MACS_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt");

            for (int i = 0; i < CONNECTED_MAC_COUNT; i++)
            {
                if (!MACS_LIST.Items.Contains(MACS_ARRAY[i]))
                {
                    MACS_LIST.Items.Add(MACS_ARRAY[i]);
                }
            }

            for (int x = 0; x < ALLOWED_MAC_COUNT; x++)
            {
                if (!ALLOWED_MACS_LIST.Items.Contains(ALLOWED_MACS_ARRAY[x]))
                {
                    ALLOWED_MACS_LIST.Items.Add(ALLOWED_MACS_ARRAY[x]);
                }
            }
          
            // Cross

            for (int y = 0; y < CONNECTED_MAC_COUNT; y++)
            {
                string CONNECTED_MAC = MACS_ARRAY[y];
                bool intrusion = true; 

                for (int z = 0; z < ALLOWED_MAC_COUNT; z++)
                {
                    bool result = CONNECTED_MAC.Equals(ALLOWED_MACS_ARRAY[z], StringComparison.Ordinal);
                    if (result == true)
                    {
                        intrusion = false;
                        break;
                    }
                }

                if (intrusion == true)
                { 
                    label2.Text = "Intrusion detected";
                }

                if (intrusion == false)
                {
                    label2.Text = "No intrusion detected";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Submit_Button_Click(object sender, EventArgs e)
        {
            string New_User = New_User_TextBox.Text;

            if (!ALLOWED_MACS_LIST.Items.Contains(New_User))
            {
                System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt", string.Format("{0}{1}", New_User, Environment.NewLine));
                ALLOWED_MACS_LIST.Items.Add(New_User);
            }
            
        }

    }
}
