using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;

namespace PostgreProject.ViewComponents
{
	public class _ServiceComponentPartial : ViewComponent
	{
		private readonly AppDbContext _context;

		public _ServiceComponentPartial(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var services = await _context.Services.ToListAsync();
			return View(services);
		}
	}
}
