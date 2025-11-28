using Microsoft.AspNetCore.Mvc;

namespace PostgreProject.ViewComponents
{
	public class _ScriptComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
