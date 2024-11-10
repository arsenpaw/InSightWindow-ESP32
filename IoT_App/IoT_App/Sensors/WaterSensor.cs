using Iot.Device.LiquidLevel;
using IoT_App.ChainOfResponcebility;
using IoT_App.Models;
using System;
using System.Device.Adc;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace IoT_App.Sensors
{
    public class WaterSensor : DataComposeHandler, ISensor
    {
        public int WaterLevel { get; set; }

        public bool IsRain { get; set; }

        private AdcController AdcController { get; set; }

        private AdcChannel adcChannel { get; set; }

        public WaterSensor(AdcController adcController)
        {
            AdcController = adcController;
            adcChannel = AdcController.OpenChannel(AppSettings.ADC_LIQUID_SENSOR_PIN);

        }
        public ISensor ReadData()
        {
            int analogValue = adcChannel.ReadValue();          
            //double voltage = (analogValue / 4095.0) * 3.3;
            Debug.WriteLine($"Analog Value: {analogValue}");
            WaterLevel = analogValue;
            IsRain = WaterLevel > AppSettings.WATTER_LEVEL_VALUE;
            return this;
        }

        public override object DataCompose(AllSensorData request)
        {
            ReadData();
            request.IsRain = IsRain;
            return base.DataCompose(request);
        }
    }
}
