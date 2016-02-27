using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> Users { get; }
        IEnumerable<UsersSchools> Schools { get; }
        IEnumerable<UsersCounselor> Counselors { get; }
        void Dispose();
    }
}
