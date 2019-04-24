using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public class Drawer
    {
        public Bitmap Map { get; set; }
        public Graphics G { get; set; }
        public int BorderSize { get; set; }
        public Rectangle[,] Board { get; set; }
        public HitTestInfo HitTest { get; set; }
        public int LastDrawnIndex { get; set; }

        public Drawer()
        {
            HitTest = new HitTestInfo();
            Board = Storage.InitializeArray<Rectangle>(Storage.Game.GameRows, Storage.Game.GameCols);            
        }

        public void Init(bool fit, int sizeX, int sizeY, int borderSize)
        {
            BorderSize = borderSize;
            if (fit)
                GenFitBoard(sizeX, sizeY);
            else
                GenFixedBoard(sizeX, sizeY);
            GraphicsExtensions.ToLowQuality(G);
        }

        public void DrawCells(int width, int height, Cell[,] cells)
        {
            DrawBackground(width, height);
            Dictionary<int, List<Rectangle>> stateGroups = new Dictionary<int, List<Rectangle>>();
            foreach (KeyValuePair<int, CellState> pair in Storage.Game.CellStates)
            {
                stateGroups.Add(pair.Key, new List<Rectangle>());
            }
            for (int i = 0; i < Storage.Game.GameRows; i++)
            {
                for (int j = 0; j < Storage.Game.GameCols; j++)
                {
                    stateGroups[cells[i, j].State].Add(Board[i, j]);
                }
            }
            foreach (KeyValuePair<int, List<Rectangle>> pair in stateGroups)
            {
                if (pair.Value.Count != 0)
                    G.FillRectangles(Storage.Game.CellStates[pair.Key].BoardBrush, pair.Value.ToArray());
            }
        }

        public void DrawIteration(int index)
        {               
            if (index != -1)
            {
                DrawCells(Map.Width, Map.Height,
                    Storage.Game.Generations[index]);
                LastDrawnIndex = index;
            }
            else
            {
                DrawCells(Map.Width, Map.Height,
                    Storage.Game.Current);
                LastDrawnIndex = Storage.Game.Generations.Count - 1;
            }         
        }

        public void DrawBackground(int width, int height)
        {
            SolidBrush background = new SolidBrush(Color.Black);
            G.FillRectangle(background, new Rectangle(0, 0, width, height));
        }


        public void DrawField(Cell c, Rectangle r)
        {
            G.FillRectangle(Storage.Game.CellStates[c.State].BoardBrush, r);
        }

        public void GenFitBoard(int boardWidth, int boardHeight)
        {
            int[] widths = FitFields(boardWidth, Storage.Game.GameCols, BorderSize);
            int[] heights = FitFields(boardHeight, Storage.Game.GameRows, BorderSize);
            int currWidth = BorderSize;
            int currHeight = BorderSize;
            for (int i = 0; i < Storage.Game.GameRows; i++)
            {
                currWidth = BorderSize;
                for (int j = 0; j < Storage.Game.GameCols; j++)
                {
                    Board[i, j] = new Rectangle(currWidth, currHeight, widths[j], heights[i]);
                    currWidth += widths[j] + BorderSize;
                }
                currHeight += heights[i] + BorderSize;
            }
            Map = new Bitmap(boardWidth, boardHeight);
            G = Graphics.FromImage(Map);
            DrawBackground(boardWidth, boardHeight);
        }

        public void GenFixedBoard(int cellWidth, int cellHeight)
        {
            int currWidth = BorderSize;
            int currHeight = BorderSize;
            for (int i = 0; i < Storage.Game.GameRows; i++)
            {
                currWidth = BorderSize;
                for (int j = 0; j < Storage.Game.GameCols; j++)
                {
                    Board[i, j] = new Rectangle(currWidth, currHeight, cellWidth, cellHeight);
                    currWidth += cellWidth + BorderSize;
                }
                currHeight += cellHeight + BorderSize;
            }
            int boardWidth = (cellWidth + BorderSize) * Storage.Game.GameCols + BorderSize;
            int boardHeight = (cellHeight + BorderSize) * Storage.Game.GameRows + BorderSize;
            Map = new Bitmap(boardWidth, boardHeight);
            G = Graphics.FromImage(Map);
            DrawBackground(boardWidth, boardHeight);
        }

        public int[] FitFields(int imageWidth, int fieldsCount, int borderSize)
        {
            int[] widths = new int[fieldsCount];
            int fieldsWidth = imageWidth - (fieldsCount + 1) * borderSize;
            int basicWidth = fieldsWidth / fieldsCount;
            float additionalWidth = (float)fieldsWidth / fieldsCount - (float)Math.Floor((float)fieldsWidth / fieldsCount);
            float rest = 0;
            int summWidth = borderSize;
            int tempWidth;
            for (int i = 0; i < fieldsCount; i++)
            {
                if (i == fieldsCount - 1)
                {
                    tempWidth = imageWidth - borderSize - summWidth;
                }
                else
                {
                    if (rest >= 1)
                    {
                        tempWidth = basicWidth + 1;
                        rest--;
                    }
                    else
                    {
                        tempWidth = basicWidth;
                    }
                    rest += additionalWidth;
                }
                widths[i] = tempWidth;
                summWidth += tempWidth + borderSize;
            }
            return widths;
        }
    }
}
