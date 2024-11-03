using IoT_App.ChainOfResponcebility;
using IoT_App.Models;
using System;
using System.Text;

namespace IoT_App.Sensors
{
   public class DHT11 : DataComposeHandler, ISensor
    {
        public int Temparature { get; set; }

        public int Humidity { get; set; }

        public ISensor ReadData()
        {
            Humidity = 50;
            Temparature = 25;
            return this;
        }

        public override object DataCompose(AllSensorData request)
        {
            this.ReadData();
           request.Humidity = Humidity;
            request.Temperature = Temparature;
            return base.DataCompose(request);
        }
    }
    
    
}
