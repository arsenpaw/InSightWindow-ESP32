using System;
using System.Text;

namespace IoT_App.Motor
{
    public class StepMotorManager
    {
        private readonly IStepMotor _stepMotor;

        public StepMotorManager(IStepMotor stepMotor)
        {
            _stepMotor = stepMotor;
        }

        public void WindowOpen()
        {
            //TODO Algo to corrext the rotation
            _stepMotor.Rotate(90);
        }

        public void WindowClose()
        {
            //TODO Algo to /corrext the rotation
            _stepMotor.Rotate(-90);
        }
        public void WindowStop()
        {
            //TODO Logic to handle next steps
            _stepMotor.Stop();
        }

    }
}
