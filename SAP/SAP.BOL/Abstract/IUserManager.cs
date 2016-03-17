using SAP.BOL.HelperClasses;
using SAP.DAL.Tables;

namespace SAP.BOL.Abstract
{
    public interface IUserManager
    {
        UserData GetUserDataById(string userId);

        bool ChangeUserSchool(string userId, string name, string sclass, string city, string houseNumber, string postalCode, string street, string phone);

        bool ChangeUserCounselor(string userId, string firstName, string lastName);

        bool AddUserCounselot(string userId, string firstName, string lastName);

        bool SendMessage(string userId, string title, string desc);

        UsersSchools GetUserSchoolById(string userId);

        UsersCounselor GetUserCounselorById(string userId);

        void Dispose();
    }
}