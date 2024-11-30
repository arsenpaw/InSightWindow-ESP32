using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Command.Implementation
{
    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            Debug.WriteLine("Open Command Executed");
        }
    }
}
