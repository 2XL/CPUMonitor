using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace CPUMonitor_1
{
    class MemoryMonitor : MonitorResource
    {
        private PerformanceCounter ramCounter;
        private LinkedList<float> ramValues;

        public MemoryMonitor()
        {
            this.ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
        }

        public void prepareMonitoring()
        {
            this.ramValues = new LinkedList<float>();
        }

        public void captureValue()
        {
            ramValues.AddLast(ramCounter.NextValue());
        }

        public void saveResults(String filename)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("mem_"+filename))
            {
                LinkedList<float>.Enumerator it = ramValues.GetEnumerator();
                while (it.MoveNext())
                {
                    file.WriteLine(it.Current);

                }
            }
        }

        /*public static void Main(string[] args)
        {
            MemoryMonitor memory = new MemoryMonitor();
            for (int i = 0; i < 1000; i++)
            {
                memory.captureValue();
                Thread.Sleep(1000);
            }
        }*/
    }
}
