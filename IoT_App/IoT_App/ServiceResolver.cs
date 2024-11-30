
using System


using System.Collections;
using IoT_App.Cimmands;
using IoT_App.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace IoT_App { 

    public static class ServiceResolversConfiguration
    {
        public static void AddServiceResolver(this IServiceCollection services, Hashtable dictionary)
        {     
            foreach (DictionaryEntry entry in dictionary)
            {
                var commandKey = (CommandEnum)entry.Key;
                var command = (Type)entry.Value;
                services.AddScoped(typeof(Command));
            }
        }
    }
}