using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GameOfLife
{
    public partial class ZoomBox : UserControl
    {
        public float Zoom { get; set; }
        public float ZoomStep { get; set; }
        public float MaxZoom { get; set; }
        public float MinZoom { get; set; }
        public Bitmap Zoomed { get; set; }
        public Bitmap Source { get; set; }

        public ZoomBox()
        {
            InitializeComponent();
        }

        public void SetUp()
        {
            UiConf();
            Zoom = 1f;
            ZoomStep = 0.1f;
            MaxZoom = 1f;
            MinZoom = 0.2f;         
        }

        public void SetSource(Bitmap bmp)
        {
            Source = bmp;
        }

        public void LoadImg()
        {
            if (Zoom == 1)
            {
                pictureBox.Image = Source;
            }
            else
            {
                int newWidth = (int)(Zoom * Source.Width);
                int newHeight = (int)(Zoom * Source.Height);
                if (newWidth != 0 && newHeight != 0)
                {
                    Zoomed = new Bitmap(newWidth, newHeight);
                    using (Graphics g = Graphics.FromImage(Zoomed))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                        g.DrawImage(Source, 0, 0, newWidth, newHeight);
                    }
                    pictureBox.Image = Zoomed;
                }
            }
            pictureBox.Invalidate();
        }

        public void ZoomIn()
        {
            if (Zoom < MaxZoom)
                Zoom = Zoom + ZoomStep;
        }

        public void ZoomOut()
        {
            if (Zoom > MinZoom)
                Zoom = Zoom - ZoomStep;
        }

        public void UiConf()
        {
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            panel.MouseWheel += new MouseEventHandler(WheelEvent);
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            Dock = DockStyle.Fill;
        }

        private void WheelEvent(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomIn();
                LoadImg();
            }
            else
            {
                ZoomOut();
                LoadImg();
            }
        }

        private void panel_MouseEnter(object sender, EventArgs e)
        {
            if (Form.ActiveForm == ParentForm)
            {
                if (!pictureBox.Focused)
                    pictureBox.Focus();
            }
                    
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (Form.ActiveForm == ParentForm)
            {
                if (!pictureBox.Focused)
                    pictureBox.Focus();
            }
        }

        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                Storage.MainForm.CreateContextMenu(p.X, p.Y);
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                Storage.MainForm.CreateContextMenu(p.X, p.Y);
        }



    }
}
