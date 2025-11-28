using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _HeadComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
