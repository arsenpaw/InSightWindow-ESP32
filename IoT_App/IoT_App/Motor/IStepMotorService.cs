using System;
using System.Text;

namespace IoT_App.Motor
{
    public interface IStepMotorService
    {
        public void Rotate(int degrees);
    }
}
