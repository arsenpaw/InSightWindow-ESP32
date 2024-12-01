using System;
using System.Text;

namespace IoT_App.Motor
{
    public interface IStepMotorManager
    {
        public bool WindowOpen();

        public bool WindowClose();

        public void WindowStop();
    }
}
