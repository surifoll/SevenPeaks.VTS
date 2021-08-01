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
        private  IModel _channel;
        private RabbitMqSettings _settings;
        private static string stringResult;
        public StandardRabbitMq(RabbitMqSettings settings)
        {
            _settings = settings;
            _vehiclePosition = settings.VehiclePositionQueue;
            _connectionString = settings.ConnectionString;
            _channel = RabbitMqChannel();
        }

        public  string Consumer()
        {
            
            var consumer = new EventingBasicConsumer(_channel);
            //var result = "";
            EventHandler<BasicDeliverEventArgs> consumerOnReceived = (sender, args) =>
            {
                var message = args.Body;
                var body = message.ToArray().GetString();
                stringResult = body;
            };
            consumer.Received += consumerOnReceived;
            _channel.BasicConsume(_vehiclePosition, true, consumer);

            return stringResult;
        }

        private void HandleMessage(string val)
        {
            stringResult = val;
        }

        public  void Publish(object message)
        {
            var body = message.GetByteArray();
            _channel.BasicPublish("", _vehiclePosition, null, body);
        }

        private  IModel RabbitMqChannel()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Hostname,
                UserName = _settings.Username,
                Password = _settings.Password,
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(_vehiclePosition, true, false, false, null);
            return channel;
        }
    }
}