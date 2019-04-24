using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{

    public partial class FormMain : CustomForm
    {

        public CellPointer LastClicked { get; set; }


        public FormMain()
        {
            LastClicked = new CellPointer(-1, -1);
            UiConfig(); 
        }

        public void UiConfig()
        {
            InitializeComponent();
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            WindowState = FormWindowState.Maximized;
        }

        public void RefreshBoard()
        {
            pictureBox1.Image = Storage.Drawer.Map;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Random r = new Random();
            Storage.Game.CellStates.Add(Storage.States, new CellState("Alive", Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0)));
           

            Point p = PointToClient(Cursor.Position);
            if (e.Button == MouseButtons.Right)
            {
                CreateContextMenu(p.X, p.Y);
            }

            if (Storage.Game.GameState == Game.GameStates.Paused && Storage.Drawer.LastDrawnIndex == Storage.Game.Generations.Count - 1)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Storage.Drawer.HitTest.Do(e.X, e.Y);
                    if (Storage.Drawer.HitTest.IsFieldHit && !LastClicked.IsEqual(Storage.Drawer.HitTest.Coord))
                    {
                        int i = Storage.Drawer.HitTest.Coord.I;
                        int j = Storage.Drawer.HitTest.Coord.J;

                        Storage.Game.Current[i, j].State = Storage.States;

                        Storage.Drawer.DrawField(Storage.Game.Current[i, j], Storage.Drawer.Board[i, j]);
                        RefreshBoard();
                        LastClicked.SetCord(Storage.Drawer.HitTest.Coord);
                    }
                }
                else
                {
                    LastClicked.I = -1;
                    LastClicked.J = -1;
                }
            }
            Storage.States++;
        }

        public void CreateContextMenu(int x,int y)
        {
            ContextMenu m = new ContextMenu();
            AddContextPosition("Control panel", new EventHandler(ContextMenu_ShowControlPanel), m.MenuItems);
            m.MenuItems.Add("-");
            AddContextPosition("Close", new EventHandler(ContextMenu_Close), m.MenuItems);
            m.Show(this, new Point(x, y));
        }

        public void AddContextPosition(string txt, EventHandler eh, Menu.MenuItemCollection mic)
        {
            MenuItem it = new MenuItem(txt);
            it.Click += eh;
            mic.Add(it);
        }

        private void ContextMenu_ShowControlPanel(object sender, EventArgs e)
        {
            Storage.ControlPanel.BringToFront();
        }

        private void ContextMenu_Close(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            Storage.ControlPanel.BringToFront();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                CreateContextMenu(p.X, p.Y);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Storage.Game.GameState == Game.GameStates.Run)
            {
                MessageBox.Show("Stop simulation before close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        ///
        ///
        ///
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            Random r = new Random();
            Storage.Game.CellStates.Add(Storage.States, new CellState("Alive", Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0)));

            if (Storage.Game.GameState == Game.GameStates.Paused && Storage.Drawer.LastDrawnIndex == Storage.Game.Generations.Count - 1)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Storage.Drawer.HitTest.Do(e.X, e.Y);
                    if (Storage.Drawer.HitTest.IsFieldHit && !LastClicked.IsEqual(Storage.Drawer.HitTest.Coord))
                    {
                        int i = Storage.Drawer.HitTest.Coord.I;
                        int j = Storage.Drawer.HitTest.Coord.J;

                            Storage.Game.Current[i, j].State = Storage.States;
                     
                            Storage.Drawer.DrawField(Storage.Game.Current[i, j], Storage.Drawer.Board[i, j]);
                            RefreshBoard();
                            LastClicked.SetCord(Storage.Drawer.HitTest.Coord);
                    }
                }
                else
                {
                    LastClicked.I = -1;
                    LastClicked.J = -1;
                }
            }
            Storage.States++;
        }    
    }
}
