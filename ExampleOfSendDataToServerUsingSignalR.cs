using System.Diagnostics;
using System.Threading;
using System.Device.Wifi;
using nanoFramework.Networking;
using System.Net.Http;
using nanoFramework.Json;
using InSightWindow_IoT;
using System.Collections;
using System.IO;
using System;
using nanoFramework.SignalR.Client;
using InSightWindow_IoT.Models;
public class Program
{
	private static void HubConnection_Closed(object sender, SignalrEventMessageArgs message)
	{
		Debug.WriteLine($"closed received with message: {message.Message}");
	}
	private static AllWindowDataDto GetPreperatInfo()
	{
		//"This method is currently a stub and have to be replaced with the final implementation later."

		AllWindowDataDto allWindowDataDto = new AllWindowDataDto();
		allWindowDataDto.DeviceType = "Window";
		allWindowDataDto.Humidity = 23;
		allWindowDataDto.Temparature = 12;
		allWindowDataDto.isRain = false;
		allWindowDataDto.IsOpen = true;
		allWindowDataDto.Id = new Guid("8CFFEDCE-B147-4B1F-891D-60097431D52F");
		return allWindowDataDto;
	}

	public static void Main()
	{
		//connectio to internet part
		const string Ssid = "PC";
		const string Password = "123456789";
		AllWindowDataDto allWindowDataDto = GetPreperatInfo();
		CancellationTokenSource cs = new(60000);
		var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
		if (!success)
			Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
		else
		{
			//
			Debug.WriteLine("Work");
			//signalR part
			var options = new HubConnectionOptions() { Reconnect = true };
			HubConnection hubConnection = new HubConnection("http://192.168.0.197:81/client-hub", options: options);

			hubConnection.Closed += HubConnection_Closed;

			hubConnection.Start();

			while (hubConnection.State == HubConnectionState.Connected)
			{
				Debug.WriteLine("sending to server ");
				hubConnection.InvokeCoreAsync("SendWidnowStatusToClient", null, new object[] { JsonConvert.SerializeObject(allWindowDataDto) });

				Thread.Sleep(1000);
			}
			//
		}
	}
}
