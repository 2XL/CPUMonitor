﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CPUMonitor_1
{

    class CPUMonitor
    {
        private static bool finish = false;
        private static LinkedList<float> cpuValues = new LinkedList<float>();

        public static void ThreadProc()
        {
            CPUMonitor.Start();
        }

        public static void Start()
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

        public LinkedList<float> end()
        {
            finish = true;
            return cpuValues;
        }

        static void Main(string[] args)
        {
            CPUMonitor monitor = new CPUMonitor();
            Thread t = new Thread(new ThreadStart(CPUMonitor.ThreadProc));
            t.Start();
            Thread.Sleep(12000);
            monitor.end();
        }
    }
}