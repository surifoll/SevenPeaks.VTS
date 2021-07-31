namespace SevenPeaks.VTS.Infrastructure.Interfaces
{
    public interface IStandardRabbitMq
    {
        object Consumer();
        void Publish(object message);
    }
}