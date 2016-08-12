using Microsoft.AspNet.Identity.Owin;
using SAP.BOL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.DbContext.SAP;
using SAP.Web.Areas.User.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SAP.Web.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private IUserManager _userManager;
        private ApplicationUserManager _aspUserManager;

        public ApplicationUserManager AspUserManager
        {
            get
            {
                return _aspUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _aspUserManager = value;
            }
        }

        public HomeController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        // GET: User/Home
        public async Task<ActionResult> Index()
        {
            var user = await AspUserManager.FindByNameAsync(User.Identity.Name);
            var data = _userManager.GetUserDataById(user.Id);

            UserDataModels modelView = new UserDataModels
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserCity = user.City,
                UserPhone = user.PhoneNumber,
                UserHouseNumber = user.HouseNumber,
                UserPostalCode = user.PostalCode,
                UserStreet = user.Street,

                SchoolCity = data.School.City,
                SchoolClass = data.School.Class,
                SchoolName = data.School.Name,
                SchoolHouseNumber = data.School.HouseNumber,
                SchoolPhone = data.School.Phone,
                SchoolPostalCode = data.School.PostalCode,
                SchoolStreet = data.School.Street,
            };

            if (data.Counselor != null)
            {
                modelView.CounselorFirstName = data.Counselor.FirstName;
                modelView.CounselorLastName = data.Counselor.LastName;
            }

            return View(modelView);
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing && _aspUserManager != null)
            {
                _userManager.Dispose();
                _aspUserManager.Dispose();

                _userManager = null;
                _aspUserManager = null;
            }

            base.Dispose(disposing);
        }

        #endregion Helpers
    }
}