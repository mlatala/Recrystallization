using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Storage.Game = new Game(200, 200);
            Storage.MainForm = new FormMain();
            Storage.ControlPanel = new FormControlPanel(); 
            Storage.Engine = new Engine();
            Storage.Drawer = new Drawer();

            Storage.ControlPanel.InitForm();

            Storage.Drawer.Init(false, 2, 2, 1);

            Storage.Engine.Init(1);
            Storage.Engine.AddRandomIter();

            Application.Run(Storage.MainForm);
        }
    }
}
