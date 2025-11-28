using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _AdminSidebarComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
