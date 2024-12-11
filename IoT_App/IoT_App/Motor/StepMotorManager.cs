using System;
using System.Text;

namespace IoT_App.Motor
{
    public class StepMotorManager: IStepMotorManager
    {
        private readonly IStepMotor _stepMotor;

        public StepMotorManager(IStepMotor stepMotor)
        {
            _stepMotor = stepMotor;
        }

        public bool WindowOpen()
        {
            //TODO Algo to corrext the rotation
            _stepMotor.Rotate(90);
            return true;
        }

        public bool WindowClose()
        {
            //TODO Algo to /corrext the rotation
            _stepMotor.Rotate(-2700);
            return true;
        }
        public void WindowStop()
        {
            //TODO Logic to handle next steps
            _stepMotor.Stop();
        }

    }
}
