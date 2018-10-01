using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Collector
{
    interface IMeasuringDevice
    {
        //MetricValue method returns unitsToUse to a metric value as a decimal
        decimal MetricValue(decimal metric);

        //ImperialValue method returns unitsToUse to an imperial value as a decimal
        decimal ImperialValue(decimal imperial);

        //StartCollecting method starts a timer which collects data from the Device class
        void StartCollecting();

        //StopCollecting method stops the timer and stops collecting data
        void StopCollecting();

        //GetRawData method returns the dataCaptured string by calling the PrintValues method
        string GetRawData();
    }
}
