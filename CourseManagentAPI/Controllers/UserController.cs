using Microsoft.AspNetCore.Mvc;

namespace CourseManagmentAPI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
