using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Data_Collector
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //fields
        private Timer timer;
        MeasureLengthDevice measurement = null;
        DisplayMain displayMain = null;
        DateTime now { get; set; }
        private decimal unitsToUse = 0m;

        //constructor
        public MainPage()
        {
            this.InitializeComponent();
            //set imperialButton as checked at startup
            imperialButton.IsChecked = true;
            //create instance of MeasureLengthDevice and DisplayMain classes
            measurement = new MeasureLengthDevice();
            displayMain = new DisplayMain
            {
                //set MostRecentMeasure in DisplayMain to the property in MeasureLengthDevice
                MostRecentMeasure = measurement.MostRecentMeasure,
                //set DataCaptured in DisplayMain to the property in MeasureLengthDevice
                DataCaptured = measurement.DataCaptured
            };
        }

        //timer_Tick will pull a number from the device every 15 seconds. It will also set unitsToUse and set
        //the enumerator to Metric or Imperial. Finally, it will display the mostRecentMeasure through
        //data binding to the form.
        private async void timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //set time stamp for the timeBlock textBlock
                now = DateTime.Now;
                timeBlock.Text = now.ToString();

                //if statements to check which radiobutton was selected to set the value for unitsToUse
                if (metricButton.IsChecked == true)
                {
                    unitsToUse = 2.54m;
                    //send unitsToUse value to MetricValue in MeasureLengthDevice
                    measurement.MetricValue(unitsToUse);

                    //set typeEnum textBlock to show Metric from the enumeration class
                    typeEnum.Text = Units.Metric.ToString();
                }
                if (imperialButton.IsChecked == true)
                {
                    unitsToUse = 1.0m;
                    //send unitsToUse value to ImperialValue in MeasureLenthDevice
                    measurement.ImperialValue(unitsToUse);

                    //set typeEnum textBlock to show Imperial from the enumeration class
                    typeEnum.Text = Units.Imperial.ToString();
                }

                //Send MostRecentMeasure to data binding
                displayMain.MostRecentMeasure = measurement.MostRecentMeasure * unitsToUse;

                //need this.DataContext = null to refresh the display
                this.DataContext = null;
                this.DataContext = displayMain;
            });
        }

        //create a definition for the imperial radio button
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
        }

        //create a definition for the metric radio button
        private void metricButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
        }

        //startButton_Click starts a timer in Main and calls the MeasureLengthDevice class's 
        //StartCollecting and GetRawData methods
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
            measurement.StartCollecting();
        }

        //event handler stopButton_Click stops the timer in Main
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Dispose();
        }

        //historyButton_Click is a user control to show the history from the Device. It will
        //call the GetRawData() method in MeasureLengthDevice and display through data binding.
        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            measurement.GetRawData();
            displayMain.DataCaptured = measurement.DataCaptured;
            this.DataContext = null;
            this.DataContext = displayMain;
        }
    }
}
