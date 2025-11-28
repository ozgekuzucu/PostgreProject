using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PostgreProject.Models;

namespace PostgreProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
