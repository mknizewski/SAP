using SAP.DAL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.DAL.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private SAPDbContext _context;

        public UserRepository()
        {
            _context = SAPDbContext.Create();
        }

        public IEnumerable<UsersCounselor> Counselors
        {
            get
            {
                return _context.UsersCounselor;
            }
        }

        public IEnumerable<Messages> Messages
        {
            get
            {
                return _context.Messages;
            }
        }

        public IEnumerable<UsersSchools> Schools
        {
            get
            {
                return _context.UsersSchools;
            }
        }

        public IEnumerable<UserSolutions> Solutions
        {
            get
            {
                return _context.UserSolutions;
            }
        }

        public IEnumerable<ApplicationUser> Users
        {
            get
            {
                return _context.Users;
            }
        }

        public bool AddSolution(UserSolutions solution)
        {
            try
            {
                _context.UserSolutions.Add(solution);
                _context.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public bool AddUserCounselor(UsersCounselor model)
        {
            try
            {
                _context.UsersCounselor.Add(model);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMessage(int messageId)
        {
            try
            {
                var message = _context.Messages.Find(messageId);

                _context.Messages.Remove(message);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _context = null;
        }

        public bool EditUserCounselor(UsersCounselor model)
        {
            try
            {
                var dbModel = _context.UsersCounselor
                    .Select(x => x)
                    .Where(x => x.UserId == model.UserId)
                    .FirstOrDefault();

                dbModel.FirstName = model.FirstName;
                dbModel.LastName = model.LastName;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditUserSchool(UsersSchools model)
        {
            try
            {
                var dbModel = _context.UsersSchools
                .Select(x => x)
                .Where(x => x.UserId == model.UserId)
                .FirstOrDefault();

                dbModel.Name = model.Name;
                dbModel.City = model.City;
                dbModel.Class = model.Class;
                dbModel.HouseNumber = model.HouseNumber;
                dbModel.Phone = model.Phone;
                dbModel.Street = model.Street;
                dbModel.PostalCode = model.PostalCode;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendMessage(Messages model)
        {
            try
            {
                _context.Messages.Add(model);
                _context.SaveChanges();

                return true;
            }
            catch
            { return false; }
        }
    }
}