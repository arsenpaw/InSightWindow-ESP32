using IoT_App.Enums;
using System;
using System.Text;

namespace IoT_App.Command
{
    public abstract class CommandDto
    {
        public CommandEnum Command { get; set; } 
    }
}
