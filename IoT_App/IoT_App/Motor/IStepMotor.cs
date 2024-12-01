using System;
using System.Text;

namespace IoT_App.Motor
{
    public interface IStepMotor
    {
        public void Rotate(int degrees);

        public void Stop();
    }
}
