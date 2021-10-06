using Microsoft.Extensions.Configuration;
using Eis.Identity.Api.Dtos;
using RabbitMQ.Client;

namespace Eis.Identity.Api.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory() 
            { 
                HostName = _config["RabbitMQHost"], 
                Port = int.Parse(_config["RabbitMQPort"])
            };
        }

        public void PublishNewPlatform(AppUserPublishedDto abc)
        {

        }
    }
}