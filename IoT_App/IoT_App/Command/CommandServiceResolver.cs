using IoT_App.Command.Implementation;
using IoT_App.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace IoT_App.Command
{
    public static class CommandServiceResolver
    {

        static  Hashtable _services = new Hashtable()
        {
            { CommandEnum.Open, typeof(OpenCommand) },
            { CommandEnum.Close, typeof(CloseCommand) },
            //{ CommandEnum.StopAlarm, typeof(StopAlarmCommand) },
            //{ CommandEnum.StartAlarm, typeof(StartAlarmCommand) },
            //{ CommandEnum.Stop, typeof(StopCommand) }
        };

        public static void AddCommandServices(this IServiceCollection services)
        {
            
            foreach (DictionaryEntry service in _services)
            {
                services.AddTransient((Type)service.Value);
            }
        }

        public static ICommand GetServiceByCommand(this IServiceProvider services, CommandEnum command)
        {
            var commandType = (Type)_services[command];
            return (ICommand)services.GetService(commandType);
        }
    }
}
