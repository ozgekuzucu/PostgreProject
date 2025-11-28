using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _AboutComponentPartial : ViewComponent
	{
		private readonly AppDbContext _context;

		public _AboutComponentPartial(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var about = await _context.Abouts.FirstOrDefaultAsync();
			return View(about);
		}
	}
}
