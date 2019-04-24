using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public class HitTestInfo
    {
        public CellPointer Coord { get; set; }
        public bool IsFieldHit { get; set; }
        public HitTestInfo()
        {
            IsFieldHit = false;
            Coord = new CellPointer();
        }

        public void Do(int x, int y)
        {
            Rectangle bf;
            Coord.SetCord(-1, -1);
            IsFieldHit = false;
            for (int i = 0; i < Storage.Game.GameRows; i++)
            {
                bf = Storage.Drawer.Board[i, 0];
                if (y >= bf.Y && y <= (bf.Y + bf.Height))
                {
                    for (int j = 0; j < Storage.Game.GameCols; j++)
                    {
                        bf = Storage.Drawer.Board[i, j];
                        if (x >= bf.X && x <= (bf.X + bf.Width))
                        {
                            Coord.SetCord(i, j);
                            IsFieldHit = true;
                            i = Storage.Game.GameRows;
                            break;
                        }
                    }
                }
            }
        }
    }
}
