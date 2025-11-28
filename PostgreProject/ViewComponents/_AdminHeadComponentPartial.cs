using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _AdminHeadComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
