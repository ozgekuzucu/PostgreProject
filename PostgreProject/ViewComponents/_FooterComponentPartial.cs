using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _FooterComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
