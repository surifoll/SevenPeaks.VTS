using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SevenPeaks.VTS.Common.ExtensionMethods;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;

namespace SevenPeaks.VTS.Infrastructure.Services
{
    public class StandardRabbitMq : IStandardRabbitMq
    {
        private readonly string _vehiclePosition;
        private readonly string _connectionString;
        private readonly IModel _channel;

        public StandardRabbitMq(RabbitMqSettings settings, string vehiclePosition)
        {
            _vehiclePosition = vehiclePosition;
            _connectionString = settings.ConnectionString;
            _channel = RabbitMqChannel();
        }

        public  object Consumer()
        {
            
            var consumer = new EventingBasicConsumer(_channel);
            var result = new object();
            consumer.Received += (sender, args) =>
            {
                var message = args.Body;
                var body = message.ToArray().GetString();
                result = body;
            };
            _channel.BasicConsume(_vehiclePosition, true, consumer);

            return result;
        }
        public  void Publish(object message)
        {
            // var message = new { DeviceId = "hgybkfiuf", Longitude = 11.3435454, Latitude = 11.323333, VehicleId = 1 };
            var body = message.GetByteArray();
            _channel.BasicPublish("", _vehiclePosition, null, body);
        }

        private  IModel RabbitMqChannel()
        {
             
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString)
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(_vehiclePosition, true, false, false, null);
            return channel;
        }
    }
}