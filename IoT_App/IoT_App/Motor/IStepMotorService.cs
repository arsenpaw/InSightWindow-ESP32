using System;
using System.Text;

namespace IoT_App.Motor
{
    internal interface IStepMotorManager
    {
        public bool WindowOpen();

        public bool WindowClose();

        public void WindowStop();
    }
}
