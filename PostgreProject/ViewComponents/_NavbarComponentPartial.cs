using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _NavbarComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
