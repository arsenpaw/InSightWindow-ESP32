using IoT_App.Observer.Enum;
using System;
using System.Text;

namespace IoT_App.Observer
{
    public interface IEventPublisher
    {
        public event EventHandler<NetworkEventsEnum> NetworkEvent;
        void RaiseNetworkEvent(NetworkEventsEnum networkEvents); 
        void RaiseServerEvent();
    }
}
