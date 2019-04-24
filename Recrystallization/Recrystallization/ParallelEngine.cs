using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace GameOfLife
{
    public class ParallelEngine
    {
        public class Segment
        {
            public int ColumnsCount { get; set; }
            public int ColumnsOffset { get; set; }
        }

        public Thread[] Threads { get; set; }
        public BackgroundWorker Launcher0 { get; set; }
        public BackgroundWorker Launcher1 { get; set; }
        public Segment[] Segments { get; set; }
        public int Steps { get; set; }
        public Stopwatch Timer { get; set; }
        public long Interval { get; set; }
        public ManualResetEvent[] ThreadsFinished { get; set; }
        public EventWaitHandle[] ThreadsLaunch { get; set; }
        public delegate void LogTextCallback(string txt);
        public delegate void ResultCallback();
        public delegate void InfoUpdateCallback(float f);

        public readonly object Block = new object();

        public ParallelEngine()
        {
            Timer = new Stopwatch();
            Interval = 1000;
        }

        public void ManualEventSetVal(ManualResetEvent[] man, bool val)
        {
            for(int i = 0;i < man.Length;i++)
            {
                if (val)
                    man[i].Set();
                else
                    man[i].Reset();
            }
        }

        public void Init(int thrCnt)
        {
            // threads
            ThreadsFinished = new ManualResetEvent[thrCnt];
            ThreadsLaunch = new EventWaitHandle[thrCnt];
            Threads = new Thread[thrCnt];
            for (int i = 0; i < thrCnt; i++)
            {
                ThreadsFinished[i] = new ManualResetEvent(false);
                ThreadsLaunch[i] = new AutoResetEvent(false);
                Threads[i] = new Thread(new ParameterizedThreadStart(ThreadWork));
                Threads[i].IsBackground = true;
            }
            // workers
            Launcher0 = new BackgroundWorker();
            Launcher0.DoWork += new DoWorkEventHandler(WithDrawing);
            Launcher0.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LauncherWorkComplete);
            Launcher1 = new BackgroundWorker();
            Launcher1.DoWork += new DoWorkEventHandler(WithoutDrawing);
            Launcher1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LauncherWorkComplete);           
            // count segments columns width and offsets
            Segments = new Segment[thrCnt];
            int[] segmentsCols = new int[thrCnt];
            int cols = Storage.Game.GameCols / thrCnt;
            for (int i = 0; i < thrCnt - 1; i++)
            {
                segmentsCols[i] = cols;
            }
            segmentsCols[thrCnt - 1] = Storage.Game.GameCols - (thrCnt - 1) * cols;
            int[] colsOffsets = CountOffsets(segmentsCols);
            for (int i = 0; i < thrCnt; i++)
            {
                Segment s = new Segment();
                s.ColumnsCount = segmentsCols[i];
                s.ColumnsOffset = colsOffsets[i];
                Segments[i] = s;
            }
            // run threads
            for (int i = 0; i < thrCnt; i++)
            {
                Threads[i].Start(i);    
            }
        }

        public void Run(int steps, bool draw)
        {
            if (Storage.Game.GameState != Game.GameStates.Run)
            {
                Storage.Game.GameState = Game.GameStates.Run;
                Storage.ControlPanel.UpdateGameInfo(-1);
                lock (Block)
                    Steps = steps;
                if (steps == 0)
                    Debug.WriteLine("SIMULATION STARTED");
                else
                    Debug.WriteLine("CUSTOM STEPS: " + steps + " WORK START");
                if (draw)
                    Launcher0.RunWorkerAsync();
                else
                    Launcher1.RunWorkerAsync();
            } 
        }

        public void Stop()
        {
            lock (Block)
                Steps = -1;
        }

        public void WithoutDrawing(object sender, DoWorkEventArgs e)
        {
            int iter = 0;
            // delegates
            ResultCallback refresh = new ResultCallback(Storage.MainForm.RefreshBoard);
            InfoUpdateCallback fps = new InfoUpdateCallback(Storage.ControlPanel.UpdateGameInfo);

            Debug.WriteLine("Launcher without drawing in thread: " + Thread.CurrentThread.ManagedThreadId + " started");
            for (; ; )
            {
                // check loop continouity
                if (Steps == -1)
                {
                    break;
                }
                else if (Steps > 0)
                {
                    if (iter >= Steps)
                        break;
                }
                Timer.Restart();
                // start threads and wait
                Storage.Game.StartNewIteration();
                Debug.WriteLine("launcher new iter");
                for (int i = 0; i < ThreadsLaunch.Length; i++)
                    ThreadsLaunch[i].Set();
                WaitHandle.WaitAll(ThreadsFinished);
                ManualEventSetVal(ThreadsFinished, false);
                Storage.Game.EndIteration();
                Debug.WriteLine("update finished in: " + Timer.ElapsedMilliseconds + "ms");
                long elapsed = Timer.ElapsedMilliseconds;
                Storage.MainForm.Invoke(fps, 1000f / elapsed);
                iter++;
            }
            Storage.Drawer.DrawIteration(-1);
            Storage.MainForm.Invoke(refresh);
            Debug.WriteLine("last iteration has been drawn");
            e.Result = iter;
        }

        public void WithDrawing(object sender, DoWorkEventArgs e)
        {
            int iter = 0;
            // delegates
            ResultCallback refresh = new ResultCallback(Storage.MainForm.RefreshBoard);
            InfoUpdateCallback fps = new InfoUpdateCallback(Storage.ControlPanel.UpdateGameInfo);

            Debug.WriteLine("Launcher with drawing in thread: " + Thread.CurrentThread.ManagedThreadId + " started");
            for (; ; )
            {
                // check loop continouity
                if (Steps == -1)
                {
                    break;
                }
                else if (Steps > 0)
                {
                    if (iter >= Steps)
                        break;
                }
                Timer.Restart();
                // start threads and wait
                Storage.Game.StartNewIteration();
                Debug.WriteLine("launcher new iter");
                for (int i = 0; i < ThreadsLaunch.Length; i++)
                    ThreadsLaunch[i].Set();
                WaitHandle.WaitAll(ThreadsFinished);
                ManualEventSetVal(ThreadsFinished, false);
                Storage.Game.EndIteration();
                Debug.WriteLine("update finished in: " + Timer.ElapsedMilliseconds + "ms");
                Storage.Drawer.DrawIteration(-1);               
                Storage.MainForm.Invoke(refresh);
                Debug.WriteLine("drawing finished in: " + Timer.ElapsedMilliseconds + "ms");
                // wait if needed
                long elapsed = Timer.ElapsedMilliseconds;
                if (iter != Steps - 1) // wait except last iteration
                {
                    int restTime = Convert.ToInt32(Interval - elapsed);
                    if (restTime > 0)
                    {
                        Thread.Sleep(restTime);
                    }
                }
                // ips
                float ips;
                if (Interval - elapsed > 0)
                {
                    ips = 1000f / Interval;
                }
                else
                {
                    ips = 1000f / elapsed;
                }
                Storage.MainForm.Invoke(fps, ips);
                iter++;
            }                     
            e.Result = iter;
        }

        private void LauncherWorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            int iters = (int)e.Result;
            Debug.WriteLine("FINISHED ITERATIONS: " + iters);
            Storage.Game.GameState = Game.GameStates.Paused;
            Storage.ControlPanel.UpdateGameInfo(-1);
        }

        private void ThreadWork(object obj)
        {
            int tId = (int)obj;
            Segment s = Segments[tId];
            while(true)
            {
                ThreadsLaunch[tId].WaitOne();
                Debug.WriteLine("thread_" + tId + " in: " + Thread.CurrentThread.ManagedThreadId + " update started");
                // logic
                Storage.Game.UpdateNewIteration(s.ColumnsOffset, s.ColumnsOffset + s.ColumnsCount - 1);
                ThreadsFinished[tId].Set();
                Debug.WriteLine("thread_" + tId + " in thread: " + Thread.CurrentThread.ManagedThreadId + " update finished");
            }
        }

        public int[] CountOffsets(int[] tab)
        {
            int[] res = new int[tab.Length];
            int off = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                res[i] = off;
                off += tab[i];
            }
            return res;
        }

        public void AddRandomIter()
        {
            Storage.Game.StartNewIteration();
            Random r = new Random();
            for (int i = 0; i < Storage.Game.GameRows; i++)
            {
                for (int j = 0; j < Storage.Game.GameCols; j++)
                {
                    Storage.Game.Current[i, j].State = (byte)r.Next(0, 2);
                }
            }          
            Storage.Game.EndIteration();
            Storage.Drawer.DrawIteration(-1);
            Storage.MainForm.RefreshBoard();
            Storage.ControlPanel.UpdateGameInfo(-1);
        }

        public void CreateIteration(bool f)
        {
            Storage.Game.StartNewIteration();
            for (int i = 0; i < Storage.Game.GameRows; i++)
            {
                for (int j = 0; j < Storage.Game.GameCols; j++)
                {
                    Storage.Game.Current[i, j].State = (byte)(f ? 1 : 0);
                }
            }            
            Storage.Game.EndIteration();
            Storage.Drawer.DrawIteration(-1);
            Storage.MainForm.RefreshBoard();
            Storage.ControlPanel.UpdateGameInfo(-1);
        }
    }
}
