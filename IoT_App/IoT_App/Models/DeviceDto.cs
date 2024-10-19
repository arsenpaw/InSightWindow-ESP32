using System;


namespace IoT_App.Models
{
    public class Device
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string DeviceType { get; set; }

        public virtual Guid UserId { get; set; }

    }
}
