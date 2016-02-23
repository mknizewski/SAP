using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.BOL.Abstract
{
    public interface IContact
    {
        bool AddNewContact(string firstName, string lastName, string email, string message);
    }
}
