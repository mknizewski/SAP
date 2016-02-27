using SAP.BOL.HelperClasses;

namespace SAP.BOL.Abstract
{
    public interface IUserManager
    {
        UserData GetUserDataById(string userId);
        void Dispose();
    }
}
