//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using IoT_App.Builder;
using IoT_App.Models;
using IoT_App.Sensors;
using nanoFramework.Json;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using nanoFramework.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using IoT_App;
using IoT_App.Services;
using Microsoft.Extensions.DependencyInjection;
#if HAS_WIFI
using System.Device.Wifi;
#endif

namespace HttpWebRequestSample
{ 

    public class Program
    {

        public static void Main()
        {
            var services = ConfigureServices();
            var application = (Application)services.GetRequiredService(typeof(Application));
            application.Run();
        }
        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(typeof(Application))
                .AddSingleton(typeof(IAesService), typeof(AesService))
                .AddSingleton(typeof(IBuilder), typeof(MicrocontrollerBuilder))
                .BuildServiceProvider();
        }
    }

 }

