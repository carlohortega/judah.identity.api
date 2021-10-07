using Microsoft.Extensions.Configuration;
using Eis.Identity.Api.Dtos;
using RabbitMQ.Client;
using System;

namespace Eis.Identity.Api.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _conn;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory() 
            { 
                HostName = _config["RabbitMQHost"], 
                Port = int.Parse(_config["RabbitMQPort"])
            };

            try 
            {
                _conn = factory.CreateConnection();
                _channel = _conn.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _conn.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to messagebus.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not connect to MessageBus: {ex.Message}.");
            }
        }

        public void PublishNewPlatform(AppUserPublishedDto abc)
        {

        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs args) 
        {
            Console.WriteLine("--> RabbitMQ connection shutdown.");
        }
    }
}