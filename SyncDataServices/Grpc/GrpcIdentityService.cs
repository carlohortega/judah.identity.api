using System.Threading.Tasks;
using AutoMapper;
using Eis.Identity.Api.Data;
using Grpc.Core;

namespace Eis.Identity.Api.SyncDataServices.Grcp
{
    public class GrpcIdentityService : GrpcIdentity.GrpcIdentityBase
    {
        private readonly IAppUserRepo _repo;
        private readonly IMapper _mapper;

        public GrpcIdentityService(IAppUserRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<AppUsersResponse> GetAllAppUsers(GetAllRequest request,
            ServerCallContext context) 
        {
            var response = new AppUsersResponse();
            var appUsers = _repo.GetAllUsers();

            foreach(var item in appUsers)
            {
                response.AppUsers.Add(_mapper.Map<GrpcIdentityModel>(item));
            }

            return Task.FromResult(response);
        }
    }
}