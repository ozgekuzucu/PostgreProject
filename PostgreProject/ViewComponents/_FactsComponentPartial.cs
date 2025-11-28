using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _FactsComponentPartial : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
