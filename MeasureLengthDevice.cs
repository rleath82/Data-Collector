using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Data_Collector
{
    class MeasureLengthDevice : IMeasuringDevice
    {
        //fields
        private decimal unitsToUse = 0m;
        private decimal mostRecentMeasure = 0m;
        FixedSizedQueue<string> dataCaptured = new FixedSizedQueue<string>();   //create an instance of what type to be stored
        private Device myDevice = null;
        private Timer timer = null;
        private int limit = 10;
        DateTime now { get; set; }
        private string unitsConv = " ";

        //Constructor to initialize fields and start timer
        public MeasureLengthDevice()
        {
            myDevice = new Device();
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(2).TotalMilliseconds, (int)TimeSpan.FromSeconds(2).TotalMilliseconds);
        }

        //Limit property sets the value at 10
        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        //Properties MostRecentMeasure and DataCaptured send values to DisplayMain for data binding
        public decimal MostRecentMeasure
        {
            get { return this.mostRecentMeasure; }
        }

        public string DataCaptured
        {
            //GetRawData calls the PrintValues(dataCaptured) method
            get { return GetRawData(); }
        }

        //timer_Tick method sets the values for mostRecentMeasure and DataCaptured from the Device. It also sets a 
        //time stamp and converts dataCaptured to the appropriate unitsToUse.
        private async void timer_Tick(object state)
        {
            dataCaptured.Limit = limit;
            now = DateTime.Now;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.mostRecentMeasure = myDevice.GetMeasurement;
                if (unitsToUse == 2.54m)
                    unitsConv = "cm";
                else
                    unitsConv = "inches";
                dataCaptured.Enqueue((mostRecentMeasure * unitsToUse) + " " + unitsConv + " - " + now.ToString());
            });
        }

        //PrintValues method converts the dataCaptured values to a string to display
        public string PrintValues(FixedSizedQueue<string> myCollection)
        {
            //create an instance of the StringBuilder class
            StringBuilder myString = new StringBuilder();
            foreach (var i in myCollection.q) // var is an open variable that can grab any type/object
            {
                myString.AppendLine(i.ToString());  //AppendLine automatically adds a new line after each
            }
            return myString.ToString();
        }

        //timer_Stop() method stops the timer in MeasureLengthDevice
        public void timer_Stop()
        {
            timer.Dispose();
        }

        //MetricValue method implements the IMeasuringDevice interface and sets unitsToUse to metric
        public decimal MetricValue(decimal metric)
        {
            this.unitsToUse = metric;
            return this.unitsToUse;
        }

        //ImperialValue method implements the IMeasuringDevice interface and sets unitsToUse to imperial
        public decimal ImperialValue(decimal imperial)
        {
            this.unitsToUse = imperial;
            return this.unitsToUse;
        }

        //StartCollecting method implements the IMeasuringDevice interface and starts a timer to collect data
        //from the Device
        public void StartCollecting()
        {
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(2).TotalMilliseconds, (int)TimeSpan.FromSeconds(2).TotalMilliseconds);
        }

        //StopCollecting method implements the IMeasuringDevice interface and stops the timer
        public void StopCollecting()
        {
            timer_Stop();
            mostRecentMeasure = 0m;
        }

        //GetRawData method implements the IMeasuringDevice interface, calls the PrintValues method and 
        //sends dataCaptured as an argument.
        public string GetRawData()
        {
            return PrintValues(dataCaptured);
        }
    }
}
