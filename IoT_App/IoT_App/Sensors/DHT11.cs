using System;
using System.Text;

namespace IoT_App.Sensors
{
   public class DHT11 : ISensor
    {
        public int Temparature { get; set; }

        public ISensor ReadData()
        {
            // Read data from sensor
            Temparature = 25;
            return this;
        }
    }
    
    
}
