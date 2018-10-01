using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Collector
{
    //DisplayMain class contains properties DataCaptured and MostRecentMeasure, sets the values sent from
    //MeasureLengthDevice and returns those values, which are used in Main to display the data through data binding.
    class DisplayMain
    {
        private string dataCaptured;
        public string DataCaptured
        {
            get { return this.dataCaptured; }
            set { this.dataCaptured = value; }
        }

        private decimal mostRecentMeasure;
        public decimal MostRecentMeasure
        {
            get { return this.mostRecentMeasure; }
            set { this.mostRecentMeasure = value; }
        }
    }
}
