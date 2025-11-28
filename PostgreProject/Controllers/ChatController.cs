using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.Controllers
{
	public class ChatController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}