using SAP.BOL.Abstract;
using SAP.BOL.HelperClasses;
using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SAP.BOL.LogicClasses.Managers
{
    public class UserManager : IUserManager, IDisposable
    {
        private IUserRepository _userRepository;

        public IEnumerable<UserSolutions> Solutions
        {
            get
            {
                return _userRepository.Solutions;
            }
        }

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ChangeUserCounselor(string userId, string firstName, string lastName)
        {
            UsersCounselor model = new UsersCounselor
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName
            };

            bool result = _userRepository.EditUserCounselor(model);

            return result;
        }

        public bool ChangeUserSchool(string userId, string name, string sClass, string city, string houseNumber, string postalCode, string street, string phone)
        {
            UsersSchools model = new UsersSchools
            {
                UserId = userId,
                Name = name,
                Class = sClass,
                City = city,
                HouseNumber = houseNumber,
                PostalCode = postalCode,
                Street = street,
                Phone = phone
            };

            bool result = _userRepository.EditUserSchool(model);

            return result;
        }

        public UserData GetUserDataById(string userId)
        {
            UserData model = new UserData
            {
                School = _userRepository.Schools
                .Select(x => x)
                .Where(x => x.UserId == userId)
                .FirstOrDefault(),

                Counselor = _userRepository.Counselors
                .Select(x => x)
                .Where(x => x.UserId == userId)
                .FirstOrDefault()
            };

            return model;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
            _userRepository = null;
        }

        public UsersSchools GetUserSchoolById(string userId)
        {
            return _userRepository.Schools
                .Select(x => x)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
        }

        public UsersCounselor GetUserCounselorById(string userId)
        {
            return _userRepository.Counselors
                .Select(x => x)
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
        }

        public bool AddUserCounselot(string userId, string firstName, string lastName)
        {
            UsersCounselor userCounselorModel = new UsersCounselor
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName
            };

            bool result = _userRepository.AddUserCounselor(userCounselorModel);

            return result;
        }

        public bool SendMessage(string userId, string title, string desc)
        {
            Messages messageRow = new Messages
            {
                UserId = userId,
                Title = title,
                Description = desc,
                SendTime = DateTime.Now
            };

            bool result = _userRepository.SendMessage(messageRow);
            return result;
        }

        public bool AddSolution(int taskId, int tourId, string userId, int compilerId, int score, string program, double memUsage, double timeUsage)
        {
            UserSolutions solution = new UserSolutions
            {
                UserId = userId,
                TournamentId = tourId,
                TaskId = taskId,
                CompilerId = compilerId,
                Score = score,
                Program = program,
                MemoryUsage = memUsage,
                ExecutedTime = timeUsage,
                InsertTime = DateTime.Now
            };

            bool result = _userRepository.AddSolution(solution);
            return result;
        }
    }
}