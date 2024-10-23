using IoT_App.Models;
using System;
using System.Text;

namespace IoT_App.ChainOfResponcebility
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object DataCompose(AllSensorData request);
    }
}
