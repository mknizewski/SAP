﻿using SAP.DAL.Abstract;
using System;
using System.Collections.Generic;
using SAP.DAL.Tables;
using SAP.DAL.DbContext;
using System.Linq;

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

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
