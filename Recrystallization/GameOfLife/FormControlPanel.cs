using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class FormControlPanel : CustomForm
    {
        public int Cnt { get; set; }

        public class GridEntry
        {
            public int Nr { get; set; }
        }

        public FormControlPanel()
        {
            Cnt = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //textBox4.Text = (GC.GetTotalMemory(true) / 1024f).ToString("0.000") + "KB";
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Storage.Engine.Interval = Convert.ToInt64(1000 / 5);
        }

        public void InitForm()
        {
            FormCollection fc = Application.OpenForms;
            int ct = 0;
            foreach (Form f in fc)
            {
                if (f is FormControlPanel)
                {
                    ct++;
                }
            }
            if (ct == 0)
            {
                InitializeComponent();
                //comboBox1.SelectedIndex = 0;
                Show();
            }
            else
            {
                Close();
            }
        }

        public void UpdateGameInfo(float ips)
        {
            if (ips != -1)
            {
                //textBox1.Text = ips.ToString("0.00");
            }
            //textBox3.Text = Storage.Game.Iterations.ToString();
            if (Storage.Game.GameState == Game.GameStates.Run)
            {
              //  textBox2.BackColor = Color.Tomato;
                //textBox2.Text = "RUN";
            }
            else
            {
                //textBox2.BackColor = Color.YellowGreen;
                //textBox2.Text = "PAUSED";
            }
            //numericUpDown4.Maximum = Storage.Game.Generations.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                button1.Text = "Stop";
                Storage.Engine.Run(0, true);
            }
            else
            {
                button1.Text = "Start";
                Storage.Engine.Stop();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Storage.Engine.Run(1,true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Storage.Engine.Run(5,true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                Storage.Engine.Run(5, false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Storage.Engine.Stop();
        }

        private void FormControlPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                //numericUpDown4.Value = numericUpDown4.Maximum;
                Storage.Drawer.DrawIteration(-1);
                Storage.MainForm.RefreshBoard();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                int x = 5;
                Storage.Drawer.DrawIteration(x);
                Storage.MainForm.RefreshBoard();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                int x =5;
                if (x > 0)
                {
                    //numericUpDown4.Value = x - 1;
                    Storage.Drawer.DrawIteration(x-1);                 
                }
                else
                    Storage.Drawer.DrawIteration(0);
                Storage.MainForm.RefreshBoard();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                int x = 5;
                if (x < Storage.Game.Iterations - 1)
                {
                 //   numericUpDown4.Value = x + 1;
                    Storage.Drawer.DrawIteration(x + 1);           
                }
                else
                    Storage.Drawer.DrawIteration(x);  
                Storage.MainForm.RefreshBoard();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public int GetEditMode()
        {
            return 0;
            //return comboBox1.SelectedIndex;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused && Storage.Drawer.LastDrawnIndex == Storage.Game.Generations.Count - 1)
            {
                for (int i = 0; i < Storage.Game.GameRows; i++)
                {
                    for (int j = 0; j < Storage.Game.GameCols; j++)
                    {
                        Storage.Game.Current[i, j].State = 0;
                    }
                }
                Storage.Drawer.DrawIteration(-1);
                Storage.MainForm.RefreshBoard();
                Storage.ControlPanel.UpdateGameInfo(-1);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused && Storage.Drawer.LastDrawnIndex == Storage.Game.Generations.Count - 1)
            {
                Random r = new Random();
                for (int i = 0; i < Storage.Game.GameRows; i++)
                {
                    for (int j = 0; j < Storage.Game.GameCols; j++)
                    {
                        Storage.Game.Current[i, j].State = (byte)r.Next(0, 2);
                    }
                }    
                Storage.Drawer.DrawIteration(-1);
                Storage.MainForm.RefreshBoard();
                Storage.ControlPanel.UpdateGameInfo(-1);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Paused)
            {
                DialogResult dr = MessageBox.Show("Are you sure want to reset all game infos?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    Storage.Game = new Game(Storage.Game.GameRows, Storage.Game.GameCols);
                    Storage.Engine.AddRandomIter();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        public void BringToFront()
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormControlPanel_Load(object sender, EventArgs e)
        {

        }

    }
}
