using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Command.Implementation
{
    public class CloseCommand
    {
        public void Execute()
        {
            Debug.WriteLine("Close Command Executed");
        }
    }
}
