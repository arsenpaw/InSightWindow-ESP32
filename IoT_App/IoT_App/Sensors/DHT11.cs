using Iot.Device.DHTxx;
using IoT_App.ChainOfResponcebility;
using IoT_App.Models;
using System;

namespace IoT_App.Sensors
{
    public class DHT11 : DataComposeHandler, ISensor
    {
        public int Temparature { get; set; }

        public int Humidity { get; set; }

        private readonly Dht11 _dht11;

        public DHT11()
        {
            _dht11 = new Dht11(AppSettings.DHT11_PIN);
        }

        public ISensor ReadData()
        {
            Humidity = (int)Math.Floor(_dht11.Humidity.Value);
            Temparature = (int)Math.Floor(_dht11.Temperature.DegreesCelsius);
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
