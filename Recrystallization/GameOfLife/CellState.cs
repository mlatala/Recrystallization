using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    [Serializable]
    public class CellState
    {
        public string Name { get; set; }
        public Color BoardColor { get; set; }
        public SolidBrush BoardBrush { get; set; }

        public CellState(string str, Color c)
        {
            Name = str;
            BoardColor = c;
            BoardBrush = new SolidBrush(c);
        }
    }
}
