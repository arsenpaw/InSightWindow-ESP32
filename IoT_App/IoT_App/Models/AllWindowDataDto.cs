using System;

namespace IoT_App.Models
{
    public class AllWindowDataDto : Window
    {
        public int Temparature { get; set; }

        public int Humidity { get; set; }

        public bool isRain { get; set; }

    }
}
