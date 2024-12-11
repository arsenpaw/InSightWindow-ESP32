namespace IoT_App.Models
{
    public class AllSensorData

    {
        private int _temperature;
        private int _humidity;
        private bool _isRain;
        private bool _isOpen;
        private bool _isAlarm;

        public int Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    IsDataChanged = true;
                }
            }
        }

        public int Humidity
        {
            get => _humidity;
            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    IsDataChanged = true;
                }
            }
        }

        public bool IsRain
        {
            get => _isRain;
            set
            {
                if (_isRain != value)
                {
                    _isRain = value;
                    IsDataChanged = true;
                }
            }
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen != value)
                {
                    _isOpen = value;
                    IsDataChanged = true;
                }
            }
        }

        //TODO: Handle in alarm service
        public bool IsAlarm
        {
            get => _isAlarm;
            set
            {
                if (_isAlarm != value)
                {
                    _isAlarm = value;
                    IsDataChanged = true;
                }
            }
        }

        public bool IsDataChanged { get; private set; } = false;

        public void ResetDataChangedFlag()
        {
            IsDataChanged = false;
        }
    }
}
