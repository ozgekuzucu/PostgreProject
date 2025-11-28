using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.Controllers
{
	public class LayoutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
