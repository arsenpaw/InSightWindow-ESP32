using System;

namespace InSightWindow_IoT.Models
{
    public class Device
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string DeviceType { get; set; }

        public virtual Guid UserId { get; set; }

    }
}
