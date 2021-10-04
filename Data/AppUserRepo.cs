using System;
using System.Collections.Generic;
using System.Linq;
using Eis.Identity.Api.Models;

namespace Eis.Identity.Api.Data
{
    public class AppUserRepo : IAppUserRepo
    {
        private readonly AppDbContext _context;

        public AppUserRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateUser(AppUser user)
        {
            if(user == null) 
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<AppUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public AppUser GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}