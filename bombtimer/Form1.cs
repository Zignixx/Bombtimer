using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSGSI;
using System.Net;
using System.Media;


using System.Runtime.InteropServices;

using GSI_Test;

namespace bombtimer
{
    public partial class Form1 : Form
    {
        private static Form1 _singleton;
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int Frequenz, int Dauer);
        public Form1()
        {
            _singleton = this;
            InitializeComponent();
        }


 
        /*public string SetText
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }*/


        public void button1_Click(object sender, EventArgs e)
        {
        }
        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you really want to exit?", "[UNiTEDSB.net] Bombtimer", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }   


        public static void Helper1(string txt)
        {
            _singleton.textBox1.Text = txt;
        }
        public static void Helper2(string txt)
        {
            _singleton.button1.Text = txt;
        }
        public static void Helper3(string txt)
        {
            _singleton.label4.Text = txt;
        }
        public static bool SoundSetting()
        {
            return _singleton.checkBox1.Checked;
        }
        public static void Progressbar_Value(int value)
        {
            _singleton.Countdown_progress.Value = value;

        }
        public static void Progressbar_Max(int value)
        {
            _singleton.Countdown_progress.Maximum = value;
        }
        public static Int16 GetC4Time()
        {
            return Convert.ToInt16(_singleton.c4time.Value);
        }
        public static void ColorRed_Phase()
        {
            _singleton.label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        public static void ColorNormal_Phase()
        {
            _singleton.label4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#190707");
        }
        public static void ColorOrange()
        {
            _singleton.button1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF8000");
        }
        public static void ColorRed()
        {
            _singleton.button1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }
        public static void ColorGreen()
        {
            _singleton.button1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00FF00");
        }
        public static void ColorStatusGreen()
        {
            _singleton.textBox1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00FF00");
        }
        public static void ColorStatusRed()
        {
            _singleton.textBox1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
        }

        public static void Updatechecker()
        {
            var url = "http://ts.unitedsb.net/bombtimer/updater.php";
            var textFromFile = (new WebClient()).DownloadString(url);
            int cleanversion_online = Int32.Parse(textFromFile.Replace(".", string.Empty));
            int cleanversion_client = Int32.Parse(Application.ProductVersion.Replace(".", string.Empty));
            //MessageBox.Show("Online: " + cleanversion_online + " Client: " + cleanversion_client, "[UNiTEDSB.net] Bombtimer", MessageBoxButtons.OK);
            if (cleanversion_client >= cleanversion_online)
            {
                MessageBox.Show("You are using the latest version.", "[UNiTEDSB.net] Bombtimer", MessageBoxButtons.OK);

            }
            else
            {
                DialogResult result = MessageBox.Show("A new version (" + textFromFile + ") is aviable to download.\nDownload it now?", "[UNiTEDSB.net] Bombtimer", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://ts.unitedsb.net/bombtimer/download.php");
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Program_Cobra.Main_cobra(null);
            _singleton.Text = "[UNiTEDSB.net] CS:GO Bombtimer " + Application.ProductVersion;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //string temp = c4time.Value.ToString("0.0");
            _singleton.button1.Text = c4time.Value.ToString("0.0");
            Globals.countdowntime = (int)c4time.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Beep(Globals.Hz, 25);
            SoundPlayer sndPlayer = new SoundPlayer(bombtimer.Properties.Resources.piep);
            sndPlayer.Play();
            System.Threading.Thread.Sleep(250);
            sndPlayer.Play();
        }

        private void Countdown_progress_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/id/Cobra/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {



        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("http://" + GSI_Test.Program_Cobra.GetIP4Address() + ":4873/index.html\n\nYou need to open the URL on your Mobile Phone / Tablet / Laptop!\n\n\nOpen it now?", "[UNiTEDSB.net] Bombtimer", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("http://" + GSI_Test.Program_Cobra.GetIP4Address() + ":4873/index.html");
            }
        
        }
    }

}



