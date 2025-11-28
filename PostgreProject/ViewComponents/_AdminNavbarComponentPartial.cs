using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _AdminNavbarComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
