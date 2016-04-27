using System.Web.Mvc;

namespace SAP.Web.Areas.User.Controllers
{
    public class MessagesController : Controller
    {
        // GET: User/Messages
        public ActionResult Index()
        {
            return View();
        }
    }
}