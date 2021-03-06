﻿using SAP.BOL.HelperClasses;
using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.BOL.Abstract
{
    public interface IUserManager
    {
        IEnumerable<UserSolutions> Solutions { get; }
        IEnumerable<Messages> Messages { get; }

        UserData GetUserDataById(string userId);

        bool ChangeUserSchool(string userId, string name, string sclass, string city, string houseNumber, string postalCode, string street, string phone);

        bool ChangeUserCounselor(string userId, string firstName, string lastName);

        bool AddUserCounselot(string userId, string firstName, string lastName);

        bool SendMessage(string userId, string title, string desc);

        bool DeleteMessage(int messageId);

        UsersSchools GetUserSchoolById(string userId);

        UsersCounselor GetUserCounselorById(string userId);

        bool AddSolution(int taskId, int tourId, int phaseId, string userId, int compilerId, int score, string program, double memUsage, double timeUsage, string error);

        void Dispose();
    }
}