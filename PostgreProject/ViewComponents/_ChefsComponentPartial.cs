using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _ChefsComponentPartial : ViewComponent
	{
		private readonly AppDbContext _context;

		public _ChefsComponentPartial(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var chefs = await _context.Chefs.ToListAsync();
			return View(chefs);
		}
	}
}
