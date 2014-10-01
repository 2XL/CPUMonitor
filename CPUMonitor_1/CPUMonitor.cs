using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CPUMonitor_1
{

    public class CPUMonitor
    {
        private bool finish = false;
        private LinkedList<float> cpuValues;
        private int interval;
        private string filename;
        private string process;

        public void ThreadProc()
        {
            this.start();
        }

        public void start()
        {
            finish = false;
            cpuValues = new LinkedList<float>();

            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter("Process", "% Processor Time", process);

            while (!finish)
            {
                Thread.Sleep(this.interval);
                float cpu = cpuCounter.NextValue();
                cpuValues.AddLast(cpu);
                Console.WriteLine(cpu);
            }
        }

        public LinkedList<float> stop()
        {
            finish = true;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.filename))
            {
                LinkedList<float>.Enumerator it = cpuValues.GetEnumerator();
                while (it.MoveNext()) {
                    file.WriteLine(it.Current);

                }
            }
            return cpuValues;
        }

        public void setInterval(int interval)
        {
            this.interval = interval;
        }

        public void setFilename(string filename)
        {
            this.filename = filename;
        }

        public void setProcess(string process)
        {
            this.process = process;
        }

        /*static void Main(string[] args)
        {
            CPUMonitor monitor = new CPUMonitor();
            Thread t = new Thread(new ThreadStart(CPUMonitor.ThreadProc));
            t.Start();
            Thread.Sleep(12000);
            monitor.end();
        }*/
    }
}
