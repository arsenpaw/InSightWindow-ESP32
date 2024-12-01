using IoT_App.Motor;
using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Command.Implementation
{
    public class CloseCommand: ICommand
    {
        private readonly IStepMotorManager _stepMotor;

        public CloseCommand(IStepMotorManager stepMotor)
        {
            _stepMotor = stepMotor;
        }

        public void Execute()
        {
            Debug.WriteLine("Close Command Executed");
        }
    }
}
