using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Game
    {

        public enum GameStates { Paused, Run }
        public GameStates GameState { get; set; }
        public Cell[,] Current { get; set; }
        public List<Cell[,]> Generations { get; set; }
        public int Iterations { get; set; }

        public Dictionary<int, CellState> CellStates { get; set; }
        public int GameRows { get; set; }
        public int GameCols { get; set; }
        public int BorderSize { get; set; }


        public Game(int r, int c)
        {
            GameRows = r;
            GameCols = c;
            BorderSize = 1;
            Iterations = 0;

            GameState = GameStates.Paused;
            Generations = new List<Cell[,]>();

            Current = Storage.InitializeArray<Cell>(GameRows, GameCols);
            CellStates = new Dictionary<int, CellState>();
            CellStates.Add(0, new CellState("Dead", Color.White));
            CellStates.Add(1, new CellState("Alive", Color.BlueViolet));
        }

        public void StartNewIteration()
        {
            Current = Storage.InitializeArray<Cell>(GameRows, GameCols);
        }

        public void UpdateNewIteration(int startCol, int endCol)
        {
            Cell[,] Previous = Generations.Last();
            int mode = Storage.Mode;
            if (startCol == -1 || endCol == -1)
            {
                startCol = 0;
                endCol = GameCols - 1;
            }
            for (int i = 0; i < GameRows; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    if(mode==0)
                        Moore(Previous,i,j);
                    else if (mode == 1)
                        VonNeumann(Previous, i, j);
                    else if (mode == 2)
                        HeksagonalL(Previous, i, j);
                    else if (mode == 3)
                        HeksagonalR(Previous, i, j);
                    else if (mode == 4)
                        HeksagonalRand(Previous, i, j);
                    else if (mode == 5)
                        PentagonalRand(Previous, i, j);
                }
            }
        }


        public void EndIteration()
        {
            Iterations++;
            Generations.Add(Current);
        }

        public Cell[] Neightbourhood(Cell[,] cells,int i,int j)
        {
            Cell[] res = new Cell[9];
            int counter = 0;
            for (int k = i - 1; k <= i + 1;k++ )
            {
                for(int l = j - 1;l <= j + 1;l++)
                {
                    if (Storage.ControlPanel.periodicBox())
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        else if(k==GameRows && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[1,l];
                        }
                        else if (k ==-1 && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[GameRows-1, l];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l ==GameCols && !(k == i && l == j))
                        {
                            res[counter] = cells[k, 1];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == -1 && !(k == i && l == j))
                        {
                            res[counter] = cells[k, GameCols-1];
                        }
                    }
                    else
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        /*else
                        {
                            res[counter] = -1;
                        }*/
                    }
                    counter++;
                }
            }
            return res;
        }

        public Cell[] Neightbourhood2(Cell[,] cells, int i, int j)
        {
            Cell[] res = new Cell[5];
            int counter = 0;
            for (int k = i - 1; k <= i + 1; k++)
            {

                if (k != i)
                {
                    int l = j;
                    if (Storage.ControlPanel.periodicBox())
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        else if (k == GameRows && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[1, l];
                        }
                        else if (k == -1 && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[GameRows - 1, l];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == GameCols && !(k == i && l == j))
                        {
                            res[counter] = cells[k, 1];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == -1 && !(k == i && l == j))
                        {
                            res[counter] = cells[k, GameCols - 1];
                        }
                    }
                    else
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                    }
                    counter++;
                }
                else if (k == i)
                {
                    for (int l = j - 1; l <= j + 1; l++)
                    {
                        if (Storage.ControlPanel.periodicBox())
                        {
                            if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                            {
                                res[counter] = cells[k, l];
                            }
                            else if (k == GameRows && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                            {
                                res[counter] = cells[1, l];
                            }
                            else if (k == -1 && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                            {
                                res[counter] = cells[GameRows - 1, l];
                            }
                            else if (k >= 0 && (k <= GameRows - 1) && l == GameCols && !(k == i && l == j))
                            {
                                res[counter] = cells[k, 1];
                            }
                            else if (k >= 0 && (k <= GameRows - 1) && l == -1 && !(k == i && l == j))
                            {
                                res[counter] = cells[k, GameCols - 1];
                            }
                        }
                        else
                        {
                            if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                            {
                                res[counter] = cells[k, l];
                            }
                            /*else
                            {
                                res[counter] = -1;
                            }*/
                        }
                        counter++;
                    }
                }               

            }
            return res;
        }

        public Cell[] Neightbourhood3(Cell[,] cells, int i, int j)
        {
            Cell[] res = new Cell[6];
            int counter = 0;
            for (int k = i - 1; k <= i + 1; k++)
            {
                for (int l = j; l <= j + 1; l++)
                {
                    if (Storage.ControlPanel.periodicBox())
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        else if (k == GameRows && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[1, l];
                        }
                        else if (k == -1 && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[GameRows - 1, l];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == GameCols && !(k == i && l == j))
                        {
                            res[counter] = cells[k, 1];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == -1 && !(k == i && l == j))
                        {
                            res[counter] = cells[k, GameCols - 1];
                        }
                    }
                    else
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        /*else
                        {
                            res[counter] = -1;
                        }*/
                    }
                    counter++;
                }

            }
            return res;
        }

        public Cell[] Neightbourhood4(Cell[,] cells, int i, int j)
        {
            Cell[] res = new Cell[6];
            int counter = 0;
            for (int k = i - 1; k <= i + 1; k++)
            {

                    for (int l = j - 1; l <= j; l++)
                    {
                    if (Storage.ControlPanel.periodicBox())
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        else if (k == GameRows && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[1, l];
                        }
                        else if (k == -1 && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[GameRows - 1, l];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == GameCols && !(k == i && l == j))
                        {
                            res[counter] = cells[k, 1];
                        }
                        else if (k >= 0 && (k <= GameRows - 1) && l == -1 && !(k == i && l == j))
                        {
                            res[counter] = cells[k, GameCols - 1];
                        }
                    }
                    else
                    {
                        if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                        {
                            res[counter] = cells[k, l];
                        }
                        /*else
                        {
                            res[counter] = -1;
                        }*/
                    }
                        counter++;
                    }
                
            }
            return res;
        }

        public Cell[] Neightbourhood5(Cell[,] cells, int i, int j)
        {
            Cell[] res = new Cell[6];
            int counter = 0;
            Random rnd = new Random();
            int[,] temp = new int[6,2];
            for (int r = 0; r <= 5; r++)
            {
                int flag = 0;
                int k = i + rnd.Next(-1, 1);
                int l = j + rnd.Next(-1, 1);
                
                if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                {
                    for (int konczamisieliterywalfabecie = 0; konczamisieliterywalfabecie < counter; konczamisieliterywalfabecie++)
                    {
                        if (temp[konczamisieliterywalfabecie, 0] == k && temp[konczamisieliterywalfabecie, 1] == l)
                        { 
                        flag = 1;
                        }
                    }

                    if (flag == 0)
                    {
                    temp[counter, 0] = k;
                    temp[counter, 1] = l;
                    res[counter] = cells[k, l];
                    }
                }
                /*else
                {
                    res[counter] = -1;
                }*/

                if (flag == 0)
                    counter++;
                else
                    r--;
                               
            }
            return res;
        }
        public Cell[] Neightbourhood6(Cell[,] cells, int i, int j)
        {
            Cell[] res = new Cell[5];
            int counter = 0;
            Random rnd = new Random();
            int[,] temp = new int[5, 2];
            for (int r = 0; r <= 4; r++)
            {
                int flag = 0;
                int k = i + rnd.Next(-1, 1);
                int l = j + rnd.Next(-1, 1);

                if (k >= 0 && (k <= GameRows - 1) && l >= 0 && (l <= GameCols - 1) && !(k == i && l == j))
                {
                    for (int konczamisieliterywalfabecie = 0; konczamisieliterywalfabecie < counter; konczamisieliterywalfabecie++)
                    {
                        if (temp[konczamisieliterywalfabecie, 0] == k && temp[konczamisieliterywalfabecie, 1] == l)
                        {
                            flag = 1;
                        }
                    }

                    if (flag == 0)
                    {
                        temp[counter, 0] = k;
                        temp[counter, 1] = l;
                        res[counter] = cells[k, l];
                    }
                }
                /*else
                {
                    res[counter] = -1;
                }*/

                if (flag == 0)
                    counter++;
                else
                    r--;

            }
            return res;
        }

        public void Moore(Cell[,] Previous,int i,int j)
        {
            int num = 0;
            Cell[] temp = Neightbourhood(Previous, i, j);
            for (int k = 0; k < temp.Length; k++)
            {
                if (temp[k].GetState() >= 1)
                {
                    num++;
                }
            }
            if (Previous[i, j].State == 0)
            {
                if (num > 0)
                {
                    for (int a = 0; a < 9; a++)
                        if (temp[a].State > 0)
                            Current[i, j].State = temp[a].State;
                }

            }
            if (Previous[i, j].State >= 1)
                Current[i, j].State = Previous[i, j].State;
        }

        public void VonNeumann(Cell[,] Previous, int i, int j)
        {
            int num = 0;
            Cell[] temp = Neightbourhood2(Previous, i, j);
            for (int k = 0; k < temp.Length; k++)
            {
                if (temp[k].GetState() >= 1)
                {
                    num++;
                }
            }
            if (Previous[i, j].State == 0)
            {
                if (num > 0)
                {
                    for (int a = 0; a < 5; a++)
                        if (temp[a].State > 0)
                            Current[i, j].State = temp[a].State;
                }

            }
            if (Previous[i, j].State >= 1)
                Current[i, j].State = Previous[i, j].State;
        }

        public void HeksagonalL(Cell[,] Previous, int i, int j)
        {
            int num = 0;
            Cell[] temp = Neightbourhood3(Previous, i, j);
            for (int k = 0; k < temp.Length; k++)
            {
                if (temp[k].GetState() >= 1)
                {
                    num++;
                }
            }
            if (Previous[i, j].State == 0)
            {
                if (num > 0)
                {
                    for (int a = 0; a < 6; a++)
                        if (temp[a].State > 0)
                            Current[i, j].State = temp[a].State;
                }

            }
            if (Previous[i, j].State >= 1)
                Current[i, j].State = Previous[i, j].State;
        }

        public void HeksagonalR(Cell[,] Previous, int i, int j)
        {
            int num = 0;
            Cell[] temp = Neightbourhood4(Previous, i, j);
            for (int k = 0; k < temp.Length; k++)
            {
                if (temp[k].GetState() >= 1)
                {
                    num++;
                }
            }
            if (Previous[i, j].State == 0)
            {
                if (num > 0)
                {
                    for (int a = 0; a < 6; a++)
                        if (temp[a].State > 0)
                            Current[i, j].State = temp[a].State;
                }

            }
            if (Previous[i, j].State >= 1)
                Current[i, j].State = Previous[i, j].State;
        }

        public void HeksagonalRand(Cell[,] Previous, int i, int j)
        {
            int num = 0;
            Cell[] temp = Neightbourhood5(Previous, i, j);
            for (int k = 0; k < temp.Length; k++)
            {
                if (temp[k].GetState() >= 1)
                {
                    num++;
                }
            }
            if (Previous[i, j].State == 0)
            {
                if (num > 0)
                {
                    for (int a = 0; a < 6; a++)
                        if (temp[a].State > 0)
                            Current[i, j].State = temp[a].State;
                }

            }
            if (Previous[i, j].State >= 1)
                Current[i, j].State = Previous[i, j].State;
        }

        public void PentagonalRand(Cell[,] Previous, int i, int j)
        {
            int num = 0;
            Cell[] temp = Neightbourhood6(Previous, i, j);
            for (int k = 0; k < temp.Length; k++)
            {
                if (temp[k].GetState() >= 1)
                {
                    num++;
                }
            }
            if (Previous[i, j].State == 0)
            {
                if (num > 0)
                {
                    for (int a = 0; a < 5; a++)
                        if (temp[a].State > 0)
                            Current[i, j].State = temp[a].State;
                }

            }
            if (Previous[i, j].State >= 1)
                Current[i, j].State = Previous[i, j].State;
        }


    }
}
