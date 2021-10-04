using System.Threading.Tasks;
using Eis.Identity.Api.Dtos;

namespace Eis.Identity.Api.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendIdentityToCommand(AppUserReadDto appUser);
    }
}