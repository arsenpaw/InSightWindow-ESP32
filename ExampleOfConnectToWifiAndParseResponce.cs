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

const string Ssid = "PC";
const string Password = "123456789";
// Give 60 seconds to the wifi join to happen
CancellationTokenSource cs = new(60000);
var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
if (!success)
{
	// Something went wrong, you can get details with the ConnectionError property:
	Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
	if (WifiNetworkHelper.HelperException != null)
	{
		Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
	}
}
else
{
	try
	{
		Debug.WriteLine("Work");
		using (HttpClient http = new HttpClient())
		{
			var responce = http.Get("http://192.168.0.198:81/api/DevicesDb");
			Debug.WriteLine();
			string strResponse = responce.Content.ReadAsString();
			var dict = (Hashtable)JsonConvert.DeserializeObject(strResponse, typeof(Hashtable));
			ArrayList t = (ArrayList)dict["$values"];
			foreach (Hashtable item in t)
			{
				item.Remove("$id");
				var jsonStr = JsonConvert.SerializeObject(item);
				Debug.WriteLine(jsonStr);

				Device device = (Device)JsonConvert.DeserializeObject(jsonStr, typeof(Device));
				Debug.WriteLine(device.deviceType);
				Debug.WriteLine(device.id.ToString());
			}


		}

	}
	catch (Exception ex)
	{

		Debug.WriteLine(ex.Message);
	}

}