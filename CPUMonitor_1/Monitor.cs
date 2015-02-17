using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CPUMonitor_1
{
    public class Monitor
    {
        private bool finish = false;
        private int interval;
        private string filename;
        private LinkedList<string> processes;
        LinkedList<MonitorResource> resources;

        public void ThreadProc()
        {
            this.start();
        }

        public void start()
        {
            finish = false;

            while (!finish)
            {
                Thread.Sleep(this.interval);
                foreach (MonitorResource resource in resources)
                {
                    resource.captureValue();
                }
            }
        }

        public void stop()
        {
            finish = true;
            foreach (MonitorResource resource in resources)
            {
                resource.saveResults(this.filename);
            }
        }

        public void prepareMonitoring()
        {
            this.resources = new LinkedList<MonitorResource>();
            this.resources.AddLast(new MemoryMonitor());
            this.resources.AddLast(new DiskResource());
            this.resources.AddLast(new CPUMonitor(this.processes));

            foreach (MonitorResource resource in this.resources)
            {
                resource.prepareMonitoring();
            }
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
