using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _CategoriesComponentPartial : ViewComponent
	{
		private readonly AppDbContext _context;

		public _CategoriesComponentPartial(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = await _context.Categories.ToListAsync();
			return View(categories);
		}
	}
}
