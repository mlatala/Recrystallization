using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Storage
    {
        public static int States = 2;
        public static int Radius = 0;
        public static int Mode;
        public static Game Game;
        public static Engine Engine;
        public static FormMain MainForm;
        public static FormControlPanel ControlPanel;
        public static Drawer Drawer;

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static T[,] InitializeArray<T>(int rows, int cols) where T : new()
        {
            T[,] array = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = new T();
                }
            }
            return array;
        }
    }
}
