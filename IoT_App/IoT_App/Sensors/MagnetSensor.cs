using IoT_App.ChainOfResponcebility;
using IoT_App.Models;
using System.Device.Gpio;

namespace IoT_App.Sensors
{
    public class MagnetSensor : DataComposeHandler, ISensor
    {
        private readonly GpioController _gpioController;


        public MagnetSensor()
        {
            _gpioController = new GpioController();
            _gpioController.OpenPin(AppSettings.magnetSensorPin, PinMode.InputPullUp);
        }
        public bool IsOpen { get; set; }

        public ISensor ReadData()
        {
            IsOpen = _gpioController.Read(AppSettings.magnetSensorPin) == PinValue.High;
            return this;
        }
        public override object DataCompose(AllSensorData request)
        {
            ReadData();
            request.IsOpen = IsOpen;
            return base.DataCompose(request);
        }
    }
}
