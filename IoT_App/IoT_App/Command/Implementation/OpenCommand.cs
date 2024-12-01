using IoT_App.Motor;
using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Command.Implementation
{
    public class OpenCommand : ICommand
    {
        private readonly IStepMotorManager _stepMotor;

        public OpenCommand(IStepMotorManager stepMotor)
        {
            _stepMotor = stepMotor;
        }

        public void Execute()
        {
            Debug.WriteLine("Open Command Executed");
            _stepMotor.WindowOpen();
        }
    }
}
