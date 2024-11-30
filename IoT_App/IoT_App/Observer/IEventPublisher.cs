using IoT_App.Observer.Enum;
using nanoFramework.Runtime.Events;
using System;
using System.Text;

namespace IoT_App.Observer
{
    public interface IEventObserver
    {
        public void EnableEventHandling();
    }
}
