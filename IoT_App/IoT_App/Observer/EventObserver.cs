using IoT_App.Observer.Enum;
using System;
using System.Text;

namespace IoT_App.Observer
{

    public class EventObserver
    {
        private readonly IEventPublisher _eventPublisher;
        public EventObserver(IEventPublisher eventPublisher)
        {
            eventPublisher.NetworkEvent += OnNetworkEvent;
        }
        private void OnNetworkEvent(object sender, NetworkEventsEnum networkEvents)
        {
            Console.WriteLine("Event1 triggered");
        }
    }
}
