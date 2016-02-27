using SAP.BOL.Abstract;
using System;
using System.Linq;
using SAP.BOL.HelperClasses;
using SAP.DAL.Abstract;

namespace SAP.BOL.LogicClasses.Managers
{
    public class UserManager : IUserManager, IDisposable
    {
        private IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }

        public UserData GetUserDataById(string userId)
        {
            UserData model = new UserData
            {
                School = _userRepository.Schools.Select(x => x).Where(x => x.UserId == userId).FirstOrDefault(),
                Counselor = _userRepository.Counselors.Select(x => x).Where(x => x.UserId == userId).FirstOrDefault()
            };

            return model;
        }
    }
}
