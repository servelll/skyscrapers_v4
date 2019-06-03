using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skyscrapers_v4
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
		[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);      
			Application.Run(new Form1());
        }
    }
	public static class CallBackMy
	{
		public delegate void callbackEvent(string what);
		public static callbackEvent callbackEventHandler;
        public delegate void callbackEvent2(string what, bool clear=false);
        public static callbackEvent2 callbackEventHandler2;
	}
}
