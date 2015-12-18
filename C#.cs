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
        // Initialization
        public Form1()
        {
            InitializeComponent();
            // Screen Size (Default)
            this.Size = new Size(309, 286);
            InitTimer();
        }
        
        // Start a windows process (Control.bat)
        public void arp_dump()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "C:\\Users\\Aviel Resnick\\Desktop\\PJAS\\Control.bat";
            process.StartInfo = startInfo;
            process.Start();
        }
        
        // Timestamp for the history option
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm.ss");
        }

        public void substring_split()
        {
            // Data Collection
            int LINES = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\arp_dump.txt").Count() - 1;
            string[] DATA_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\arp_dump.txt");
            
            // Clears the exisiting file
            System.IO.File.WriteAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt", string.Empty);
            
            // Timestamp for History
            String timestamp = GetTimestamp(DateTime.Now);
            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", timestamp);
            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", string.Format("{0}{1}", "", Environment.NewLine));
            
            // For every line in arp_dump.txt
            for (int i = 0; i < LINES; i++)
            {
                string LINE = DATA_ARRAY[i];
                int LINE_LENGTH = LINE.Count();
                // Verify that the line is greater than 4 characters
                if (LINE_LENGTH > 4)
                {   
                    // Use substrings to identify the lines with the MAC address by checking the structure
                    string CHAR = LINE.Substring(0, 5);

                    bool CORRECT_LINE = CHAR.Equals("  192", StringComparison.Ordinal);
                    
                    // For correct lines collect the MAC address and store the string
                    if (CORRECT_LINE == true)
                    {
                        string MAC = LINE.Substring(24, 17);
                        // Append the MAC address to MACS.txt and History.txt
                        System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt", string.Format("{0}{1}", MAC, Environment.NewLine));
                        System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", string.Format("{0}{1}", MAC, Environment.NewLine));
                    }
                }
            }
            
            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\History.txt", string.Format("{0}{1}", "", Environment.NewLine));

        }

        public void InitTimer()
        {
            // Every 2 Seconds (2000 Milliseconds) call timer1_Tick
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 2000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // is_open is used to prevent overlapping between arp_dump, substring_split(), and Main()
            bool is_open = false;
            
            // MACS_LIST and ALLOWED_MACS_LIST are modified dropdown boxes used to present information to the user
            // Clearing is required for showing real-time data
            MACS_LIST.Items.Clear();
            ALLOWED_MACS_LIST.Items.Clear();
            
            // While arp_dump is using files that substring_split() and Main() require is_open is true
            is_open = true;
            arp_dump();
            is_open = false;
            
            // If is_open is false, meaning none of the files are being used, substring_split can refine the raw data from arp_dump
            // without risk of overlapping and crashing the program.
            if (is_open == false)
            {
                is_open = true;
                substring_split();
                is_open = false;
            }
            
            // The same bool based system is applied to the transition from substring_split to Main()
            if (is_open == false)
            {
                Main();
            }
        }

        public void Main()
        {
            // Read lines from MACS.txt (a refined list of connected users) and Allowed_Users.txt (Self-Explanatory) and store them in arrays 
            int CONNECTED_MAC_COUNT = System.IO.File.ReadLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt").Count();
            int ALLOWED_MAC_COUNT = System.IO.File.ReadLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt").Count();
            string[] MACS_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Refined\MACS.txt");
            string[] ALLOWED_MACS_ARRAY = System.IO.File.ReadAllLines(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt");
            
            // Read each line of MACS_ARRAY and add each item to MACS_LIST (dropdown menu)
            for (int i = 0; i < CONNECTED_MAC_COUNT; i++)
            {
                if (!MACS_LIST.Items.Contains(MACS_ARRAY[i]))
                {
                    MACS_LIST.Items.Add(MACS_ARRAY[i]);
                }
            }
            
            // Read each line of ALLOWED_MACS_ARRAY and add each item to ALLOWED_MACS_LIST (dropdown menu)
            for (int x = 0; x < ALLOWED_MAC_COUNT; x++)
            {
                if (!ALLOWED_MACS_LIST.Items.Contains(ALLOWED_MACS_ARRAY[x]))
                {
                    ALLOWED_MACS_LIST.Items.Add(ALLOWED_MACS_ARRAY[x]);
                }
            }
          
            // For every connected device compare the MAC with every MAC in Allowed_MACS_ARRAY
            // If during any iteration a MAC is identified as true, break out of the loop
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
                
                // If a connected device is not on the list of regestered users alert the user
                if (intrusion == true)
                {
                    // Create a large label with red letters to spell Intrusion Detected
                    label2.Left = 51;
                    label2.ForeColor = System.Drawing.Color.Red;
                    label2.Text = "Intrusion detected";
                    
                    //Expand the window size, as well as give the user the option to add the "intruder" to regestered users
                    // Or shutdown to prevent data loss.
                    this.Size = new Size(309, 340);
                    Intrusion_Info.Visible = true;
                    Intruder_Mac.Visible = true;
                    Add_To_Regestered.Visible = true;
                    Shutdown.Visible = true;
                    
                    // Show the user the MAC address of the possible intruder
                    Intruder_Mac.Text = CONNECTED_MAC;
                    break;
                }
                
                // If there is no intrusions reset to the defualt look
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
            //Empty
        }
        private void Submit_Button_Click(object sender, EventArgs e)
        {
            string New_User = New_User_TextBox.Text;
            
            // Add a user to regestered users, as well as the dropdown menu, if the MAC in the input field is not already on the list
            if (!ALLOWED_MACS_LIST.Items.Contains(New_User))
            {
                System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt", string.Format("{0}{1}", New_User, Environment.NewLine));
                ALLOWED_MACS_LIST.Items.Add(New_User);
            }
        }
        
        // When the user clicks the history button open the history text file
        private void History_Button_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "C:\\Users\\Aviel Resnick\\Desktop\\PJAS\\Data\\Refined\\History.txt");
        }
        
        // If the user selects to add the possible intruder to the regestered users list, proccede similarly to adding a new user
        public void Add_To_Regestered_Click(object sender, EventArgs e)
        {
            string CONNECTED_MAC = Intruder_Mac.Text;

            System.IO.File.AppendAllText(@"C:\Users\Aviel Resnick\Desktop\PJAS\Data\Allowed_Users.txt", string.Format("{0}{1}", CONNECTED_MAC, Environment.NewLine));
            ALLOWED_MACS_LIST.Items.Add(CONNECTED_MAC);
        }

        private void Shutdown_Click(object sender, EventArgs e)
        
        // If the user selects the shutdown option to prevent data loss, execute a shutdown command
        {
            var shutdown = new ProcessStartInfo("shutdown", "/s /t 0");
            shutdown.CreateNoWindow = true;
            shutdown.UseShellExecute = false;
            Process.Start(shutdown);
        }
    }
}
