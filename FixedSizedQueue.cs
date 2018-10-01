using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Data_Collector
{
    public class FixedSizedQueue<T>
    {
        public ConcurrentQueue<T> q = new ConcurrentQueue<T>(); //type of collection object
        private object lockObject = new object();   //create an instance of the object class
        public int Limit { get; set; }  //Limit property specifies how many objects to be stored
        public void Enqueue(T obj)  //putting the value in the ConcurrentQueue
        {
            q.Enqueue(obj);     //Enqueue the object
            lock (lockObject)   //in the event that drops the count once max value is reached
            {
                T overflow;
                while (q.Count > Limit && q.TryDequeue(out overflow));  //once count reaches 10, drop the least recent value
            }
        }
    }
}
