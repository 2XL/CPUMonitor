using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace CPUMonitor_1
{
    class DiskResource : MonitorResource
    {
        private DriveInfo[] allDrives;
        private LinkedList<float> diskValues;

        public DiskResource()
        {
            this.allDrives = DriveInfo.GetDrives();
        }

        public void prepareMonitoring()
        {
            this.diskValues = new LinkedList<float>();
        }

        public void captureValue()
        {
            foreach (DriveInfo d in allDrives)
            {
                if (!d.Name.Contains("C:"))
                {
                    continue;
                }

                if (d.IsReady == true)
                {
                    diskValues.AddLast(d.TotalFreeSpace);
                    /*Console.Clear();
                    
                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);*/
                }
            }
        }

        public void saveResults(String filename)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("disk_" + filename))
            {
                LinkedList<float>.Enumerator it = diskValues.GetEnumerator();
                while (it.MoveNext())
                {
                    file.WriteLine(it.Current / 1024);

                }
            }
        }

        /*public static void Main(string[] args)
        {
            DiskResource disk = new DiskResource();
            for (int i = 0; i < 100; i++)
            {
                disk.captureValue();
                Thread.Sleep(100);
            }
        }*/
    }
}
