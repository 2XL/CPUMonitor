using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CPUMonitor_1
{

    public class CPUMonitor : MonitorResource
    {
        private LinkedList<float> cpuValues;
        private LinkedList<string> processes;
        LinkedList<PerformanceCounter> cpuCounter;

        public CPUMonitor(LinkedList<string> processes)
        {
            this.cpuCounter = new LinkedList<PerformanceCounter>();
            this.processes = processes;
        }

        public void prepareMonitoring()
        {
            this.cpuValues = new LinkedList<float>();
            LinkedList<PerformanceCounter> cpuCounter = new LinkedList<PerformanceCounter>();
            foreach (string process in processes)
            {
                cpuCounter.AddLast(new PerformanceCounter("Process", "% Processor Time", process));
            }
        }

        public void captureValue()
        {
            float cpu = 0;
            foreach (PerformanceCounter counter in cpuCounter) {
                cpu += counter.NextValue();
            }
            cpuValues.AddLast(cpu);
        }

        public void saveResults(string filename)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("cpu_" + filename))
            {
                LinkedList<float>.Enumerator it = cpuValues.GetEnumerator();
                while (it.MoveNext()) {
                    file.WriteLine(it.Current);

                }
            }
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
