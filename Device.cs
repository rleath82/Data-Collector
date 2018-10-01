using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Data_Collector
{
    //Device class generates random numbers from a timer and sends them to the GetMeasurement property
    class Device
    {
        //fields
        Timer timer = null;
        Random rnd = new Random();
        private decimal getMeasurement = 0m;

        //GetMeasurement property gets the random numbers generated from the timer_Tick method
        public decimal GetMeasurement
        {
            get { return getMeasurement; }
        }

        public Device()
        {
            //create a timer and set the timer event to a method
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(1).TotalMilliseconds);
        }

        private async void timer_Tick(object state)
        {
            //code to randomly generate a new value and update GetMeasurement data
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    getMeasurement = rnd.Next(1, 11);
                });
        }
    }
}
