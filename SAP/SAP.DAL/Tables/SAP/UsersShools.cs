using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class UsersSchools
    {
        [Key]
        public int Id { get; set; }

        [Key, ForeignKey("User")]
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Class { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}