using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace bombtimer
{
    class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        //[STAThread]
        public static Form1 formInstance;
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(formInstance = new Form1());
            formInstance = new Form1();
            Application.Run(formInstance);
        }
    }
}
