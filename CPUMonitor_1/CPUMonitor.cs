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
        private LinkedList<float> cpuValues = new LinkedList<float>();

        public void ThreadProc()
        {
            this.start();
        }

        public void start()
        {

            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            while (!finish)
            {
                Thread.Sleep(100);
                float cpu = cpuCounter.NextValue();
                cpuValues.AddLast(cpu);
                Console.WriteLine(cpu);
            }
        }

        public LinkedList<float> stop()
        {
            finish = true;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"test.txt"))
            {
                LinkedList<float>.Enumerator it = cpuValues.GetEnumerator();
                while (it.MoveNext()) {
                    file.WriteLine(it.Current);

                }
            }
            return cpuValues;
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
