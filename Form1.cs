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
            this.Size = new Size(309, 286);
            InitTimer();

        }

        public void arp_dump()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "C:\\Users\\Aviel Resnick\\Desktop\\PJAS\\Control.bat";
            process.StartInfo = startInfo;
            process.Start();
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm.ss");
        }

        public void substring_split()
        {
            int LINES = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\arp_dump.txt").Count() - 1;
            string[] DATA_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\arp_dump.txt");

            System.IO.File.WriteAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt", string.Empty);

            String timestamp = GetTimestamp(DateTime.Now);
            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", timestamp);
            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", string.Format("{0}{1}", "", Environment.NewLine));

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
                        System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", string.Format("{0}{1}", MAC, Environment.NewLine));
                    }
                }
            }

            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", string.Format("{0}{1}", "", Environment.NewLine));

        }

        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 2000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool is_open = false;

            MACS_LIST.Items.Clear();
            ALLOWED_MACS_LIST.Items.Clear();

            is_open = true;
            arp_dump();
            is_open = false;

            if (is_open == false)
            {
                is_open = true;
                substring_split();
                is_open = false;
            }

            if (is_open == false)
            {
                Main();
            }
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
                    label2.Left = 51;
                    label2.ForeColor = System.Drawing.Color.Red;
                    label2.Text = "Intrusion detected";

                    this.Size = new Size(309, 340);
                    Intrusion_Info.Visible = true;
                    Intruder_Mac.Visible = true;
                    Add_To_Regestered.Visible = true;
                    Shutdown.Visible = true;

                    Intruder_Mac.Text = CONNECTED_MAC;

                    break;
                }

                if (intrusion == false)
                {
                    label2.Left = 38;
                    label2.ForeColor = System.Drawing.Color.Blue;
                    label2.Text = "No intrusion detected";

                    this.Size = new Size(309, 286);
                    Intrusion_Info.Visible = false;
                    Intruder_Mac.Visible = false;
                    Add_To_Regestered.Visible = false;
                    Shutdown.Visible = false;
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

        private void History_Button_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "C:\\Users\\Aviel Resnick\\Desktop\\PJAS\\Data\\Refined\\History.txt");
        }

        public void Add_To_Regestered_Click(object sender, EventArgs e)
        {
            string CONNECTED_MAC = Intruder_Mac.Text;

            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt", string.Format("{0}{1}", CONNECTED_MAC, Environment.NewLine));
            ALLOWED_MACS_LIST.Items.Add(CONNECTED_MAC);
        }

        private void Shutdown_Click(object sender, EventArgs e)
        {
            var shutdown = new ProcessStartInfo("shutdown", "/s /t 0");
            shutdown.CreateNoWindow = true;
            shutdown.UseShellExecute = false;
            Process.Start(shutdown);
        }
    }
}
