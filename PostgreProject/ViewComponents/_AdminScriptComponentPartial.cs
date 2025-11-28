using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _AdminScriptComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
