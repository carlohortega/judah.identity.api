using Eis.Identity.Api.Dtos;

namespace Eis.Identity.Api.AsyncDataServices 
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(AppUserPublishedDto abc);
    }
}