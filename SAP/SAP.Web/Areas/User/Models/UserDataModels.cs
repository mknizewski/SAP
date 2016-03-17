namespace SAP.Web.Areas.User.Models
{
    public class UserDataModels
    {
        //User
        public string UserId { get; set; }

        public string UserEmail { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPostalCode { get; set; }
        public string UserStreet { get; set; }
        public string UserHouseNumber { get; set; }
        public string UserCity { get; set; }
        public string UserPhone { get; set; }

        //Szkoła
        public string SchoolName { get; set; }

        public string SchoolClass { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolStreet { get; set; }
        public string SchoolHouseNumber { get; set; }
        public string SchoolPostalCode { get; set; }
        public string SchoolPhone { get; set; }

        //Opiekun
        public string CounselorFirstName { get; set; }

        public string CounselorLastName { get; set; }
    }
}