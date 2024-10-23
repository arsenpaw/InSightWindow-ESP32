using System;


namespace IoT_App.Models
{
    public abstract class DeviceDto
    {

        public readonly Guid Id = Guid.NewGuid();

        public const string DeviceType = "Window";

        public virtual Guid UserId { get; set; }

    }
}
