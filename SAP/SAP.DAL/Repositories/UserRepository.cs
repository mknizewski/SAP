using SAP.DAL.Abstract;
using System;
using System.Collections.Generic;
using SAP.DAL.Tables;
using SAP.DAL.DbContext;

namespace SAP.DAL.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private ApplicationDbContext _context;

        public UserRepository()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<UsersCounselor> Counselors
        {
            get
            {
                return _context.UsersCounselor;
            }

        }

        public IEnumerable<UsersSchools> Schools
        {
            get
            {
                return _context.UsersSchools;
            }

        }

        public IEnumerable<ApplicationUser> Users
        {
            get
            {
                return _context.Users;
            }

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
