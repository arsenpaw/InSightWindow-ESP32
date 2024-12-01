using Iot.Device.Uln2003;
using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Motor
{
    public class StepMotor : IStepMotor
    {
        private Uln2003 Motor { get; set; }
        public StepMotor(Uln2003 motor)
        {
            Motor = motor;
            Motor.Mode = StepperMode.FullStepDualPhase;
            Motor.RPM = short.MaxValue;
        }

        public void Rotate(int degrees)
        {
            Debug.WriteLine($"Rotating {degrees} degrees");        
             Motor.Step(degrees); 

        }

        public void Stop()
        {
            Debug.WriteLine("Stopping Motor");
            Motor.Stop();
        }
    }
    
}
