using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPUMonitor_1
{
    interface MonitorResource
    {
        void prepareMonitoring();

        void captureValue();

        void saveResults(String filename);
    }
}
