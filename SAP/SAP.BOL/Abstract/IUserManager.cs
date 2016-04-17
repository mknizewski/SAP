using SAP.BOL.HelperClasses;
using SAP.DAL.Tables;
using System.Collections;
using System.Collections.Generic;

namespace SAP.BOL.Abstract
{
    public interface IUserManager
    {
        IEnumerable<UserSolutions> Solutions { get; }

        UserData GetUserDataById(string userId);

        bool ChangeUserSchool(string userId, string name, string sclass, string city, string houseNumber, string postalCode, string street, string phone);

        bool ChangeUserCounselor(string userId, string firstName, string lastName);

        bool AddUserCounselot(string userId, string firstName, string lastName);

        bool SendMessage(string userId, string title, string desc);

        UsersSchools GetUserSchoolById(string userId);

        UsersCounselor GetUserCounselorById(string userId);

        bool AddSolution(int taskId, int tourId, string userId, int compilerId, int score, string program, double memUsage, double timeUsage, string error);

        void Dispose();
    }
}