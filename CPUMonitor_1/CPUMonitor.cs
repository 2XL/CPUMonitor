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
        private LinkedList<string> processes;

        public void ThreadProc()
        {
            this.start();
        }

        public void start()
        {
            finish = false;
            cpuValues = new LinkedList<float>();

            LinkedList<PerformanceCounter> cpuCounter = new LinkedList<PerformanceCounter>();
            foreach (string process in processes) {
                cpuCounter.AddLast(new PerformanceCounter("Process", "% Processor Time", process));
            }
            //cpuCounter = new PerformanceCounter("Process", "% Processor Time", process);

            while (!finish)
            {
                Thread.Sleep(this.interval);
                float cpu = 0;
                foreach (PerformanceCounter counter in cpuCounter) {
                    cpu += counter.NextValue();
                }
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

        public void setProcess(LinkedList<string> processes)
        {
            this.processes = processes;
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
