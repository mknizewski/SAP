using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> Users { get; }
        IEnumerable<UsersSchools> Schools { get; }
        IEnumerable<UsersCounselor> Counselors { get; }
        IEnumerable<UserSolutions> Solutions { get; }

        bool AddUserCounselor(UsersCounselor model);

        bool AddSolution(UserSolutions solution);

        bool EditUserSchool(UsersSchools model);

        bool EditUserCounselor(UsersCounselor model);

        bool SendMessage(Messages model);

        void Dispose();
    }
}