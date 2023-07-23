using Microsoft.AspNetCore.Mvc;

namespace CourseManagementWebClient.Controllers
{
    public class MaterialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
