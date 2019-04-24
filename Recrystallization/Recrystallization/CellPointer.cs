using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class CellPointer
    {
        public int I { get; set; }
        public int J { get; set; }

        public CellPointer()
        {
            I = -1;
            J = -1;
        }

        public CellPointer(int i,int j)
        {
            I = i;
            J = j;
        }

        public bool IsEqual(CellPointer cp)
        {
            if (I == cp.I && J == cp.J)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCord(CellPointer cp)
        {
            I = cp.I;
            J = cp.J;
        }

        public void SetCord(int i,int j)
        {
            I = i;
            J = j;
        }

    }
}
