using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public class Paterns
    {
        public List<byte[,]> Stables { get; set; }

        public Paterns()
        {
            Stables = new List<byte[,]>();
            

        }

        public byte[,] TxtToPatern(string[] pat)
        {
            int width = pat[0].Length;
            int height = pat.Length;
            byte[,] res = new byte[width,height];
            for (int i = 0; i < height;i++)
            {
                for(int j = 0;j < width;j++)
                {
                    res[i, j] = Byte.Parse(pat[i][j].ToString());
                }
            }
            return res;
        }
    }
}
