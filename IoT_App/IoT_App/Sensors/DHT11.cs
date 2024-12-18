using Iot.Device.DHTxx;
using IoT_App.ChainOfResponcebility;
using IoT_App.Models;
using System;
using System.Diagnostics;

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
            var temp_temperature = _dht11.Temperature;
            var temp_humidity = _dht11.Humidity;
            if (_dht11.IsLastReadSuccessful)
            {
                Debug.WriteLine($"Temperature: {temp_temperature.DegreesCelsius} \u00B0C, Humidity: {temp_humidity.Percent} %");
                Humidity = (int)Math.Floor(_dht11.Humidity.Value);
                Temparature = (int)Math.Floor(_dht11.Temperature.DegreesCelsius);
            }
            else
            {
                Humidity = 78;
                Temparature = 21;
                Debug.WriteLine("Error reading DHT sensor");
            }

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
