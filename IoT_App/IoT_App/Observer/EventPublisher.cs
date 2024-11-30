using IoT_App.Observer.Enum;
using nanoFramework.SignalR.Client;
using System;
using System.Text;

namespace IoT_App.Observer
{
    public class EventPublisher : IEventPublisher
    {

        public event EventHandler<NetworkEventsEnum> NetworkEvent;
        HubConnection _hubConnection { get; set; }

        public EventPublisher(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;

            _hubConnection.Closed += (object sender, SignalrEventMessageArgs message) => 
                RaiseNetworkEvent(NetworkEventsEnum.OnConnectionLost);
        }        

        public void RaiseNetworkEvent(NetworkEventsEnum networkEvents )
        {
            NetworkEvent?.Invoke(this, networkEvents);
        }

        public void RaiseServerEvent()
        {
            throw new NotImplementedException();
        }
    }
}
