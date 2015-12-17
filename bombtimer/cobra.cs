using System;
using CSGSI;
using bombtimer;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Media;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SimpleWebServer;


public class Globals
{
    public static Int32 countdown = 0;
    public static Int32 countdowntime = 40;
    public static Int32 Hz = 1000;
    public static Int32 c4time;
    public static string Phase = "None";
    public static string Color = "#00FF00";
}


namespace GSI_Test
{
    class Program_Cobra
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int Frequenz, int Dauer);
        private static string s_path;

        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        public static void StartPhoneServer()
        {
            //
            //Webserver for Mobiles
            s_path = Environment.CurrentDirectory + "/Web";
            //s_path = bombtimer.Properties.Resources.index;
            //WebServer ws = new WebServer(SendResponse, "http://localhost:4873/");
            WebServer ws = new WebServer(SendResponse, "http://" + GetIP4Address() + ":4873/");
            ws.Run();
        }

       
        public static void Main_cobra(string[] args)
        {
            string sPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //subscribe to the NewGameState event
            GSIListener.NewGameState += new EventHandler(OnNewGameState);
            //start listening on http://127.0.0.1:4872/
            if (GSIListener.Start(4872))
            {

                Form1.Helper1("Started...");
                Form1.ColorGreen();
                Form1.ColorStatusGreen();
                StartPhoneServer();
            }
            else
            {
                Form1.Helper1("Error. Start me as Admin!");
                Beep(Globals.Hz, 25);
                Form1.ColorRed();
                Form1.ColorStatusRed();
                Form1.Helper2("0");
            }
            Form1.Updatechecker();
        }

        private static Response SendResponse(HttpListenerRequest request)
        {
            string physicalRequest = request.Url.AbsolutePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            string physicalFile = Path.Combine(s_path, physicalRequest);
            string tmp;
            string tmp2;
            byte[] b1;
            //Console.WriteLine("Serving : {0}", physicalRequest);
            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = File.OpenRead(physicalFile))
            {
                byte[] buffer = new byte[4096];
                int read;
                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                {

                    tmp = Encoding.Default.GetString(buffer);
                    tmp2 = tmp.Replace("%TIME%", (Globals.countdowntime).ToString("0")).Replace("%PHASE%", Globals.Phase).Replace("%COLOR%", Globals.Color);
                    b1 = System.Text.Encoding.UTF8.GetBytes(tmp2);
                    ms.Write(b1, 0, read);
                }
                //Console.WriteLine(ms);
                return new Response()
                {
                    Content = ms.ToArray()
                };
            }
        }

        public static void OnNewGameState(object sender, EventArgs e)
        {
            
            GameState gs = (GameState)sender;
            GameStateNode playerNode = gs.Round;

            string bomb_state = playerNode.GetValue("bomb");
            int C4Time = Form1.GetC4Time();
            Globals.c4time = C4Time;
            string round_phase = playerNode.GetValue("phase");
            bool sounds = Form1.SoundSetting();
            
            
            if(round_phase == "freezetime")
            {
                Form1.Helper3("Freezetime");
                Form1.ColorRed_Phase();
                Globals.Phase = "Freezetime";
            }
            if (round_phase == "live") {
                Form1.Helper3("Live");
                Form1.ColorRed_Phase();
                Globals.Phase = "Live";
            }
            if (round_phase == "warmup")
            {
                Form1.Helper3("Warmup");
                Form1.ColorRed_Phase();
                Globals.Phase = "Warmup";
            }
            if (round_phase == "over")
            {
                Form1.Helper3("Round is over");
                Form1.ColorRed_Phase();
                Globals.Phase = "Round is over";
            }
            if (!(round_phase == "freezetime") && !(round_phase == "live") && !(round_phase == "warmup") && !(round_phase == "over"))
            {
                Form1.Helper3("None");
                Form1.ColorNormal_Phase();
                Globals.Phase = "None";
            }
            if (bomb_state == "defused" || bomb_state == "exploded" || bomb_state == "")
            {
                Globals.countdowntime = C4Time;
                Globals.countdown = 0;
                Form1.Helper2(C4Time.ToString());
                Form1.ColorGreen();
                Globals.Color = "#00FF00";
                Form1.Progressbar_Max(C4Time);
                Form1.Progressbar_Value(C4Time);

            }
            if (bomb_state == "planted" && Globals.countdown == 0)
            {
                Globals.countdown = 1;
                for (int i = C4Time; i >= 0;i-- )
                    
                {
                    if (Globals.countdown == 1)
                    {
                        if (i <= 5)
                        {
                            Form1.ColorRed();
                            Globals.Color = "#FF0000";
                        }
                        if (i <= 10 && i > 5)
                        {
                            Form1.ColorOrange();
                            Globals.Color = "#FF8000";
                        }
                        if(i > 10)
                        {
                            Form1.ColorGreen();
                            Globals.Color = "#00FF00";
                        }
                        if(i == 10 && sounds)
                        {
                            //Beep(Globals.Hz, 25);
                            SoundPlayer sndPlayer = new SoundPlayer(bombtimer.Properties.Resources.piep);
                            sndPlayer.Play();
                        }
                        if (i == 5 && sounds)
                        {
                            //Beep(Globals.Hz, 25);
                            //Beep(Globals.Hz, 25);
                            SoundPlayer sndPlayer = new SoundPlayer(bombtimer.Properties.Resources.piep);
                            sndPlayer.Play();
                            System.Threading.Thread.Sleep(250);
                            sndPlayer.Play();
                        }
                        Form1.Helper2(string.Format("{0}", i));
                        Form1.Progressbar_Value((int)i);
                        Globals.countdowntime = i;
                        System.Threading.Thread.Sleep(995);
                        
                    }
                }

            }
        }
    }
}