using IoT_App.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace IoT_App.ChainOfResponcebility
{
    public abstract class DataComposeHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual object DataCompose(AllSensorData request)
        {
            Debug.WriteLine("DataComposeHandler.DataCompose");
            if (this._nextHandler != null)
            {
                return this._nextHandler.DataCompose(request);
            }
            else
            {
                return null;
            }
        }

    }
}
