using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;
using SevenPeaks.VTS.Common.ExtensionMethods;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SevenPeaks.VTS.Web.BackgroundServices
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IAddVehiclePositionCommand _positionCommand;
        private readonly IStandardRabbitMq _rabbitMq;
        private readonly IModel _channel;
        private readonly RabbitMqSettings _settings;
        private IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IStandardRabbitMq rabbitMq)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                _positionCommand = scope.ServiceProvider.GetRequiredService<IAddVehiclePositionCommand>();
                _settings = scope.ServiceProvider.GetRequiredService<RabbitMqSettings>();
            }

            _channel = RabbitMqChannel();
            _rabbitMq = rabbitMq;
            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation($"Queue [{_rabbitMq.GetType()}] is waiting for messages.");

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
             
            var consumer = new EventingBasicConsumer(_channel);
            //var result = "";
            EventHandler<BasicDeliverEventArgs> consumerOnReceived = async (sender, args) =>
            {
                var message = args.Body;
                var body = message.ToArray().GetString();
                HandleResponse(body);
            };
            consumer.Received += consumerOnReceived;
            _channel.BasicConsume(_settings.VehiclePositionQueue, true, consumer);
          
        }

        private void HandleResponse(string body)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            _positionCommand = scope.ServiceProvider.GetRequiredService<IAddVehiclePositionCommand>();
            if(!string.IsNullOrWhiteSpace(body))
            {
                MessageResponse<int> result;
                result =  _positionCommand.Execute(JsonSerializer.Deserialize<AddVehiclePositionModel>(body)).GetAwaiter().GetResult();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _logger.LogInformation("RabbitMQ connection is closed.");
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
            channel.QueueDeclare(_settings.VehiclePositionQueue, true, false, false, null);
            return channel;
        }
    }
}