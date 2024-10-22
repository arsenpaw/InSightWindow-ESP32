using System;

namespace IoT_App.Models
{
    public class WindpwDataDto : DeviceDto
    {
        public int Temparature { get; set; }

        public int Humidity { get; set; }

        public bool isRain { get; set; }

        public bool IsOpen { get; set; }

    }
}
