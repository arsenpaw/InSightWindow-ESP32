using Iot.Device.Uln2003;
using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Motor
{
    public class StepMotor: IStepMotor
    {
        private Uln2003 Motor { get; set; }
        public StepMotor(int pin1,int pin2, int pin3, int pin4)
        {
            Motor = new Uln2003 (pin1, pin2, pin3, pin4);
            Motor.Mode = StepperMode.FullStepDualPhase;
            Motor.RPM = short.MaxValue;
        }

        public void Rotate(int degrees)
        {
            Debug.WriteLine($"Rotating {degrees} degrees");        
             Motor.Step(degrees); 

        }
    }
    
}
