using System.Collections.Generic;
using Eis.Identity.Api.Models;

namespace Eis.Identity.Api.Data
{
    public interface IAppUserRepo
    {
        bool SaveChanges();
        IEnumerable<AppUser> GetAllUsers();
        AppUser GetUserById(int id);
        void CreateUser(AppUser user);
    }
}