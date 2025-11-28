using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.Controllers
{
	public class AdminLayoutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
