using IoT_App.Observer.Enum;
using nanoFramework.Networking;
using nanoFramework.SignalR.Client;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace IoT_App.Observer
{
    public class EventObserver : IEventObserver

    {
        HubConnection _hubConnection { get; set; }
        private bool _isReconnecting = false;

        public EventObserver(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
        }

        public void EnableEventHandling()
        {
            NetworkChange.NetworkAddressChanged += (sender, e) => HandleNetworkAddressChange(NetworkInterface.GetAllNetworkInterfaces());
            _hubConnection.Closed += (sender, args) => RaiseNetworkEvent(NetworkEventsEnum.OnConnectionLost);
            //_hubConnection.Reconnecting += (sender, args) => RaiseNetworkEvent(NetworkEventsEnum.OnConnectionReconnecting);
        }
        //TODO Make that normal because this
        private void HandleNetworkAddressChange(NetworkInterface[] networkInterfaces)
        {
            if (networkInterfaces is null)
            {
                return;
            }

            foreach (var netInterface in networkInterfaces)
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    if (netInterface.IPv4Address == null || netInterface.IPv4Address == "0.0.0.0")
                    {
                        // Disconnected
                        Debug.WriteLine("Wi-Fi connection lost.");
                        RaiseNetworkEvent(NetworkEventsEnum.OnConnectionLost);
                    }
                    else
                    {
                        // Connected or reconnected
                        Debug.WriteLine($"Wi-Fi connected. IP: {netInterface.IPv4Address}");
                        RaiseNetworkEvent(NetworkEventsEnum.OnConnectionReconnecting);
                    }
                }
            }
        }

        public void RaiseNetworkEvent(NetworkEventsEnum networkEvents)
        {
            switch (networkEvents)
            {
                case NetworkEventsEnum.OnConnectionLost:
                    AttemptReconnect();
                    break;

                case NetworkEventsEnum.OnConnectionReconnecting:

                    while (_hubConnection.State != HubConnectionState.Connected)
                    {
                        _hubConnection.Start();
                    }

                    _isReconnecting = false;
                    Debug.WriteLine("Wi-Fi connection established.");
                    break;

                default:
                    Debug.WriteLine("Unknown network event.");
                    break;
            }
        }

        private void AttemptReconnect()
        {
            if (!_isReconnecting)
            {
                _isReconnecting = true;
                Debug.WriteLine("Attempting to reconnect to Wi-Fi...");
                var reconnectResult = WifiNetworkHelper.Reconnect();

                if (reconnectResult)
                {
                    Debug.WriteLine("Wi-Fi reconnection successful.");
                    RaiseNetworkEvent(NetworkEventsEnum.OnConnectionReconnecting);
                }
                else
                {
                    Debug.WriteLine("Wi-Fi reconnection failed.");
                }

                _isReconnecting = false;
            }
            else
            {
                Debug.WriteLine("Already attempting to reconnect. Skipping redundant reconnect attempt.");
            }
        }
    }
}
