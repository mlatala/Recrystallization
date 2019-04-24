using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameOfLife
{
    public class CustomForm : Form
    {
        public bool CloseEvent { get; set; }

        public CustomForm()
        {
            CloseEvent = true;
            //FormClosing += new FormClosingEventHandler(CloseAllEventHandler);
        }

        private void CloseAllEventHandler(object sender, EventArgs args)
        {
            if (CloseEvent)
            {
                if (sender is FormMain)
                {
                    Storage.ControlPanel.CloseEvent = false;
                    Storage.ControlPanel.Close();
                }
                else
                {
                    Storage.MainForm.CloseEvent = false;
                    Storage.MainForm.Close();
                }
            }
        }
    }
}
