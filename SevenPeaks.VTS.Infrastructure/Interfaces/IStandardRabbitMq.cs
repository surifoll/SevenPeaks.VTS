using System;
using RabbitMQ.Client.Events;

namespace SevenPeaks.VTS.Infrastructure.Interfaces
{
    public interface IStandardRabbitMq
    {
        void Publish(object message);
    }
}